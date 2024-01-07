using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports.PdfReports.PdfReportOfMultipleAlgorithms
{
    public class PdfReportOfMultipleAlgorithms : IRequest<ReportResult>
    {
        public List<int> ExecutedIds { get; set; } = default!;
    }
}
