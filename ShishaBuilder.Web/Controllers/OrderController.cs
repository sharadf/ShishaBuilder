using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ShishaBuilder.Core.DTOs.OrderDtos;
using ShishaBuilder.Core.DTOs.TobaccoDtos;
using ShishaBuilder.Core.Enums;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Services.MasterServices;
using ShishaBuilder.Core.Services.OrderServices;
using ShishaBuilder.Core.Services.TableServices;
using ShishaBuilder.Core.Services.TobaccoServices;

namespace ShishaBuilder.Web.Controllers;

[Authorize]
[Route("[controller]")]
public class OrderController : Controller
{
    private readonly IHookahService hookahService;
    private readonly ITobaccoService tobaccoService;
    private readonly ITableService tableService;
    private readonly IOrderService orderService;
    private readonly IMasterService masterService;

    public OrderController(
        IHookahService hookahService,
        ITobaccoService tobaccoService,
        ITableService tableService,
        IOrderService orderService,
        IMasterService masterService)
    {
        this.hookahService = hookahService;
        this.tobaccoService = tobaccoService;
        this.tableService = tableService;
        this.orderService = orderService;
        this.masterService = masterService;
    }

    [HttpGet("SelectHookah")]
    public async Task<IActionResult> SelectHookah()
    {
        var hookahs = await hookahService.GetAllHookahsAsync();
        return View(hookahs);
    }

    [HttpGet("SelectTobacco")]
    public async Task<IActionResult> SelectTobacco(int hookahId)
    {
        var tobaccos = await tobaccoService.GetAllTobaccosAsync();
        ViewData["HookahId"] = hookahId;
        return View(tobaccos);
    }

    [HttpGet("PreviewOrder")]
    public async Task<IActionResult> PreviewOrder(
        int hookahId,
        int tableNumber,
        [FromQuery] Dictionary<int, int> tobaccoPercentages)
    {
        var validPercentages = tobaccoPercentages
            .Where(x => x.Value > 0)
            .ToDictionary(x => x.Key, x => x.Value);

        if (validPercentages.Sum(x => x.Value) != 100)
        {
            return BadRequest("Total percentage must be exactly 100%");
        }

        var hookah = await hookahService.GetByIdHookahAsync(hookahId);
        var table = await tableService.GetByTableNumber(tableNumber);

        var tobaccoMix = new List<TobaccoPercentageDto>();
        foreach (var (tobaccoId, percentage) in validPercentages)
        {
            var tobacco = await tobaccoService.GetTobaccoByIdAsync(tobaccoId);
            tobaccoMix.Add(new TobaccoPercentageDto
            {
                TobaccoId = tobaccoId,
                TobaccoName = tobacco.Name,
                Brand = tobacco.Brand,
                Percentage = percentage
            });
        }

        return View(new OrderPreviewViewModelDto
        {
            Hookah = hookah,
            Table = table,
            TobaccoMix = tobaccoMix,
            TobaccoPercentages = validPercentages
        });
    }

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder(OrderPreviewViewModelDto model)
    {
        if (model.TobaccoPercentages.Sum(x => x.Value) != 100)
        {
            return BadRequest("Total percentage must be exactly 100%");
        }

        var order = new Order
        {
            HookahId = model.Hookah.Id,
            TableId = model.Table.Id,
            CreatedAt = DateTime.UtcNow,
            OrderStatus = OrderStatus.Pending
        };

        foreach (var (tobaccoId, percentage) in model.TobaccoPercentages)
        {
            order.OrderTobaccos.Add(new OrderTobacco
            {
                TobaccoId = tobaccoId,
                Percentage = percentage
            });
        }

        try
        {
            await orderService.AddOrderAsync(order);
            return RedirectToAction("OrderSuccess", new { orderId = order.Id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error creating order: {ex.Message}");
        }
    }

    [HttpGet("OrderSuccess")]
    public IActionResult OrderSuccess()
    {
        return View();
    }

    [HttpGet("AllOrders")]
    public async Task<IActionResult> AllOrders()
    {
        var orders = await orderService.GetAllOrdersAsync();
        var result = new List<AllOrdersViewModelDto>();

        foreach (var order in orders)
        {
            var table = await tableService.GetByIdTableAsync(order.TableId);
            Master? master = order.MasterId != 0
                ? await masterService.GetMasterByIdAsync(order.MasterId)
                : null;
            var hookah = await hookahService.GetByIdHookahAsync(order.HookahId);

            var tobaccos = new List<TobaccoShowInfoViewModelDto>();
            foreach (var ot in order.OrderTobaccos)
            {
                var tobacco = await tobaccoService.GetTobaccoByIdAsync(ot.TobaccoId);
                tobaccos.Add(new TobaccoShowInfoViewModelDto
                {
                    Name = tobacco.Name,
                    Brand = tobacco.Brand,
                    Percentage = ot.Percentage
                });
            }

            result.Add(new AllOrdersViewModelDto
            {
                Id = order.Id,
                Table = table,
                Master = master,
                Hookah = hookah,
                CreatedAt = order.CreatedAt,
                Tobaccos = tobaccos
            });
        }

        return View(result);
    }

    [HttpGet("OrderDetails/{orderId}")]
    public async Task<IActionResult> OrderDetails(int orderId)
    {
        var order = await orderService.GetOrderByIdAsync(orderId);
        return View(order);
    }
}
