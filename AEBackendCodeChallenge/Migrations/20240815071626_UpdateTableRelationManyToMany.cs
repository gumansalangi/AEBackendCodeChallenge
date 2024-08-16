using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AEBackendCodeChallenge.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableRelationManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShipUser");

            migrationBuilder.CreateTable(
                name: "UserShip",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ShipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserShip", x => new { x.UserId, x.ShipId });
                    table.ForeignKey(
                        name: "FK_UserShip_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserShip_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserShip_ShipId",
                table: "UserShip",
                column: "ShipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserShip");

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
    }
}
