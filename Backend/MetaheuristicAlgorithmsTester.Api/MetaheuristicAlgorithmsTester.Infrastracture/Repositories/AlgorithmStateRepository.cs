using Azure.Storage.Blobs;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    public class AlgorithmStateRepository(IExecutedAlgorithmsRepository executedAlgorithmsRepository, BlobServiceClient blobServiceClient, IConfiguration configuration) : IAlgorithmStateRepository
    {
        public async Task<string> GetState(int executedId)
        {
            var executed = await executedAlgorithmsRepository.GetExecutedAlgorithmById(executedId);
            if (executed != null)
            {
                var fileName = executed.AlgorithmStateFileName;

                var containerName = configuration.GetSection("Storage:StorageNameAlgorithmsStates").Value;
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName + ".txt");

                try
                {
                    var response = await blobClient.DownloadAsync();

                    using (var streamReader = new StreamReader(response.Value.Content))
                    {
                        string content = await streamReader.ReadToEndAsync();
                        return content;
                    }
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public async Task SaveState(string state, int executedId, string fileName)
        {
            try
            {
                byte[] stateBytes = Encoding.UTF8.GetBytes(state);

                var containerName = configuration.GetSection("Storage:StorageNameAlgorithmsStates").Value;
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName + ".txt");

                using (MemoryStream stream = new MemoryStream(stateBytes))
                {
                    await blobClient.UploadAsync(stream, true);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
