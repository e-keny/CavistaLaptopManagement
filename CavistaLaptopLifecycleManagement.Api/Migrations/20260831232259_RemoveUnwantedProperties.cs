using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnwantedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaptopHistories_Tickets_TicketID",
                table: "LaptopHistories");

            migrationBuilder.DropIndex(
                name: "IX_LaptopHistories_TicketID",
                table: "LaptopHistories");

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "LaptopHistories");

            migrationBuilder.DropColumn(
                name: "ClosedBy",
                table: "LaptopHistories");

            migrationBuilder.DropColumn(
                name: "LaptopHistoryType",
                table: "LaptopHistories");

            migrationBuilder.DropColumn(
                name: "RepairID",
                table: "LaptopHistories");

            migrationBuilder.DropColumn(
                name: "TicketID",
                table: "LaptopHistories");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_LaptopHistories_LaptopHistoryId",
                table: "Tickets",
                column: "LaptopHistoryId",
                principalTable: "LaptopHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_LaptopHistories_LaptopHistoryId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_LaptopHistoryId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LaptopHistoryId",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ClosedAt",
                table: "LaptopHistories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "ClosedBy",
                table: "LaptopHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LaptopHistoryType",
                table: "LaptopHistories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RepairID",
                table: "LaptopHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketID",
                table: "LaptopHistories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaptopHistories_TicketID",
                table: "LaptopHistories",
                column: "TicketID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopHistories_Tickets_TicketID",
                table: "LaptopHistories",
                column: "TicketID",
                principalTable: "Tickets",
                principalColumn: "Id");
        }
    }
}
