using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace beetobee.order.infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("Database")));


        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
