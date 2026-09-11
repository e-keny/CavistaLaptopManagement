using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyAndReceiptColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "Idx_UserLaptop_UserId",
                table: "UserLaptops",
                newName: "Idx_Laptop_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "UserLaptops",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Receipt",
                table: "UserLaptops",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "UserLaptops");

            migrationBuilder.DropColumn(
                name: "Receipt",
                table: "UserLaptops");

            migrationBuilder.RenameIndex(
                name: "Idx_Laptop_UserId",
                table: "UserLaptops",
                newName: "Idx_UserLaptop_UserId");
        }
    }
}
