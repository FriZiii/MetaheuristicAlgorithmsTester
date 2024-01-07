using MetaheuristicAlgorithmsTester.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Persistence
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {

        }


        public DbSet<ExecutedAlgorithm> ExecutedAlgorithms { get; set; }
        public DbSet<Algorithm> Algorithms { get; set; } = null!;
        public DbSet<ParamInfo> Parameters { get; set; }
        public DbSet<FitnessFunction> FitnessFunctions { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
