﻿using MediatR;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports.TxtReports.TxtReportOfMultipleAlgorithms
{
    public class TxtReportOfMultipleAlgorithms : IRequest<ReportResult>
    {
        public List<int> ExecutedIds { get; set; } = default!;
    }
}
