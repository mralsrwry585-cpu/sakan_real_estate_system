using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MyRealEstate.Web.Helpers;

namespace MyRealEstate.Web.Middleware
{
    /// <summary>
    /// Restores the authenticated <see cref="ClaimsPrincipal"/> from the server-side
    /// session. This replaces cookie-based authentication: no auth cookie is written
    /// to the browser and the identity lives only in the session (JWT + owner profile).
    /// Must run after UseSession and after UseAuthentication, before UseAuthorization.
    /// </summary>
    public class OwnerSessionAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public OwnerSessionAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var principal = context.Session.BuildPrincipal();
                if (principal is not null)
                {
                    context.User = principal;
                }
            }
            catch
            {
                // Session unreadable (corrupt/expired) → treat as anonymous.
                try
                {
                    context.Session.Clear();
                }
                catch
                {
                    // ignore
                }
            }

            await _next(context);
        }
    }

    public static class OwnerSessionAuthenticationExtensions
    {
        public static IApplicationBuilder UseOwnerSessionAuthentication(this IApplicationBuilder app)
            => app.UseMiddleware<OwnerSessionAuthenticationMiddleware>();
    }
}