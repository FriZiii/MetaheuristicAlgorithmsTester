using Azure.Storage.Blobs;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    public class ExecutedSingleAlgorithmsRepository(BlobServiceClient blobServiceClient, IConfiguration configuration, Context context) : IExecutedSingleAlgorithmsRepository
    {
        public async Task<int> AddExecudedAlgorithm(ExecutedSingleAlgorithm executedAlgorithm)
        {
            context.ExecutedSingleAlgorithms.Add(executedAlgorithm);
            await context.SaveChangesAsync();
            return executedAlgorithm.Id;
        }

        public async Task UpdateExecutedAlgorithm(int id, ExecutedSingleAlgorithm updatedExecutedAlgorithm)
        {
            var existingExecutedAlgorithm = await context.ExecutedSingleAlgorithms.FindAsync(id);

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

        public async Task<ExecutedSingleAlgorithm?> GetExecutedAlgorithmById(int algorithmId)
            => await context.ExecutedSingleAlgorithms.FirstOrDefaultAsync(x => x.Id == algorithmId);

        public async Task<IEnumerable<ExecutedSingleAlgorithm?>> GetAllExecutedAlgorithms()
            => await context.ExecutedSingleAlgorithms.ToListAsync();

        public async Task<bool> DeleteExecutedAlgorithmById(int id)
        {
            var executedToDelete = await context.ExecutedSingleAlgorithms.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (executedToDelete != null)
            {
                if (!string.IsNullOrEmpty(executedToDelete.AlgorithmStateFileName))
                {
                    var containerName = configuration.GetSection("Storage:StorageNameAlgorithmsStates").Value;
                    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    var blobClient = containerClient.GetBlobClient(executedToDelete.AlgorithmStateFileName + ".txt");

                    await blobClient.DeleteIfExistsAsync();
                }

                context.ExecutedSingleAlgorithms.Remove(executedToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
