using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }
    }
}
