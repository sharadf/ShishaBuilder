using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ShishaBuilder.Core.DTOs;

public class EditMasterPhotoOnlyDto
{
    public int MasterId { get; init; }
    public string? AppUserId { get; init; }

    public string FullName { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public int Age { get; init; }
    public int ExperienceYears { get; init; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; } // Это единственное редактируемое поле

    public string? ExistingImageUrl { get; set; } // Это тоже можно менять, если хочешь обновить URL после сохранения

    public string Email { get; init; } = null!;
}
