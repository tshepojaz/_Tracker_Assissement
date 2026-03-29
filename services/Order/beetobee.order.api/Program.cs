using beetobee.order.api;
using beetobee.order.api.Auth;
using beetobee.order.application;
using beetobee.order.application.Services;
using beetobee.order.infrastructure;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//Logging Message Handler
builder.Services.AddTransient<LoggingDelegatingHandler>();

// Add services to the container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

builder.Services.AddRefitClient<IProductService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:ProductService"]!));
         


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Keycloak:ValidIssuer"],
            ValidateAudience = false,
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                if (context.Principal?.Identity is ClaimsIdentity identity)
                {
                    // Extract client-level roles from resource_access.<client>.roles
                    var resourceAccess = identity.FindFirst("resource_access")?.Value;
                    if (resourceAccess != null)
                    {
                        using var doc = JsonDocument.Parse(resourceAccess);
                        foreach (var client in doc.RootElement.EnumerateObject())
                            if (client.Value.TryGetProperty("roles", out var roles))
                                foreach (var role in roles.EnumerateArray())
                                    if (role.GetString() is { } r)
                                        identity.AddClaim(new Claim(ClaimTypes.Role, r));
                    }
                }
                return Task.CompletedTask;
            }
        };
    }
);

builder.Services.AddAuthorization(options =>
{
    // customer role → create/view orders, view products
    options.AddPolicy(Permissions.CreateOrder, p =>
        p.RequireAssertion(ctx => ctx.User.HasClaim(ClaimTypes.Role, "customer")));
    options.AddPolicy(Permissions.ViewOrder, p =>
        p.RequireAssertion(ctx => ctx.User.HasClaim(ClaimTypes.Role, "customer")));
});

var app = builder.Build();

// Auto-apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}

app.UseApiServices();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
