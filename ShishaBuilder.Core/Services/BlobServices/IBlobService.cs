using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ShishaBuilder.Core.Services.BlobServices;

public interface IBlobService
{
    Task<string> UploadPhotoAsync(IFormFile file, string containerName);
    Task<List<string>> DownloadAllPhotos(string containerName);
    
}
