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
            migrationBuilder.DropForeignKey(
                name: "FK_Turkey_Turkey_MotherId",
                table: "Turkey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Turkey",
                table: "Turkey");

            migrationBuilder.RenameTable(
                name: "Turkey",
                newName: "Turkeys");

            migrationBuilder.RenameIndex(
                name: "IX_Turkey_MotherId",
                table: "Turkeys",
                newName: "IX_Turkeys_MotherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Turkeys",
                table: "Turkeys",
                column: "TurkeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turkeys_Turkeys_MotherId",
                table: "Turkeys",
                column: "MotherId",
                principalTable: "Turkeys",
                principalColumn: "TurkeyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turkeys_Turkeys_MotherId",
                table: "Turkeys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Turkeys",
                table: "Turkeys");

            migrationBuilder.RenameTable(
                name: "Turkeys",
                newName: "Turkey");

            migrationBuilder.RenameIndex(
                name: "IX_Turkeys_MotherId",
                table: "Turkey",
                newName: "IX_Turkey_MotherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Turkey",
                table: "Turkey",
                column: "TurkeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turkey_Turkey_MotherId",
                table: "Turkey",
                column: "MotherId",
                principalTable: "Turkey",
                principalColumn: "TurkeyId");
        }
    }
}
