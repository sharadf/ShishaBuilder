using ShishaBuilder.Core.DTOs.TobaccoDtos;
using ShishaBuilder.Core.Enums;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.DTOs.OrderDtos;

public class AllOrdersViewModelDto
{
    public int Id { get; set; }
    public Table Table { get; set; }
    public Hookah Hookah { get; set; }
    public Master Master { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public List<TobaccoShowInfoViewModelDto> Tobaccos { get; set; } = new();
    
}