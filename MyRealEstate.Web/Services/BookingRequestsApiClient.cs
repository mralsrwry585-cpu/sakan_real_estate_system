using Microsoft.Extensions.Options;
using MyRealEstate.Web.Models;
using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Services
{
    /// <summary>
    /// Typed client for the SAKAN BookingRequests API — list/detail/status updates.
    /// </summary>
    public class BookingRequestsApiClient : ApiClientBase
    {
        public BookingRequestsApiClient(HttpClient http, IOptions<ApiSettings> options,
            IHttpContextAccessor contextAccessor)
            : base(http, options, contextAccessor)
        {
        }

        /// <summary>GET api/bookingrequests — paged, filterable.</summary>
        public async Task<PagedResult<BookingRequestDto>?> GetAllAsync(
            GetAllBookingRequestsQuery query, CancellationToken ct = default)
        {
            var route = Settings.Routes.BookingRequests + BuildQueryString(query);
            return await GetAsync<PagedResult<BookingRequestDto>>(route, ct);
        }

        /// <summary>GET api/bookingrequests/{id}.</summary>
        public async Task<BookingRequestDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.BookingRequestById, id);
            return await GetAsync<BookingRequestDto>(route, ct);
        }

        /// <summary>PUT api/bookingrequests/{id}/status.</summary>
        public async Task<BookingRequestDto?> UpdateStatusAsync(
            UpdateBookingRequestStatusCommand command, CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.BookingRequestStatus, command.Id);
            var response = await Http.PutAsync(route, ToJsonContent(command), ct);
            return await HandleResponseAsync<BookingRequestDto>(response, ct);
        }
    }
}
