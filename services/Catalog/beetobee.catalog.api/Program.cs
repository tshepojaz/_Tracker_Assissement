using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using commonblock.Behaviors;
using commonblock.Exceptions.Handler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;
using beetobee.catalog.api.Auth;

var builder = WebApplication.CreateBuilder(args);
//builder.Host.UseSerilog(SeriLogger.Configure);

var assembly = typeof(Program).Assembly;

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// Data Services
builder.Services.AddDbContext<CatalogDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("Database")!);

// Cross-Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

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
    // admin role → create/update/delete products
    options.AddPolicy(Permissions.CreateProduct, p =>
        p.RequireAssertion(ctx => ctx.User.HasClaim(ClaimTypes.Role, "demoadmin")));
    // admin or customer → view products
    options.AddPolicy(Permissions.ViewProduct, p =>
        p.RequireAssertion(ctx =>
            ctx.User.HasClaim(ClaimTypes.Role, "demoadmin") ||
            ctx.User.HasClaim(ClaimTypes.Role, "customer")));
});

var app = builder.Build();

// Auto-apply migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    await db.Database.MigrateAsync();
}

app.MapCarter();
app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.UseAuthentication();
app.UseAuthorization();

app.Run();

