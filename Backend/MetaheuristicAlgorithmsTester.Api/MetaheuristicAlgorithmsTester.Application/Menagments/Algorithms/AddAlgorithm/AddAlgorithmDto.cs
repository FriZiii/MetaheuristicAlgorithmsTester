using Microsoft.AspNetCore.Http;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm
{
    public class AddAlgorithmDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public IFormFile DllFile { get; set; } = default!;
    }
}
