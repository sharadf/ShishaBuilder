using System.ComponentModel.DataAnnotations;

public class LoginDto
{
    [Required]
    public required string Login { get; set; }

    [Required]
    public required string Password { get; set; }
}
