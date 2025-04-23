
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ShishaBuilder.Core.Dtos;

public class CreateTobaccoViewModel
{
    public required string Name { get; set; }
    public string? Brand { get; set; }
    public string? Flavor { get; set; }
    public required string Strength { get; set; }


    
    public required IFormFile ImageFile { get; set; }
}
