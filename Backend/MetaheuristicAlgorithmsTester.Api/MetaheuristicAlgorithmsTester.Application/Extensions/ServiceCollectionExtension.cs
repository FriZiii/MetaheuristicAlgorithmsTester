using MediatR;
using MetaheuristicAlgorithmsTester.Application.Menagments.Algorithms.AddAlgorithm;
using Microsoft.Extensions.DependencyInjection;

namespace MetaheuristicAlgorithmsTester.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(AddAlgorithm));
        }
    }
}
