namespace AlgorithmInterfaces
{
    public interface IAlgorithm
    {
        string Name { get; set; }
        void Solve(IFitnessFunction fitnessFunction, params double[] parameters);
        ParamInfo[] ParamsInfo { get; set; }

        double[] XBest { get; set; }
        double FBest { get; set; }
        int NumberOfEvaluationFitnessFunction { get; set; }

        IStateWriter StateWriter { get; set; }
        IStateReader StateReader { get; set; }

        IGenerateTextReport StringReportGenerator { get; set; }
        IGeneratePDFReport PdfReportGenerator { get; set; }
    }
}