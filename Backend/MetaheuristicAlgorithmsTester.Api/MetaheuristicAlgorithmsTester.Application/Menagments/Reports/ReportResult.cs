namespace MetaheuristicAlgorithmsTester.Application.Menagments.Reports
{
    public class ReportResult
    {
        public byte[] FileContent { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public string? FileName { get; set; } = default!;

        public string Message { get; set; } = default!;
        public bool IsSuccesfull { get; set; }
    }
}
