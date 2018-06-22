using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ecovadis.DAL.Migrations
{
    public partial class InitialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Court_CourtId",
                table: "Match");

            migrationBuilder.DropTable(
                name: "Court");

            migrationBuilder.DropIndex(
                name: "IX_Match_CourtId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "CourtId",
                table: "Match");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourtId",
                table: "Match",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Court",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Court", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Match_CourtId",
                table: "Match",
                column: "CourtId");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Court_CourtId",
                table: "Match",
                column: "CourtId",
                principalTable: "Court",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
