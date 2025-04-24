using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ShishaBuilder.Core.Dtos;
    
public class EditTobaccoViewModel
{
    public required string Name { get; set; }
    public string? Brand { get; set; }
    public string? Flavor { get; set; }
    public required string Strength { get; set; }

    public IFormFile? ImageFile { get; set; }
}
