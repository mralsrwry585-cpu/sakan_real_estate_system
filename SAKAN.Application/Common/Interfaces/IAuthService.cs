using SAKAN.Domain.Entities;

namespace SAKAN.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateJwtTokenAsync(User user, CancellationToken cancellationToken = default);
        string ComputeHash(string password);
        bool VerifyHash(string password, string hash);
    }
}
