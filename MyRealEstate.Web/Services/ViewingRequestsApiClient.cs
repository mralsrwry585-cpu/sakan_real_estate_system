using Microsoft.Extensions.Options;
using MyRealEstate.Web.Models;
using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Services
{
    /// <summary>
    /// Typed client for the SAKAN ViewingRequests API — list/detail/status updates.
    /// </summary>
    public class ViewingRequestsApiClient : ApiClientBase
    {
        public ViewingRequestsApiClient(HttpClient http, IOptions<ApiSettings> options,
            IHttpContextAccessor contextAccessor)
            : base(http, options, contextAccessor)
        {
        }

        /// <summary>GET api/viewingrequests — paged, filterable.</summary>
        public async Task<PagedResult<ViewingRequestDto>?> GetAllAsync(
            GetAllViewingRequestsQuery query, CancellationToken ct = default)
        {
            var route = Settings.Routes.ViewingRequests + BuildQueryString(query);
            return await GetAsync<PagedResult<ViewingRequestDto>>(route, ct);
        }

        /// <summary>GET api/viewingrequests/{id}.</summary>
        public async Task<ViewingRequestDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.ViewingRequestById, id);
            return await GetAsync<ViewingRequestDto>(route, ct);
        }

        /// <summary>PUT api/viewingrequests/{id}/status.</summary>
        public async Task<ViewingRequestDto?> UpdateStatusAsync(
            UpdateViewingRequestStatusCommand command, CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.ViewingRequestStatus, command.Id);
            var response = await Http.PutAsync(route, ToJsonContent(command), ct);
            return await HandleResponseAsync<ViewingRequestDto>(response, ct);
        }
    }
}
