namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions
{
    public class FitnessFunctionResult
    {
        public FitnessFunctionDto FitnessFunction { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsSuccesfull { get; set; }
    }
}
