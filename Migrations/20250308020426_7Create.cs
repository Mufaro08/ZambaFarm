using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZambaFarm.Migrations
{
    /// <inheritdoc />
    public partial class _7Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Turkey",
                columns: table => new
                {
                    TurkeyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBreeding = table.Column<bool>(type: "bit", nullable: false),
                    BreedingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    MotherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turkey", x => x.TurkeyId);
                    table.ForeignKey(
                        name: "FK_Turkey_Turkey_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Turkey",
                        principalColumn: "TurkeyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Turkey_MotherId",
                table: "Turkey",
                column: "MotherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Turkey");
        }
    }
}
