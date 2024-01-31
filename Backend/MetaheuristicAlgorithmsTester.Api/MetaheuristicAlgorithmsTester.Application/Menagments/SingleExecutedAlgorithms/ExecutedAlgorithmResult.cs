namespace MetaheuristicAlgorithmsTester.Application.Menagments.SingleExecutedAlgotrithms
{
    public class ExecutedAlgorithmResult
    {
        public SingleExecutedAlgorithmDto ExecutedAlgorithm { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsSuccesfull { get; set; }
    }
}
