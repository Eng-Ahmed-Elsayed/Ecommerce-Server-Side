using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70f96035-3264-4a4c-ba6a-5787748dfeda");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef3185e3-6146-470a-bf46-8595b369005a");

            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2a287818-ecc9-4c43-b568-74ab7d8db7a1", null, "Viewer", "VIEWER" },
                    { "c39c5b9c-8f95-4565-8526-67b1852630c4", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a287818-ecc9-4c43-b568-74ab7d8db7a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c39c5b9c-8f95-4565-8526-67b1852630c4");

            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "Categories");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "70f96035-3264-4a4c-ba6a-5787748dfeda", null, "Viewer", "VIEWER" },
                    { "ef3185e3-6146-470a-bf46-8595b369005a", null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
