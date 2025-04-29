using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.DTOs.OrderDtos;

public class TobaccoPercentageDto
{
    public int TobaccoId { get; set; }
    public string TobaccoName { get; set; }
    public string Brand { get; set; }
    public int Percentage { get; set; }
}