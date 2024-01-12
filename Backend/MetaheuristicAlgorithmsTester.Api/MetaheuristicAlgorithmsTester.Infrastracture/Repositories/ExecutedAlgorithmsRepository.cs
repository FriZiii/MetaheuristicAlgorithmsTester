using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    public class ExecutedAlgorithmsRepository(Context context) : IExecutedAlgorithmsRepository
    {
        public async Task AddExecudedAlgorithm(ExecutedAlgorithm executedAlgorithm)
        {
            context.ExecutedAlgorithms.Add(executedAlgorithm);
            await context.SaveChangesAsync();
        }

        public async Task<ExecutedAlgorithm?> GetExecutedAlgorithmById(int algorithmId)
            => await context.ExecutedAlgorithms.FirstOrDefaultAsync(x => x.Id == algorithmId);

        public async Task<IEnumerable<ExecutedAlgorithm?>> GetAllExecutedAlgorithms()
            => await context.ExecutedAlgorithms.ToListAsync();

        public async Task<bool> DeleteExecutedAlgorithmById(int id)
        {
            var executedToDelete = await context.ExecutedAlgorithms.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (executedToDelete != null)
            {
                context.ExecutedAlgorithms.Remove(executedToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> GetCurrentExecutedId()
        {
            var maxId = await context.ExecutedAlgorithms.MaxAsync(a => (int?)a.Id) ?? 0;
            return maxId + 1;
        }
    }
}
