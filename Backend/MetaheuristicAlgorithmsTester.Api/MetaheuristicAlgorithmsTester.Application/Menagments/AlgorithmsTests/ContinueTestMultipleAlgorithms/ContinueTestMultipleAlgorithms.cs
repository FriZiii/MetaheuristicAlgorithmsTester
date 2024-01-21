using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.ContinueTestMultipleAlgorithms
{
    public class ContinueTestMultipleAlgorithms : IRequest<MultipleAlgorithmTestResult>
    {
        public int Id { get; set; }
    }
}
