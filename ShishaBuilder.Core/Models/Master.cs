using System.Globalization;

namespace ShishaBuilder.Core.Models;

public class Master
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public string? PhotoUrl { get; set; }
}