using Azure.Storage.Blobs;
using MetaheuristicAlgorithmsTester.Domain.Entities;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using MetaheuristicAlgorithmsTester.Infrastracture.Persistence;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    internal class AlgorithmsRepository : IAlgorithmsRepository
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly IConfiguration configuration;
        private readonly Context context;

        public AlgorithmsRepository(BlobServiceClient blobServiceClient, IConfiguration configuration, Context context)
        {
            this.blobServiceClient = blobServiceClient;
            this.configuration = configuration;
            this.context = context;
        }

        public async Task<Algorithm?> AddAlgorithm(Algorithm algorithm)
        {
            if (algorithm.DllFile != null)
            {
                var containerName = configuration.GetSection("Storage:StorageName").Value;

                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(algorithm.DllFile.FileName);

                using var stream = algorithm.DllFile.OpenReadStream();
                blobClient.Upload(stream, true);

                algorithm.FileName = algorithm.DllFile.FileName;
                try
                {
                    context.Algorithms.Add(algorithm);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
            }

            return algorithm;
        }

        public async Task<Algorithm?> GetAlgorithm(int id)
        {
            var algorithm = context.Algorithms.FirstOrDefault(a => a.Id == id);
            if (algorithm != null)
            {
                var containerName = configuration.GetSection("Storage:StorageName").Value;
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(algorithm.FileName);

                try
                {
                    var response = await blobClient.DownloadAsync();

                    using (var streamReader = new StreamReader(response.Value.Content))
                    {
                        var content = await streamReader.ReadToEndAsync();
                        algorithm.DllFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(content)), 0, content.Length, null, blobClient.Uri.ToString());
                    }
                    return algorithm;
                }
                catch (Exception ex)
                {
                    return null!;
                }
            }
            return null!;
        }
    }
}
