using AlgorithmInterfaces;
using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithms;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Timers;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithmsSafeMode
{
    public class TestMultipleAlgorithmsSafeModeHandler(
        IAlgorithmsRepository algorithmsRepository,
        IFitnessFunctionRepository fitnessFunctionRepository,
        IMediator mediator,
        IExecutedMultipleAlgorithmsRepository executedMultipleAlgorithmsRepositor,
        IAlgorithmStateRepository algorithmStateRepository
        )
        : IRequestHandler<TestMultipleAlgorithmsSafeMode, MultipleAlgorithmTestResult>
    {
        private int executedId;
        private object algorithmInstance;
        private Type algorithmType;
        private System.Timers.Timer timer;
        private string currentState;
        string executedStateFileName;
        string multipleExecutedId;
        DateOnly date;
        private int timerFrequency;
        List<int> usedParameterIndexes;
        List<List<double>> parameterCombinations;
        public async Task<MultipleAlgorithmTestResult> Handle(TestMultipleAlgorithmsSafeMode request, CancellationToken cancellationToken)
        {
            timerFrequency = request.TimerFrequency;

            var results = new Result[request.Algorithms.Count];
            multipleExecutedId = Guid.NewGuid().ToString("N");
            date = DateOnly.FromDateTime(DateTime.Now);

            List<int> executedIds = new List<int>();
            var functionTemp = await fitnessFunctionRepository.GetFitnessFunctionById(request.FitnessFunctionID);
            foreach (var algorithm in request.Algorithms)
            {
                var algorithmTemp = await algorithmsRepository.GetAlgorithmById(algorithm.Id);
                var executedAlgorithm = new ExecutedMultipleAlgorithms()
                {
                    MultipleTestId = multipleExecutedId,
                    TestedFitnessFunctionId = request.FitnessFunctionID,
                    TestedFitnessFunctionName = functionTemp.Name,
                    TestedAlgorithmName = algorithmTemp.Name,
                    TestedAlgorithmId = algorithm.Id,
                    TimerFrequency = timerFrequency,
                    IsFailed = true,
                    Date = date,
                    Parameters = [],
                    Dimension = request.Dimension,
                    SatisfiedResult = request.SatisfiedResult,
                    Depth = request.Depth,
                };

                executedId = await executedMultipleAlgorithmsRepositor.AddExecudedAlgorithm(executedAlgorithm);
                executedIds.Add(executedId);
            }

            try
            {
                for (int i = 0; i < results.Length; i++)
                {
                    results[i] = await ExecuteWithFindBestParameters(request.Algorithms[i].Id, request.FitnessFunctionID, request.Depth, request.Dimension, request.SatisfiedResult, executedIds[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var executedAlgorithms = await executedMultipleAlgorithmsRepositor.GetExecutedAlgorithmsByExecutedId(multipleExecutedId);

            var result = new MultipleAlgorithmTestResult()
            {
                TotalExecutionTime = TimeSpan.FromTicks(executedAlgorithms.Where(x => x.ExecutionTime != null).Sum(x => x.ExecutionTime.Value.Ticks)),
                MultipleExecutedId = multipleExecutedId,
                ExecutedAlgorithms = executedAlgorithms
            };

            return result;
        }

        public async Task<Result> ExecuteWithFindBestParameters(int algorithmId, int fitnessFunctionId, int depth, int dimension, double satisfiedResult, int executedId)
        {
            var fileGuid = Guid.NewGuid().ToString("N");
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
            foreach (Type algorithmTypeTemp in algoriuthmTypes)
            {
                if (typeof(IAlgorithm).IsAssignableFrom(algorithmTypeTemp))
                {
                    algorithmType = algorithmTypeTemp;
                    MethodInfo method = algorithmTypeTemp.GetMethod("Solve");
                    if (method != null)
                    {
                        algorithmInstance = Activator.CreateInstance(algorithmTypeTemp);

                        Assembly fitnessFunctionAssembly = Assembly.Load(fitnessFunction.DllFileBytes);
                        Type[] fitnessFunctionTypes = fitnessFunctionAssembly.GetTypes();
                        foreach (Type fitnessFunctionType in fitnessFunctionTypes)
                        {
                            if (typeof(IFitnessFunction).IsAssignableFrom(fitnessFunctionType))
                            {
                                object fitnessFunctionInstance = Activator.CreateInstance(fitnessFunctionType);

                                var currentParameter = new List<double>();
                                Stopwatch stopwatch = new Stopwatch();
                                stopwatch.Start();
                                try
                                {
                                    parameterCombinations = GenerateVariance(depth, algorithm.Parameters.ToArray(), algorithm.Parameters.Count - 1);
                                    Result lastResult = new Result();

                                    executedStateFileName = $"{multipleExecutedId}-{algorithm.Name}-{fileGuid}";
                                    var executedAlgorithm = new ExecutedMultipleAlgorithms()
                                    {
                                        MultipleTestId = multipleExecutedId,
                                        AlgorithmStateFileName = executedStateFileName,

                                        TestedFitnessFunctionId = fitnessFunction.Id,
                                        TestedFitnessFunctionName = fitnessFunction.Name,

                                        TestedAlgorithmName = algorithm.Name,
                                        TestedAlgorithmId = algorithm.Id,
                                        TimerFrequency = timerFrequency,
                                        IsFailed = true,
                                        Date = date,
                                        Parameters = parameterCombinations[0],
                                        Dimension = dimension,
                                        SatisfiedResult = satisfiedResult,
                                    };

                                    await executedMultipleAlgorithmsRepositor.UpdateExecutedAlgorithm(executedId, executedAlgorithm);
                                    int currentParameterIndex = 0;
                                    usedParameterIndexes = new List<int>();

                                    foreach (var parameters in parameterCombinations)
                                    {
                                        bool isLastParameter = parameters == parameterCombinations.Last();

                                        PropertyInfo xBestProperty = algorithmTypeTemp.GetProperty("XBest");
                                        PropertyInfo fBestProperty = algorithmTypeTemp.GetProperty("FBest");
                                        PropertyInfo numberOfEvaluationFitnessFunctionProperty = algorithmTypeTemp.GetProperty("NumberOfEvaluationFitnessFunction");
                                        PropertyInfo executedSuccessfullyProperty = algorithmTypeTemp.GetProperty("ExecutedSuccessfully")!;

                                        double?[] xBestValue;
                                        double? fBestValue;
                                        int numberOfEvaluationFitnessFunctionValue;
                                        bool executedSuccessfullyValue;

                                        currentParameter = parameters;
                                        List<double> resultParametes = [.. parameters, dimension];
                                        object[] methodArgs = [fitnessFunctionInstance, resultParametes.ToArray()];
                                        try
                                        {
                                            //Start timer
                                            timer = new System.Timers.Timer(timerFrequency);
                                            timer.Elapsed += SaveAlgorithState;
                                            timer.Start();

                                            //Execute method
                                            method.Invoke(algorithmInstance, methodArgs);

                                            //Assign values
                                            xBestValue = (double?[])xBestProperty!.GetValue(algorithmInstance);
                                            fBestValue = (double)fBestProperty!.GetValue(algorithmInstance);
                                            numberOfEvaluationFitnessFunctionValue = (int)numberOfEvaluationFitnessFunctionProperty!.GetValue(algorithmInstance)!;
                                            executedSuccessfullyValue = (bool)executedSuccessfullyProperty!.GetValue(algorithmInstance)!;

                                            if (fBestValue <= satisfiedResult && fBestValue != null && satisfiedResult != double.NaN)
                                            {
                                                stopwatch.Stop();
                                                executedAlgorithm.ExecutionTime = stopwatch.Elapsed;
                                                executedAlgorithm.Parameters = resultParametes;
                                                executedAlgorithm.XBest = xBestValue;
                                                executedAlgorithm.FBest = fBestValue;
                                                executedAlgorithm.IsFailed = false;
                                                executedAlgorithm.NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue;

                                                await executedMultipleAlgorithmsRepositor.UpdateExecutedAlgorithm(executedId, executedAlgorithm);

                                                return new Result()
                                                {
                                                    ExecutionTime = stopwatch.Elapsed,
                                                    TestedAlgorithmId = algorithm.Id,
                                                    TestedAlgorithmName = algorithm.Name,
                                                    TestedFitnessFunctionId = fitnessFunction.Id,
                                                    TestedFitnessFunctionName = fitnessFunction.Name,
                                                    FBest = fBestValue,
                                                    XBest = xBestValue,
                                                    Parameters = resultParametes,
                                                    NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue,
                                                    IsFailed = false,
                                                };
                                            }
                                            else
                                            {
                                                if (lastResult.FBest > fBestValue || lastResult.FBest == null)
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
                                        catch (Exception ex)
                                        {
                                            stopwatch.Stop();
                                            throw ex;
                                        }
                                        finally
                                        {
                                            if (timer != null)
                                                timer.Dispose();
                                        }
                                        usedParameterIndexes.Add(currentParameterIndex);
                                        currentParameterIndex++;
                                    }

                                    stopwatch.Stop();
                                    lastResult.IsFailed = false;
                                    lastResult.ExecutionTime = stopwatch.Elapsed;

                                    executedAlgorithm.ExecutionTime = stopwatch.Elapsed;
                                    executedAlgorithm.Parameters = lastResult.Parameters;
                                    executedAlgorithm.XBest = lastResult.XBest;
                                    executedAlgorithm.FBest = lastResult.FBest;
                                    executedAlgorithm.NumberOfEvaluationFitnessFunction = lastResult.NumberOfEvaluationFitnessFunction;
                                    executedAlgorithm.IsFailed = false;

                                    await executedMultipleAlgorithmsRepositor.UpdateExecutedAlgorithm(executedId, executedAlgorithm);
                                    return lastResult;
                                }
                                catch (Exception ex)
                                {
                                    stopwatch.Stop();
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

        private async void SaveAlgorithState(object? sender, ElapsedEventArgs e)
        {
            PropertyInfo stateWriterProperty = algorithmType!.GetProperty("StateWriter")!;
            object stateWriterInstance = stateWriterProperty!.GetValue(algorithmInstance)!;
            MethodInfo getCurrentStateMethod = stateWriterProperty!.PropertyType.GetMethod("GetCurrentState")!;
            string algorithmState = (string)getCurrentStateMethod!.Invoke(stateWriterInstance, null)!;

            List<List<double>> notUsedParameters = parameterCombinations
                .Where((param, index) => !usedParameterIndexes.Contains(index))
                .ToList();

            string parametersNotUsed = JsonConvert.SerializeObject(notUsedParameters, Formatting.Indented);
            var stateToSave = $"!{parametersNotUsed}!" + algorithmState;

            if (!string.IsNullOrEmpty(stateToSave) && stateToSave != currentState && !string.IsNullOrEmpty(algorithmState))
            {
                await algorithmStateRepository.SaveState(stateToSave, executedStateFileName);
                currentState = algorithmState;
            }
        }
    }
}