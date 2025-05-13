using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using ShishaBuilder.Core.DTOs.LoginDtos;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Services;

public class AccountController : Controller
{
    // private readonly IAccountService _accountService;
    private readonly SignInManager<AppUser> signInManager;
    private readonly UserManager<AppUser> userManager;

    public AccountController(
        // IAccountService accountService,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager
    )
    {
        // _accountService = accountService;
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    [HttpGet("RegisterAdmin")]
    public IActionResult RegisterAdmin() => View();

    [HttpPost("RegisterAdmin")]
    public async Task<IActionResult> RegisterAdmin([FromForm] AdminRegistrationDto newUser)
    {
        try
        {
            var user = new AppUser
            {
                Email = newUser.Login,
                PhoneNumber = newUser.PhoneNumber,
                UserName = newUser.Login,
            };

            var createResult = await this.userManager.CreateAsync(user, newUser.Password!);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new Exception("User creation failed: " + errors);
            }

            var roleResult = await this.userManager.AddToRoleAsync(user, nameof(Roles.Admin));

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception("Role assignment failed: " + errors);
            }
        }
        catch (Exception ex)
        {
            // Временно просто возвращай текст ошибки — для отладки
            return BadRequest(ex.Message);
        }

        return RedirectToAction(nameof(Login));
    }

    [HttpGet("RegisterMaster")]
    public IActionResult RegisterMaster() => View();

    [HttpPost("RegisterMaster")]
    public async Task<IActionResult> RegisterMaster([FromForm] MasterRegistrationDto dto)
    {
        // var (succeeded, errors) = await _accountService.RegisterMasterAsync(dto);

        // if (!succeeded)
        // {
        //     foreach (var error in errors)
        //         ModelState.AddModelError("", error);

        //     return View(dto);
        // }

        return RedirectToAction(nameof(Login));
    }

    [HttpGet("Login")]
    public ActionResult Login()
    {
        var errorMessage = base.TempData["Error"];

        if (errorMessage != null)
        {
            base.ModelState.AddModelError("All", errorMessage.ToString()!);
        }

        return base.View();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromForm] LoginDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.Login);

        if (user == null)
        {
            base.TempData["Error"] = "Incorrect login or password";
            return base.RedirectToAction(actionName: nameof(Login));
        }

        var result = await signInManager.PasswordSignInAsync(
            user,
            dto.Password,
            isPersistent: false,
            lockoutOnFailure: false
        );

        if (result.Succeeded == false)
        {
            base.TempData["Error"] = "Incorrect login or password";
            return base.RedirectToAction(actionName: nameof(Login));
        }

        return base.RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return base.RedirectToAction(actionName: "Index", controllerName: "Home");
    }
}
