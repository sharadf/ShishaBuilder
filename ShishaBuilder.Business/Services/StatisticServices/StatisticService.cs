using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.DTOs.StatisticDtos;
using ShishaBuilder.Core.Repositories.StatisticRepository;
using ShishaBuilder.Core.Services.StatisticServices;

namespace ShishaBuilder.Business.Services.StatisticServices;

public class StatisticService : IStatisticService
{
    private IStatisticRepository statisticRepository;

    public StatisticService(IStatisticRepository statisticRepository)
    {
        this.statisticRepository = statisticRepository;
    }
    public async Task<List<MasterStatisticDto>> GetMasterStatisticsAsync(DateTime? startDate, DateTime? endDate)
    {
        return await statisticRepository.GetMasterStatisticsAsync(startDate,endDate);
    }
}
