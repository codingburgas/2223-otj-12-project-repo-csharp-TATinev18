using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateTicketTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "SeatNumber",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01b62bcc-98a6-4619-93de-5d87216e805d", "d6c9e98b-26e3-4154-8b93-1c8488a2f6c8", "Unassigned", "UNASSIGNED" },
                    { "35ce37ee-c361-4ea9-b3d3-f97e531a1339", "87299470-60f2-4936-9325-e8dfcedc80bc", "User", "USER" },
                    { "8be343dd-810f-4fa4-850c-146d4c57282d", "3b522126-aead-4039-a5cd-1571ecd590c8", "Company Manager", "COMPANY MANAGER" },
                    { "f8171a84-4382-41c2-ac95-207f91e3fb2a", "be968c68-4c52-426f-bbee-fac5370e96a2", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01b62bcc-98a6-4619-93de-5d87216e805d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35ce37ee-c361-4ea9-b3d3-f97e531a1339");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8be343dd-810f-4fa4-850c-146d4c57282d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f8171a84-4382-41c2-ac95-207f91e3fb2a");

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "Tickets");

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
    }
}
