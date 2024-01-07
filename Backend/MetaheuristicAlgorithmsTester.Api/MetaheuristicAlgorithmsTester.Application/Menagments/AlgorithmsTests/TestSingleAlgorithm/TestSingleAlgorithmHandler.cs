using AlgorithmInterfaces;
using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using System.Reflection;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestSingleAlgorithm
{
    public class TestSingleAlgorithmHandler(IMediator mediator, IAlgorithmsRepository algorithmsRepository, IFitnessFunctionRepository fitnessFunctionRepository, IExecutedAlgorithmsRepository executedAlgorithmsRepository)
        : IRequestHandler<TestSingleAlgorithm, AlgorithmTestResult>
    {
        public async Task<AlgorithmTestResult> Handle(TestSingleAlgorithm request, CancellationToken cancellationToken)
        {
            var algorithm = await algorithmsRepository.GetAlgorithmById(request.AlgorithmId);
            var fitnessFunction = await fitnessFunctionRepository.GetFitnessFunctionById(request.FitnessFunctionID);

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
            foreach (Type algorithmType in algoriuthmTypes)
            {
                if (typeof(IAlgorithm).IsAssignableFrom(algorithmType))
                {
                    MethodInfo method = algorithmType.GetMethod("Solve");
                    if (method != null)
                    {
                        object algorithmInstance = Activator.CreateInstance(algorithmType);

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
                                object fitnessFunctionInstance = Activator.CreateInstance(fitnessFunctionType);

                                try
                                {
                                    object[] methodArgs = [fitnessFunctionInstance, request.Parameters.ToArray()];
                                    method.Invoke(algorithmInstance, methodArgs);

                                    PropertyInfo xBestProperty = algorithmType.GetProperty("XBest");
                                    PropertyInfo fBestProperty = algorithmType.GetProperty("FBest");
                                    PropertyInfo numberOfEvaluationFitnessFunctionProperty = algorithmType.GetProperty("NumberOfEvaluationFitnessFunction");

                                    double[] xBestValue = (double[])xBestProperty.GetValue(algorithmInstance);
                                    double fBestValue = (double)fBestProperty.GetValue(algorithmInstance);
                                    int numberOfEvaluationFitnessFunctionValue = (int)numberOfEvaluationFitnessFunctionProperty.GetValue(algorithmInstance);

                                    await executedAlgorithmsRepository.AddExecudedAlgorithm(new Domain.Entities.ExecutedAlgorithm()
                                    {
                                        TestedAlgorithmId = algorithm.Id,
                                        TestedAlgorithmName = algorithm.Name,
                                        TestedFitnessFunctionId = fitnessFunction.Id,
                                        TestedFitnessFunctionName = fitnessFunction.Name,
                                        Parameters = request.Parameters,
                                        NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunctionValue,
                                        FBest = fBestValue,
                                        XBest = xBestValue,
                                    });

                                    return new AlgorithmTestResult()
                                    {
                                        TestedAlgorithmId = algorithm.Id,
                                        TestedAlgorithmName = algorithm.Name,
                                        TestedFitnessFunctionId = fitnessFunction.Id,
                                        TestedFitnessFunctionName = fitnessFunction.Name,
                                        IsSuccesfull = true,
                                        Message = $"Succesfull!",
                                        FBest = fBestValue,
                                        XBest = xBestValue,
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
    }
}
