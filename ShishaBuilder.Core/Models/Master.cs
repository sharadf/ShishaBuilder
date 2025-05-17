using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ShishaBuilder.Core.Models;

public class Master
{
    public int Id { get; set; }

    [Required]
    public string AppUserId { get; set; } = null!;

    [ForeignKey("AppUserId")]
    public AppUser AppUser { get; set; } = null!;

    public string? PhotoUrl { get; set; }
    public bool IsActive { get; set; } = true;
}
