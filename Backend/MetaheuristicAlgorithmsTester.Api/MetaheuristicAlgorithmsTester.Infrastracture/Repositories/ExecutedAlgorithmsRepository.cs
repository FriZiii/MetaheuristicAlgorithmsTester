using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    public class ExecutedAlgorithmsRepository(Context context) : IExecutedAlgorithmsRepository
    {
        public async Task AddExecudedAlgorithm(ExecutedAlgorithm executedAlgorithm)
        {
            context.ExecutedAlgorithms.Add(executedAlgorithm);
            await context.SaveChangesAsync();
        }
    }
}
