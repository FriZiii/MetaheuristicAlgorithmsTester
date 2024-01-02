using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm
{
    public class AddAlgorithmResult
    {
        public Algorithm Algorithm { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsSuccesfull { get; set; }
    }
}
