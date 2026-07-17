using SAKAN.Domain.Enums;

namespace SAKAN.Domain.Entities
{
    public class PropertyMedia
    {
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public string Url { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
        public bool IsCover { get; set; }
        public int DisplayOrder { get; set; }

        // Navigation property
        public Property Property { get; set; } = null!;
    }
}
