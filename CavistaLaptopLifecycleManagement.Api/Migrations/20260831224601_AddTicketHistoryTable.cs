using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaptopHistories_LaptopUpdates_LaptopUpdateID",
                table: "LaptopHistories");

            migrationBuilder.DropTable(
                name: "LaptopUpdates");

            migrationBuilder.DropIndex(
                name: "IX_LaptopHistories_LaptopUpdateID",
                table: "LaptopHistories");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "LaptopHistoryStatus",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ResolvedBy",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "LaptopUpdateID",
                table: "LaptopHistories",
                newName: "ActionBy");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "LaptopHistories",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TicketHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLaptopID = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketID = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ClosedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ActionBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    AssignedTo = table.Column<Guid>(type: "uuid", nullable: true),
                    ResolvedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TicketHistoryStatus = table.Column<int>(type: "integer", nullable: true),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    Created_At = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketHistories_Tickets_TicketID",
                        column: x => x.TicketID,
                        principalTable: "Tickets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketHistories_UserLaptops_UserLaptopID",
                        column: x => x.UserLaptopID,
                        principalTable: "UserLaptops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_TicketID",
                table: "TicketHistories",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_UserLaptopID",
                table: "TicketHistories",
                column: "UserLaptopID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "LaptopHistories");

            migrationBuilder.RenameColumn(
                name: "ActionBy",
                table: "LaptopHistories",
                newName: "LaptopUpdateID");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedTo",
                table: "Tickets",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LaptopHistoryStatus",
                table: "Tickets",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ResolvedBy",
                table: "Tickets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LaptopUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Created_At = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsAssigned = table.Column<bool>(type: "boolean", nullable: true),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaptopUpdates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaptopHistories_LaptopUpdateID",
                table: "LaptopHistories",
                column: "LaptopUpdateID");

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopHistories_LaptopUpdates_LaptopUpdateID",
                table: "LaptopHistories",
                column: "LaptopUpdateID",
                principalTable: "LaptopUpdates",
                principalColumn: "Id");
        }
    }
}
