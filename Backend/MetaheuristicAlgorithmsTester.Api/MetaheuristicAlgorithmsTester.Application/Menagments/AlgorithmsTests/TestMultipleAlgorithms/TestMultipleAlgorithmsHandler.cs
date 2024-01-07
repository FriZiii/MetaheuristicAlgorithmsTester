using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithms
{
    public class TestMultipleAlgorithmsHandler(IAlgorithmsRepository algorithmsRepository, IFitnessFunctionRepository fitnessFunctionRepository, IMediator mediator) : IRequestHandler<TestMultipleAlgorithms, IEnumerable<AlgorithmTestResult>>
    {
        public async Task<IEnumerable<AlgorithmTestResult>> Handle(TestMultipleAlgorithms request, CancellationToken cancellationToken)
        {
            var result = new AlgorithmTestResult[request.Algorithms.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = await mediator.Send(new TestSingleAlgorithm.TestSingleAlgorithm() { AlgorithmId = request.Algorithms[i].Id, FitnessFunctionID = request.FitnessFunctionID, Parameters = request.Algorithms[i].Parameters });
            }

            return result;
        }
    }
}

