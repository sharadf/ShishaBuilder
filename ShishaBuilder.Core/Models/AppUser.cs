using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ShishaBuilder.Core.Models;

[Table("AspNetUsers")]
public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    public int? Age { get; set; }
    public int? ExperienceYears { get; set; }
    public string? Gender { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public bool IsActive { get; set; } = true;
}
