using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.Reports.PdfReports.PdfReportOfMultipleAlgorithms;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using System.Text.Json;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports.TxtReports.TxtReportOfMultipleAlgorithms
{
    public class TxtReportOfMultipleAlgorithmsHandler(IExecutedAlgorithmsRepository executedAlgorithmsRepository, IAlgorithmsRepository algorithmsRepository, IFitnessFunctionRepository fitnessFunctionRepository) : IRequestHandler<TxtReportOfMultipleAlgorithms, ReportResult>
    {
        public async Task<ReportResult> Handle(TxtReportOfMultipleAlgorithms request, CancellationToken cancellationToken)
        {
            var execudedAlgorithmsData = new List<ExecutedAlgorithm>();
            foreach (var executedId in request.ExecutedIds)
            {
                var tempExecutedData = await executedAlgorithmsRepository.GetExecutedAlgorithmById(executedId);
                if (tempExecutedData != null)
                {
                    execudedAlgorithmsData.Add(tempExecutedData);
                }
            }

            if(execudedAlgorithmsData.Count <= 0)
            {
                return new ReportResult() { IsSuccesfull = false, Message = $"The executed multiple test with ids {string.Join(", ", request.ExecutedIds)} was not found" };
            }

            var algorithms = new List<Algorithm>();
            foreach (var algorithmData in execudedAlgorithmsData)
            {
                var tempAlgorithm = await algorithmsRepository.GetAlgorithmById(algorithmData.TestedAlgorithmId);
                if (tempAlgorithm != null)
                {
                    algorithms.Add(tempAlgorithm);
                }
            }

            var fitnessFunctions = await fitnessFunctionRepository.GetFitnessFunctionById(execudedAlgorithmsData[0].TestedFitnessFunctionId);

            var fileContentRaw = GenerateReportContent.GenerateTxtContentOfMultipleAlgorithmsTest(execudedAlgorithmsData, algorithms, fitnessFunctions);
            var jsonString = JsonSerializer.Serialize(fileContentRaw, new JsonSerializerOptions { WriteIndented = true });
            var fileContent = System.Text.Encoding.UTF8.GetBytes(jsonString);

            var fileName = $"multiple-{execudedAlgorithmsData[0].TestedAlgorithmName}-{execudedAlgorithmsData[0].TestedFitnessFunctionName}-{execudedAlgorithmsData[0].Date.ToString("dd-MM-yyyy")}.txt";
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return new ReportResult() { IsSuccesfull = true, Message = "Report generated succesfully", ContentType = contentType, FileContent = fileContent, FileName = fileName };
        }
    }
}
