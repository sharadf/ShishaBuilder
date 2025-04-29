using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.DTOs.OrderDtos;

public class OrderPreviewViewModelDto
{
    public required Hookah Hookah { get; set; }
    public required Table Table { get; set; }
    public List<TobaccoPercentageDto> TobaccoMix { get; set; } = new List<TobaccoPercentageDto>();
    public Dictionary<int, int> TobaccoPercentages { get; set; } = new();
}
