using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ShishaBuilder.Core.DTOs
{
    public class EditMasterViewModel
    {
        public int MasterId { get; set; }
        public string? AppUserId { get; set; }

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        [Range(0, 40)]
        public int ExperienceYears { get; set; }

        [Required]
        public string Gender { get; set; } = null!;

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImageUrl { get; set; }
    }
}
