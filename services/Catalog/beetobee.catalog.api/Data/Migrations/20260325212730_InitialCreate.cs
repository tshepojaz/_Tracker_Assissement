using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace beetobee.catalog.api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AvailableQuantity", "Category", "Description", "Name", "Price", "SKU" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000001"), 50, "Electronics", "Industry-leading noise cancelling wireless headphones.", "Sony WH-1000XM5 Headphones", 349.99m, "ELEC-001" },
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000002"), 20, "Electronics", "4K Smart TV with Quantum HDR and Alexa built-in.", "Samsung 65\" QLED TV", 1299.99m, "ELEC-002" },
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000003"), 100, "Clothing", "Men's running shoes with Air cushioning.", "Nike Air Max 270", 129.99m, "CLTH-001" },
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000004"), 75, "Clothing", "Classic straight fit jeans in dark wash.", "Levi's 501 Original Jeans", 69.99m, "CLTH-002" },
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000005"), 60, "Home", "Electric pressure cooker with 7 cooking functions.", "Instant Pot Duo 7-in-1", 89.99m, "HOME-001" },
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000006"), 30, "Home", "Cordless vacuum with laser dust detection.", "Dyson V15 Detect Vacuum", 749.99m, "HOME-002" },
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000007"), 200, "Books", "A handbook of agile software craftsmanship.", "Clean Code by Robert C. Martin", 34.99m, "BOOK-001" },
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000008"), 15, "Toys", "1:8 scale model with 3599 pieces.", "LEGO Technic Bugatti Chiron", 449.99m, "TOYS-001" },
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000009"), 80, "Sports", "High-performance running shoes with BOOST midsole.", "Adidas Ultraboost 22 Running Shoes", 189.99m, "SPRT-001" },
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000010"), 25, "Home", "Professional-grade blender for smoothies and soups.", "Vitamix 5200 Blender", 549.99m, "HOME-003" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
