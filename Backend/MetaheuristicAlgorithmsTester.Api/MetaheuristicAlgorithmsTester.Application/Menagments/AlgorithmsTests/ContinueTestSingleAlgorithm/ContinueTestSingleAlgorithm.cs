using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.ContinueTestSingleAlgorithm
{
    public class ContinueTestSingleAlgorithm : IRequest<AlgorithmTestResult>
    {
        public int ExecutedId { get; set; }
    }
}
