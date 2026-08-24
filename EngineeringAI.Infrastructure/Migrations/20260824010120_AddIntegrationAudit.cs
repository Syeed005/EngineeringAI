using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIntegrationAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegrationAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: true),
                    EquipmentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProcessedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationAudit", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationAudit_EquipmentId",
                table: "IntegrationAudit",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationAudit_EventId",
                table: "IntegrationAudit",
                column: "EventId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegrationAudit");
        }
    }
}
