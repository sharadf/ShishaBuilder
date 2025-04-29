using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShishaBuilder.Core.Dtos.MasterDtos;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Services.BlobServices;
using ShishaBuilder.Core.Services.MasterServices;
 
namespace ShishaBuilder.Web.Controllers;
 
[Authorize]
[Route("[controller]")]
public class MasterController : Controller
{
    private IMasterService masterService;
    private IBlobService blobService;
    private string containerName="masters";
 
    public MasterController(IMasterService masterService, IBlobService blobService)
    {
        this.masterService=masterService;
        this.blobService=blobService;
    }
 
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }
 
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateMasterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        string? imageUrl=null;
        if (model.ImageFile!=null && model.ImageFile.Length > 0)
        {
            imageUrl=await blobService.UploadPhotoAsync(model.ImageFile,containerName);
        }
        else
        {
            var defaultPhoto = "https://jamal771.blob.core.windows.net/masters/master%20default%20photo.jpg";
            imageUrl=defaultPhoto;
        }
 
        var master= new Master
        {
            FirstName=model.FirstName,
            LastName=model.LastName,
            PhotoUrl=imageUrl,
            PhoneNumber = model.PhoneNumber
        };
       
        await masterService.AddMasterAsync(master);
        return RedirectToAction("AllMasters");
 
    }
 
    [HttpGet("AllMasters")]
    public async Task<IActionResult> AllMasters()
    {
        var masters= await masterService.GetAllMastersAsync();
        return View(masters);
    }
 
    [HttpGet ("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var master = await masterService.GetMasterByIdAsync(id);
        if (master == null)
        {
            return NotFound();
        }
        var model = new CreateMasterViewModel
        {
            FirstName=master.FirstName,
            LastName=master.LastName,
            PhoneNumber=master.PhoneNumber
        };
 
        ViewBag.MasterId=master.Id;
        ViewBag.ExistingImage=master.PhotoUrl;
 
        return View(model);
    }
 
    [HttpPost("Edit")]
    public  async Task<IActionResult> Edit(int id ,CreateMasterViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
       
        var master=await masterService.GetMasterByIdAsync(id);
        if (master==null || master.IsActive==false)
            return NotFound();
       
        master.FirstName=model.FirstName;
        master.LastName=model.LastName;
        master.PhoneNumber=model.PhoneNumber;
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var imageUrl = await blobService.UploadPhotoAsync(model.ImageFile,containerName);
            master.PhotoUrl = imageUrl;
        }    
        await masterService.UpdateMasterAsync(master);
 
        return RedirectToAction("AllMasters");
    }
 
    [HttpGet("DeletedMasters")]
    public async Task <IActionResult> DeletedMasters()
    {
        var deletedMasters=await masterService.GetAllDeletedMastersAsync();
        ViewBag.ShowDeleted=true;    
       
        return View("AllMasters",deletedMasters);
    }
   
    [HttpPost("SoftDelete")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        await masterService.SoftDeleteMasterAsync(id);
        return RedirectToAction("AllMasters");
    }

}
 