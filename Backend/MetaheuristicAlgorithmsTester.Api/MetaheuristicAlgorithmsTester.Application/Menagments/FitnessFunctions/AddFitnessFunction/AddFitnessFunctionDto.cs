using Microsoft.AspNetCore.Http;

namespace MetaheuristicAlgorithmsTester.Application.Menagments.FitnessFunctions.AddFitnessFunction
{
    public class AddFitnessFunctionDto
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public IFormFile DllFile { get; set; } = default!;
    }
}
