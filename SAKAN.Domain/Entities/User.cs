using SAKAN.Domain.Enums;

namespace SAKAN.Domain.Entities
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Role ActiveRole { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
