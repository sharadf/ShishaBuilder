using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShishaBuilder.Business.Services.HookahServices;
using ShishaBuilder.Core.DTOs.OrderDtos;
using ShishaBuilder.Core.Enums;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Services.MasterServices;
using ShishaBuilder.Core.Services.OrderServices;
using ShishaBuilder.Core.Services.TableServices;
using ShishaBuilder.Core.Services.TobaccoServices;

namespace ShishaBuilder.Web.Controllers;

[Route("[controller]")]
public class OrderController : Controller
{
    private IHookahService hookahService;
    private ITobaccoService tobaccoService;
    private ITableService tableService;
    private IOrderService orderService;

    public OrderController(IHookahService hookahService,ITobaccoService tobaccoService,ITableService tableService,IOrderService orderService)
    {
        this.hookahService=hookahService;

        this.tobaccoService=tobaccoService;

        this.tableService=tableService;
        this.orderService=orderService;
    }


    [HttpGet("SelectHookah")]
    public async Task<ActionResult> SelectHookah()
    {
        var hookahs = await hookahService.GetAllHookahsAsync();
        return View(hookahs);
    }


    [HttpGet("SelectTobacco")]
    public async Task<ActionResult> SelectTobacco(int hookahId)
    {
        var tobaccos = await tobaccoService.GetAllTobaccosAsync();
        ViewData["HookahId"]=hookahId;
        return View(tobaccos);
    }

    [HttpGet("PreviewOrder")]
    public async Task<IActionResult> PreviewOrder(
        int hookahId,
        int tableId,
        [FromQuery] Dictionary<int, int> tobaccoPercentages
    )
    {
        var validPercentages= tobaccoPercentages
            .Where(x=>x.Value>0)
            .ToDictionary(x=>x.Key,x=>x.Value);

        if (validPercentages.Sum(x => x.Value) != 100)
        {
            return BadRequest("Total percentage must be exactly 100%");
        }

        var hookah=await hookahService.GetByIdHookahAsync(hookahId);
        var selectedTobaccos=new List<(Tobacco tobacco,int Percentage)>();
        
        foreach (var (tobaccoId, percentage) in validPercentages)
        {
            var tobacco = await tobaccoService.GetTobaccoByIdAsync(tobaccoId);
            selectedTobaccos.Add((tobacco, percentage));
        }
        var table = await tableService.GetByIdTableAsync(tableId);
        return View(model: new OrderPreviewViewModelDto
        {
            Hookah = hookah,
            Table = table,
            SelectedTobaccos = selectedTobaccos,
            TobaccoPercentages = validPercentages 
        });
    }

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder(OrderPreviewViewModelDto model)
    {
        // Проверка суммы процентов
        if (model.TobaccoPercentages.Sum(x => x.Value) != 100)
        {
            return BadRequest("Total percentage must be exactly 100%");
        }

        // Создаем список OrderTobacco
        var orderTobaccos = model.SelectedTobaccos
            .Select(t => new OrderTobacco
            {
                TobaccoId = t.Tobacco.Id,
                Percentage = t.Percentage
            })
            .ToList();

        // Создаем заказ
        var order = new Order
        {
            HookahId = model.Hookah.Id,
            TableId = model.Table.Id,
            CreatedAt = DateTime.UtcNow,
            OrderStatus = OrderStatus.Pending,
            OrderTobaccos = orderTobaccos
        };

        // Сохраняем заказ
        try
        {
            var result = await orderService.AddOrderAsync(order);
            return RedirectToAction("OrderSuccess", new { orderId = result.Id });
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            return StatusCode(500, "Internal server error");
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
        var all= await orderService.GetAllOrdersAsync();
        return View(all);
    }

    [HttpGet("OrderDetails/{orderId}")]
    public async Task<IActionResult> OrderDetails(int orderId)
    {
        
       var order=await orderService.GetOrderByIdAsync(orderId);
       return View(order);

    }
    
}
