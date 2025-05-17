using System.ComponentModel.DataAnnotations;

public class AdminRegistrationDto
{
    [Required]
    public string Login { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    [Phone(ErrorMessage = "Неверный формат номера телефона")]
    public string PhoneNumber { get; set; } = null!;
}
