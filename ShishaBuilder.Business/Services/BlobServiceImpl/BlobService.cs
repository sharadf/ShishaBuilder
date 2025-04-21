using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

public class BlobService : IBlobService
{
    private readonly string connectionString;
    private readonly string containerName;

    public BlobService(IConfiguration configuration)
    {
        this.connectionString = configuration.GetConnectionString("DefaultConnectionForBlob");

        this.containerName = configuration["AzureBlob:ContainerName"];
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        await containerClient.CreateIfNotExistsAsync();

        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var blobClient = containerClient.GetBlobClient(fileName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        return blobClient.Uri.ToString();
    }

    public async Task<List<string>> DownloadAllPhotos()
    {
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        var result = new List<string>();
        await foreach (var blob in containerClient.GetBlobsAsync())
        {
            var blobClient = containerClient.GetBlobClient(blob.Name);
            result.Add(blobClient.Uri.ToString());
        }

        return result;
    }
}