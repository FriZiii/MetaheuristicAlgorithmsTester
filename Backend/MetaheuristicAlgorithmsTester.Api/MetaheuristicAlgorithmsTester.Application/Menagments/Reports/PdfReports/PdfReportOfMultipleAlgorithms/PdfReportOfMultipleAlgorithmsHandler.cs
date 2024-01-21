using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using Microsoft.AspNetCore.StaticFiles;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports.PdfReports.PdfReportOfMultipleAlgorithms
{
    public class PdfReportOfMultipleAlgorithmsHandler(IExecutedMultipleAlgorithmsRepository executedMultipleAlgorithmsRepository, IAlgorithmsRepository algorithmsRepository, IFitnessFunctionRepository fitnessFunctionRepository) : IRequestHandler<PdfReportOfMultipleAlgorithms, ReportResult>
    {
        public async Task<ReportResult> Handle(PdfReportOfMultipleAlgorithms request, CancellationToken cancellationToken)
        {
            var execudedAlgorithmsData = await executedMultipleAlgorithmsRepository.GetExecutedAlgorithmsByExecutedId(request.ExecutedId);

            if (execudedAlgorithmsData.Count <= 0)
            {
                return new ReportResult() { IsSuccesfull = false, Message = $"The executed multiple test with ids {string.Join(", ", request.ExecutedId)} was not found" };
            }

            var algorithms = new List<Algorithm>();
            foreach (var data in execudedAlgorithmsData)
            {
                var tempAlgorithm = await algorithmsRepository.GetAlgorithmById(data.TestedAlgorithmId);
                if (tempAlgorithm != null)
                {
                    algorithms.Add(tempAlgorithm);
                }
            }

            var fitnessFunctions = await fitnessFunctionRepository.GetFitnessFunctionById(execudedAlgorithmsData[0].TestedFitnessFunctionId);

            var fileContentRaw = GenerateReportContent.GeneratePpfContentOfMultipleAlgorithmsTest(execudedAlgorithmsData, algorithms, fitnessFunctions);
            var pdfRenderer = new IronPdf.ChromePdfRenderer();
            var fileContent = pdfRenderer.RenderHtmlAsPdf(fileContentRaw).BinaryData;

            var fileName = $"multiple-{execudedAlgorithmsData[0].TestedFitnessFunctionName}-{execudedAlgorithmsData[0].Date}.pdf";

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return new ReportResult() { IsSuccesfull = true, Message = "Report generated succesfully", ContentType = contentType, FileContent = fileContent, FileName = fileName };
        }
    }
}
