using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.MultipleExecutedAlgorithms.GetAllMultipleExecutedAlgorithms
{
    public class AllMultipleExecutedAlgorithmResult
    {
        public DateOnly Date { get; set; } = default!;
        public string MultipleTestId { get; set; } = default!;
        public List<ExecutedMultipleAlgorithms> ExecutedMultipleAlgorithms { get; set; } = default!;
    }
}
