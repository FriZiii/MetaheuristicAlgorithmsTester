namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.GetAllFitnessFunctions
{
    public class AllFitnessFunctionsResult
    {
        public IEnumerable<FitnessFunctionDto?> FitnessFunctions { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsSuccesfull { get; set; }
    }
}
