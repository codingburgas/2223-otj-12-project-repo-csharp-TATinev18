using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateColum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Destinations",
                newName: "Price");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1fed302b-1702-4117-a010-1fcdd6796ab1", "f488fe2d-b140-4159-90bb-f1f714b4c289", "Admin", "ADMIN" },
                    { "317317c3-7ff5-4136-b542-de8dc9083c5d", "ed883855-24d7-4872-b013-c0828c6cc4f6", "Unassigned", "UNASSIGNED" },
                    { "4f2b18d1-6875-44da-a65c-14b24004a106", "38005b19-0d0c-4d47-80ad-db6c9842058b", "User", "USER" },
                    { "8ba7834d-d348-4323-a4cf-4e99ad0b8e3c", "9376bad6-495a-4533-9482-5a085ff72760", "Company Manager", "COMPANY MANAGER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fed302b-1702-4117-a010-1fcdd6796ab1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "317317c3-7ff5-4136-b542-de8dc9083c5d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f2b18d1-6875-44da-a65c-14b24004a106");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ba7834d-d348-4323-a4cf-4e99ad0b8e3c");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Destinations",
                newName: "TotalPrice");

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
    }
}
