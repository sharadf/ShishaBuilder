using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.DTOs;
using ShishaBuilder.Core.DTOs.OrderDtos;
using ShishaBuilder.Core.DTOs.TobaccoDtos;
using ShishaBuilder.Core.Enums;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Services.BlobServices;
using ShishaBuilder.Core.Services.MasterServices;
using ShishaBuilder.Core.Services.OrderServices;
using ShishaBuilder.Core.Services.TableServices;
using ShishaBuilder.Core.Services.TobaccoServices;

namespace ShishaBuilder.Web.Controllers;

[Route("[controller]")]
public class MasterController : Controller
{
    private IMasterService masterService;
    private IBlobService blobService;
    private readonly IOrderService orderService;
    private string containerName = "masters";

    private readonly AppDbContext context;
    private readonly ITableService tableService;
    private readonly IHookahService hookahService;
    private readonly ITobaccoService tobaccoService;
    private readonly UserManager<AppUser> userManager;

    public MasterController(
        IMasterService masterService,
        IBlobService blobService,
        IOrderService orderService,
        ITableService tableService,
        ITobaccoService tobaccoService,
        IHookahService hookahService,
        UserManager<AppUser> userManager,
        AppDbContext context
    )
    {
        this.masterService = masterService;
        this.blobService = blobService;
        this.orderService = orderService;
        this.userManager = userManager;
        this.context = context;
        this.tableService = tableService;
        this.hookahService = hookahService;
        this.tobaccoService = tobaccoService;
    }

    [Authorize(Roles = "Master")]
    [HttpGet("MasterPanel")]
    public IActionResult MasterPanel()
    {
        return View();
    }

    [HttpGet("Create")]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet("AllMasters")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AllMasters()
    {
        var masters = await masterService.GetAllMastersAsync();
        return View(masters);
    }

    [HttpGet("Edit")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var master = await masterService.GetMasterByIdAsync(id);
        if (master == null || master.IsActive == false)
            return NotFound();

        var appUser = await userManager.FindByIdAsync(master.AppUserId);
        if (appUser == null)
            return NotFound();

        var model = new EditMasterViewModel
        {
            MasterId = master.Id,
            AppUserId = appUser.Id,
            FullName = appUser.FullName!,
            PhoneNumber = appUser.PhoneNumber!,
            Age = appUser.Age ?? 0,
            Gender = appUser.Gender!,
            ExperienceYears = appUser.ExperienceYears ?? 0,
            ExistingImageUrl = master.PhotoUrl,
        };

        return View(model);
    }

    [HttpPost("Edit")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(EditMasterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var master = await masterService.GetMasterByIdAsync(model.MasterId);
        if (master == null || master.IsActive == false)
            return NotFound();

        var user = await userManager.FindByIdAsync(model.AppUserId);
        if (user == null)
            return NotFound();

        // Обновление AppUser
        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;
        user.Age = model.Age;
        user.Gender = model.Gender;
        user.ExperienceYears = model.ExperienceYears;

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            ModelState.AddModelError("", "Ошибка при обновлении данных пользователя");
            return View(model);
        }

        // Обновление фото
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var imageUrl = await blobService.UploadPhotoAsync(model.ImageFile, containerName);
            master.PhotoUrl = imageUrl;
        }

        await masterService.UpdateMasterAsync(master);

        return RedirectToAction("AllMasters");
    }

    [HttpGet("DeletedMasters")]
    public async Task<IActionResult> DeletedMasters()
    {
        var deletedMasters = await masterService.GetAllDeletedMastersAsync();
        ViewBag.ShowDeleted = true;

        return View("AllMasters", deletedMasters);
    }

    [HttpPost("SoftDelete")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        await masterService.SoftDeleteMasterAsync(id);
        return RedirectToAction("AllMasters");
    }

    [Authorize(Roles = "Master")]
    [HttpGet("IncomingOrders")]
    public async Task<IActionResult> IncomingOrders()
    {
        var orders = await orderService.GetAllOrdersAsync();
        orders = orders.Where(o => o.OrderStatus == Core.Enums.OrderStatus.Pending).ToList();
        var result = new List<AllOrdersViewModelDto>();
        foreach (var order in orders)
        {
            var table = await tableService.GetByIdTableAsync(order.TableId);
            Master? master =
                order.MasterId != 0 ? await masterService.GetMasterByIdAsync(order.MasterId) : null;
            var hookah = await hookahService.GetByIdHookahAsync(order.HookahId);

            var tobaccos = new List<TobaccoShowInfoViewModelDto>();
            foreach (var ot in order.OrderTobaccos)
            {
                var tobacco = await tobaccoService.GetTobaccoByIdAsync(ot.TobaccoId);
                tobaccos.Add(
                    new TobaccoShowInfoViewModelDto
                    {
                        Name = tobacco.Name,
                        Brand = tobacco.Brand,
                        Percentage = ot.Percentage,
                    }
                );
            }

            result.Add(
                new AllOrdersViewModelDto
                {
                    Id = order.Id,
                    Table = table,
                    Master = master,
                    Hookah = hookah,
                    CreatedAt = order.CreatedAt,
                    Tobaccos = tobaccos,
                }
            );
        }
        return View(result);
    }

    private async Task<int> GetCurrentMasterIdAsync()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdStr))
            throw new Exception("User is not authenticated");

        var master = await masterService.GetMasterByAppUserIdAsync(userIdStr);
        if (master == null)
            throw new Exception("Master not found");

        return master.Id;
    }

    [Authorize(Roles = "Master")]
    [HttpPost("TakeOrder")]
    public async Task<IActionResult> TakeOrder(int orderId)
    {
        int masterId = await GetCurrentMasterIdAsync();
        if(masterId == null)
        {
            throw new Exception("masterId null");
            
        }
        await orderService.AssignOrderToMasterAsync(orderId, masterId);

        return RedirectToAction("CurrentOrders");
    }
    [Authorize(Roles = "Master")]
    [HttpGet("CurrentOrders")]
    public async Task<IActionResult> CurrentOrders()
    {
        int masterId = await GetCurrentMasterIdAsync();

        var orders = await orderService.GetOrdersByMasterAsync(masterId);

        return View(orders);
    }
    [HttpPost("CompleteOrder")]
    [Authorize(Roles = "Master")]
    public async Task<IActionResult> CompleteOrder(int orderId)
    {
        int masterId = await GetCurrentMasterIdAsync();

        var success = await orderService.UpdateOrderStatusAsync(orderId, masterId, OrderStatus.Completed);
        if (!success)
            return BadRequest("Could not complete the order");

        return RedirectToAction("CurrentOrders");
    }

    [HttpPost("CancelOrder")]
    [Authorize(Roles = "Master")]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        int masterId = await GetCurrentMasterIdAsync();

        var success = await orderService.UpdateOrderStatusAsync(orderId, masterId, OrderStatus.Cancelled);
        if (!success)
            return BadRequest("Could not cancel the order");

        return RedirectToAction("CurrentOrders");
    }
}
