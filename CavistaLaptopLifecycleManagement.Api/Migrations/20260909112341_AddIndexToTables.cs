using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_UserLaptops_UserId",
                table: "UserLaptops",
                newName: "Idx_UserLaptop_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketHistories_TicketID",
                table: "TicketHistories",
                newName: "Idx_TicketHistory_TicketID");

            migrationBuilder.CreateIndex(
                name: "Idx_User_Auth0UserId",
                table: "Users",
                column: "Auth0UserId");

            migrationBuilder.CreateIndex(
                name: "Idx_Ticket_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "Idx_TicketComment_TicketId",
                table: "TicketComments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "Idx_Notification_UserId",
                table: "Notifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Idx_User_Auth0UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "Idx_Ticket_UserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "Idx_TicketComment_TicketId",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "Idx_Notification_UserId",
                table: "Notifications");

            migrationBuilder.RenameIndex(
                name: "Idx_UserLaptop_UserId",
                table: "UserLaptops",
                newName: "IX_UserLaptops_UserId");

            migrationBuilder.RenameIndex(
                name: "Idx_TicketHistory_TicketID",
                table: "TicketHistories",
                newName: "IX_TicketHistories_TicketID");
        }
    }
}
