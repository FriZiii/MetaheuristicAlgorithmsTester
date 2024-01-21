using MetaheuristicAlgorithmsTester.Domain.Entities;
using System.Text;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports
{
    public static class GenerateReportContent
    {
        public static string GeneratePpfContentOfSingleAlgorithmTest(ExecutedSingleAlgorithm executedAlgorithm, Algorithm? algorithm, FitnessFunction? fitnessFunction)
        {
            if (algorithm == null || fitnessFunction == null)
            {
                //raport ale bez ParamName i description
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<head>\r\n    <style>\r\n        body {\r\n            font-family: Arial, Helvetica, sans-serif;\r\n            display: flex;\r\n            flex-direction: column;\r\n            align-items: center;\r\n        }\r\n        .container {\r\n            background-color: rgba(104, 158, 222, 0.42);\r\n            padding: 10px;\r\n            margin: 10px;\r\n            width: 450px;\r\n        }\r\n        h4{\r\n            font-size: 15px;\r\n            font-weight: 100;\r\n            margin: 0;\r\n        }\r\n        h3{\r\n            margin: 0 0 10 0;\r\n            font-size: 30px;\r\n        }\r\n        p{\r\n            margin: 0;\r\n            font-size:18px;\r\n        }\r\n        li{\r\n            list-style-type: decimal;\r\n            font-size: 14px;\r\n            margin-left: 10;\r\n            font-size: 20px;\r\n            margin-bottom: 3px;\r\n        }\r\n    </style>\r\n</head>");

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
                htmlBuilder.AppendLine("<head>\r\n    <style>\r\n        body {\r\n            font-family: Arial, Helvetica, sans-serif;\r\n            display: flex;\r\n            flex-direction: column;\r\n            align-items: center;\r\n        }\r\n        .container {\r\n            background-color: rgba(104, 158, 222, 0.42);\r\n            padding: 10px;\r\n            margin: 10px;\r\n            width: 450px;\r\n        }\r\n        h4{\r\n            font-size: 15px;\r\n            font-weight: 100;\r\n            margin: 0;\r\n        }\r\n        h3{\r\n            margin: 0 0 10 0;\r\n            font-size: 30px;\r\n        }\r\n        p{\r\n            margin: 0;\r\n            font-size:18px;\r\n        }\r\n        li{\r\n            list-style-type: decimal;\r\n            font-size: 14px;\r\n            margin-left: 10;\r\n            font-size: 20px;\r\n            margin-bottom: 3px;\r\n        }\r\n    </style>\r\n</head>");
                htmlBuilder.AppendLine($"<body>");
                htmlBuilder.AppendLine($"<h1>Test of single algorithm  -  {executedAlgorithm.Date}</h1>");
                htmlBuilder.AppendLine($"<h2>Execution time  -  execution time</h2>");
                htmlBuilder.AppendLine($"<div class=\"container\">");
                htmlBuilder.AppendLine($"<h4>Tested Algorithm</h4>");
                htmlBuilder.AppendLine($"<h3>{executedAlgorithm.TestedAlgorithmName}</h3>");
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

        public static string GeneratePpfContentOfMultipleAlgorithmsTest(List<ExecutedMultipleAlgorithms> executedAlgorithms, List<Algorithm> algorithms, FitnessFunction fitnessFunction)
        {
            if (algorithms.Count == executedAlgorithms.Count && algorithms.All(x => x != null) && executedAlgorithms.All(y => y != null))
            {
                StringBuilder htmlBuilder = new StringBuilder();

                for (int i = 0; i < executedAlgorithms.Count; i++)
                {
                    htmlBuilder.AppendLine("<head>\r\n    <style>\r\n        body {\r\n            font-family: Arial, Helvetica, sans-serif;\r\n            display: flex;\r\n            flex-direction: column;\r\n            align-items: center;\r\n        }\r\n        .container {\r\n            background-color: rgba(104, 158, 222, 0.42);\r\n            padding: 10px;\r\n            margin: 10px;\r\n            width: 450px;\r\n        }\r\n        h4{\r\n            font-size: 15px;\r\n            font-weight: 100;\r\n            margin: 0;\r\n        }\r\n        h3{\r\n            margin: 0 0 10 0;\r\n            font-size: 30px;\r\n        }\r\n        p{\r\n            margin: 0;\r\n            font-size:18px;\r\n        }\r\n        li{\r\n            list-style-type: decimal;\r\n            font-size: 14px;\r\n            margin-left: 10;\r\n            font-size: 20px;\r\n            margin-bottom: 3px;\r\n        }\r\n    \r\n        hr{\r\n            width: 500px;\r\n        }</style>\r\n</head>");
                    htmlBuilder.AppendLine($"<body>");
                    htmlBuilder.AppendLine($"<h1>Test of multiple algorithm  -  {executedAlgorithms[i].Date}</h1>");
                    htmlBuilder.AppendLine($"<h2>Execution time  -  execution time</h2>");
                    htmlBuilder.AppendLine($"<div class=\"container\">");
                    htmlBuilder.AppendLine($"<h4>Tested Algorithm</h4>");
                    htmlBuilder.AppendLine($"<h3>{executedAlgorithms[i].TestedAlgorithmName}</h3>");
                    htmlBuilder.AppendLine($"<p>Parameters: </p>");
                    if (executedAlgorithms[i].Parameters.Count() == algorithms[i].Parameters.Count())
                    {
                        for (int j = 0; j < executedAlgorithms[j].Parameters.Count; j++)
                        {
                            htmlBuilder.AppendLine($"<li>{algorithms[j].Parameters[j].Name}: {executedAlgorithms[j].Parameters[j]} - {algorithms[j].Parameters[j].Description}</li>");
                        }
                    }
                    htmlBuilder.AppendLine($"</div>");
                    htmlBuilder.AppendLine($"<div  class=\"container\">");
                    htmlBuilder.AppendLine($"<h4>Fitness Function</h4>");
                    htmlBuilder.AppendLine($"<h3>{executedAlgorithms[i].TestedFitnessFunctionName}</h3>");
                    htmlBuilder.AppendLine($"<p>Number of parameters: {executedAlgorithms[i].XBest.Count()}</p>");
                    htmlBuilder.AppendLine($"</div>");
                    htmlBuilder.AppendLine($"<div  class=\"container\">");
                    htmlBuilder.AppendLine($"<h3>Result</h3>");
                    htmlBuilder.AppendLine($"<p>Number of evaluation of fitness function:  {executedAlgorithms[i].NumberOfEvaluationFitnessFunction}</p>");
                    htmlBuilder.AppendLine($"<p>Fitness Function Value: {executedAlgorithms[i].FBest}</p>");
                    htmlBuilder.AppendLine($"<p>Best X Values:</p>");
                    foreach (var x in executedAlgorithms[i].XBest)
                    {
                        htmlBuilder.AppendLine($"<li>{x}</li>");
                    }


                    htmlBuilder.AppendLine("<hr/>");
                }

                return htmlBuilder.ToString();
            }
            else
            {
                StringBuilder htmlBuilder = new StringBuilder();

                for (int i = 0; i < executedAlgorithms.Count; i++)
                {
                    htmlBuilder.AppendLine("<head>\r\n    <style>\r\n        body {\r\n            font-family: Arial, Helvetica, sans-serif;\r\n            display: flex;\r\n            flex-direction: column;\r\n            align-items: center;\r\n        }\r\n        .container {\r\n            background-color: rgba(104, 158, 222, 0.42);\r\n            padding: 10px;\r\n            margin: 10px;\r\n            width: 450px;\r\n        }\r\n        h4{\r\n            font-size: 15px;\r\n            font-weight: 100;\r\n            margin: 0;\r\n        }\r\n        h3{\r\n            margin: 0 0 10 0;\r\n            font-size: 30px;\r\n        }\r\n        p{\r\n            margin: 0;\r\n            font-size:18px;\r\n        }\r\n        li{\r\n            list-style-type: decimal;\r\n            font-size: 14px;\r\n            margin-left: 10;\r\n            font-size: 20px;\r\n            margin-bottom: 3px;\r\n        }\r\n    \r\n        hr{\r\n            width: 500px;\r\n        }</style>\r\n</head>");
                    htmlBuilder.AppendLine($"<body>");
                    htmlBuilder.AppendLine($"<h1>Test of multiple algorithm  -  {executedAlgorithms[i].Date}</h1>");
                    htmlBuilder.AppendLine($"<h2>Execution time  -  execution time</h2>");
                    htmlBuilder.AppendLine($"<div class=\"container\">");
                    htmlBuilder.AppendLine($"<h4>Tested Algorithm</h4>");
                    htmlBuilder.AppendLine($"<h3>{executedAlgorithms[i].TestedAlgorithmName}</h3>");
                    htmlBuilder.AppendLine($"<p>Parameters: </p>");
                    foreach (var parameter in executedAlgorithms[i].Parameters)
                    {
                        htmlBuilder.AppendLine($"<li>{parameter}</li>");
                    }
                    htmlBuilder.AppendLine($"</div>");
                    htmlBuilder.AppendLine($"<div  class=\"container\">");
                    htmlBuilder.AppendLine($"<h4>Fitness Function</h4>");
                    htmlBuilder.AppendLine($"<h3>{executedAlgorithms[i].TestedFitnessFunctionName}</h3>");
                    htmlBuilder.AppendLine($"<p>Number of parameters: {executedAlgorithms[i].XBest.Count()}</p>");
                    htmlBuilder.AppendLine($"</div>");
                    htmlBuilder.AppendLine($"<div  class=\"container\">");
                    htmlBuilder.AppendLine($"<h3>Result</h3>");
                    htmlBuilder.AppendLine($"<p>Number of evaluation of fitness function:  {executedAlgorithms[i].NumberOfEvaluationFitnessFunction}</p>");
                    htmlBuilder.AppendLine($"<p>Fitness Function Value: {executedAlgorithms[i].FBest}</p>");
                    htmlBuilder.AppendLine($"<p>Best X Values:</p>");
                    foreach (var x in executedAlgorithms[i].XBest)
                    {
                        htmlBuilder.AppendLine($"<li>{x}</li>");
                    }


                    htmlBuilder.AppendLine("<hr/>");
                }

                return htmlBuilder.ToString();
            }
        }

        public static string GenerateTxtContentOfSingleAlgorithmTest(ExecutedSingleAlgorithm executedAlgorithm, Algorithm? algorithm, FitnessFunction? fitnessFunction)
        {

            string report = "";
            report += $"Test of single algorithm -  {executedAlgorithm.Date} \n";
            report += $"Tested Algorithm -  {executedAlgorithm.TestedAlgorithmName} \n";
            report += $"Parameters:  {executedAlgorithm.TestedAlgorithmName} \n";
            if (executedAlgorithm.Parameters.Count() == algorithm.Parameters.Count())
            {
                for (int i = 0; i < executedAlgorithm.Parameters.Count; i++)
                {
                    report += $"{algorithm.Parameters[i].Name}: {executedAlgorithm.Parameters[i]} - {algorithm.Parameters[i].Description} \n";
                }
            }
            report += $"Fitness Function:  {executedAlgorithm.TestedFitnessFunctionName} \n";
            report += $"Number of parameters:   {executedAlgorithm.XBest.Count()} \n";
            report += $"Number of evaluation of fitness function:  {executedAlgorithm.NumberOfEvaluationFitnessFunction} \n";
            report += $"Fitness Function Value:  {executedAlgorithm.FBest} \n";
            report += $"Best X Values: \n";
            foreach (var x in executedAlgorithm.XBest)
            {
                report += $"{x} \n";
            }

            return report;
        }

        public static string GenerateTxtContentOfMultipleAlgorithmsTest(List<ExecutedSingleAlgorithm> executedAlgorithms, List<Algorithm> algorithms, FitnessFunction fitnessFunction)
        {
            string report = "";
            for (int i = 0; i < executedAlgorithms.Count; i++)
            {
                report += $"Test of single algorithm -  {executedAlgorithms[i].Date} \n";
                report += $"Tested Algorithm -  {executedAlgorithms[i].TestedAlgorithmName} \n";
                report += $"Parameters:  {executedAlgorithms[i].TestedAlgorithmName} \n";
                if (executedAlgorithms[i].Parameters.Count() == algorithms[i].Parameters.Count())
                {
                    for (int j = 0; j < executedAlgorithms[j].Parameters.Count; j++)
                    {
                        report += $"{algorithms[i].Parameters[j].Name}: {executedAlgorithms[i].Parameters[j]} - {algorithms[i].Parameters[j].Description} \n";
                    }
                }
                report += $"Fitness Function:  {executedAlgorithms[i].TestedFitnessFunctionName} \n";
                report += $"Number of parameters:   {executedAlgorithms[i].XBest.Count()} \n";
                report += $"Number of evaluation of fitness function:  {executedAlgorithms[i].NumberOfEvaluationFitnessFunction} \n";
                report += $"Fitness Function Value:  {executedAlgorithms[i].FBest} \n";
                report += $"Best X Values: \n";
                foreach (var x in executedAlgorithms[i].XBest)
                {
                    report += $"{x} \n";
                }
            }
            return report;
        }

    }
}
