using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ShishaBuilder.Core.DTOs.LoginDtos;

public class MasterRegistrationDto
{
    [Required]
    public string Login { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string FullName { get; set; } = null!;

    [Required]
    [Range(18, 65, ErrorMessage = "Age must be between 18 and 55")]
    public int Age { get; set; }

    [Required]
    [Range(2, 40, ErrorMessage = "ExperienceYears must be greater than 0 and less than 40")]
    public int ExperienceYears { get; set; }

    [Required]
    public string Gender { get; set; } = null!;

    [Required]
    [Phone(ErrorMessage = "Incorrect format of phone number")]
    public string PhoneNumber { get; set; } = null!;

    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
