using Microsoft.Extensions.Options;
using MyRealEstate.Web.Models;
using MyRealEstate.Web.Models.Api;

namespace MyRealEstate.Web.Services
{
    /// <summary>
    /// Typed client for the SAKAN Properties API — list/detail/create/update/delete,
    /// status updates and media management endpoints.
    /// </summary>
    public class PropertiesApiClient : ApiClientBase
    {
        public PropertiesApiClient(HttpClient http, IOptions<ApiSettings> options,
            IHttpContextAccessor contextAccessor)
            : base(http, options, contextAccessor)
        {
        }

        /// <summary>GET api/properties — paged, filterable.</summary>
        public async Task<PagedResult<PropertyListDto>?> GetAllAsync(
            GetAllPropertiesQuery query, CancellationToken ct = default)
        {
            var route = Settings.Routes.Properties + BuildQueryString(query);
            return await GetAsync<PagedResult<PropertyListDto>>(route, ct);
        }

        /// <summary>GET api/properties/{id}.</summary>
        public async Task<PropertyDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.PropertyById, id);
            return await GetAsync<PropertyDto>(route, ct);
        }

        /// <summary>POST api/properties.</summary>
        public async Task<PropertyDto?> CreateAsync(CreatePropertyCommand command,
            CancellationToken ct = default)
        {
            var response = await Http.PostAsync(Settings.Routes.Properties, ToJsonContent(command), ct);
            return await HandleResponseAsync<PropertyDto>(response, ct);
        }

        /// <summary>PUT api/properties/{id}.</summary>
        public async Task<PropertyDto?> UpdateAsync(UpdatePropertyCommand command,
            CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.PropertyById, command.Id);
            var response = await Http.PutAsync(route, ToJsonContent(command), ct);
            return await HandleResponseAsync<PropertyDto>(response, ct);
        }

        /// <summary>DELETE api/properties/{id}.</summary>
        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.PropertyById, id);
            var response = await Http.DeleteAsync(route, ct);
            await EnsureSuccessAsync(response, ct);
        }

        /// <summary>PUT api/properties/{id}/status.</summary>
        public async Task<PropertyDto?> UpdateStatusAsync(UpdatePropertyStatusCommand command,
            CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.PropertyStatus, command.Id);
            var response = await Http.PutAsync(route, ToJsonContent(command), ct);
            return await HandleResponseAsync<PropertyDto>(response, ct);
        }

        // ---- Media ----

        /// <summary>GET api/properties/{id}/media.</summary>
        public async Task<List<PropertyMediaDto>?> GetMediaAsync(Guid propertyId,
            CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.PropertyMedia, propertyId);
            return await GetAsync<List<PropertyMediaDto>>(route, ct);
        }

        /// <summary>POST api/properties/{id}/media.</summary>
        public async Task<PropertyMediaDto?> AddMediaAsync(AddPropertyMediaCommand command,
            CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.PropertyMedia, command.PropertyId);
            var response = await Http.PostAsync(route, ToJsonContent(command), ct);
            return await HandleResponseAsync<PropertyMediaDto>(response, ct);
        }

        /// <summary>DELETE api/properties/{id}/media/{mediaId}.</summary>
        public async Task DeleteMediaAsync(Guid propertyId, Guid mediaId,
            CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.PropertyMediaDelete, propertyId, mediaId);
            var response = await Http.DeleteAsync(route, ct);
            await EnsureSuccessAsync(response, ct);
        }

        /// <summary>PUT api/properties/{id}/media/cover — body: SetCoverMediaCommand.</summary>
        public async Task<PropertyMediaDto?> SetCoverAsync(Guid propertyId, Guid mediaId,
            CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.PropertyMediaCover, propertyId);
            var payload = new SetCoverMediaCommand { PropertyId = propertyId, MediaId = mediaId };
            var response = await Http.PutAsync(route, ToJsonContent(payload), ct);
            return await HandleResponseAsync<PropertyMediaDto>(response, ct);
        }

        /// <summary>PUT api/properties/{id}/media/reorder — body: ReorderMediaCommand.</summary>
        public async Task<List<PropertyMediaDto>?> ReorderMediaAsync(ReorderMediaCommand command,
            CancellationToken ct = default)
        {
            var route = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                Settings.Routes.PropertyMediaReorder, command.PropertyId);
            var response = await Http.PutAsync(route, ToJsonContent(command), ct);
            return await HandleResponseAsync<List<PropertyMediaDto>>(response, ct);
        }
    }
}
