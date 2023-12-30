using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAlgorithmsRepository, AlgorithmsRepository>();
        }
    }
}
