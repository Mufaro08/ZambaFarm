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
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "turkeys",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Rabbits",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Pigs",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Goats",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Cattles",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "turkeys");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Rabbits");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Pigs");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Goats");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Cattles");
        }
    }
}
