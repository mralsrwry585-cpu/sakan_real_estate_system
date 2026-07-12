using MediatR;
using SAKAN.Application.Features.Auth.DTOs;

namespace SAKAN.Application.Features.Auth.Queries.Login
{
    public class LoginQuery : IRequest<AuthResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
