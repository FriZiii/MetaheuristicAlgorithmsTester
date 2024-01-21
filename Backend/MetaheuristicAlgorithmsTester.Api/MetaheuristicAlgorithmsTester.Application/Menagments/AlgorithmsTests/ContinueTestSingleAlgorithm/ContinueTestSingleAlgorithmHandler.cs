using AlgorithmInterfaces;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using System.Diagnostics;
using System.Reflection;
using System.Timers;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.ContinueTestSingleAlgorithm
{
    public class ContinueTestSingleAlgorithmHandler
        (
        IAlgorithmsRepository algorithmsRepository,
        IFitnessFunctionRepository fitnessFunctionRepository,
        IExecutedSingleAlgorithmsRepository executedAlgorithmsRepository,
        IAlgorithmStateRepository algorithmStateRepository
        )
        : IRequestHandler<ContinueTestSingleAlgorithm, AlgorithmTestResult>
    {
        private int executedId;
        private object algorithmInstance;
        private Type algorithmType;
        private System.Timers.Timer timer;
        private string currentState;
        string executedStateFileName;
        public async Task<AlgorithmTestResult> Handle(ContinueTestSingleAlgorithm request, CancellationToken cancellationToken)
        {
            var executed = await executedAlgorithmsRepository.GetExecutedAlgorithmById(request.ExecutedId);
            var algorithm = await algorithmsRepository.GetAlgorithmById(executed.TestedAlgorithmId);
            var fitnessFunction = await fitnessFunctionRepository.GetFitnessFunctionById(executed.TestedFitnessFunctionId);
            executedId = executed.Id;
            executedStateFileName = $"{algorithm.Name}-{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}";

            if (algorithm == null || algorithm.DllFileBytes == null)
            {
                return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"Algorithm with id {executed.TestedAlgorithmId} not found" };
            }
            if (fitnessFunction == null || fitnessFunction.DllFileBytes == null)
            {
                return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"Fitnness function with id {executed.TestedFitnessFunctionId} not found" };
            }

            Assembly algorithmAssembly = Assembly.Load(algorithm.DllFileBytes);
            Type[] algoriuthmTypes = algorithmAssembly.GetTypes();
            foreach (Type algorithmTypeTemp in algoriuthmTypes)
            {
                if (typeof(IAlgorithm).IsAssignableFrom(algorithmTypeTemp))
                {
                    algorithmType = algorithmTypeTemp;
                    MethodInfo method = algorithmType.GetMethod("Solve")!;
                    if (method != null)
                    {
                        algorithmInstance = Activator.CreateInstance(algorithmType)!;
                        var errors = ParametersValidator.Validate(algorithm.Parameters, executed.Parameters);
                        if (errors.Count > 0)
                        {
                            return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"Parameters error", ParametersErrors = errors };
                        }

                        Assembly fitnessFunctionAssembly = Assembly.Load(fitnessFunction.DllFileBytes);
                        Type[] fitnessFunctionTypes = fitnessFunctionAssembly.GetTypes();
                        foreach (Type fitnessFunctionType in fitnessFunctionTypes)
                        {
                            if (typeof(IFitnessFunction).IsAssignableFrom(fitnessFunctionType))
                            {
                                object fitnessFunctionInstance = Activator.CreateInstance(fitnessFunctionType)!;

                                try
                                {
                                    Stopwatch stopwatch = new Stopwatch();
                                    stopwatch.Start();
                                    object[] methodArgs = [fitnessFunctionInstance!, executed.Parameters.ToArray()];
                                    PropertyInfo xBestProperty = algorithmType.GetProperty("XBest")!;
                                    PropertyInfo fBestProperty = algorithmType.GetProperty("FBest")!;
                                    PropertyInfo numberOfEvaluationFitnessFunctionProperty = algorithmType.GetProperty("NumberOfEvaluationFitnessFunction")!;
                                    PropertyInfo executedSuccessfullyProperty = algorithmType.GetProperty("ExecutedSuccessfully")!;

                                    double?[] xBestValue;
                                    double fBestValue;
                                    int numberOfEvaluationFitnessFunctionValue;
                                    bool executedSuccessfullyValue;

                                    try
                                    {
                                        //LoadState
                                        var state = await algorithmStateRepository.GetStateOfSingleTest(executed.Id);

                                        PropertyInfo stateReaderProperty = algorithmType.GetProperty("StateReader")!;
                                        object stateReaderInstance = stateReaderProperty.GetValue(algorithmInstance)!;
                                        MethodInfo loadStateMethod = stateReaderInstance.GetType().GetMethod("LoadState")!;
                                        loadStateMethod.Invoke(stateReaderInstance, new object[] { state });

                                        //Run timer
                                        timer = new System.Timers.Timer((int)executed.TimerFrequency!);
                                        timer.Elapsed += SaveAlgorithState;
                                        timer.Start();

                                        //Execute methodSystem.Reflection.TargetInvocationException: „Exception has been thrown by the target of an invocation.”
                                        method.Invoke(algorithmInstance, methodArgs);

                                        //Set results
                                        xBestValue = (double?[]?)xBestProperty!.GetValue(algorithmInstance)!;
                                        fBestValue = (double)fBestProperty!.GetValue(algorithmInstance)!;
                                        numberOfEvaluationFitnessFunctionValue = (int)numberOfEvaluationFitnessFunctionProperty!.GetValue(algorithmInstance)!;
                                        executedSuccessfullyValue = (bool)executedSuccessfullyProperty!.GetValue(algorithmInstance)!;
                                    }
                                    catch (Exception ex)
                                    {
                                        xBestValue = (double?[]?)xBestProperty!.GetValue(algorithmInstance)!;
                                        fBestValue = (double)fBestProperty!.GetValue(algorithmInstance)!;
                                        numberOfEvaluationFitnessFunctionValue = (int)numberOfEvaluationFitnessFunctionProperty!.GetValue(algorithmInstance)!;
                                        executedSuccessfullyValue = (bool)executedSuccessfullyProperty!.GetValue(algorithmInstance)!;
                                        stopwatch.Stop();
                                        await executedAlgorithmsRepository.UpdateExecutedAlgorithm(executedId, new Domain.Entities.ExecutedSingleAlgorithm()
                                        {
                                            ExecutionTime = TimeSpan.FromTicks(executed.ExecutionTime.Value.Ticks + stopwatch.Elapsed.Ticks),
                                            Date = DateOnly.FromDateTime(DateTime.Now),
                                            TestedAlgorithmId = algorithm.Id,
                                            TestedAlgorithmName = algorithm.Name,
                                            TestedFitnessFunctionId = fitnessFunction.Id,
                                            TestedFitnessFunctionName = fitnessFunction.Name,
                                            Parameters = executed.Parameters,
                                            NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue,
                                            FBest = fBestValue,
                                            XBest = xBestValue,
                                            IsFailed = !executedSuccessfullyValue,
                                            AlgorithmStateFileName = executedStateFileName,
                                        });

                                        return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}, u can continue" };
                                    }
                                    finally
                                    {
                                        stopwatch.Stop();
                                        if (timer != null)
                                            timer.Dispose();
                                    }

                                    await executedAlgorithmsRepository.UpdateExecutedAlgorithm(executedId, new Domain.Entities.ExecutedSingleAlgorithm()
                                    {
                                        ExecutionTime = TimeSpan.FromTicks(executed.ExecutionTime.Value.Ticks + stopwatch.Elapsed.Ticks),
                                        Date = DateOnly.FromDateTime(DateTime.Now),
                                        TestedAlgorithmId = algorithm.Id,
                                        TestedAlgorithmName = algorithm.Name,
                                        TestedFitnessFunctionId = fitnessFunction.Id,
                                        TestedFitnessFunctionName = fitnessFunction.Name,
                                        Parameters = executed.Parameters,
                                        NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue,
                                        FBest = fBestValue,
                                        XBest = xBestValue!,
                                        IsFailed = !executedSuccessfullyValue,
                                        AlgorithmStateFileName = executedStateFileName,
                                    });

                                    return new AlgorithmTestResult()
                                    {
                                        ExecutionTime = TimeSpan.FromTicks(executed.ExecutionTime.Value.Ticks + stopwatch.Elapsed.Ticks),
                                        ExecutedTestId = executedId,
                                        TestedAlgorithmId = algorithm.Id,
                                        TestedAlgorithmName = algorithm.Name,
                                        TestedFitnessFunctionId = fitnessFunction.Id,
                                        TestedFitnessFunctionName = fitnessFunction.Name,
                                        IsSuccesfull = true,
                                        Message = $"Succesfull!",
                                        FBest = fBestValue,
                                        XBest = xBestValue!,
                                        NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue
                                    };
                                }
                                catch (Exception ex)
                                {
                                    return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}" };
                                }
                            }
                        }
                        return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"The dll file does not have a class implementing the IFitnessFunction interface" };
                    }
                    return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"Method Solve not found" };
                }
            }
            return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"The dll file does not have a class implementing the IAlgorithm interface" };
        }
        private async void SaveAlgorithState(object? sender, ElapsedEventArgs e)
        {
            PropertyInfo stateWriterProperty = algorithmType!.GetProperty("StateWriter")!;
            object stateWriterInstance = stateWriterProperty!.GetValue(algorithmInstance)!;

            MethodInfo getCurrentStateMethod = stateWriterProperty!.PropertyType.GetMethod("GetCurrentState")!;
            string algorithmState = (string)getCurrentStateMethod!.Invoke(stateWriterInstance, null)!;
            if (!string.IsNullOrEmpty(algorithmState) && algorithmState != currentState)
            {
                await algorithmStateRepository.SaveState(algorithmState, executedStateFileName);
                currentState = algorithmState;
            }
        }
    }
}
