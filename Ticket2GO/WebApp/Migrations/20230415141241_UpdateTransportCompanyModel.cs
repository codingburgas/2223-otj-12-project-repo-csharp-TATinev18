using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateTransportCompanyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "TransportCompanies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEdited",
                table: "TransportCompanies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "106bb3ae-d215-46b5-9e15-43cf9ca1ffc4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1327c42e-a9b9-4001-a9a1-22f6ab945f1d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40ac5a7e-915a-47f2-a949-15a101a2745f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca02e689-1da6-435e-96a1-b354dd21a1d5");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "TransportCompanies");

            migrationBuilder.DropColumn(
                name: "DateEdited",
                table: "TransportCompanies");
        }
    }
}
