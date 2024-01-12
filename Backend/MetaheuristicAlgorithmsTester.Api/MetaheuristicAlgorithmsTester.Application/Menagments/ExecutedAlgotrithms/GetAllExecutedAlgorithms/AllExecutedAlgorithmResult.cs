namespace MetaheuristicAlgorithmsTester.Application.Menagments.ExecutedAlgotrithms.GetAllExecutedAlgorithms
{
    public class AllExecutedAlgorithmResult
    {
        public List<ExecutedAlgorithmDto> ExecutedAlgorithms { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsSuccesfull { get; set; }
    }
}
