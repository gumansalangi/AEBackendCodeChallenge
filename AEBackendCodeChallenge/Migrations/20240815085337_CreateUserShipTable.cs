using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AEBackendCodeChallenge.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserShipTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShip_Ships_ShipId",
                table: "UserShip");

            migrationBuilder.DropForeignKey(
                name: "FK_UserShip_Users_UserId",
                table: "UserShip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserShip",
                table: "UserShip");

            migrationBuilder.RenameTable(
                name: "UserShip",
                newName: "UserShips");

            migrationBuilder.RenameIndex(
                name: "IX_UserShip_ShipId",
                table: "UserShips",
                newName: "IX_UserShips_ShipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserShips",
                table: "UserShips",
                columns: new[] { "UserId", "ShipId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserShips_Ships_ShipId",
                table: "UserShips",
                column: "ShipId",
                principalTable: "Ships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserShips_Users_UserId",
                table: "UserShips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserShips_Ships_ShipId",
                table: "UserShips");

            migrationBuilder.DropForeignKey(
                name: "FK_UserShips_Users_UserId",
                table: "UserShips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserShips",
                table: "UserShips");

            migrationBuilder.RenameTable(
                name: "UserShips",
                newName: "UserShip");

            migrationBuilder.RenameIndex(
                name: "IX_UserShips_ShipId",
                table: "UserShip",
                newName: "IX_UserShip_ShipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserShip",
                table: "UserShip",
                columns: new[] { "UserId", "ShipId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserShip_Ships_ShipId",
                table: "UserShip",
                column: "ShipId",
                principalTable: "Ships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserShip_Users_UserId",
                table: "UserShip",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
