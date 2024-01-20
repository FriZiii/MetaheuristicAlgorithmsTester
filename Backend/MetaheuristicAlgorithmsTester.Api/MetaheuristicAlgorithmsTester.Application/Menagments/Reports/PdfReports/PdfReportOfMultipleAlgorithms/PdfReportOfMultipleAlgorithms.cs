using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports.PdfReports.PdfReportOfMultipleAlgorithms
{
    public class PdfReportOfMultipleAlgorithms : IRequest<ReportResult>
    {
        public string ExecutedId { get; set; } = default!;
    }
}
