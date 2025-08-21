using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.DTOs.StatisticDtos;
using ShishaBuilder.Core.Repositories.StatisticRepository;

namespace ShishaBuilder.Business.Repositories.StatisticRepositories;

public class StatisticRepository : IStatisticRepository
{
    private readonly AppDbContext context;

    public StatisticRepository(AppDbContext context)
    {
        this.context = context;
    }

    // Фильтр по полуинтервалу [startUtc, endUtcExclusive)
    public async Task<List<MasterStatisticDto>> GetMasterStatisticsAsync(DateTime? startUtc, DateTime? endUtcExclusive)
    {
        var orders = context.Orders.AsNoTracking().AsQueryable();

        if (startUtc.HasValue)
            orders = orders.Where(o => o.CreatedAt >= startUtc.Value);

        if (endUtcExclusive.HasValue)
            orders = orders.Where(o => o.CreatedAt < endUtcExclusive.Value);

        // Базируемся на Masters -> LEFT JOIN к orders
        var result = await
            (from m in context.Masters.AsNoTracking().Where(m => m.IsActive)
             join u in context.Users.AsNoTracking() on m.AppUserId equals u.Id
             join o in orders on m.Id equals o.MasterId into og // LEFT JOIN (group)
             select new MasterStatisticDto
             {
                 MasterId = m.Id,
                 MasterName = u.FullName,
                 PhotoUrl = m.PhotoUrl,
                 TotalOrders = og.Count()   // 0, если заказов нет
             })
            .OrderByDescending(x => x.TotalOrders)
            .ThenBy(x => x.MasterName)
            .ToListAsync();

        return result;
    }
}
