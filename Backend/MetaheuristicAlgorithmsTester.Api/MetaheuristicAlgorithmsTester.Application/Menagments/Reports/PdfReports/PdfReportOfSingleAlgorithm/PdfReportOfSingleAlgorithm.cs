using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports.PdfReports.PdfReportOfSingleAlgorithm
{
    public class PdfReportOfSingleAlgorithm : IRequest<ReportResult>
    {
        public int ExecutedId { get; set; } = default!;
    }
}
