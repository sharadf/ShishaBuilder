

using Microsoft.AspNetCore.Http;

namespace ShishaBuilder.Core.DTOs.HookahDtos;

public class CreateHookah
{
    public string? ModelName { get; set; }
    public string? CompanyName { get; set; }
    public required IFormFile ImageFile { get; set; }
}