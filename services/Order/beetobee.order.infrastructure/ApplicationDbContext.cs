namespace beetobee.order.infrastructure;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasConversion(id => id.Value, value => OrderId.Of(value));

            entity.Property(e => e.CustomerReference).IsRequired();

            entity.OwnsMany(e => e.OrderItems, orderItem =>
            {
                orderItem.WithOwner().HasForeignKey("OrderId");
                orderItem.Property<Guid>("OrderId");
                orderItem.Property(i => i.ProductId).IsRequired();
                orderItem.Property(i => i.Quantity).IsRequired();
                orderItem.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
                orderItem.HasKey("OrderId", nameof(OrderItem.ProductId));

                // Seed order items referencing Catalog product IDs
                orderItem.HasData(
                    // Order 1: Sony WH-1000XM5 Headphones (x1 @ $349.99) + Nike Air Max 270 (x2 @ $129.99)
                    new { OrderId = OrderId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000001")), ProductId = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000001"), Quantity = 1, UnitPrice = 349.99m },
                    new { OrderId = OrderId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000001")), ProductId = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000003"), Quantity = 2, UnitPrice = 129.99m },
                    // Order 2: Instant Pot Duo 7-in-1 (x1 @ $89.99) + Clean Code book (x3 @ $34.99)
                    new { OrderId = OrderId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000002")), ProductId = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000005"), Quantity = 1, UnitPrice = 89.99m },
                    new { OrderId = OrderId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000002")), ProductId = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000007"), Quantity = 3, UnitPrice = 34.99m },
                    // Order 3: Samsung 65" QLED TV (x1 @ $1299.99) + Adidas Ultraboost 22 (x1 @ $189.99)
                    new { OrderId = OrderId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000003")), ProductId = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000002"), Quantity = 1, UnitPrice = 1299.99m },
                    new { OrderId = OrderId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000003")), ProductId = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000009"), Quantity = 1, UnitPrice = 189.99m }
                );
            });

            entity.HasData(
                new { Id = OrderId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000001")), CustomerReference = Guid.Parse("c0000000-0000-0000-0000-000000000001"), CreatedAt = (DateTime?)new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
                new { Id = OrderId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000002")), CustomerReference = Guid.Parse("c0000000-0000-0000-0000-000000000002"), CreatedAt = (DateTime?)new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
                new { Id = OrderId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000003")), CustomerReference = Guid.Parse("c0000000-0000-0000-0000-000000000003"), CreatedAt = (DateTime?)new DateTime(2026, 2, 20, 0, 0, 0, DateTimeKind.Utc) }
            );
        });
    }
}
