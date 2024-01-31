namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.GetAllAlgorithms
{
    public class AllAlgorithmsResult
    {
        public IEnumerable<AlgorithmDto?> Algorithms { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsSuccesfull { get; set; }
    }
}
