using Azure.Storage.Blobs;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    public class ExecutedMultipleAlgorithmsRepository(BlobServiceClient blobServiceClient, IConfiguration configuration, Context context) : IExecutedMultipleAlgorithmsRepository
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

        public async Task<ExecutedMultipleAlgorithms?> GetExecutedAlgorithmById(int id)
            => await context.ExecutedMultipleAlgorithms.FirstOrDefaultAsync(a => a.Id == id);

        public async Task UpdateExecutedAlgorithm(int id, ExecutedMultipleAlgorithms updatedExecutedAlgorithm)
        {
            var existingExecutedAlgorithm = await context.ExecutedMultipleAlgorithms.FindAsync(id);

            if (existingExecutedAlgorithm != null)
            {
                if (updatedExecutedAlgorithm.ExecutionTime != null)
                {
                    existingExecutedAlgorithm.ExecutionTime = updatedExecutedAlgorithm.ExecutionTime;
                }
                existingExecutedAlgorithm.AlgorithmStateFileName = updatedExecutedAlgorithm.AlgorithmStateFileName;

                existingExecutedAlgorithm.TestedAlgorithmName = updatedExecutedAlgorithm.TestedAlgorithmName;
                existingExecutedAlgorithm.TestedFitnessFunctionName = updatedExecutedAlgorithm.TestedFitnessFunctionName;

                existingExecutedAlgorithm.Date = updatedExecutedAlgorithm.Date;
                existingExecutedAlgorithm.TimerFrequency = updatedExecutedAlgorithm.TimerFrequency;
                existingExecutedAlgorithm.Dimension = updatedExecutedAlgorithm.Dimension;

                existingExecutedAlgorithm.Parameters = updatedExecutedAlgorithm.Parameters;
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
                    if (!string.IsNullOrEmpty(toDelete.AlgorithmStateFileName))
                    {
                        var containerName = configuration.GetSection("Storage:StorageNameAlgorithmsStates").Value;
                        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                        var blobClient = containerClient.GetBlobClient(toDelete.AlgorithmStateFileName + ".txt");

                        await blobClient.DeleteIfExistsAsync();
                    }

                    context.ExecutedMultipleAlgorithms.Remove(toDelete);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }
    }
}
