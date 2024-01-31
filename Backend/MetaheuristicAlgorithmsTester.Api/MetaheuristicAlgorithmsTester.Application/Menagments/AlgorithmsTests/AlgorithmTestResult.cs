namespace MetaheuristicAlgorithmsTester.Application.Menagments.AlgorithmsTests
{
    public class AlgorithmTestResult
    {
        public int ExecutedTestId { get; set; }
        public TimeSpan? ExecutionTime { get; set; }
        public int TestedAlgorithmId { get; set; }
        public string TestedAlgorithmName { get; set; } = default!;

        public int TestedFitnessFunctionId { get; set; }
        public string TestedFitnessFunctionName { get; set; } = default!;

        public double?[] XBest { get; set; } = default!;
        public double? FBest { get; set; }
        public int NumberOfEvaluationFitnessFunction { get; set; }
        public List<ParametersError> ParametersErrors { get; set; } = default!;
        public string Message { get; set; } = default!;
        public bool IsSuccesfull { get; set; }
    }
}
