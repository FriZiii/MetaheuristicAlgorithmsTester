using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithms;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithmsSafeMode
{
    public class TestMultipleAlgorithmsSafeMode : IRequest<MultipleAlgorithmTestResult>
    {
        public List<TestAlgorithmDto> Algorithms { get; set; }
        public int FitnessFunctionID { get; set; }
        public int Depth { get; set; }
        public int Dimension { get; set; }
        public double SatisfiedResult { get; set; }
        public int TimerFrequency { get; set; }
    }
}
