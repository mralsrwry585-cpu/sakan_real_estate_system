using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SAKAN.API.Middleware;
using SAKAN.Application;
using SAKAN.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// ================================
// Dependency Injection
// ================================

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


// ================================
// Authentication & JWT Setup
// ================================

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),

        // —»ÿ ’—ÌÕ ·‰Ê⁄ «·‹ Claim «·Œ«’ »«·œÊ— ·÷„«‰ ⁄„· [Authorize(Roles)]
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };
});


// ================================
// Controllers
// ================================

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });


// ================================
// CORS
// ================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


// ================================
// Swagger / OpenAPI
// ================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SAKAN Real Estate Management API",
        Version = "v1",
        Description = "A comprehensive RESTful API for managing real estate properties, bookings, and viewing requests."
    });

    // JWT Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token in the format: Bearer {token}"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});


// ================================
// Application Pipeline
// ================================

var app = builder.Build();

// Global Exception Handler (Ì›÷· Ê÷⁄Â √Ê·« ·Ì· ﬁÿ √Ì Œÿ√ ﬁ«œ„ „‰ «·‹ Pipeline √Ê «·‹ Controllers)
app.UseMiddleware<ExceptionMiddleware>();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS (ÌÃ» √‰ Ì „  ›⁄Ì·Â ﬁ»· «·‹ Authentication Ê«·‹ Authorization Êﬁ»· «·‹ Routing)
app.UseCors("AllowAll");

// Authentication & Authorization («· — Ì» Õ „Ì: Authentication √Ê·« À„ Authorization)
app.UseAuthentication();
app.UseAuthorization();

// Controllers Mapping
app.MapControllers();

app.Run();