using Microsoft.Extensions.Options;
using MyRealEstate.Web.Models;
using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Services
{
    /// <summary>
    /// Typed client for the SAKAN Auth API (login/register).
    /// Returns the JWT + user profile from the backend.
    /// </summary>
    public class AuthApiClient : ApiClientBase
    {
        public AuthApiClient(HttpClient http, IOptions<ApiSettings> options,
            IHttpContextAccessor contextAccessor)
            : base(http, options, contextAccessor)
        {
        }

        /// <summary>POST api/auth/login — returns AuthResponse with JWT.</summary>
        public async Task<AuthResponse?> LoginAsync(string email, string password,
            CancellationToken ct = default)
        {
            var request = new LoginRequest { Email = email, Password = password };
            var response = await Http.PostAsync(Settings.Routes.AuthLogin, ToJsonContent(request), ct);
            return await HandleResponseAsync<AuthResponse>(response, ct);
        }

        /// <summary>POST api/auth/register — creates an Owner account.</summary>
        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request,
            CancellationToken ct = default)
        {
            var response = await Http.PostAsync(Settings.Routes.AuthRegister, ToJsonContent(request), ct);
            return await HandleResponseAsync<AuthResponse>(response, ct);
        }
    }
}
