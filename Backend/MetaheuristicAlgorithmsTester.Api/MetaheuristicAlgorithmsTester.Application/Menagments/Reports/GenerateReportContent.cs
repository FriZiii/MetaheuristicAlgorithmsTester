using MetaheuristicAlgorithmsTester.Domain.Entities;
using System.Text;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports
{
    public static class GenerateReportContent
    {
        public static string GeneratePpfContentOfSingleAlgorithmTest(ExecutedAlgorithm executedAlgorithm, Algorithm? algorithm, FitnessFunction? fitnessFunction)
        {
            if (algorithm == null || fitnessFunction == null)
            {
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<head>\r\n    <style>\r\n        body {\r\n            font-family: Arial, Helvetica, sans-serif;\r\n            display: flex;\r\n            flex-direction: column;\r\n            align-items: center;\r\n        }\r\n        .container {\r\n            background-color: rgba(104, 158, 222, 0.42);\r\n            padding: 10px;\r\n            margin: 10px;\r\n            width: 430px;\r\n        }\r\n        h4{\r\n            font-size: 15px;\r\n            font-weight: 100;\r\n            margin: 0;\r\n        }\r\n        h3{\r\n            margin: 0 0 10 0;\r\n            font-size: 30px;\r\n        }\r\n        p{\r\n            margin: 0;\r\n            font-size:18px;\r\n        }\r\n        li{\r\n            list-style-type: decimal;\r\n            font-size: 14px;\r\n            margin-left: 10;\r\n            font-size: 20px;\r\n        }\r\n    </style>\r\n</head>");
                htmlBuilder.AppendLine($"<body>");
                htmlBuilder.AppendLine($"<h1>Test of single algorithm  -  {executedAlgorithm.Date}</h1>");
                htmlBuilder.AppendLine($"<h2>Execution time  -  execution time</h2>");
                htmlBuilder.AppendLine($"<div class=\"container\">");
                htmlBuilder.AppendLine($"<h4>Tested Algorithm</h4>");
                htmlBuilder.AppendLine($"<h3>{executedAlgorithm.TestedAlgorithmName}</h3>");
                htmlBuilder.AppendLine($"<p>Parameters: </p>");
                foreach (var parameter in executedAlgorithm.Parameters)
                {
                    htmlBuilder.AppendLine($"<li>{parameter}</li>");
                }
                htmlBuilder.AppendLine($"</div>");
                htmlBuilder.AppendLine($"<div  class=\"container\">");
                htmlBuilder.AppendLine($"<h4>Fitness Function</h4>");
                htmlBuilder.AppendLine($"<h3>{executedAlgorithm.TestedFitnessFunctionName}</h3>");
                htmlBuilder.AppendLine($"<p>Number of parameters: {executedAlgorithm.XBest.Count()}</p>");
                htmlBuilder.AppendLine($"</div>");
                htmlBuilder.AppendLine($"<div  class=\"container\">");
                htmlBuilder.AppendLine($"<h3>Result</h3>");
                htmlBuilder.AppendLine($"<p>Number of evaluation of fitness function:  {executedAlgorithm.NumberOfEvaluationFitnessFunction}</p>");
                htmlBuilder.AppendLine($"<p>Fitness Function Value: {executedAlgorithm.FBest}</p>");
                htmlBuilder.AppendLine($"<p>Best X Values:</p>");
                foreach (var x in executedAlgorithm.XBest)
                {
                    htmlBuilder.AppendLine($"<li>{x}</li>");
                }
                return htmlBuilder.ToString();
            }
            else
            {
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<head>\r\n    <style>\r\n        body {\r\n            font-family: Arial, Helvetica, sans-serif;\r\n            display: flex;\r\n            flex-direction: column;\r\n            align-items: center;\r\n        }\r\n        .container {\r\n            background-color: rgba(104, 158, 222, 0.42);\r\n            padding: 10px;\r\n            margin: 10px;\r\n            width: 430px;\r\n        }\r\n        h4{\r\n            font-size: 15px;\r\n            font-weight: 100;\r\n            margin: 0;\r\n        }\r\n        h3{\r\n            margin: 0 0 10 0;\r\n            font-size: 30px;\r\n        }\r\n        p{\r\n            margin: 0;\r\n            font-size:18px;\r\n        }\r\n        li{\r\n            list-style-type: decimal;\r\n            font-size: 14px;\r\n            margin-left: 10;\r\n            font-size: 20px;\r\n        }\r\n    </style>\r\n</head>");
                htmlBuilder.AppendLine($"<body>");
                htmlBuilder.AppendLine($"<h1>Test of single algorithm  -  {executedAlgorithm.Date}</h1>");
                htmlBuilder.AppendLine($"<h2>Execution time  -  execution time</h2>");
                htmlBuilder.AppendLine($"<div class=\"container\">");
                htmlBuilder.AppendLine($"<h4>{executedAlgorithm.TestedAlgorithmName}</h4>");
                htmlBuilder.AppendLine($"<p>Parameters: </p>");
                if (executedAlgorithm.Parameters.Count() == algorithm.Parameters.Count())
                {
                    for (int i = 0; i < executedAlgorithm.Parameters.Count; i++)
                    {
                        htmlBuilder.AppendLine($"<li>{algorithm.Parameters[i].Name}: {executedAlgorithm.Parameters[i]} - {algorithm.Parameters[i].Description}</li>");
                    }
                }
                htmlBuilder.AppendLine($"</div>");
                htmlBuilder.AppendLine($"<div  class=\"container\">");
                htmlBuilder.AppendLine($"<h4>Fitness Function</h4>");
                htmlBuilder.AppendLine($"<h3>{executedAlgorithm.TestedFitnessFunctionName}</h3>");
                htmlBuilder.AppendLine($"<p>Number of parameters: {executedAlgorithm.XBest.Count()}</p>");
                htmlBuilder.AppendLine($"</div>");
                htmlBuilder.AppendLine($"<div  class=\"container\">");
                htmlBuilder.AppendLine($"<h3>Result</h3>");
                htmlBuilder.AppendLine($"<p>Number of evaluation of fitness function:  {executedAlgorithm.NumberOfEvaluationFitnessFunction}</p>");
                htmlBuilder.AppendLine($"<p>Fitness Function Value: {executedAlgorithm.FBest}</p>");
                htmlBuilder.AppendLine($"<p>Best X Values:</p>");
                foreach (var x in executedAlgorithm.XBest)
                {
                    htmlBuilder.AppendLine($"<li>{x}</li>");
                }
                return htmlBuilder.ToString();
            }
        }

        public static string GeneratePpfContentOfMultipleAlgorithmsTest(List<ExecutedAlgorithm> executedAlgorithms, List<Algorithm> algorithms, FitnessFunction fitnessFunction)
        {
            if (algorithms.Count == executedAlgorithms.Count && algorithms.All(x => x != null) && executedAlgorithms.All(y => y != null))
            {
                //TODO: zwróc raport
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
            else
            {
                //TODO: zwróc raport ale bez ParamName i description
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
}
