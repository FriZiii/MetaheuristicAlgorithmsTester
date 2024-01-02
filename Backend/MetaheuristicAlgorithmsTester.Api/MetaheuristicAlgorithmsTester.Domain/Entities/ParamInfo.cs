namespace MetaheuristicAlgorithmsTester.Domain.Entities
{
    public class ParamInfo
    {
        public int Id { get; set; }
        public int AlgorithmId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public double UpperBoundary { get; set; }
        public double LowerBoundary { get; set; }
    }
}
