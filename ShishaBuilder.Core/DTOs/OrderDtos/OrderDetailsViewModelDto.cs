
using ShishaBuilder.Core.DTOs.TobaccoDtos;
using ShishaBuilder.Core.Enums;
using ShishaBuilder.Core.Helpers;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.DTOs.OrderDtos;

public class OrderDetailsViewModelDto
{
    public int OrderId { get; set; }

    public ICollection<TobaccoShowInfoViewModelDto> OrderTobaccos { get; set; } = new List<TobaccoShowInfoViewModelDto>();
    public required Hookah Hookah { get; set; }
    public required Table Table { get; set; }
    public Master Master { get; set; }
    public DateTime CreatedAt { get; set; } =DateTimeHelper.GetBakuTime() ;
    public OrderStatus OrderStatus { get; set; }
}
