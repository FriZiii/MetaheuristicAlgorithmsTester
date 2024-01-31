using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests
{
    public class MultipleAlgorithmTestResult
    {
        public TimeSpan? TotalExecutionTime { get; set; }
        public string MultipleExecutedId { get; set; }
        public List<ExecutedMultipleAlgorithms> ExecutedAlgorithms { get; set; }
    }
}
