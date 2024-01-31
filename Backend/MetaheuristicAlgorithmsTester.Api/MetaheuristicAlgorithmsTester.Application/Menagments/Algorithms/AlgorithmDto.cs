using MetaheuristicAlgorithmsTester.Domain.Entities;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms
{
    public class AlgorithmDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public List<ParamInfo> Parameters { get; set; } = default!;
    }
}
