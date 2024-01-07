using MetaheuristicAlgorithmsTester.Domain.Entities;
using System.Text;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports
{
    public static class GenerateReportContent
    {
        public static string GeneratePpfContentOfSingleAlgorithmTest(ExecutedAlgorithm executedAlgorithm)
        {
            return $@"
                    <div>
                        <p><strong>Id:</strong> {executedAlgorithm.Id}</p>
                        <p><strong>Date:</strong> {executedAlgorithm.Date}</p>
                        <p><strong>TestedAlgorithmId:</strong> {executedAlgorithm.TestedAlgorithmId}</p>
                        <p><strong>TestedAlgorithmName:</strong> {executedAlgorithm.TestedAlgorithmName}</p>
                        <p><strong>TestedFitnessFunctionId:</strong> {executedAlgorithm.TestedFitnessFunctionId}</p>
                        <p><strong>TestedFitnessFunctionName:</strong> {executedAlgorithm.TestedFitnessFunctionName}</p>
                        <p><strong>XBest:</strong> {string.Join(", ", executedAlgorithm.XBest)}</p>
                        <p><strong>FBest:</strong> {executedAlgorithm.FBest}</p>
                        <p><strong>NumberOfEvaluationFitnessFunction:</strong> {executedAlgorithm.NumberOfEvaluationFitnessFunction}</p>
                        <p><strong>Parameters:</strong> {string.Join(", ", executedAlgorithm.Parameters)}</p>
                    </div>";
        }

        public static string GeneratePpfContentOfMultipleAlgorithmsTest(List<ExecutedAlgorithm> executedAlgorithms)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            foreach (var executedAlgorithm in executedAlgorithms)
            {
                htmlBuilder.AppendLine("<div>");
                htmlBuilder.AppendLine($"<p><strong>Id:</strong> {executedAlgorithm.Id}</p>");
                htmlBuilder.AppendLine($"<p><strong>Date:</strong> {executedAlgorithm.Date}</p>");
                htmlBuilder.AppendLine($"<p><strong>TestedAlgorithmId:</strong> {executedAlgorithm.TestedAlgorithmId}</p>");
                htmlBuilder.AppendLine($"<p><strong>TestedAlgorithmName:</strong> {executedAlgorithm.TestedAlgorithmName}</p>");
                htmlBuilder.AppendLine($"<p><strong>TestedFitnessFunctionId:</strong> {executedAlgorithm.TestedFitnessFunctionId}</p>");
                htmlBuilder.AppendLine($"<p><strong>TestedFitnessFunctionName:</strong> {executedAlgorithm.TestedFitnessFunctionName}</p>");
                htmlBuilder.AppendLine($"<p><strong>XBest:</strong> {string.Join(", ", executedAlgorithm.XBest)}</p>");
                htmlBuilder.AppendLine($"<p><strong>FBest:</strong> {executedAlgorithm.FBest}</p>");
                htmlBuilder.AppendLine($"<p><strong>NumberOfEvaluationFitnessFunction:</strong> {executedAlgorithm.NumberOfEvaluationFitnessFunction}</p>");
                htmlBuilder.AppendLine($"<p><strong>Parameters:</strong> {string.Join(", ", executedAlgorithm.Parameters)}</p>");
                htmlBuilder.AppendLine("</div>");
                htmlBuilder.AppendLine("<hr/>");
            }

            return htmlBuilder.ToString();
        }
    }
}
