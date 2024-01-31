using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using System.Text.Json;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports.TxtReports.TxtReportOfSingleAlgorithm
{
    public class TxtReportOfSingleAlgorithmHandler(IExecutedSingleAlgorithmsRepository executedAlgorithmsRepository, IAlgorithmsRepository algorithmsRepository, IFitnessFunctionRepository fitnessFunctionRepository) : IRequestHandler<TxtReportOfSingleAlgorithm, ReportResult>
    {
        public async Task<ReportResult> Handle(TxtReportOfSingleAlgorithm request, CancellationToken cancellationToken)
        {
            var execudedAlgorithmData = await executedAlgorithmsRepository.GetExecutedAlgorithmById(request.ExecutedId);
            if (execudedAlgorithmData == null)
            {
                return new ReportResult() { IsSuccesfull = false, Message = $"The executed test with id {request.ExecutedId} was not found" };
            }
            var fitnessFunction = await fitnessFunctionRepository.GetFitnessFunctionById(execudedAlgorithmData.TestedFitnessFunctionId);
            var algorithm = await algorithmsRepository.GetAlgorithmById(execudedAlgorithmData.TestedAlgorithmId);

            var fileContentRaw = GenerateReportContent.GenerateTxtContentOfSingleAlgorithmTest(execudedAlgorithmData, algorithm, fitnessFunction);
            var jsonString = JsonSerializer.Serialize(fileContentRaw, new JsonSerializerOptions { WriteIndented = true });
            var fileContent = System.Text.Encoding.UTF8.GetBytes(jsonString);

            var fileName = $"{execudedAlgorithmData.TestedAlgorithmName}-{execudedAlgorithmData.TestedFitnessFunctionName}-{execudedAlgorithmData.Date.ToString("dd-MM-yyyy")}.txt";

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return new ReportResult() { IsSuccesfull = true, Message = "Report generated succesfully", ContentType = contentType, FileContent = fileContent, FileName = fileName };
        }
    }
}
