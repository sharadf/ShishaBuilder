using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.DTOs.StatisticDtos;

namespace ShishaBuilder.Core.Repositories.StatisticRepository;

public interface IStatisticRepository
{
    Task<List<MasterStatisticDto>> GetMasterStatisticsAsync(DateTime? startDate, DateTime? endDate);
}
