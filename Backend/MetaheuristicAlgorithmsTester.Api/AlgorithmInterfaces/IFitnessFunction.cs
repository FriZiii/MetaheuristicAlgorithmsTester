namespace AlgorithmInterfaces
{
    public interface IFitnessFunction
    {
        double Function(params double[] x);
        double[] LowerBounds { get; set; }
        double[] UpperBounds { get; set; }
    }
}