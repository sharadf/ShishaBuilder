using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShishaBuilder.Core.DTOs.HookahDtos;
using ShishaBuilder.Core.DTOs.TableDtos;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Services.TableServices;

namespace ShishaBuilder.Web.Controllers;

[Authorize(Roles = "Admin")]
[Route("[controller]")]
public class TableController : Controller
{
    private readonly ITableService tableService;

    public TableController(ITableService tableService)
    {
        this.tableService = tableService;
    }

    [HttpGet("AllTables")]
    public async Task<ActionResult<IEnumerable<Table>>> AllTables()
    {
        var tables = await tableService.GetAllTablesAsync();
        return View(tables);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Table>> GetById(int id)
    {
        var table = await tableService.GetByIdTableAsync(id);
        if (table == null)
            return NotFound();

        return Ok(table);
    }

    [HttpGet("Create")]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    public async Task<ActionResult> Create([FromForm] CreateTable tableDto)
    {
        if (!ModelState.IsValid)
            return View(tableDto);

        var table = new Table { TableNumber = tableDto.TableNumber };

        await tableService.AddTableAsync(table);
        return RedirectToAction("AllTables");
    }

    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var table = await tableService.GetByIdTableAsync(id);
        if (table == null)
        {
            return NotFound();
        }

        var model = new EditTable { TableNumber = table.TableNumber, IsBusy = table.IsBusy };

        ViewBag.TableId = table.Id;
        return View(model);
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(int id, EditTable model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var table = await tableService.GetByIdTableAsync(id);
        if (table == null)
            return NotFound();

        table.TableNumber = model.TableNumber;
        table.IsBusy = model.IsBusy;

        await tableService.UpdateTableAsync(table);
        return RedirectToAction("AllTables");
    }

    [HttpGet("DeletedTables")]
    public async Task<IActionResult> DeletedHookahs()
    {
        var deleted = await tableService.GetAllDeletedTablesAsync();
        ViewBag.ShowDeleted = true;
        return View("Alltables", deleted);
    }

    [HttpPost("SoftDelete")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        await tableService.SoftDeleteTableAsync(id);
        return RedirectToAction("Alltables");
    }
}
