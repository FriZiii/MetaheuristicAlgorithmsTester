using AlgorithmInterfaces;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using System.Reflection;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithms
{
    public class TestMultipleAlgorithmsHandler(IAlgorithmsRepository algorithmsRepository, IFitnessFunctionRepository fitnessFunctionRepository, IMediator mediator) : IRequestHandler<TestMultipleAlgorithms, IEnumerable<AlgorithmTestResult>>
    {
        public async Task<IEnumerable<AlgorithmTestResult>> Handle(TestMultipleAlgorithms request, CancellationToken cancellationToken)
        {
            var result = new AlgorithmTestResult[request.Algorithms.Count];

            for (int i = 0; i < result.Length; i++)
            {
                var parameters = await FindBestParameters(request.Algorithms[i].Id, request.FitnessFunctionID, request.Depth, request.Dimension, request.SatisfiedResult);
                result[i] = await mediator.Send(new TestSingleAlgorithm.TestSingleAlgorithm() { AlgorithmId = request.Algorithms[i].Id, FitnessFunctionID = request.FitnessFunctionID, Parameters = parameters });
            }

            return result;
        }

        public async Task<List<double>> FindBestParameters(int algorithmId, int fitnessFunctionId, int depth, int dimension, double satisfiedResult)
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

                                try
                                {
                                    List<List<double>> parameterCombinations = GenerateVariance(depth, algorithm.Parameters.ToArray(), algorithm.Parameters.Count - 1);
                                    List<BestParameters> tempResult = new List<BestParameters>();
                                    foreach (var parameters in parameterCombinations)
                                    {
                                        List<double> resultParametes = [.. parameters, dimension];
                                        object[] methodArgs = [fitnessFunctionInstance, resultParametes.ToArray()];
                                        method.Invoke(algorithmInstance, methodArgs);
                                        PropertyInfo fBestProperty = algorithmType.GetProperty("FBest");
                                        double fBestValue = (double)fBestProperty.GetValue(algorithmInstance);

                                        if (fBestValue <= satisfiedResult)
                                        {
                                            return resultParametes;
                                        }
                                        else
                                        {
                                            tempResult.Add(new BestParameters()
                                            {
                                                FBest = fBestValue,
                                                Parameters = resultParametes.ToArray(),
                                            });
                                        }
                                    }
                                    tempResult = tempResult.OrderBy(bp => bp.FBest).ToList();
                                    return tempResult[0].Parameters.ToList();
                                }
                                catch (Exception ex)
                                {
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
            var y = GenerateParameterCombinations(x);
            return y;
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

        #region chat
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
        #endregion
    }
    class BestParameters
    {
        public double FBest { get; set; }
        public double[] Parameters { get; set; }
    }
}
