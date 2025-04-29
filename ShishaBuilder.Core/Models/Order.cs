using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.DTOs.OrderDtos;
using ShishaBuilder.Core.Enums;

namespace ShishaBuilder.Core.Models;

public class Order
{
    public int Id { get; set; }

    public ICollection<OrderTobacco> OrderTobaccos { get; set; } = new List<OrderTobacco>();
    public required int HookahId { get; set; }
    public required int TableId { get; set; }
    public int MasterId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public OrderStatus OrderStatus { get; set; }

}
