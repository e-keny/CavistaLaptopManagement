using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTabletoLaptopUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaptopHistories_Assignabilities_LaptopUpdateId",
                table: "LaptopHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignabilities",
                table: "Assignabilities");

            migrationBuilder.RenameTable(
                name: "Assignabilities",
                newName: "LaptopUpdates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaptopUpdates",
                table: "LaptopUpdates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopHistories_LaptopUpdates_LaptopUpdateId",
                table: "LaptopHistories",
                column: "LaptopUpdateId",
                principalTable: "LaptopUpdates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaptopHistories_LaptopUpdates_LaptopUpdateId",
                table: "LaptopHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaptopUpdates",
                table: "LaptopUpdates");

            migrationBuilder.RenameTable(
                name: "LaptopUpdates",
                newName: "Assignabilities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignabilities",
                table: "Assignabilities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopHistories_Assignabilities_LaptopUpdateId",
                table: "LaptopHistories",
                column: "LaptopUpdateId",
                principalTable: "Assignabilities",
                principalColumn: "Id");
        }
    }
}
