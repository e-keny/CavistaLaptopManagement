using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CavistaLaptopLifecycleManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignabilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsAssigned = table.Column<bool>(type: "boolean", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    ActionBy = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    Created_At = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignabilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    ActionBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionOn = table.Column<string>(type: "text", nullable: false),
                    ActionOnId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedTo = table.Column<Guid>(type: "uuid", nullable: true),
                    ResolvedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    LaptopHistoryStatus = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    Created_At = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Auth0UserId = table.Column<string>(type: "text", nullable: true),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LastLogin = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Roles = table.Column<string>(type: "text", nullable: false),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    Created_At = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLaptops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: true),
                    AssetName = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    AssetLocation = table.Column<string>(type: "text", nullable: false),
                    EmployeeDepartment = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    EstimationUsefulLifeYear = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DepreciationEstimationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    WarrantyExpirationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PurchaseYear = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    Created_At = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLaptops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLaptops_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LaptopHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserLaptopID = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketID = table.Column<Guid>(type: "uuid", nullable: true),
                    AssignabilityID = table.Column<Guid>(type: "uuid", nullable: true),
                    RepairID = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ClosedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ClosedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LaptopHistoryType = table.Column<int>(type: "integer", nullable: true),
                    LaptopUpdateId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeprecated = table.Column<bool>(type: "boolean", nullable: false),
                    Created_At = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaptopHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaptopHistories_Assignabilities_LaptopUpdateId",
                        column: x => x.LaptopUpdateId,
                        principalTable: "Assignabilities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LaptopHistories_Tickets_TicketID",
                        column: x => x.TicketID,
                        principalTable: "Tickets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LaptopHistories_UserLaptops_UserLaptopID",
                        column: x => x.UserLaptopID,
                        principalTable: "UserLaptops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaptopHistories_LaptopUpdateId",
                table: "LaptopHistories",
                column: "LaptopUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_LaptopHistories_TicketID",
                table: "LaptopHistories",
                column: "TicketID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaptopHistories_UserLaptopID",
                table: "LaptopHistories",
                column: "UserLaptopID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLaptops_UserID",
                table: "UserLaptops",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "LaptopHistories");

            migrationBuilder.DropTable(
                name: "Assignabilities");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "UserLaptops");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
