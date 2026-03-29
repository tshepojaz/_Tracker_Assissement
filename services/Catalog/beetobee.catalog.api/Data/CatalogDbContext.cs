using Microsoft.EntityFrameworkCore;
using beetobee.catalog.api.Models;
using beetobee.catalog.api.Enums;

namespace beetobee.catalog.api.Data;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Description).HasMaxLength(1000);
            entity.Property(p => p.SKU).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            entity.Property(p => p.Category).HasConversion<string>();
        });

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000001"), Name = "Sony WH-1000XM5 Headphones", Description = "Industry-leading noise cancelling wireless headphones.", SKU = "ELEC-001", Price = 349.99m, AvailableQuantity = 50, Category = Category.Electronics },
            new Product { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000002"), Name = "Samsung 65\" QLED TV", Description = "4K Smart TV with Quantum HDR and Alexa built-in.", SKU = "ELEC-002", Price = 1299.99m, AvailableQuantity = 20, Category = Category.Electronics },
            new Product { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000003"), Name = "Nike Air Max 270", Description = "Men's running shoes with Air cushioning.", SKU = "CLTH-001", Price = 129.99m, AvailableQuantity = 100, Category = Category.Clothing },
            new Product { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000004"), Name = "Levi's 501 Original Jeans", Description = "Classic straight fit jeans in dark wash.", SKU = "CLTH-002", Price = 69.99m, AvailableQuantity = 75, Category = Category.Clothing },
            new Product { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000005"), Name = "Instant Pot Duo 7-in-1", Description = "Electric pressure cooker with 7 cooking functions.", SKU = "HOME-001", Price = 89.99m, AvailableQuantity = 60, Category = Category.Home },
            new Product { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000006"), Name = "Dyson V15 Detect Vacuum", Description = "Cordless vacuum with laser dust detection.", SKU = "HOME-002", Price = 749.99m, AvailableQuantity = 30, Category = Category.Home },
            new Product { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000007"), Name = "Clean Code by Robert C. Martin", Description = "A handbook of agile software craftsmanship.", SKU = "BOOK-001", Price = 34.99m, AvailableQuantity = 200, Category = Category.Books },
            new Product { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000008"), Name = "LEGO Technic Bugatti Chiron", Description = "1:8 scale model with 3599 pieces.", SKU = "TOYS-001", Price = 449.99m, AvailableQuantity = 15, Category = Category.Toys },
            new Product { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000009"), Name = "Adidas Ultraboost 22 Running Shoes", Description = "High-performance running shoes with BOOST midsole.", SKU = "SPRT-001", Price = 189.99m, AvailableQuantity = 80, Category = Category.Sports },
            new Product { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000010"), Name = "Vitamix 5200 Blender", Description = "Professional-grade blender for smoothies and soups.", SKU = "HOME-003", Price = 549.99m, AvailableQuantity = 25, Category = Category.Home }
        );
    }
}
