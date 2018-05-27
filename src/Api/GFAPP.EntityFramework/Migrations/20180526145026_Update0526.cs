using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GFAPP.EntityFramework.Migrations
{
    public partial class Update0526 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "APP.CompanyInfo");

            migrationBuilder.DropColumn(
                name: "District",
                table: "APP.CompanyInfo");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "APP.CompanyInfo");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "APP.CompanyInfo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_APP.CompanyInfo_CityId",
                table: "APP.CompanyInfo",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_APP.CompanyInfo_Districts_CityId",
                table: "APP.CompanyInfo",
                column: "CityId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APP.CompanyInfo_Districts_CityId",
                table: "APP.CompanyInfo");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_APP.CompanyInfo_CityId",
                table: "APP.CompanyInfo");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "APP.CompanyInfo");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "APP.CompanyInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "APP.CompanyInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "APP.CompanyInfo",
                nullable: true);
        }
    }
}
