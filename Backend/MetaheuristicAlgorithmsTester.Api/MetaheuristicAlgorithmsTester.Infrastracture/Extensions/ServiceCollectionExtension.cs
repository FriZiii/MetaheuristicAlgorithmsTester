using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;
using MetaheuristicAlgorithmsTester.Infrastracture.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAlgorithmsRepository, AlgorithmsRepository>();
            services.AddScoped<IFitnessFunctionRepository, FitnessFunctionRepository>();
            services.AddScoped<IExecutedAlgorithmsRepository, ExecutedAlgorithmsRepository>();

            services.AddDbContext<Context>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")));

            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(configuration.GetSection("Storage:ConnectionString").Value);
            });
        }
    }
}
