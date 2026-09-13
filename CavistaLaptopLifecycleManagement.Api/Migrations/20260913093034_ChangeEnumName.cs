using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEnumName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserLaptopStatus",
                table: "UserLaptops",
                newName: "LaptopStatus");

            migrationBuilder.AlterColumn<int>(
                name: "TicketStatus",
                table: "Tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "Idx_TicketNumber_UserId",
                table: "Tickets",
                column: "TicketNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Idx_TicketNumber_UserId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "LaptopStatus",
                table: "UserLaptops",
                newName: "UserLaptopStatus");

            migrationBuilder.AlterColumn<int>(
                name: "TicketStatus",
                table: "Tickets",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
