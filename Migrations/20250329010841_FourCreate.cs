using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZambaFarm.Migrations
{
    /// <inheritdoc />
    public partial class FourCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNursing",
                table: "Pigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBabiesNursed",
                table: "Pigs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBabiesNursed",
                table: "Cattles",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNursing",
                table: "Pigs");

            migrationBuilder.DropColumn(
                name: "NumberOfBabiesNursed",
                table: "Pigs");

            migrationBuilder.DropColumn(
                name: "NumberOfBabiesNursed",
                table: "Cattles");
        }
    }
}
