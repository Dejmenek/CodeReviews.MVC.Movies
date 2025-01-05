using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Movies.Dejmenek.Services;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobService(BlobServiceClient blobServiceClient, IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _containerName = configuration["AzureStorage:ContainerName"];
        if (string.IsNullOrWhiteSpace(_containerName))
        {
            throw new InvalidOperationException("The Azure Storage container name is not configured. Please set 'AzureStorage:ContainerName' in the configuration.");
        }
        EnsureContainerCreated().GetAwaiter().GetResult();
    }

    private async Task EnsureContainerCreated()
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
    }

    public async Task DeleteAsync(string fileUri)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        var fileName = Path.GetFileNameWithoutExtension(new Uri(fileUri).LocalPath);
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        var fileId = Guid.NewGuid();
        BlobClient blobClient = containerClient.GetBlobClient(fileId.ToString());

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders { ContentType = file.ContentType }
        );

        return blobClient.Uri.ToString();
    }
}
