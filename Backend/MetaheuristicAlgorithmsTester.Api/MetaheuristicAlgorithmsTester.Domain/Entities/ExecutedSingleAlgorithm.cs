namespace MetaheuristicAlgorithmsTester.Domain.Entities
{
    public class ExecutedSingleAlgorithm
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; } = default!;
        public TimeSpan? ExecutionTime { get; set; }

        public int TestedAlgorithmId { get; set; }
        public string TestedAlgorithmName { get; set; } = default!;

        public int TestedFitnessFunctionId { get; set; }
        public string TestedFitnessFunctionName { get; set; } = default!;

        public double?[]? XBest { get; set; } = default!;
        public double? FBest { get; set; }
        public int? NumberOfEvaluationFitnessFunction { get; set; }
        public List<double> Parameters { get; set; } = default!;

        public bool IsFailed { get; set; }
        public string? AlgorithmStateFileName { get; set; }
        public int? TimerFrequency { get; set; }
    }
}
