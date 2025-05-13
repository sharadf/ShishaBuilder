// using Microsoft.AspNetCore.Identity;
// using ShishaBuilder.Core.Data;
// using ShishaBuilder.Core.DTOs.LoginDtos;
// using ShishaBuilder.Core.Models;

// public class AccountService : IAccountService
// {
//     private readonly UserManager<AppUser> _userManager;
//     private readonly RoleManager<IdentityRole> _roleManager;
//     private readonly AppDbContext _dbContext;

//     public AccountService(
//         UserManager<AppUser> userManager,
//         RoleManager<IdentityRole> roleManager,
//         AppDbContext dbContext
//     )
//     {
//         _userManager = userManager;
//         _roleManager = roleManager;
//         _dbContext = dbContext;
//     }

//     public async Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAdminAsync(
//         AdminRegistrationDto dto
//     )
//     {
//         var user = new AppUser { UserName = dto.Login, PhoneNumber = dto.PhoneNumber };

//         var result = await _userManager.CreateAsync(user, dto.Password);
//         if (!result.Succeeded)
//             return (false, result.Errors.Select(e => e.Description));

//         await EnsureRoleExistsAsync("Admin");
//         await _userManager.AddToRoleAsync(user, "Admin");

//         // Если нужны дополнительные действия через dbContext — пример:
//         // _dbContext.CustomLogs.Add(new Log { Message = "Admin registered", UserId = user.Id });
//         await _dbContext.SaveChangesAsync();

//         return (true, Enumerable.Empty<string>());
//     }

//     public async Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterMasterAsync(
//         MasterRegistrationDto dto
//     )
//     {
//         var user = new AppUser
//         {
//             UserName = dto.Login,
//             FullName = dto.FullName,
//             Age = dto.Age,
//             ExperienceYears = dto.ExperienceYears,
//             Gender = dto.Gender,
//             PhoneNumber = dto.PhoneNumber,
//         };

//         var result = await _userManager.CreateAsync(user, dto.Password);
//         if (!result.Succeeded)
//             return (false, result.Errors.Select(e => e.Description));

//         await EnsureRoleExistsAsync("Master");
//         await _userManager.AddToRoleAsync(user, "Master");

//         // Возможные действия с dbContext в будущем:
//         // _dbContext.Masters.Add(new MasterInfo { AppUserId = user.Id, Specialization = dto.Specialization });
//         await _dbContext.SaveChangesAsync();

//         return (true, Enumerable.Empty<string>());
//     }

//     private async Task EnsureRoleExistsAsync(string role)
//     {
//         if (!await _roleManager.RoleExistsAsync(role))
//             await _roleManager.CreateAsync(new IdentityRole(role));
//     }

//     public async Task<AppUser?> FindByEmailOrLoginAsync(string login)
//     {
//         var user = await _userManager.FindByEmailAsync(login);
//         if (user == null)
//             user = await _userManager.FindByNameAsync(login);

//         return user;
//     }
// }
