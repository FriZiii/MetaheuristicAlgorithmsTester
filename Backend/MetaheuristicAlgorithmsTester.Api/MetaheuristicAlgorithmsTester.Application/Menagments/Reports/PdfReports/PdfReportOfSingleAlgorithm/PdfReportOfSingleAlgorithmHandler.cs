﻿using MediatR;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using Microsoft.AspNetCore.StaticFiles;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports.PdfReports.PdfReportOfSingleAlgorithm
{
    public class PdfReportOfSingleAlgorithmHandler(IExecutedSingleAlgorithmsRepository executedAlgorithmsRepository, IAlgorithmsRepository algorithmsRepository, IFitnessFunctionRepository fitnessFunctionRepository) : IRequestHandler<PdfReportOfSingleAlgorithm, ReportResult>
    {
        public async Task<ReportResult> Handle(PdfReportOfSingleAlgorithm request, CancellationToken cancellationToken)
        {
            var execudedAlgorithmData = await executedAlgorithmsRepository.GetExecutedAlgorithmById(request.ExecutedId);
            if (execudedAlgorithmData == null)
            {
                return new ReportResult() { IsSuccesfull = false, Message = $"The executed test with id {request.ExecutedId} was not found" };
            }
            var fitnessFunction = await fitnessFunctionRepository.GetFitnessFunctionById(execudedAlgorithmData.TestedFitnessFunctionId);
            var algorithm = await algorithmsRepository.GetAlgorithmById(execudedAlgorithmData.TestedAlgorithmId);

            var fileContentRaw = GenerateReportContent.GeneratePdfContentOfSingleAlgorithmTest(execudedAlgorithmData, algorithm, fitnessFunction);

            var pdfRenderer = new IronPdf.ChromePdfRenderer();
            var fileContent = pdfRenderer.RenderHtmlAsPdf(fileContentRaw).BinaryData;

            var fileName = $"{execudedAlgorithmData.TestedAlgorithmName}-{execudedAlgorithmData.TestedFitnessFunctionName}-{execudedAlgorithmData.Date.ToString("dd-MM-yyyy")}.pdf";

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return new ReportResult() { IsSuccesfull = true, Message = "Report generated succesfully", ContentType = contentType, FileContent = fileContent, FileName = fileName };
        }
    }
}
