using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    public class ExecutedMultipleAlgorithmsRepositor(Context context) : IExecutedMultipleAlgorithmsRepositor
    {
        public async Task<int> AddExecudedAlgorithm(ExecutedMultipleAlgorithms executedAlgorithm)
        {
            context.ExecutedMultipleAlgorithms.Add(executedAlgorithm);
            await context.SaveChangesAsync();
            return executedAlgorithm.Id;
        }

        public async Task<IEnumerable<ExecutedMultipleAlgorithms?>> GetAllExecutedAlgorithms()
            => await context.ExecutedMultipleAlgorithms.ToListAsync();

        public async Task<List<ExecutedMultipleAlgorithms?>> GetExecutedAlgorithmsByExecutedId(string executedId)
            => await context.ExecutedMultipleAlgorithms.Where(a => a.MultipleTestId == executedId).ToListAsync();

        public async Task UpdateExecutedAlgorithm(int id, ExecutedMultipleAlgorithms updatedExecutedAlgorithm)
        {
            var existingExecutedAlgorithm = await context.ExecutedMultipleAlgorithms.FindAsync(id);

            if (existingExecutedAlgorithm != null)
            {
                existingExecutedAlgorithm.FBest = updatedExecutedAlgorithm.FBest;
                existingExecutedAlgorithm.XBest = updatedExecutedAlgorithm.XBest;
                existingExecutedAlgorithm.NumberOfEvaluationFitnessFunction = updatedExecutedAlgorithm.NumberOfEvaluationFitnessFunction;
                existingExecutedAlgorithm.IsFailed = updatedExecutedAlgorithm.IsFailed;
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"ExecutedAlgorithm with Id {id} was not found.");
            }
        }

        public async Task<bool> DeleteExecutedAlgorithmById(string id)
        {
            var executedToDelete = await context.ExecutedMultipleAlgorithms.Where(a => a.MultipleTestId == id).ToListAsync();
            if (executedToDelete != null)
            {
                foreach (var toDelete in executedToDelete)
                {
                    context.ExecutedMultipleAlgorithms.Remove(toDelete);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }
    }
}
