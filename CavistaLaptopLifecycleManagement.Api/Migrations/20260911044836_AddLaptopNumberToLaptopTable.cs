using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLaptopNumberToLaptopTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LaptopNumber",
                table: "UserLaptops",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LaptopNumber",
                table: "UserLaptops");
        }
    }
}
