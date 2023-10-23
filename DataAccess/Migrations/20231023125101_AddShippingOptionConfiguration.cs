using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingOptionConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "method",
                table: "ShippingOptions",
                newName: "Method");

            migrationBuilder.InsertData(
                table: "ShippingOptions",
                columns: new[] { "Id", "Cost", "CreatedAt", "DeletedAt", "DeliveryTime", "IsDeleted", "Method", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("47d3d393-b3aa-43a3-8f06-64387ba6bb8d"), 9.99m, new DateTime(2023, 10, 23, 15, 51, 0, 956, DateTimeKind.Local).AddTicks(7131), null, "2-3 business days", null, "Expedited Shipping", null },
                    { new Guid("48ea541f-3ae3-4c10-9e52-6d43a2a33c5b"), 5.99m, new DateTime(2023, 10, 23, 15, 51, 0, 956, DateTimeKind.Local).AddTicks(7042), null, "3-5 business days", null, "Standard Shipping", null },
                    { new Guid("60046871-ba3e-4a2b-b02a-221075b3f9e4"), 19.99m, new DateTime(2023, 10, 23, 15, 51, 0, 956, DateTimeKind.Local).AddTicks(7136), null, "1 business day", null, "Overnight Shipping", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ShippingOptions",
                keyColumn: "Id",
                keyValue: new Guid("47d3d393-b3aa-43a3-8f06-64387ba6bb8d"));

            migrationBuilder.DeleteData(
                table: "ShippingOptions",
                keyColumn: "Id",
                keyValue: new Guid("48ea541f-3ae3-4c10-9e52-6d43a2a33c5b"));

            migrationBuilder.DeleteData(
                table: "ShippingOptions",
                keyColumn: "Id",
                keyValue: new Guid("60046871-ba3e-4a2b-b02a-221075b3f9e4"));

            migrationBuilder.RenameColumn(
                name: "Method",
                table: "ShippingOptions",
                newName: "method");
        }
    }
}
