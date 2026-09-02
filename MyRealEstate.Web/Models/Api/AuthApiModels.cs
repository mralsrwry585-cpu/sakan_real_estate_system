namespace MyRealEstate.Web.Models.Api
{
    /// <summary>Mirror of SAKAN.Application.Features.Auth.DTOs.RegisterRequest.</summary>
    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; }
    }

    /// <summary>Mirror of SAKAN.Application.Features.Auth.DTOs.LoginRequest.</summary>
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>Mirror of SAKAN.Application.Features.Auth.DTOs.AuthResponse.</summary>
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
        public Guid UserId { get; set; }
    }

    /// <summary>Mirror of SAKAN.Application.Features.Auth.DTOs.UserDto.</summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role ActiveRole { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>Generic paged envelope — mirrors SAKAN.Application.Common.Models.PagedResult<T>.</summary>
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
