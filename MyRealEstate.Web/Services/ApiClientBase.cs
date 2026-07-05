using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using MyRealEstate.Web.Models;

namespace MyRealEstate.Web.Services
{
    /// <summary>
    /// Base class for all typed API clients.
    /// Handles:
    ///   - Base URL resolution via IOptions<ApiSettings>
    ///   - JWT Bearer token forwarding from the current HttpContext session
    ///   - JSON serialization (System.Text.Json, camelCase)
    ///   - HTTP error mapping (400, 401, 404, 500) with optional ModelState population
    /// </summary>
    public abstract class ApiClientBase
    {
        protected readonly HttpClient Http;
        protected readonly ApiSettings Settings;
        protected readonly JsonSerializerOptions JsonOptions;

        // HttpContextAccessor must be injected by each derived client
        protected IHttpContextAccessor? ContextAccessor { get; }

        protected ApiClientBase(HttpClient http, IOptions<ApiSettings> options,
            IHttpContextAccessor? contextAccessor = null)
        {
            Http = http;
            Settings = options.Value;
            ContextAccessor = contextAccessor;

// NOTE: The backend SAKAN.API uses default System.Text.Json (numeric enums,
            // camelCase properties). We must NOT add a string enum converter here or
            // request bodies (Role, PropertyType, ...) would send "owner" instead of 1
            // and fail model binding. Responses are decoded as numeric enums too.
            JsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            Http.BaseAddress = new Uri(Settings.BaseUrl.TrimEnd('/') + "/");
            Http.Timeout = TimeSpan.FromSeconds(Settings.TimeoutSeconds > 0 ? Settings.TimeoutSeconds : 30);

            // Forward JWT Bearer token from session if available
            var token = GetSessionToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                Http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        /// <summary>Get the JWT from the current HttpContext session.</summary>
        protected string? GetSessionToken()
        {
            try
            {
                return ContextAccessor?.HttpContext?.Session.GetString("JwtToken");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Serialize request body to JSON.</summary>
        protected StringContent ToJsonContent<T>(T payload) =>
            new StringContent(JsonSerializer.Serialize(payload, JsonOptions),
                Encoding.UTF8, MediaTypeNames.Application.Json);

        /// <summary>Deserialize response JSON to target type.</summary>
        protected async Task<T?> FromJsonAsync<T>(HttpResponseMessage response,
            CancellationToken ct = default)
        {
            var body = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<T>(body, JsonOptions);
        }

        /// <summary>
        /// Attempt to deserialize an error payload from a non-success response.
        /// Returns null if the body is not valid JSON or parsing fails.
        /// </summary>
        protected async Task<ApiErrorResponse?> ParseErrorAsync(HttpResponseMessage response,
            CancellationToken ct = default)
        {
            try
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                return JsonSerializer.Deserialize<ApiErrorResponse>(body, JsonOptions);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Build a formatted error message from a failed response.
        /// </summary>
        protected async Task<string> BuildErrorMessageAsync(HttpResponseMessage response,
            CancellationToken ct = default)
        {
            var error = await ParseErrorAsync(response, ct);
            if (error != null && !string.IsNullOrWhiteSpace(error.Title))
                return $"{response.StatusCode}: {error.Title}";
            var body = await response.Content.ReadAsStringAsync(ct);
            return $"{response.StatusCode}: {(string.IsNullOrWhiteSpace(body) ? "Unknown error" : body)}";
        }

        /// <summary>
        /// Perform a GET against the API and deserialize a strongly-typed result.
        /// Throws <see cref="ApiClientException"/> on non-success status.
        /// </summary>
        protected async Task<T?> GetAsync<T>(string route, CancellationToken ct = default)
        {
            var response = await Http.GetAsync(route, ct);
            return await HandleResponseAsync<T>(response, ct);
        }

        /// <summary>Deserialize a successful response or throw a formatted API exception.</summary>
        protected async Task<T?> HandleResponseAsync<T>(HttpResponseMessage response,
            CancellationToken ct = default)
        {
            if (response.IsSuccessStatusCode)
                return await FromJsonAsync<T>(response, ct);

            var message = await BuildErrorMessageAsync(response, ct);
            throw new ApiClientException(message, response.StatusCode);
        }

        /// <summary>Throw a formatted API exception when the response is not successful.</summary>
        protected async Task EnsureSuccessAsync(HttpResponseMessage response,
            CancellationToken ct = default)
        {
            if (!response.IsSuccessStatusCode)
            {
                var message = await BuildErrorMessageAsync(response, ct);
                throw new ApiClientException(message, response.StatusCode);
            }
        }

        /// <summary>
        /// Builds a query string from an object's public properties using camelCase names.
        /// Null properties are skipped; enums are serialized by name (PascalCase) to match
        /// ASP.NET Core model binding of enums in the backend API.
        /// </summary>
        protected static string BuildQueryString(object query)
        {
            if (query is null)
                return string.Empty;

            var pairs = new List<string>();

            foreach (var prop in query.GetType().GetProperties(
                         BindingFlags.Instance | BindingFlags.Public))
            {
                var value = prop.GetValue(query);
                if (value is null)
                    continue;

                var name = JsonNamingPolicy.CamelCase.ConvertName(prop.Name);

                string rendered = value switch
                {
                    Enum e => e.ToString(),
                    bool b => b ? "true" : "false",
                    DateTime dt => dt.ToString("O", CultureInfo.InvariantCulture),
                    IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
                    _ => value.ToString() ?? string.Empty
                };

                pairs.Add($"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(rendered)}");
            }

            return pairs.Count == 0
                ? string.Empty
                : "?" + string.Join("&", pairs);
        }
    }

    /// <summary>
    /// Standard API error envelope matching ASP.NET Core ProblemDetails.
    /// </summary>
    public class ApiErrorResponse
    {
        public string? Title { get; set; }
        public string? Detail { get; set; }
        public int? Status { get; set; }
        public string? Type { get; set; }
        public string? TraceId { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
