namespace AlgorithmInterfaces
{
    public class ParamInfo
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsFloatingPoint { get; set; }
        public double UpperBoundary { get; set; }
        public double LowerBoundary { get; set; }
    }
}