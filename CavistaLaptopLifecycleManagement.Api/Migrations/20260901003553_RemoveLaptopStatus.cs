using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLaptopStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserLaptops");

            migrationBuilder.AddColumn<int>(
                name: "UserLaptopHistoryStatus",
                table: "LaptopHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserLaptopHistoryStatus",
                table: "LaptopHistories");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserLaptops",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
