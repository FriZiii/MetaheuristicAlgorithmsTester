using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestSingleAlgorithm
{
    public class TestSingleAlgorithm : IRequest<AlgorithmTestResult>
    {
        public int AlgorithmId { get; set; }
        public List<double> Parameters { get; set; } = default!;
        public int FitnessFunctionID { get; set; }
    }
}
