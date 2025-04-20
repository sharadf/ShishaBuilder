using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ShishaBuilder.Core.Services.BlobServices;

public interface IBlobService
{
    public Task<string> UploadPhotoAsync(IFormFile file);
    public Task<List<string>> DownloadAllPhotos();
    

}
