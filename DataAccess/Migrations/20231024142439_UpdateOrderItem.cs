using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems");

            migrationBuilder.UpdateData(
                table: "ShippingOptions",
                keyColumn: "Id",
                keyValue: new Guid("47d3d393-b3aa-43a3-8f06-64387ba6bb8d"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 24, 17, 22, 10, 623, DateTimeKind.Local).AddTicks(8786));

            migrationBuilder.UpdateData(
                table: "ShippingOptions",
                keyColumn: "Id",
                keyValue: new Guid("48ea541f-3ae3-4c10-9e52-6d43a2a33c5b"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 24, 17, 22, 10, 623, DateTimeKind.Local).AddTicks(8786));

            migrationBuilder.UpdateData(
                table: "ShippingOptions",
                keyColumn: "Id",
                keyValue: new Guid("60046871-ba3e-4a2b-b02a-221075b3f9e4"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 24, 17, 22, 10, 623, DateTimeKind.Local).AddTicks(8786));

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems");

            migrationBuilder.UpdateData(
                table: "ShippingOptions",
                keyColumn: "Id",
                keyValue: new Guid("47d3d393-b3aa-43a3-8f06-64387ba6bb8d"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 23, 15, 51, 0, 956, DateTimeKind.Local).AddTicks(7131));

            migrationBuilder.UpdateData(
                table: "ShippingOptions",
                keyColumn: "Id",
                keyValue: new Guid("48ea541f-3ae3-4c10-9e52-6d43a2a33c5b"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 23, 15, 51, 0, 956, DateTimeKind.Local).AddTicks(7042));

            migrationBuilder.UpdateData(
                table: "ShippingOptions",
                keyColumn: "Id",
                keyValue: new Guid("60046871-ba3e-4a2b-b02a-221075b3f9e4"),
                column: "CreatedAt",
                value: new DateTime(2023, 10, 23, 15, 51, 0, 956, DateTimeKind.Local).AddTicks(7136));

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId",
                unique: true);
        }
    }
}
