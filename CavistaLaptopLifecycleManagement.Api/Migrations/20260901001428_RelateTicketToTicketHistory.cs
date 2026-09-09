using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class RelateTicketToTicketHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_LaptopHistories_LaptopHistoryId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_LaptopHistoryId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_TicketHistories_TicketID",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "LaptopHistoryId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_TicketID",
                table: "TicketHistories",
                column: "TicketID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketHistories_TicketID",
                table: "TicketHistories");

            migrationBuilder.AddColumn<Guid>(
                name: "LaptopHistoryId",
                table: "Tickets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_LaptopHistoryId",
                table: "Tickets",
                column: "LaptopHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_TicketID",
                table: "TicketHistories",
                column: "TicketID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_LaptopHistories_LaptopHistoryId",
                table: "Tickets",
                column: "LaptopHistoryId",
                principalTable: "LaptopHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
