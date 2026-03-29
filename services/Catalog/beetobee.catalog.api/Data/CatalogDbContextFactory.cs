using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace beetobee.catalog.api.Data;

public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
{
    public CatalogDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=BeetobeeDb;User Id=sa;Password=beetobee@123;Encrypt=False;TrustServerCertificate=True;");

        return new CatalogDbContext(optionsBuilder.Options);
    }
}
