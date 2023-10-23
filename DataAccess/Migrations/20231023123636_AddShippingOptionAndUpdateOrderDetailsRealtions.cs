using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingOptionAndUpdateOrderDetailsRealtions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentsDetails");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "OrdersDetails",
                newName: "UserPaymentId");

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingOptionId",
                table: "OrdersDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserAddressId",
                table: "OrdersDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ShippingOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    DeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingOptions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDetails_ShippingOptionId",
                table: "OrdersDetails",
                column: "ShippingOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDetails_UserAddressId",
                table: "OrdersDetails",
                column: "UserAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDetails_UserPaymentId",
                table: "OrdersDetails",
                column: "UserPaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDetails_ShippingOptions_ShippingOptionId",
                table: "OrdersDetails",
                column: "ShippingOptionId",
                principalTable: "ShippingOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDetails_UserAddresses_UserAddressId",
                table: "OrdersDetails",
                column: "UserAddressId",
                principalTable: "UserAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersDetails_UserPayments_UserPaymentId",
                table: "OrdersDetails",
                column: "UserPaymentId",
                principalTable: "UserPayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDetails_ShippingOptions_ShippingOptionId",
                table: "OrdersDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDetails_UserAddresses_UserAddressId",
                table: "OrdersDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersDetails_UserPayments_UserPaymentId",
                table: "OrdersDetails");

            migrationBuilder.DropTable(
                name: "ShippingOptions");

            migrationBuilder.DropIndex(
                name: "IX_OrdersDetails_ShippingOptionId",
                table: "OrdersDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrdersDetails_UserAddressId",
                table: "OrdersDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrdersDetails_UserPaymentId",
                table: "OrdersDetails");

            migrationBuilder.DropColumn(
                name: "ShippingOptionId",
                table: "OrdersDetails");

            migrationBuilder.DropColumn(
                name: "UserAddressId",
                table: "OrdersDetails");

            migrationBuilder.RenameColumn(
                name: "UserPaymentId",
                table: "OrdersDetails",
                newName: "PaymentId");

            migrationBuilder.CreateTable(
                name: "PaymentsDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    OrderDetailsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsDetails_OrdersDetails_OrderDetailsId",
                        column: x => x.OrderDetailsId,
                        principalTable: "OrdersDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsDetails_OrderDetailsId",
                table: "PaymentsDetails",
                column: "OrderDetailsId",
                unique: true);
        }
    }
}
