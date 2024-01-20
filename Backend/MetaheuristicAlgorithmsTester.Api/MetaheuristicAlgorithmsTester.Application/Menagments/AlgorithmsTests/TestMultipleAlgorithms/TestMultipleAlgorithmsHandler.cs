using AlgorithmInterfaces;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using System.Reflection;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithms
{
    public class TestMultipleAlgorithmsHandler(IAlgorithmsRepository algorithmsRepository, IFitnessFunctionRepository fitnessFunctionRepository, IMediator mediator, IExecutedMultipleAlgorithmsRepositor executedMultipleAlgorithmsRepositor) : IRequestHandler<TestMultipleAlgorithms, MultipleAlgorithmTestResult>
    {
        public async Task<MultipleAlgorithmTestResult> Handle(TestMultipleAlgorithms request, CancellationToken cancellationToken)
        {
            var results = new Result[request.Algorithms.Count];
            string multipleExecutedId = Guid.NewGuid().ToString("N");
            DateOnly date = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = await ExecuteWithFindBestParameters(request.Algorithms[i].Id, request.FitnessFunctionID, request.Depth, request.Dimension, request.SatisfiedResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Save to db
            foreach (var algorithmResult in results)
            {
                if (algorithmResult != null)
                {
                    var x = new ExecutedMultipleAlgorithms()
                    {
                        Date = date,
                        MultipleTestId = multipleExecutedId,
                        FBest = algorithmResult.FBest,
                        XBest = algorithmResult.XBest,
                        TestedAlgorithmId = algorithmResult.TestedAlgorithmId,
                        TestedFitnessFunctionId = algorithmResult.TestedFitnessFunctionId,
                        TestedAlgorithmName = algorithmResult.TestedFitnessFunctionName,
                        TestedFitnessFunctionName = algorithmResult.TestedFitnessFunctionName,
                        IsFailed = false,
                        NumberOfEvaluationFitnessFunction = algorithmResult.NumberOfEvaluationFitnessFunction,
                        Parameters = algorithmResult.Parameters
                    };
                    await executedMultipleAlgorithmsRepositor.AddExecudedAlgorithm(x);
                }
            }
            var executedAlgorithms = await executedMultipleAlgorithmsRepositor.GetExecutedAlgorithmsByExecutedId(multipleExecutedId);

            var result = new MultipleAlgorithmTestResult()
            {
                MultipleExecutedId = multipleExecutedId,
                ExecutedAlgorithms = executedAlgorithms
            };

            return result;
        }

        public async Task<Result> ExecuteWithFindBestParameters(int algorithmId, int fitnessFunctionId, int depth, int dimension, double satisfiedResult)
        {
            var algorithm = await algorithmsRepository.GetAlgorithmById(algorithmId);
            var fitnessFunction = await fitnessFunctionRepository.GetFitnessFunctionById(fitnessFunctionId);

            if (algorithm == null || algorithm.DllFileBytes == null)
            {
                throw new Exception($"Algorithm with id {algorithmId} not found");
            }
            if (fitnessFunction == null || fitnessFunction.DllFileBytes == null)
            {
                throw new Exception($"Fitnness function with id {fitnessFunctionId} not found");
            }

            Assembly algorithmAssembly = Assembly.Load(algorithm.DllFileBytes);
            Type[] algoriuthmTypes = algorithmAssembly.GetTypes();
            foreach (Type algorithmType in algoriuthmTypes)
            {
                if (typeof(IAlgorithm).IsAssignableFrom(algorithmType))
                {
                    MethodInfo method = algorithmType.GetMethod("Solve");
                    if (method != null)
                    {
                        object algorithmInstance = Activator.CreateInstance(algorithmType);

                        Assembly fitnessFunctionAssembly = Assembly.Load(fitnessFunction.DllFileBytes);
                        Type[] fitnessFunctionTypes = fitnessFunctionAssembly.GetTypes();
                        foreach (Type fitnessFunctionType in fitnessFunctionTypes)
                        {
                            if (typeof(IFitnessFunction).IsAssignableFrom(fitnessFunctionType))
                            {
                                object fitnessFunctionInstance = Activator.CreateInstance(fitnessFunctionType);

                                var currentParameter = new List<double>();
                                try
                                {
                                    List<List<double>> parameterCombinations = GenerateVariance(depth, algorithm.Parameters.ToArray(), algorithm.Parameters.Count - 1);
                                    Result lastResult = new Result();
                                    foreach (var parameters in parameterCombinations)
                                    {
                                        currentParameter = parameters;
                                        List<double> resultParametes = [.. parameters, dimension];
                                        object[] methodArgs = [fitnessFunctionInstance, resultParametes.ToArray()];
                                        method.Invoke(algorithmInstance, methodArgs);

                                        PropertyInfo xBestProperty = algorithmType.GetProperty("XBest");
                                        PropertyInfo fBestProperty = algorithmType.GetProperty("FBest");
                                        PropertyInfo numberOfEvaluationFitnessFunctionProperty = algorithmType.GetProperty("NumberOfEvaluationFitnessFunction");

                                        double[] xBestValue = (double[])xBestProperty.GetValue(algorithmInstance);
                                        double fBestValue = (double)fBestProperty.GetValue(algorithmInstance);
                                        int numberOfEvaluationFitnessFunctionValue = (int)numberOfEvaluationFitnessFunctionProperty.GetValue(algorithmInstance);

                                        if (fBestValue <= satisfiedResult)
                                        {
                                            return new Result()
                                            {
                                                TestedAlgorithmId = algorithm.Id,
                                                TestedAlgorithmName = algorithm.Name,
                                                TestedFitnessFunctionId = fitnessFunction.Id,
                                                TestedFitnessFunctionName = fitnessFunction.Name,
                                                FBest = fBestValue,
                                                XBest = xBestValue,
                                                Parameters = resultParametes,
                                                NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue,
                                            };
                                        }
                                        else
                                        {
                                            if (lastResult.FBest > fBestValue)
                                            {
                                                lastResult.TestedAlgorithmId = algorithm.Id;
                                                lastResult.TestedAlgorithmName = algorithm.Name;
                                                lastResult.TestedFitnessFunctionId = fitnessFunction.Id;
                                                lastResult.TestedFitnessFunctionName = fitnessFunction.Name;
                                                lastResult.Parameters = resultParametes;
                                                lastResult.XBest = xBestValue;
                                                lastResult.FBest = fBestValue;
                                                lastResult.NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue;
                                            }
                                        }
                                    }
                                    return lastResult;
                                }
                                catch (Exception ex)
                                {
                                    var x = currentParameter;
                                    throw new Exception($"Something went wrong: {ex.Message}");
                                }
                            }
                        }
                        throw new Exception($"The dll file does not have a class implementing the IFitnessFunction interface");
                    }
                    throw new Exception("Method Solve not found");
                }
            }
            return null;
        }

        public static List<List<double>> GenerateVariance(int depth, Domain.Entities.ParamInfo[] paramsInfo, int numbOfParams)
        {
            var x = GenerateParameterPossibleValues(depth, paramsInfo, numbOfParams).ToList();
            return GenerateParameterCombinations(x);
        }
        private static List<List<double>> GenerateParameterPossibleValues(int depth, Domain.Entities.ParamInfo[] paramsInfo, int numbOfParams)
        {
            List<List<double>> possibleValues = new List<List<double>>();
            for (int i = 0; i < numbOfParams; i++)
            {
                possibleValues.Add(new List<double>());
                for (int j = 0; j < depth - 1; j++)
                {
                    double step = (paramsInfo[i].UpperBoundary - paramsInfo[i].LowerBoundary) / (double)(depth - 1);

                    if (!paramsInfo[i].IsFloatingPoint)
                    {
                        int stepInt = (int)step;
                        possibleValues[i].Add(paramsInfo[i].LowerBoundary + j * stepInt);
                    }
                    else
                    {
                        possibleValues[i].Add(paramsInfo[i].LowerBoundary + j * step);
                    }
                }
                possibleValues[i].Add(paramsInfo[i].UpperBoundary);

            }
            return possibleValues;
        }
        static List<List<double>> GenerateParameterCombinations(List<List<double>> inputParameters)
        {
            List<List<double>> outputCombinations = new List<List<double>>();
            GenerateCombinations(inputParameters, 0, new List<double>(), outputCombinations);
            return outputCombinations;
        }
        static void GenerateCombinations(List<List<double>> inputParameters, int currentIndex, List<double> currentCombination, List<List<double>> outputCombinations)
        {
            if (currentIndex == inputParameters.Count)
            {
                outputCombinations.Add(new List<double>(currentCombination));
                return;
            }

            foreach (var value in inputParameters[currentIndex])
            {
                currentCombination.Add(value);
                GenerateCombinations(inputParameters, currentIndex + 1, currentCombination, outputCombinations);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }
    }
    public class Result
    {
        public int TestedAlgorithmId { get; set; }
        public string TestedAlgorithmName { get; set; } = default!;

        public int TestedFitnessFunctionId { get; set; }
        public string TestedFitnessFunctionName { get; set; } = default!;
        public double[]? XBest { get; set; }
        public double FBest { get; set; }
        public List<double> Parameters { get; set; }

        public int NumberOfEvaluationFitnessFunction { get; set; }
    }
}
