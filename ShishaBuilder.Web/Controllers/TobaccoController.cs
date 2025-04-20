using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShishaBuilder.Business.Services.BlobServices;
using ShishaBuilder.Core.Dtos;
using ShishaBuilder.Core.Enums;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Services.BlobServices;
using ShishaBuilder.Core.Services.TobaccoServices;

namespace ShishaBuilder.Web.Controllers;

[Route("[controller]")]
public class TobaccoController : Controller
{
    private ITobaccoService tobaccoService;
    private IBlobService blobService;

    public TobaccoController(ITobaccoService tobaccoService,IBlobService blobService)
    {
        this.tobaccoService=tobaccoService;
        this.blobService=blobService;
    }

    [HttpGet("Create")]
    public ActionResult Create()
    {
        ViewBag.StrengthLevels = GetStrengthLevels();
        return View();
    }
    
    [HttpPost("Create")]
    public async Task<ActionResult> Create(CreateTobaccoViewModel model)
    {
        string? imageUrl= null;
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            imageUrl = await blobService.UploadPhotoAsync(model.ImageFile); 
        }
        var tobacco=new Tobacco
        {
            Name=model.Name!,
            Brand=model.Brand,
            Flavor=model.Flavor,
            Strength=model.Strength,
            ImageUrl=imageUrl
        };

        await tobaccoService.AddTobaccoAsync(tobacco);
        return RedirectToAction("AllTobaccos");
    }

    private List<string> GetStrengthLevels()
    {
        return Enum.GetNames(typeof(TobaccoStrength)).ToList();
    }

    [HttpGet("AllTobaccos")]
    public async Task<ActionResult> AllTobaccos()
    {
        var tobaccos=await tobaccoService.GetAllTobaccosAsync();
        return View(tobaccos);
    }

    [HttpGet ("DeletedTobaccos")]
    public async Task<IActionResult> DeletedTobaccos()
    {
        var deleted = await tobaccoService.GetAllDeletedTobaccosAsync();
        ViewBag.ShowDeleted = true;
        return View("AllTobaccos", deleted); 
    }

    [HttpGet ("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var tobacco = await tobaccoService.GetTobaccoByIdAsync(id);
        if (tobacco == null || tobacco.IsDeleted)
        {
            return NotFound();
        }

        var model = new EditTobaccoViewModel
        {
            Name = tobacco.Name,
            Brand = tobacco.Brand,
            Flavor = tobacco.Flavor,
            Strength = tobacco.Strength
        };

        ViewBag.TobaccoId = tobacco.Id;
        ViewBag.ExistingImage = tobacco.ImageUrl;
        ViewBag.StrengthLevels = GetStrengthLevels();

        return View(model);
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(int id, EditTobaccoViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var tobacco = await tobaccoService.GetTobaccoByIdAsync(id);
        if (tobacco == null || tobacco.IsDeleted)
            return NotFound();

        
        tobacco.Name = model.Name;
        tobacco.Brand = model.Brand;
        tobacco.Flavor = model.Flavor;
        tobacco.Strength = model.Strength;

        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var imageUrl = await blobService.UploadPhotoAsync(model.ImageFile);
            tobacco.ImageUrl = imageUrl;
        }

        await tobaccoService.UpdateTobaccoAsync(tobacco);

        return RedirectToAction("AllTobaccos");
    }


    public async Task<IActionResult> SoftDelete(int id)
    {
        await tobaccoService.SoftDeleteTobaccoAsync(id);
        return RedirectToAction("AllTobaccos");
    }

}
