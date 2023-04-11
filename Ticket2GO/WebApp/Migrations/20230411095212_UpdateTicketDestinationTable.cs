using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateTicketDestinationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20d0b2a2-5984-472e-80eb-c783ff794753");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "830ae501-6313-4595-9d60-bd75ccd3358e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "acfd33a0-3ee7-423f-b99c-e738f717120d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f23c5425-1b05-45f5-9e16-03b80448d40b");

            migrationBuilder.AddColumn<int>(
                name: "SeatNumber",
                table: "TicketsDestinations",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "TicketsDestinations");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "20d0b2a2-5984-472e-80eb-c783ff794753", "7bf973de-0ca4-458d-ab3a-dc5b67d8dc82", "Unassigned", "UNASSIGNED" },
                    { "830ae501-6313-4595-9d60-bd75ccd3358e", "d804b1a2-b0c9-46c5-989e-2bb3ff43b140", "Company Manager", "COMPANY MANAGER" },
                    { "acfd33a0-3ee7-423f-b99c-e738f717120d", "a743d6a0-8c2d-4970-ae67-1a772a1cab4f", "User", "USER" },
                    { "f23c5425-1b05-45f5-9e16-03b80448d40b", "51d4dd6f-c724-4568-a1e9-342210788bf3", "Admin", "ADMIN" }
                });
        }
    }
}
