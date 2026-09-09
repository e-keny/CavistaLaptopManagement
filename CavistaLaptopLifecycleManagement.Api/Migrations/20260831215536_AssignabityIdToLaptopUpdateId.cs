using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AssignabityIdToLaptopUpdateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaptopHistories_LaptopUpdates_LaptopUpdateId",
                table: "LaptopHistories");

            migrationBuilder.DropColumn(
                name: "AssignabilityID",
                table: "LaptopHistories");

            migrationBuilder.RenameColumn(
                name: "LaptopUpdateId",
                table: "LaptopHistories",
                newName: "LaptopUpdateID");

            migrationBuilder.RenameIndex(
                name: "IX_LaptopHistories_LaptopUpdateId",
                table: "LaptopHistories",
                newName: "IX_LaptopHistories_LaptopUpdateID");

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopHistories_LaptopUpdates_LaptopUpdateID",
                table: "LaptopHistories",
                column: "LaptopUpdateID",
                principalTable: "LaptopUpdates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaptopHistories_LaptopUpdates_LaptopUpdateID",
                table: "LaptopHistories");

            migrationBuilder.RenameColumn(
                name: "LaptopUpdateID",
                table: "LaptopHistories",
                newName: "LaptopUpdateId");

            migrationBuilder.RenameIndex(
                name: "IX_LaptopHistories_LaptopUpdateID",
                table: "LaptopHistories",
                newName: "IX_LaptopHistories_LaptopUpdateId");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignabilityID",
                table: "LaptopHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopHistories_LaptopUpdates_LaptopUpdateId",
                table: "LaptopHistories",
                column: "LaptopUpdateId",
                principalTable: "LaptopUpdates",
                principalColumn: "Id");
        }
    }
}
