using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestSingleAlgorithmSafeMode
{
    public class TestSingleAlgorithmSafeMode : IRequest<AlgorithmTestResult>
    {
        public int AlgorithmId { get; set; }
        public List<double> Parameters { get; set; } = default!;
        public int FitnessFunctionID { get; set; }
        public int TimerFrequency { get; set; }
    }
}
