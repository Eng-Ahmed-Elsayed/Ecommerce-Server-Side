using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "UserPayments");

            migrationBuilder.AddColumn<int>(
                name: "Cvv",
                table: "UserPayments",
                type: "int",
                maxLength: 3,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserPayments",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cvv",
                table: "UserPayments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserPayments");

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "UserPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
