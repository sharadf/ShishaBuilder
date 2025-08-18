using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShishaBuilder.Core.Services.StatisticServices;

namespace ShishaBuilder.Web.Controllers;

[Route("[controller]")]
public class StatisticController : Controller
{
    private IStatisticService statisticService;

    public StatisticController(IStatisticService statisticService)
    {
        this.statisticService = statisticService;
    }

    [HttpGet("AllStatistics")]
    public async Task<IActionResult> AllStatistics(DateTime? startDate, DateTime? endDate)
    {
        DateTime? startUtc = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Utc)
            : (DateTime?)null;

        DateTime? endUtcExclusive = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value.Date.AddDays(1), DateTimeKind.Utc)
            : (DateTime?)null;


       var statistics = await statisticService.GetMasterStatisticsAsync(startUtc, endUtcExclusive);
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        return View(statistics);
    }

   

 

}