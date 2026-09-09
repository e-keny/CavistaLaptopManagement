using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLaptops_Users_UserID",
                table: "UserLaptops");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserLaptops",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLaptops_UserID",
                table: "UserLaptops",
                newName: "IX_UserLaptops_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLaptops_Users_UserId",
                table: "UserLaptops",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLaptops_Users_UserId",
                table: "UserLaptops");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserLaptops",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_UserLaptops_UserId",
                table: "UserLaptops",
                newName: "IX_UserLaptops_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLaptops_Users_UserID",
                table: "UserLaptops",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
