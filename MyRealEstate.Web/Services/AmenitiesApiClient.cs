using Microsoft.Extensions.Options;
using MyRealEstate.Web.Models;
using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Services
{
    /// <summary>
    /// Typed client for the SAKAN Amenities API — grouped amenity catalog.
    /// </summary>
    public class AmenitiesApiClient : ApiClientBase
    {
        public AmenitiesApiClient(HttpClient http, IOptions<ApiSettings> options,
            IHttpContextAccessor contextAccessor)
            : base(http, options, contextAccessor)
        {
        }

        /// <summary>GET api/amenities — grouped by category.</summary>
        public async Task<List<AmenityGroupDto>?> GetAllAsync(bool includeInactive = false,
            CancellationToken ct = default)
        {
            var route = Settings.Routes.Amenities +
                        (includeInactive ? "?includeInactive=true" : string.Empty);
            return await GetAsync<List<AmenityGroupDto>>(route, ct);
        }
    }
}
