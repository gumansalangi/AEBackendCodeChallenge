﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AEBackendCodeChallenge.Migrations
{
    /// <inheritdoc />
    public partial class UpdateshipColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipId",
                table: "Ships",
                newName: "ShipCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipCode",
                table: "Ships",
                newName: "ShipId");
        }
    }
}
