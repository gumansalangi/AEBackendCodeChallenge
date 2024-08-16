using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AEBackendCodeChallenge.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ships_Users_UserId",
                table: "Ships");

            migrationBuilder.DropIndex(
                name: "IX_Ships_UserId",
                table: "Ships");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ships");

            migrationBuilder.CreateTable(
                name: "ShipUser",
                columns: table => new
                {
                    ShipId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipUser", x => new { x.ShipId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ShipUser_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShipUser_UserId",
                table: "ShipUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipUser");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ships",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ships_UserId",
                table: "Ships",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_Users_UserId",
                table: "Ships",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
