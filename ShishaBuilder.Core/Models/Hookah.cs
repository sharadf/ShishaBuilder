namespace ShishaBuilder.Core.Models
{
    public class Hookah
    {
        public int Id { get; set; }
        public required string ModelName { get; set; }
        public required string CompanyName { get; set; }
        public required string Image { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}