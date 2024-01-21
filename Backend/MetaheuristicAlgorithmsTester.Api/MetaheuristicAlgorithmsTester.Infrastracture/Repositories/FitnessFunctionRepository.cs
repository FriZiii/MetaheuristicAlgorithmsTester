using Azure.Storage.Blobs;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    public class FitnessFunctionRepository(BlobServiceClient blobServiceClient, IConfiguration configuration, Context context) : IFitnessFunctionRepository
    {
        public async Task<FitnessFunction?> AddFitnessFunction(FitnessFunction fitnessFunction)
        {
            if (fitnessFunction.DllFileBytes != null)
            {
                var containerName = configuration.GetSection("Storage:StorageNameFitnessFunctions").Value;

                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fitnessFunction.FileName);

                using (MemoryStream stream = new MemoryStream(fitnessFunction.DllFileBytes))
                {
                    await blobClient.UploadAsync(stream, true);
                }

                try
                {
                    context.FitnessFunctions.Add(fitnessFunction);
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return null;
                }

                return fitnessFunction;
            }
            return null;
        }

        public async Task<FitnessFunction?> GetFitnessFunctionById(int id)
        {
            var fitnessFunctions = context.FitnessFunctions.FirstOrDefault(a => a.Id == id);
            if (fitnessFunctions != null)
            {
                var containerName = configuration.GetSection("Storage:StorageNameFitnessFunctions").Value;
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fitnessFunctions.FileName);

                try
                {
                    var response = await blobClient.DownloadAsync();

                    using (var memoryStream = new MemoryStream())
                    {
                        await response.Value.Content.CopyToAsync(memoryStream);
                        fitnessFunctions.DllFileBytes = memoryStream.ToArray();
                    }

                    return fitnessFunctions;
                }
                catch (Exception)
                {
                    return null!;
                }
            }
            return null!;
        }

        public async Task<IEnumerable<FitnessFunction?>> GetAllFitnessFunctions()
            => await context.FitnessFunctions.ToListAsync();

        public async Task<bool> DeleteFitnessFunctionById(int id)
        {
            var fitnessFunctionToDelete = await context.FitnessFunctions.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (fitnessFunctionToDelete != null)
            {
                var containerName = configuration.GetSection("Storage:StorageNameFitnessFunctions").Value;
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fitnessFunctionToDelete.FileName + ".dll");

                await blobClient.DeleteIfExistsAsync();

                context.FitnessFunctions.Remove(fitnessFunctionToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
