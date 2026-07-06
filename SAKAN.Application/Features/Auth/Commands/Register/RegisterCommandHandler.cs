using MediatR;
using SAKAN.Application.Common.Interfaces;
using SAKAN.Application.Features.Auth.DTOs;
using SAKAN.Domain.Entities;
using SAKAN.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace SAKAN.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IApplicationDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser != null)
                throw new InvalidOperationException("Email already registered.");

            // Hash password
            var passwordHash = _authService.ComputeHash(request.Password);

            // Create user based on role
            User user;
            if (request.Role == Role.Tenant)
            {
                user = new Tenant
                {
                    Id = Guid.NewGuid(),
                    FullName = request.FullName,
                    Mobile = request.Mobile,
                    NationalId = request.NationalId,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    ActiveRole = Role.Tenant,
                    CreatedAt = DateTime.UtcNow
                };
            }
            else
            {
                user = new Owner
                {
                    Id = Guid.NewGuid(),
                    FullName = request.FullName,
                    Mobile = request.Mobile,
                    NationalId = request.NationalId,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    ActiveRole = Role.Owner,
                    CreatedAt = DateTime.UtcNow
                };
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            // Generate JWT token
            var token = await _authService.GenerateJwtTokenAsync(user, cancellationToken);

            return new AuthResponse
            {
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.ActiveRole,
                UserId = user.Id
            };
        }
    }
}
