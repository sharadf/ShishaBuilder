using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaBuilder.Core.DTOs.TobaccoDtos;

public class TobaccoShowInfoViewModelDto
{
    public string Name { get; set; } = null!;
    public string Brand { get; set; } = null!;
    public int Percentage { get; set; }
}
