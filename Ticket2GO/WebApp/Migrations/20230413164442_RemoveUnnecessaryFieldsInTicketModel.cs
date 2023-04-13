using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class RemoveUnnecessaryFieldsInTicketModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5e53d1fa-705f-4544-bcc3-e12f6ab11b94", "c8fb101c-b149-4f09-82d7-51eafe75dabc", "Company Manager", "COMPANY MANAGER" },
                    { "671fba0c-747c-49ad-bda6-f02550110946", "64712978-e9c0-4e93-b45f-7ac61388414e", "Admin", "ADMIN" },
                    { "73651040-e9e3-45bb-aa33-a8891899773f", "e657dfe3-101b-4c26-b6ea-80894f2816da", "User", "USER" },
                    { "f63f7cc3-dbb6-4967-92ac-8d92a5b94319", "38de4672-8c8a-4c48-8e76-c8e251d188fb", "Unassigned", "UNASSIGNED" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e53d1fa-705f-4544-bcc3-e12f6ab11b94");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "671fba0c-747c-49ad-bda6-f02550110946");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73651040-e9e3-45bb-aa33-a8891899773f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f63f7cc3-dbb6-4967-92ac-8d92a5b94319");

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
    }
}
