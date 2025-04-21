using Microsoft.AspNetCore.Http;
public interface IBlobService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<List<string>> DownloadAllPhotos();
}