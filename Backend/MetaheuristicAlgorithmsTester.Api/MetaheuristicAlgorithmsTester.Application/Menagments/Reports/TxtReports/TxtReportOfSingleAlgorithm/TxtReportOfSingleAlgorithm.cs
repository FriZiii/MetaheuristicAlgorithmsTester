using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports.TxtReports.TxtReportOfSingleAlgorithm
{
    public class TxtReportOfSingleAlgorithm : IRequest<ReportResult>
    {
        public int ExecutedId { get; set; } = default!;
    }
}
