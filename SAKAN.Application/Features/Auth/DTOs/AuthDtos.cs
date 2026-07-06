using SAKAN.Domain.Enums;

namespace SAKAN.Application.Features.Auth.DTOs
{
    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
        public Guid UserId { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role ActiveRole { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
