using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateFieldsInDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Tickets");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Tickets",
                type: "decimal(6,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Destinations",
                type: "decimal(6,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ccb6b1b-9f3d-425b-9cf3-af56cf217236", "e7cc49ea-86c4-4a04-af17-94243fd8010b", "Admin", "ADMIN" },
                    { "57597c21-9303-4690-a13b-282004f37aa5", "5ad6f012-7617-4251-beb8-e49795c6416b", "Unassigned", "UNASSIGNED" },
                    { "bec2c936-c7f3-426b-ba31-adf65f1b6742", "60876845-972d-49e9-8207-bc747ba7d841", "User", "USER" },
                    { "f5efebbb-1f46-474c-97f4-3f24061109ec", "4110a480-5c96-4b9d-8306-980169dd9475", "Company Manager", "COMPANY MANAGER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ccb6b1b-9f3d-425b-9cf3-af56cf217236");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57597c21-9303-4690-a13b-282004f37aa5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bec2c936-c7f3-426b-ba31-adf65f1b6742");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5efebbb-1f46-474c-97f4-3f24061109ec");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Destinations");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Tickets",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

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
    }
}
