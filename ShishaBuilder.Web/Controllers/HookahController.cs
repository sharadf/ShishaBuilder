using Microsoft.AspNetCore.Mvc;
using ShishaBuilder.Core.DTOs;
using ShishaBuilder.Core.Models;
using System.Net;

namespace ShishaBuilder.Web.Controllers
{

    [Route("[controller]")]

    public class HookahController : Controller
    {
        private readonly IHookahService hookahService;
        private readonly IBlobService blobService;

        public HookahController(IHookahService hookahService, IBlobService blobService)
        {
            this.hookahService = hookahService;
            this.blobService = blobService;
        }

        [HttpGet("AllHookahs")]
        public async Task<ActionResult<IEnumerable<Hookah>>> AllHookahs()
        {
            var hookahs = await hookahService.GetAllHookahsAsync();
            return View(hookahs);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Hookah>> GetById(int id)
        {
            var hookah = await hookahService.GetByIdHookahAsync(id);
            if (hookah == null)
                return NotFound();

            return Ok(hookah);
        }

        [HttpGet("Create")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromForm] CreateHookah hookahDto)
        {
            string imageUrl = string.Empty;

            if (hookahDto.ImageFile != null)
                imageUrl = await blobService.UploadFileAsync(hookahDto.ImageFile);

            var hookah = new Hookah
            {
                ModelName = hookahDto.ModelName,
                CompanyName = hookahDto.CompanyName,
                Image = imageUrl
            };

            await hookahService.AddHookahAsync(hookah);
            return RedirectToAction("AllHookahs");
        }


        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var hookah = await hookahService.GetByIdHookahAsync(id);
            if (hookah == null || hookah.IsDeleted)
            {
                return NotFound();
            }

            var model = new EditHookah
            {
                ModelName = hookah.ModelName,
                CompanyName = hookah.CompanyName
            };

            ViewBag.HookahId = hookah.Id;
            ViewBag.ExistingImage = hookah.Image;
            return View(model);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(int id, EditHookah model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var hookah = await hookahService.GetByIdHookahAsync(id);
            if (hookah == null || hookah.IsDeleted)
                return NotFound();

            hookah.ModelName = model.ModelName;
            hookah.CompanyName = model.CompanyName;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var imageUrl = await blobService.UploadFileAsync(model.ImageFile);
                hookah.Image = imageUrl;
            }

            await hookahService.UpdateHookahAsync(hookah);

            return RedirectToAction("AllHookahs");
        }


        [HttpGet("DeletedHookahs")]
        public async Task<IActionResult> DeletedHookahs()
        {
            var deleted = await hookahService.GetAllDeletedHookahsAsync();
            ViewBag.ShowDeleted = true;
            return View("AllHookahs", deleted);
        }

        [HttpPost("SoftDelete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await hookahService.SoftDeleteHookahAsync(id);
            return RedirectToAction("AllHookahs");
        }
    }
}

