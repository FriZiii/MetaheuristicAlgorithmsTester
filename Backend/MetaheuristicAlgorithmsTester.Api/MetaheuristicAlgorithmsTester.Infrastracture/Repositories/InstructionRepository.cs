using Azure.Storage.Blobs;
using MetaheuristicAlgorithmsTester.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MetaheuristicAlgorithmsTester.Infrastracture.Repositories
{
    public class InstructionRepository(BlobServiceClient blobServiceClient, IConfiguration configuration) : IInstructionRepository
    {
        public async Task<byte[]> GetDll()
        {
            var containerName = configuration.GetSection("Storage:StorageNameInstruction").Value;
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient("AlgorithmInterfaces.dll");
            try
            {
                var response = await blobClient.DownloadAsync();

                using (var memoryStream = new MemoryStream())
                {
                    await response.Value.Content.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<byte[]> GetInstruction()
        {
            var containerName = configuration.GetSection("Storage:StorageNameInstruction").Value;
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient("Instruction.pdf");
            try
            {
                var response = await blobClient.DownloadAsync();

                using (var memoryStream = new MemoryStream())
                {
                    await response.Value.Content.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
