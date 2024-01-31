using AlgorithmInterfaces;
using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithms;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Timers;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.ContinueTestMultipleAlgorithms
{
    public class ContinueTestMultipleAlgorithmsHandler(
        IAlgorithmsRepository algorithmsRepository,
        IFitnessFunctionRepository fitnessFunctionRepository,
        IExecutedMultipleAlgorithmsRepository executedMultipleAlgorithmsRepository,
        IAlgorithmStateRepository algorithmStateRepository
        ) : IRequestHandler<ContinueTestMultipleAlgorithms, MultipleAlgorithmTestResult>
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

        public async Task<MultipleAlgorithmTestResult> Handle(ContinueTestMultipleAlgorithms request, CancellationToken cancellationToken)
        {
            date = DateOnly.FromDateTime(DateTime.Now);
            var result = new MultipleAlgorithmTestResult();
            List<ExecutedMultipleAlgorithms> executedAlgorithmsToResult;
            var executed = await executedMultipleAlgorithmsRepository.GetExecutedAlgorithmById(request.Id);
            var algorithm = await algorithmsRepository.GetAlgorithmById(executed.TestedAlgorithmId);
            var fitnessFunction = await fitnessFunctionRepository.GetFitnessFunctionById(executed.TestedFitnessFunctionId);

            if (executed == null)
            {
                throw new Exception($"Execution with id {request.Id} not found");
            }
            if (algorithm == null || algorithm.DllFileBytes == null)
            {
                throw new Exception($"Algorithm with id {executed.TestedAlgorithmId} not found");
            }
            if (fitnessFunction == null || fitnessFunction.DllFileBytes == null)
            {
                throw new Exception($"Fitnness function with id {executed.TestedFitnessFunctionId} not found");
            }

            multipleExecutedId = executed.MultipleTestId;
            timerFrequency = (int)executed.TimerFrequency;
            var state = await algorithmStateRepository.GetStateOfMultipleTest(executed.Id);

            if (!string.IsNullOrEmpty(state))
            {
                string programState;

                int startIndex = state.IndexOf("!");
                int endIndex = state.LastIndexOf("!");
                if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
                {
                    parameterCombinations = JsonConvert.DeserializeObject<List<List<double>>>(state.Substring(startIndex + 1, endIndex - startIndex - 1).Replace("\r\n", "").Replace(" ", ""));
                    programState = state.Substring(endIndex + 1);

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
                                            Result lastResult = new Result();
                                            executedStateFileName = executed.AlgorithmStateFileName;
                                            var executedAlgorithm = new ExecutedMultipleAlgorithms()
                                            {
                                                MultipleTestId = multipleExecutedId,
                                                AlgorithmStateFileName = executedStateFileName,
                                                TestedFitnessFunctionId = fitnessFunction.Id,
                                                TestedFitnessFunctionName = fitnessFunctionType.Name,
                                                TestedAlgorithmName = fitnessFunction.Name,
                                                TestedAlgorithmId = algorithm.Id,
                                                Dimension = executed.Dimension,
                                                TimerFrequency = timerFrequency,
                                                IsFailed = true,
                                                Date = date,
                                                Parameters = parameterCombinations[0]
                                            };

                                            executedId = executed.Id;
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
                                                List<double> resultParametes = [.. parameters, executed.Dimension];
                                                object[] methodArgs = [fitnessFunctionInstance, resultParametes.ToArray()];
                                                try
                                                {
                                                    //LoadState
                                                    PropertyInfo stateReaderProperty = algorithmType.GetProperty("StateReader")!;
                                                    object stateReaderInstance = stateReaderProperty.GetValue(algorithmInstance)!;
                                                    MethodInfo loadStateMethod = stateReaderInstance.GetType().GetMethod("LoadState")!;
                                                    loadStateMethod.Invoke(stateReaderInstance, new object[] { programState });

                                                    //Start timer
                                                    timer = new System.Timers.Timer(timerFrequency);
                                                    timer.Elapsed += SaveAlgorithState;
                                                    timer.Start();

                                                    //Execute method
                                                    method.Invoke(algorithmInstance, methodArgs);

                                                    //Assign values
                                                    xBestValue = (double?[])xBestProperty!.GetValue(algorithmInstance)!;
                                                    fBestValue = (double?)fBestProperty!.GetValue(algorithmInstance)!;
                                                    numberOfEvaluationFitnessFunctionValue = (int)numberOfEvaluationFitnessFunctionProperty!.GetValue(algorithmInstance)!;
                                                    executedSuccessfullyValue = (bool)executedSuccessfullyProperty!.GetValue(algorithmInstance)!;

                                                    if (fBestValue <= executed.SatisfiedResult && executed.SatisfiedResult != null && fBestValue != null)
                                                    {
                                                        stopwatch.Stop();
                                                        executedAlgorithm.ExecutionTime = TimeSpan.FromTicks(executed.ExecutionTime.Value.Ticks + stopwatch.Elapsed.Ticks);
                                                        executedAlgorithm.TestedAlgorithmId = algorithm.Id;
                                                        executedAlgorithm.TestedAlgorithmName = algorithm.Name;
                                                        executedAlgorithm.TestedFitnessFunctionId = fitnessFunction.Id;
                                                        executedAlgorithm.TestedFitnessFunctionName = fitnessFunction.Name;
                                                        executedAlgorithm.Parameters = resultParametes;
                                                        executedAlgorithm.XBest = xBestValue;
                                                        executedAlgorithm.FBest = fBestValue;
                                                        executedAlgorithm.IsFailed = false;
                                                        executedAlgorithm.NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue;

                                                        await executedMultipleAlgorithmsRepository.UpdateExecutedAlgorithm(executedId, executedAlgorithm);

                                                        executedAlgorithmsToResult = await executedMultipleAlgorithmsRepository.GetExecutedAlgorithmsByExecutedId(multipleExecutedId);

                                                        if (timer != null)
                                                            timer.Dispose();

                                                        return new MultipleAlgorithmTestResult()
                                                        {
                                                            TotalExecutionTime = TimeSpan.FromTicks(executedAlgorithmsToResult.Where(x => x.ExecutionTime != null).Sum(x => x.ExecutionTime.Value.Ticks)),
                                                            MultipleExecutedId = multipleExecutedId,
                                                            ExecutedAlgorithms = executedAlgorithmsToResult
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
                                                            lastResult.IsFailed = false;
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
                                            executedAlgorithm.ExecutionTime = TimeSpan.FromTicks(executed.ExecutionTime.Value.Ticks + stopwatch.Elapsed.Ticks);
                                            executedAlgorithm.Parameters = lastResult.Parameters;
                                            executedAlgorithm.XBest = lastResult.XBest;
                                            executedAlgorithm.FBest = lastResult.FBest;
                                            executedAlgorithm.NumberOfEvaluationFitnessFunction = lastResult.NumberOfEvaluationFitnessFunction;
                                            executedAlgorithm.IsFailed = false;

                                            await executedMultipleAlgorithmsRepository.UpdateExecutedAlgorithm(executedId, executedAlgorithm);

                                            executedAlgorithmsToResult = await executedMultipleAlgorithmsRepository.GetExecutedAlgorithmsByExecutedId(multipleExecutedId);
                                            return new MultipleAlgorithmTestResult()
                                            {
                                                TotalExecutionTime = TimeSpan.FromTicks(executedAlgorithmsToResult.Where(x => x.ExecutionTime != null).Sum(x => x.ExecutionTime.Value.Ticks)),
                                                MultipleExecutedId = multipleExecutedId,
                                                ExecutedAlgorithms = executedAlgorithmsToResult
                                            };
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
                throw new Exception("Saved state is invalid");
            }
            else
            {
                var fileGuid = Guid.NewGuid().ToString("N");
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
                                        parameterCombinations = GenerateVariance(executed.Depth, algorithm.Parameters.ToArray(), algorithm.Parameters.Count - 1);
                                        executedId = executed.Id;
                                        Result lastResult = new Result();

                                        executedStateFileName = $"{multipleExecutedId}-{algorithm.Name}-{fileGuid}";
                                        var executedAlgorithm = new ExecutedMultipleAlgorithms()
                                        {
                                            MultipleTestId = multipleExecutedId,
                                            AlgorithmStateFileName = executedStateFileName,
                                            TestedFitnessFunctionId = fitnessFunction.Id,
                                            TestedFitnessFunctionName = fitnessFunctionType.Name,
                                            TestedAlgorithmName = fitnessFunction.Name,
                                            TestedAlgorithmId = algorithm.Id,
                                            TimerFrequency = timerFrequency,
                                            IsFailed = true,
                                            Date = date,
                                            Parameters = parameterCombinations[0],
                                            Dimension = executed.Dimension,
                                            SatisfiedResult = executed.SatisfiedResult,
                                        };

                                        await executedMultipleAlgorithmsRepository.UpdateExecutedAlgorithm(executedId, executedAlgorithm);
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
                                            List<double> resultParametes = [.. parameters, executed.Dimension];
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
                                                xBestValue = (double?[])xBestProperty!.GetValue(algorithmInstance)!;
                                                fBestValue = (double?)fBestProperty!.GetValue(algorithmInstance)!;
                                                numberOfEvaluationFitnessFunctionValue = (int)numberOfEvaluationFitnessFunctionProperty!.GetValue(algorithmInstance)!;
                                                executedSuccessfullyValue = (bool)executedSuccessfullyProperty!.GetValue(algorithmInstance)!;


                                                xBestValue = (double?[])xBestProperty.GetValue(algorithmInstance);
                                                fBestValue = (double?)fBestProperty.GetValue(algorithmInstance);
                                                numberOfEvaluationFitnessFunctionValue = (int)numberOfEvaluationFitnessFunctionProperty.GetValue(algorithmInstance);

                                                if (fBestValue <= executed.SatisfiedResult && executed.SatisfiedResult != null && fBestValue != null)
                                                {
                                                    stopwatch.Stop();
                                                    executedAlgorithm.ExecutionTime = stopwatch.Elapsed;
                                                    executedAlgorithm.Parameters = resultParametes;
                                                    executedAlgorithm.XBest = xBestValue;
                                                    executedAlgorithm.FBest = fBestValue;
                                                    executedAlgorithm.IsFailed = false;
                                                    executedAlgorithm.NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue;

                                                    await executedMultipleAlgorithmsRepository.UpdateExecutedAlgorithm(executedId, executedAlgorithm);

                                                    executedAlgorithmsToResult = await executedMultipleAlgorithmsRepository.GetExecutedAlgorithmsByExecutedId(multipleExecutedId);
                                                    return new MultipleAlgorithmTestResult()
                                                    {
                                                        TotalExecutionTime = TimeSpan.FromTicks(executedAlgorithmsToResult.Where(x => x.ExecutionTime != null).Sum(x => x.ExecutionTime.Value.Ticks)),
                                                        MultipleExecutedId = multipleExecutedId,
                                                        ExecutedAlgorithms = executedAlgorithmsToResult
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
                                                        lastResult.IsFailed = false;
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
                                        executedAlgorithm.ExecutionTime = stopwatch.Elapsed;
                                        executedAlgorithm.Parameters = lastResult.Parameters;
                                        executedAlgorithm.XBest = lastResult.XBest;
                                        executedAlgorithm.FBest = lastResult.FBest;
                                        executedAlgorithm.NumberOfEvaluationFitnessFunction = lastResult.NumberOfEvaluationFitnessFunction;

                                        await executedMultipleAlgorithmsRepository.UpdateExecutedAlgorithm(executedId, executedAlgorithm);
                                        executedAlgorithmsToResult = await executedMultipleAlgorithmsRepository.GetExecutedAlgorithmsByExecutedId(multipleExecutedId);
                                        return new MultipleAlgorithmTestResult()
                                        {
                                            TotalExecutionTime = TimeSpan.FromTicks(executedAlgorithmsToResult.Where(x => x.ExecutionTime != null).Sum(x => x.ExecutionTime.Value.Ticks)),
                                            MultipleExecutedId = multipleExecutedId,
                                            ExecutedAlgorithms = executedAlgorithmsToResult
                                        };
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

            if (!string.IsNullOrEmpty(stateToSave) && stateToSave != currentState)
            {
                await algorithmStateRepository.SaveState(stateToSave, executedStateFileName);
                currentState = algorithmState;
            }
        }
    }
}
