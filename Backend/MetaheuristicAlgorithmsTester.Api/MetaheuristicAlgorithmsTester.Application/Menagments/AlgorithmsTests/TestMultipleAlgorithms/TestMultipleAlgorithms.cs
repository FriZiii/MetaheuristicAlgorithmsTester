using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests.TestMultipleAlgorithms
{
    public class TestMultipleAlgorithms : IRequest<IEnumerable<AlgorithmTestResult>>
    {
        public List<TestAlgorithmDto> Algorithms { get; set; }
        public int FitnessFunctionID { get; set; }
        public int Depth { get; set; }
    }

    public class TestAlgorithmDto
    {
        public int Id { get; set; }
        public List<double> Parameters { get; set; } = default!;
    }
}
