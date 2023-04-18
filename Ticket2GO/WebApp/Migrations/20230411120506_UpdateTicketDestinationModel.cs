using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateTicketDestinationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14211ce7-a33a-456f-b05e-062a8eac82ca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24d083d2-adb2-4c9d-9fd5-ed3726c42d20");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6e1a46f-4a9f-4b8a-94d2-f994856765ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "febe12b6-5fcb-4349-a4ab-92b4a6ced624");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "50723e7e-1e97-47ca-a82e-a64ed13b54c5", "dee6c16e-db51-41a7-bbb6-f9855400375b", "Admin", "ADMIN" },
                    { "8045c328-1bda-4556-a3a6-6a074a17bab4", "e347c9d1-17e2-4083-9ffe-2a343f18c61c", "Unassigned", "UNASSIGNED" },
                    { "910c74c7-17b1-41d4-be56-ab1bfc5f0c1b", "fe38c179-d32f-4856-9900-d98d36e0241d", "Company Manager", "COMPANY MANAGER" },
                    { "d83a0b7a-a98c-4675-a708-d15d44089391", "a1cd6160-452a-4a1a-9e90-cd1abdce71e1", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50723e7e-1e97-47ca-a82e-a64ed13b54c5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8045c328-1bda-4556-a3a6-6a074a17bab4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "910c74c7-17b1-41d4-be56-ab1bfc5f0c1b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d83a0b7a-a98c-4675-a708-d15d44089391");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14211ce7-a33a-456f-b05e-062a8eac82ca", "286ea1b7-691c-4c3c-a28e-d6ae2299747b", "Unassigned", "UNASSIGNED" },
                    { "24d083d2-adb2-4c9d-9fd5-ed3726c42d20", "ee798def-b2d6-4f37-a7e2-55849422f7d9", "User", "USER" },
                    { "e6e1a46f-4a9f-4b8a-94d2-f994856765ad", "0ea98ce7-9831-42e4-8e8b-e4ff61e919a7", "Admin", "ADMIN" },
                    { "febe12b6-5fcb-4349-a4ab-92b4a6ced624", "24700fcd-65c5-4519-9dfd-ad7876cffd40", "Company Manager", "COMPANY MANAGER" }
                });
        }
    }
}
