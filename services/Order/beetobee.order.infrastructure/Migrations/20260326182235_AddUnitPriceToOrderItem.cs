using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace beetobee.order.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitPriceToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-0001-0000-0000-000000000001") },
                column: "UnitPrice",
                value: 349.99m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-0001-0000-0000-000000000003") },
                column: "UnitPrice",
                value: 129.99m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-0001-0000-0000-000000000005") },
                column: "UnitPrice",
                value: 89.99m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-0001-0000-0000-000000000007") },
                column: "UnitPrice",
                value: 34.99m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-0001-0000-0000-000000000002") },
                column: "UnitPrice",
                value: 1299.99m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-0001-0000-0000-000000000009") },
                column: "UnitPrice",
                value: 189.99m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "OrderItems");
        }
    }
}
