using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateBusModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "201e9864-fc2b-425e-87bf-bf1f01e7f8f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59c95e67-0b74-4a28-b0cc-241381f846d8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a68b2b98-039f-4392-a3dd-77581ecd4996");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5163184-0822-4d74-a829-dd95c19941af");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Buses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Buses");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "201e9864-fc2b-425e-87bf-bf1f01e7f8f0", "e8ada6c8-c2db-4be5-8113-fc00f7b904cb", "User", "USER" },
                    { "59c95e67-0b74-4a28-b0cc-241381f846d8", "a7f0e8bb-08c5-4456-a435-60dfbdb19448", "Company Manager", "COMPANY MANAGER" },
                    { "a68b2b98-039f-4392-a3dd-77581ecd4996", "80557d7f-2a56-4d56-9734-4cfa6f0dc29e", "Admin", "ADMIN" },
                    { "c5163184-0822-4d74-a829-dd95c19941af", "e10d034f-5905-4bb1-b22d-59af96212d1e", "Unassigned", "UNASSIGNED" }
                });
        }
    }
}
