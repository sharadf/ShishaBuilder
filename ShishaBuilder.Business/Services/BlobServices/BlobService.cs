using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ShishaBuilder.Core.Services.BlobServices;
using ShishaBuilder.Core.Settings;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;

namespace ShishaBuilder.Business.Services.BlobServices;


public class BlobService : IBlobService
{
    private readonly BlobSettings blobSettings;
    private readonly StorageClient storageClient;

    public BlobService(IOptions<BlobSettings> blobSettings)
    {
        this.blobSettings = blobSettings.Value;

        var credential = GoogleCredential.GetApplicationDefault();
        storageClient = StorageClient.Create(credential);
    }

    public async Task<string> UploadPhotoAsync(IFormFile file, string folderName)
    {
        if (file == null)
            throw new ArgumentException("File cannot be null");

        string objectName = $"{folderName}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        using var stream = file.OpenReadStream();
        // Загружаем объект
        var uploadedObject = await storageClient.UploadObjectAsync(
            bucket: blobSettings.BucketName,
            objectName: objectName,
            contentType: file.ContentType,
            source: stream
        );

        // Возвращаем прямую ссылку на публичный файл
        return $"https://storage.googleapis.com/{blobSettings.BucketName}/{objectName}";
    }

    public async Task<List<string>> DownloadAllPhotos(string folderName)
    {
        var result = new List<string>();

        await foreach (
            var obj in storageClient.ListObjectsAsync(blobSettings.BucketName, folderName)
        )
        {
            result.Add($"https://storage.googleapis.com/{blobSettings.BucketName}/{obj.Name}");
        }

        return result;
    }
}
