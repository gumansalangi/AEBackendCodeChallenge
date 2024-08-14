using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AEBackendCodeChallenge.Migrations
{
    /// <inheritdoc />
    public partial class UpdateField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ships_Users_UserId",
                table: "Ships");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Ships",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_Users_UserId",
                table: "Ships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ships_Users_UserId",
                table: "Ships");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Ships",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_Users_UserId",
                table: "Ships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
