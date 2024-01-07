using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    public class ExecutedAlgorithmsRepository(Context context) : IExecutedAlgorithmsRepository
    {
        public async Task<int> AddExecudedAlgorithm(ExecutedAlgorithm executedAlgorithm)
        {
            context.ExecutedAlgorithms.Add(executedAlgorithm);
            await context.SaveChangesAsync();

            return executedAlgorithm.Id;
        }

        public Task<ExecutedAlgorithm?> GetExecutedAlgorithmById(int algorithmId)
            => context.ExecutedAlgorithms.FirstOrDefaultAsync(x => x.Id == algorithmId);
    }
}
