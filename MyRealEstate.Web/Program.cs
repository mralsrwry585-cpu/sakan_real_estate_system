using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyRealEstate.Web.Middleware;
using MyRealEstate.Web.Models;
using MyRealEstate.Web.Services;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

// ============================================================
// Configuration
// ============================================================

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection(ApiSettings.SectionName));

builder.Services
    .AddOptions<ApiSettings>()
    .ValidateDataAnnotations()
    .ValidateOnStart();

// ============================================================
// MVC + Razor
// ============================================================

builder.Services
    .AddControllersWithViews()

    // TempData lives in the session instead of a browser cookie, so the
    // framework never writes the ".AspNetCore.Mvc.CookieTempDataProvider" cookie.
    .AddSessionStateTempDataProvider()

    .ConfigureApiBehaviorOptions(_ => { })
    .AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
        options.HtmlHelperOptions.ValidationMessageElement = "span";
    })
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Insert(
            0,
            "/Views/Lessor/{1}/{0}.cshtml"
        );

        options.ViewLocationFormats.Insert(
            1,
            "/Views/Lessor/Shared/{0}.cshtml"
        );
    });

// ============================================================
// Global Anti-Forgery
// ============================================================

builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// ============================================================
// Session
// ============================================================

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);

    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;

    options.Cookie.Name = ".Sakan.Lessor";
});

// ============================================================
// Authentication
// ============================================================
//
// The cookie scheme below exists ONLY to map [Authorize] challenge /
// access-denied responses to Account pages. No auth cookie is ever
// written: the user identity is restored from the server-side session
// by OwnerSessionAuthenticationMiddleware (cookie-less authentication).
// The only browser cookie set by the app is the .Sakan.Lessor session
// cookie (which is required for server-side sessions).

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;

        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SameSite = SameSiteMode.Lax;

        options.Cookie.Name = ".Sakan.Lessor.Auth";
    });

// ============================================================
// HTTP Context
// ============================================================

builder.Services.AddHttpContextAccessor();

// ============================================================
// Typed API Clients
// ============================================================

builder.Services.AddHttpClient<AuthApiClient>();
builder.Services.AddHttpClient<PropertiesApiClient>();
builder.Services.AddHttpClient<ViewingRequestsApiClient>();
builder.Services.AddHttpClient<BookingRequestsApiClient>();
builder.Services.AddHttpClient<AmenitiesApiClient>();
builder.Services.AddHttpClient<AnalyticsApiClient>();

// ============================================================
// Localization
// ============================================================

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var arabic = new CultureInfo("ar-SA");

    arabic.NumberFormat.CurrencySymbol = "ر.س";
    arabic.NumberFormat.CurrencyGroupSeparator = ",";
    arabic.NumberFormat.NumberGroupSeparator = ",";

    options.DefaultRequestCulture = new RequestCulture(arabic);

    options.SupportedCultures = new[]
    {
        arabic,
        new CultureInfo("en-US")
    };

    options.SupportedUICultures = new[]
    {
        arabic,
        new CultureInfo("en-US")
    };

    options.RequestCultureProviders =
    [
        new AcceptLanguageHeaderRequestCultureProvider()

        // Note: CookieRequestCultureProvider was removed so the app
        // never writes a culture cookie (.AspNetCore.Culture).
    ];
});

// ============================================================
// Build
// ============================================================

var app = builder.Build();

// ============================================================
// Error Handling
// ============================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ============================================================
// HTTP Pipeline
// ============================================================

// HTTPS intentionally disabled for local development.
// Enable in production when HTTPS is configured.
// app.UseHttpsRedirection();

app.UseStaticFiles();

var localizationOptions =
    app.Services
        .GetRequiredService<IOptions<RequestLocalizationOptions>>()
        .Value;

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseSession();

app.UseAuthentication();

// Cookie-less authentication: rebuild the ClaimsPrincipal from the session.
app.UseOwnerSessionAuthentication();

app.UseAuthorization();
// Attribute Routing
app.MapControllers();

// ============================================================
// Default Routes
// ============================================================
//
// All /owner/* pages use attribute routing (see Controllers/Lessor).
// Only Account + Home are conventional.
// The previous "lessor" conventional route (owner/{controller}/{action})
// was removed because it duplicated the attribute routes and could make
// generated URLs ambiguous.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();