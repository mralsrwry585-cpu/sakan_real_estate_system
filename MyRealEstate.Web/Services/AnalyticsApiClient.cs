using Microsoft.Extensions.Options;
using MyRealEstate.Web.Models;
using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Services
{
    /// <summary>
    /// Typed client for the SAKAN Analytics API — owner dashboard statistics.
    /// </summary>
    public class AnalyticsApiClient : ApiClientBase
    {
        public AnalyticsApiClient(HttpClient http, IOptions<ApiSettings> options,
            IHttpContextAccessor contextAccessor)
            : base(http, options, contextAccessor)
        {
        }

        /// <summary>GET api/analytics/owner/{ownerId} — dashboard stats.</summary>
        public async Task<OwnerDashboardStatsDto?> GetOwnerDashboardStatsAsync(Guid ownerId,
            CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.OwnerDashboardStats, ownerId);
            return await GetAsync<OwnerDashboardStatsDto>(route, ct);
        }
    }
}
