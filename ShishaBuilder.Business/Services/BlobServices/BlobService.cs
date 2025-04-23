using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShishaBuilder.Core.Services.BlobServices;
using ShishaBuilder.Core.Settings;
namespace ShishaBuilder.Business.Services.BlobServices;

public class BlobService : IBlobService
{
    private readonly BlobSettings blobSettings;

    public BlobService(IOptions<BlobSettings> blobSettings)
    {
        this.blobSettings = blobSettings.Value;
    }

    public async Task<string> UploadPhotoAsync(IFormFile file, string containerName)
    {
        if (file == null)
            throw new ArgumentException("File cannot be null");

        var blobContainerClient = new BlobContainerClient(blobSettings.ConnectionString, containerName);
        await blobContainerClient.CreateIfNotExistsAsync();

        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
        }

        return blobClient.Uri.ToString();
    }

    public async Task<List<string>> DownloadAllPhotos(string containerName)
    {
        var blobContainerClient = new BlobContainerClient(blobSettings.ConnectionString, containerName);
        var result = new List<string>();

        await foreach (var blob in blobContainerClient.GetBlobsAsync())
        {
            var uri = blobContainerClient.GetBlobClient(blob.Name).Uri.ToString();
            result.Add(uri);
        }

        return result;
    }
}
