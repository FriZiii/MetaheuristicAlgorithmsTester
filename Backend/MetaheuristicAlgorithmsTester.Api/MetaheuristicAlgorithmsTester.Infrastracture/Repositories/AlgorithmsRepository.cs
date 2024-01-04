using Azure.Storage.Blobs;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    internal class AlgorithmsRepository(BlobServiceClient blobServiceClient, IConfiguration configuration, Context context) : IAlgorithmsRepository
    {
        public async Task<Algorithm?> AddAlgorithm(Algorithm algorithm)
        {
            if (algorithm.DllFileBytes != null)
            {
                var containerName = configuration.GetSection("Storage:StorageNameAlgorithms").Value;

                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(algorithm.FileName);

                using (MemoryStream stream = new MemoryStream(algorithm.DllFileBytes))
                {
                    await blobClient.UploadAsync(stream, true);
                }

                try
                {
                    context.Algorithms.Add(algorithm);
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return null;
                }

                return algorithm;
            }
            return null;
        }

        public async Task<bool> DeleteAlgorithmById(int id)
        {
            var algorithmToDelete = await context.Algorithms.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (algorithmToDelete != null)
            {
                var containerName = configuration.GetSection("Storage:StorageNameAlgorithms").Value;
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(algorithmToDelete.FileName);

                await blobClient.DeleteIfExistsAsync();

                context.Algorithms.Remove(algorithmToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Algorithm?> GetAlgorithmById(int id)
        {
            var algorithm = context.Algorithms.Include(o => o.Parameters).FirstOrDefault(a => a.Id == id);
            if (algorithm != null)
            {
                var containerName = configuration.GetSection("Storage:StorageNameAlgorithms").Value;
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(algorithm.FileName);

                try
                {
                    var response = await blobClient.DownloadAsync();

                    using (var memoryStream = new MemoryStream())
                    {
                        await response.Value.Content.CopyToAsync(memoryStream);
                        algorithm.DllFileBytes = memoryStream.ToArray();
                    }

                    return algorithm;
                }
                catch (Exception)
                {
                    return null!;
                }
            }
            return null!;
        }

        public async Task<IEnumerable<Algorithm?>> GetAllAlgorithms()
             => await context.Algorithms.Include(o => o.Parameters).ToListAsync();
    }
}
