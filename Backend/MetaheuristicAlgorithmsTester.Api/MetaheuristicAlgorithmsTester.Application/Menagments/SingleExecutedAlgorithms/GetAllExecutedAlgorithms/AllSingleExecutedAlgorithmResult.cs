namespace MetaheuristicAlgorithmsTester.Application.Menagments.SingleExecutedAlgotrithms.GetAllExecutedAlgorithms
{
    public class AllSingleExecutedAlgorithmResult
    {
        public List<SingleExecutedAlgorithmDto> ExecutedAlgorithms { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsSuccesfull { get; set; }
    }
}
