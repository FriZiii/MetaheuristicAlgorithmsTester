using AlgorithmInterfaces;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using System.Reflection;
using System.Timers;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestSingleAlgorithmSafeMode
{
    public class TestSingleAlgorithmSafeModeHandler(
        IAlgorithmsRepository algorithmsRepository,
        IFitnessFunctionRepository fitnessFunctionRepository,
        IExecutedSingleAlgorithmsRepository executedAlgorithmsRepository,
        IAlgorithmStateRepository algorithmStateRepository
        )
        : IRequestHandler<TestSingleAlgorithmSafeMode, AlgorithmTestResult>
    {
        private int executedId;
        private object algorithmInstance;
        private Type algorithmType;
        private System.Timers.Timer timer;
        private string currentState;
        string executedStateFileName;
        public async Task<AlgorithmTestResult> Handle(TestSingleAlgorithmSafeMode request, CancellationToken cancellationToken)
        {
            var algorithm = await algorithmsRepository.GetAlgorithmById(request.AlgorithmId);
            var fitnessFunction = await fitnessFunctionRepository.GetFitnessFunctionById(request.FitnessFunctionID);
            executedStateFileName = $"{algorithm.Name}-{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}";

            if (algorithm == null || algorithm.DllFileBytes == null)
            {
                return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"Algorithm with id {request.AlgorithmId} not found" };
            }
            if (fitnessFunction == null || fitnessFunction.DllFileBytes == null)
            {
                return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"Fitnness function with id {request.FitnessFunctionID} not found" };
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
                        var errors = ParametersValidator.Validate(algorithm.Parameters, request.Parameters);
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
                                    object[] methodArgs = [fitnessFunctionInstance!, request.Parameters.ToArray()];
                                    PropertyInfo xBestProperty = algorithmType.GetProperty("XBest")!;
                                    PropertyInfo fBestProperty = algorithmType.GetProperty("FBest")!;
                                    PropertyInfo numberOfEvaluationFitnessFunctionProperty = algorithmType.GetProperty("NumberOfEvaluationFitnessFunction")!;
                                    PropertyInfo executedSuccessfullyProperty = algorithmType.GetProperty("ExecutedSuccessfully")!;

                                    double?[] xBestValue;
                                    double? fBestValue;
                                    int numberOfEvaluationFitnessFunctionValue;
                                    bool executedSuccessfullyValue;

                                    try
                                    {
                                        //Add executed
                                        executedId = await executedAlgorithmsRepository.AddExecudedAlgorithm(new Domain.Entities.ExecutedSingleAlgorithm()
                                        {
                                            Date = DateOnly.FromDateTime(DateTime.Now),
                                            TestedAlgorithmId = algorithm.Id,
                                            TestedAlgorithmName = algorithm.Name,
                                            TestedFitnessFunctionId = fitnessFunction.Id,
                                            TestedFitnessFunctionName = fitnessFunction.Name,
                                            Parameters = request.Parameters,
                                            IsFailed = true,
                                            AlgorithmStateFileName = executedStateFileName,
                                            TimerFrequency = request.TimerFrequency,
                                        });

                                        //Start timer
                                        timer = new System.Timers.Timer(request.TimerFrequency);
                                        timer.Elapsed += SaveAlgorithState;
                                        timer.Start();

                                        //Execute method
                                        method.Invoke(algorithmInstance, methodArgs);

                                        //Assign values
                                        xBestValue = (double?[])xBestProperty!.GetValue(algorithmInstance)!;
                                        fBestValue = (double)fBestProperty!.GetValue(algorithmInstance)!;
                                        numberOfEvaluationFitnessFunctionValue = (int)numberOfEvaluationFitnessFunctionProperty!.GetValue(algorithmInstance)!;
                                        executedSuccessfullyValue = (bool)executedSuccessfullyProperty!.GetValue(algorithmInstance)!;
                                    }
                                    catch (Exception ex)
                                    {
                                        xBestValue = (double?[])xBestProperty!.GetValue(algorithmInstance)!;
                                        fBestValue = (double?)fBestProperty!.GetValue(algorithmInstance)!;
                                        numberOfEvaluationFitnessFunctionValue = (int)numberOfEvaluationFitnessFunctionProperty!.GetValue(algorithmInstance)!;
                                        executedSuccessfullyValue = (bool)executedSuccessfullyProperty!.GetValue(algorithmInstance)!;

                                        //UpdateExecuted
                                        await executedAlgorithmsRepository.UpdateExecutedAlgorithm(executedId, new Domain.Entities.ExecutedSingleAlgorithm()
                                        {
                                            Date = DateOnly.FromDateTime(DateTime.Now),
                                            TestedAlgorithmId = algorithm.Id,
                                            TestedAlgorithmName = algorithm.Name,
                                            TestedFitnessFunctionId = fitnessFunction.Id,
                                            TestedFitnessFunctionName = fitnessFunction.Name,
                                            Parameters = request.Parameters,
                                            NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue,
                                            FBest = fBestValue,
                                            XBest = xBestValue,
                                            IsFailed = !executedSuccessfullyValue,
                                            AlgorithmStateFileName = executedStateFileName,
                                            TimerFrequency = request.TimerFrequency,
                                        });

                                        return new AlgorithmTestResult() { IsSuccesfull = false, Message = $"Something went wrong: {ex.Message}, u can continue" };
                                    }
                                    finally
                                    {
                                        if (timer != null)
                                            timer.Dispose();
                                    }

                                    //UpdateExecuted
                                    await executedAlgorithmsRepository.UpdateExecutedAlgorithm(executedId, new Domain.Entities.ExecutedSingleAlgorithm()
                                    {
                                        Date = DateOnly.FromDateTime(DateTime.Now),
                                        TestedAlgorithmId = algorithm.Id,
                                        TestedAlgorithmName = algorithm.Name,
                                        TestedFitnessFunctionId = fitnessFunction.Id,
                                        TestedFitnessFunctionName = fitnessFunction.Name,
                                        Parameters = request.Parameters,
                                        NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue,
                                        FBest = fBestValue,
                                        XBest = xBestValue!,
                                        IsFailed = executedSuccessfullyValue,
                                        AlgorithmStateFileName = executedStateFileName,
                                        TimerFrequency = request.TimerFrequency,
                                    });

                                    return new AlgorithmTestResult()
                                    {
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