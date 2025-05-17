using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.DTOs;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Services.BlobServices;
using ShishaBuilder.Core.Services.MasterServices;

namespace ShishaBuilder.Web.Controllers;

[Route("[controller]")]
public class MasterController : Controller
{
    private IMasterService masterService;
    private IBlobService blobService;
    private string containerName = "masters";

    private readonly AppDbContext context;

    private readonly UserManager<AppUser> userManager;

    public MasterController(
        IMasterService masterService,
        IBlobService blobService,
        UserManager<AppUser> userManager,
        AppDbContext context
    )
    {
        this.masterService = masterService;
        this.blobService = blobService;
        this.userManager = userManager;
        this.context = context;
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
}
