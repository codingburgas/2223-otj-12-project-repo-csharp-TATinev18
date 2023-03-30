using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class AddBridgeTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportCompanies_AspNetUsers_ApplicationUserId",
                table: "TransportCompanies");

            migrationBuilder.DropIndex(
                name: "IX_TransportCompanies_ApplicationUserId",
                table: "TransportCompanies");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "TransportCompanies");

            migrationBuilder.CreateTable(
                name: "TransportCompaniesAspNetUsers",
                columns: table => new
                {
                    TransportCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportCompaniesAspNetUsers", x => new { x.TransportCompanyId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_TransportCompaniesAspNetUsers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransportCompaniesAspNetUsers_TransportCompanies_TransportCompanyId",
                        column: x => x.TransportCompanyId,
                        principalTable: "TransportCompanies",
                        principalColumn: "TransportCompanyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportCompaniesAspNetUsers_ApplicationUserId",
                table: "TransportCompaniesAspNetUsers",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportCompaniesAspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "TransportCompanies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TransportCompanies_ApplicationUserId",
                table: "TransportCompanies",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportCompanies_AspNetUsers_ApplicationUserId",
                table: "TransportCompanies",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
