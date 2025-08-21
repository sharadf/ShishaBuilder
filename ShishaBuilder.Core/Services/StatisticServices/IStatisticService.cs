using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.DTOs.StatisticDtos;

namespace ShishaBuilder.Core.Services.StatisticServices;

public interface IStatisticService
{
    Task<List<MasterStatisticDto>> GetMasterStatisticsAsync(DateTime? startDate, DateTime? endDate);
}
