using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZambaFarm.Migrations
{
    /// <inheritdoc />
    public partial class _2Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rabbits_Rabbits_MotherId",
                table: "Rabbits");

            migrationBuilder.DropIndex(
                name: "IX_Rabbits_MotherId",
                table: "Rabbits");

            migrationBuilder.DropColumn(
                name: "MotherId",
                table: "Rabbits");

            migrationBuilder.AddColumn<int>(
                name: "RabbitId1",
                table: "Rabbits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rabbits_RabbitId1",
                table: "Rabbits",
                column: "RabbitId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Rabbits_Rabbits_RabbitId1",
                table: "Rabbits",
                column: "RabbitId1",
                principalTable: "Rabbits",
                principalColumn: "RabbitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rabbits_Rabbits_RabbitId1",
                table: "Rabbits");

            migrationBuilder.DropIndex(
                name: "IX_Rabbits_RabbitId1",
                table: "Rabbits");

            migrationBuilder.DropColumn(
                name: "RabbitId1",
                table: "Rabbits");

            migrationBuilder.AddColumn<int>(
                name: "MotherId",
                table: "Rabbits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rabbits_MotherId",
                table: "Rabbits",
                column: "MotherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rabbits_Rabbits_MotherId",
                table: "Rabbits",
                column: "MotherId",
                principalTable: "Rabbits",
                principalColumn: "RabbitId");
        }
    }
}
