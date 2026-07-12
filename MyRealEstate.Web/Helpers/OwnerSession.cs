using System.Security.Claims;
using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Helpers
{
    /// <summary>
    /// Helpers for reading the current logged-in Owner from the server-side session.
    /// The session stores the raw JWT (for typed API clients) plus owner profile claims.
    /// No authentication cookie is used — the identity is rebuilt per request by
    /// OwnerSessionAuthenticationMiddleware.
    /// </summary>
    public static class OwnerSession
    {
        public const string SessionJwtKey = "JwtToken";
        public const string SessionOwnerIdKey = "OwnerId";
        public const string SessionOwnerNameKey = "OwnerName";
        public const string SessionOwnerEmailKey = "OwnerEmail";

        public static void StoreAuth(this ISession session, AuthResponse auth)
        {
            session.SetString(SessionJwtKey, auth.Token);
            session.SetString(SessionOwnerIdKey, auth.UserId.ToString());
            session.SetString(SessionOwnerNameKey, auth.FullName);
            session.SetString(SessionOwnerEmailKey, auth.Email);
        }

        public static Guid? GetOwnerId(this ISession session)
        {
            var raw = session.GetString(SessionOwnerIdKey);
            return Guid.TryParse(raw, out var id) ? id : null;
        }

        public static string? GetOwnerName(this ISession session) =>
            session.GetString(SessionOwnerNameKey);

        public static string? GetOwnerEmail(this ISession session) =>
            session.GetString(SessionOwnerEmailKey);

        /// <summary>
        /// Builds the authenticated principal purely from server-side session data
        /// (cookie-less auth). Returns null when the owner is not signed in.
        /// </summary>
        public static ClaimsPrincipal? BuildPrincipal(this ISession session)
        {
            var id = session.GetOwnerId();
            if (id is null)
                return null;

            var identity = new ClaimsIdentity(
                claims: new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.Value.ToString()),
                    new Claim(ClaimTypes.Name, session.GetOwnerName() ?? "المالك"),
                    new Claim(ClaimTypes.Email, session.GetOwnerEmail() ?? string.Empty),
                    new Claim(ClaimTypes.Role, "Owner")
                },
                authenticationType: "SakanSessionAuth",
                nameType: ClaimTypes.Name,
                roleType: ClaimTypes.Role);

            return new ClaimsPrincipal(identity);
        }
    }
}
