using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZambaFarm.Migrations
{
    /// <inheritdoc />
    public partial class _8Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chicken",
                columns: table => new
                {
                    ChickenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMated = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfEggsLaid = table.Column<int>(type: "int", nullable: true),
                    MatingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotherChickenId = table.Column<int>(type: "int", nullable: true),
                    MotherTagNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEggLaying = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfEggs = table.Column<int>(type: "int", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chicken", x => x.ChickenId);
                    table.ForeignKey(
                        name: "FK_Chicken_Chicken_MotherChickenId",
                        column: x => x.MotherChickenId,
                        principalTable: "Chicken",
                        principalColumn: "ChickenId");
                });

            migrationBuilder.CreateTable(
                name: "Duck",
                columns: table => new
                {
                    DuckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsMated = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfEggsLaid = table.Column<int>(type: "int", nullable: true),
                    MatingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotherDuckId = table.Column<int>(type: "int", nullable: true),
                    MotherTagNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEggLaying = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfEggs = table.Column<int>(type: "int", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duck", x => x.DuckId);
                    table.ForeignKey(
                        name: "FK_Duck_Duck_MotherDuckId",
                        column: x => x.MotherDuckId,
                        principalTable: "Duck",
                        principalColumn: "DuckId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chicken_MotherChickenId",
                table: "Chicken",
                column: "MotherChickenId");

            migrationBuilder.CreateIndex(
                name: "IX_Duck_MotherDuckId",
                table: "Duck",
                column: "MotherDuckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chicken");

            migrationBuilder.DropTable(
                name: "Duck");
        }
    }
}
