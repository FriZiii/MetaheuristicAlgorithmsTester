namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions
{
    public class FitnessFunctionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public int NumberOfParameters { get; set; }
    }
}