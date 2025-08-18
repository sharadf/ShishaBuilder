using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShishaBuilder.Core.DTOs.LoginDtos;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Services.BlobServices;
using ShishaBuilder.Core.Services.MasterServices;

public class AccountController : Controller
{
    // private readonly IAccountService _accountService;
    private readonly SignInManager<AppUser> signInManager;
    private readonly UserManager<AppUser> userManager;
    private IMasterService masterService;
    private IBlobService blobService;

    private ISmtpService smtpService;

    private string containerName = "masters";

    public AccountController(
        // IAccountService accountService,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        IMasterService masterService,
        IBlobService blobService,
        ISmtpService smtpService
    )
    {
        // _accountService = accountService;
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.masterService = masterService;
        this.blobService = blobService;
        this.smtpService = smtpService;
    }

    [HttpGet("RegisterAdmin")]
    public IActionResult RegisterAdmin() => View();

    [Authorize(Roles = "Master")]
    [HttpGet("MasterPanel")]
    public IActionResult MasterPanel()
    {
        return View();
    }

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
            return BadRequest(ex.Message);
        }

        return RedirectToAction(nameof(Login));
    }

    [HttpGet("RegisterMaster")]
    public IActionResult RegisterMaster() => View(new MasterRegistrationDto());

    [HttpPost("RegisterMaster")]
    public async Task<IActionResult> RegisterMaster([FromForm] MasterRegistrationDto newUser)
    {
        try
        {
            var existingUser = await userManager.FindByNameAsync(newUser.Login);
            if (existingUser != null)
                return BadRequest("Пользователь с таким логином уже существует.");

            // Создание пользователя
            var user = new AppUser
            {
                Email = newUser.Login,
                UserName = newUser.Login,
                PhoneNumber = newUser.PhoneNumber,
                FullName = newUser.FullName,
                Age = newUser.Age,
                Gender = newUser.Gender,
                ExperienceYears = newUser.ExperienceYears,
                ImageFile = newUser.ImageFile,
            };

            var createResult = await userManager.CreateAsync(user, newUser.Password!);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new Exception("User creation failed: " + errors);
            }

            var roleResult = await userManager.AddToRoleAsync(user, nameof(Roles.Master));
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception("Role assignment failed: " + errors);
            }

            // Обработка фотографии
            string? imageUrl = null;
            if (newUser.ImageFile != null && newUser.ImageFile.Length > 0)
            {
                imageUrl = await blobService.UploadPhotoAsync(newUser.ImageFile, containerName);
            }
            else
            {
                imageUrl =
                    "https://storage.googleapis.com/hishabuilder-blob/masters/26996d6e-625c-4bdb-8b91-e3ed32709d57.jpg";
            }

            // Создание Master
            var master = new Master
            {
                AppUserId = user.Id,
                PhotoUrl = imageUrl,
                IsActive = true,
            };

            // Отправка email с логином и паролем (используем newUser.Password)
            await smtpService.SendEmailAsync(
                to: newUser.Login,
                subject: "Добро пожаловать!",
                body: $"Здравствуйте, {newUser.FullName}! Ваш логин: {newUser.Login}, пароль: {newUser.Password}"
            );

            await masterService.AddMasterAsync(master);
            TempData["SuccessMessage"] = "Master added successfully!";
            return base.RedirectToAction(actionName: "AllMasters", controllerName: "Master");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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


//postgrest image
//sql server image
//custom image - dotnet sdk: dockerFile
//dotnet run time - start of project
//
