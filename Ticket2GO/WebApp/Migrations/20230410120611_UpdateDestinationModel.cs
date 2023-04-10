using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateDestinationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "241f23d3-e105-4ffe-85ea-f9a80383b287");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49a90bd4-b91c-4e73-a4a3-f2f7f853cbb6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51369df4-6680-4347-9546-d41a4ba9216f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54a6255b-e462-40d2-8230-de66f4ee95f9");

            migrationBuilder.AddColumn<int>(
                name: "RepeatingDayOfWeek",
                table: "Destinations",
                type: "int",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "RepeatingDayOfWeek",
                table: "Destinations");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "241f23d3-e105-4ffe-85ea-f9a80383b287", "4fd3e3c2-167c-42dd-be4f-dae4f532ed66", "User", "USER" },
                    { "49a90bd4-b91c-4e73-a4a3-f2f7f853cbb6", "851bc094-9ef0-420a-b88b-d2b7d5397a5a", "Admin", "ADMIN" },
                    { "51369df4-6680-4347-9546-d41a4ba9216f", "62f1b7d2-88e9-48ce-9b1e-46dcafc40269", "Unassigned", "UNASSIGNED" },
                    { "54a6255b-e462-40d2-8230-de66f4ee95f9", "efcf53e0-9e3d-4f86-8d66-2a34445d8726", "Company Manager", "COMPANY MANAGER" }
                });
        }
    }
}
