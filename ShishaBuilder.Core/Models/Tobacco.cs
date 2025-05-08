using System.ComponentModel.DataAnnotations.Schema;

namespace ShishaBuilder.Core.Models;

public class Tobacco
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public required string Name { get; set; }
    public string? Brand { get; set; }
    public string? Flavor { get; set; }
    public required string Strength { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    [NotMapped]
    public double SelectionRate { get; set; } 

}
