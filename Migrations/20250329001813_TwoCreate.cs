using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZambaFarm.Migrations
{
    /// <inheritdoc />
    public partial class TwoCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNursing",
                table: "Goats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBabiesNursed",
                table: "Goats",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNursing",
                table: "Goats");

            migrationBuilder.DropColumn(
                name: "NumberOfBabiesNursed",
                table: "Goats");
        }
    }
}
