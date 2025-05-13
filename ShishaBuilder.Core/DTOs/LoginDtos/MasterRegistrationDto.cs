using System.ComponentModel.DataAnnotations;

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
    [Range(18, 65, ErrorMessage = "Возраст должен быть от 18 до 65")]
    public int Age { get; set; }

    [Required]
    [Range(2, 40, ErrorMessage = "Стаж должен быть от 0 до 40 лет")]
    public int ExperienceYears { get; set; }

    [Required]
    public string Gender { get; set; } = null!;

    [Required]
    [Phone(ErrorMessage = "Неверный формат номера телефона")]
    public string PhoneNumber { get; set; } = null!;
}
