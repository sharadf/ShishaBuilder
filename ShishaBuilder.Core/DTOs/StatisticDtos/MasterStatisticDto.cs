using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaBuilder.Core.DTOs.StatisticDtos;

public class MasterStatisticDto
{
    public int MasterId { get; set; }
    public string? MasterName { get; set; }
    public string? PhotoUrl { get; set; }
    public int TotalOrders { get; set; }
}
