using Microsoft.AspNetCore.Http;

namespace ShishaBuilder.Core.DTOs.HookahDtos;
public class EditHookah
{
    public required string ModelName { get; set; }
    public required string CompanyName { get; set; }
    public IFormFile? ImageFile { get; set; }
}