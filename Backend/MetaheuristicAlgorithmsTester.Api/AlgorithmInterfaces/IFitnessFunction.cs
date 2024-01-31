namespace AlgorithmInterfaces
{
    public interface IFitnessFunction
    {
        double Function(params double[] x);
        double[] LowerBounds { get; set; }
        double[] UpperBounds { get; set; }

        /// <summary>
        /// Gets or sets the number of parameters for the fitness function.
        /// </summary>
        /// <remarks>
        /// This property represents the number of parameters accepted by the fitness function.
        /// If the fitness function accepts an infinite number of parameters, set this value to 0.
        /// </remarks>
        public int NumberOfParameters { get; set; }
    }
}