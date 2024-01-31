namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms
{
    public class AlgorithmResult
    {
        public AlgorithmDto Algorithm { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsSuccesfull { get; set; }
    }
}
