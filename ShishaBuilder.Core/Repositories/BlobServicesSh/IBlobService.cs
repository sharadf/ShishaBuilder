using Microsoft.AspNetCore.Http;
public interface IBlobService
{
    Task<string> UploadPhotoAsync(IFormFile file, string containerName);
    Task<List<string>> DownloadAllPhotos(string containerName);
}