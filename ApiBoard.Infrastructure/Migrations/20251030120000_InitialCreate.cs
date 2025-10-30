using System;
using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiBoard.Infrastructure.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Environments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Notes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Environments", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RequestCollections",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Notes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RequestCollections", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "EnvironmentVars",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                EnvironmentId = table.Column<Guid>(type: "TEXT", nullable: false),
                Key = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Value = table.Column<string>(type: "TEXT", nullable: false),
                IsSecret = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EnvironmentVars", x => x.Id);
                table.ForeignKey(
                    name: "FK_EnvironmentVars_Environments_EnvironmentId",
                    column: x => x.EnvironmentId,
                    principalTable: "Environments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ApiRequests",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CollectionId = table.Column<Guid>(type: "TEXT", nullable: false),
                EnvironmentId = table.Column<Guid>(type: "TEXT", nullable: true),
                Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Method = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                Url = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                HeadersJson = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "{}"),
                Body = table.Column<string>(type: "TEXT", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ApiRequests", x => x.Id);
                table.ForeignKey(
                    name: "FK_ApiRequests_Environments_EnvironmentId",
                    column: x => x.EnvironmentId,
                    principalTable: "Environments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_ApiRequests_RequestCollections_CollectionId",
                    column: x => x.CollectionId,
                    principalTable: "RequestCollections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "HealthChecks",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ApiRequestId = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                IntervalSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                AssertionsJson = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "{}"),
                CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HealthChecks", x => x.Id);
                table.ForeignKey(
                    name: "FK_HealthChecks_ApiRequests_ApiRequestId",
                    column: x => x.ApiRequestId,
                    principalTable: "ApiRequests",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ResponseLogs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                ApiRequestId = table.Column<Guid>(type: "TEXT", nullable: false),
                StatusCode = table.Column<int>(type: "INTEGER", nullable: false),
                DurationMs = table.Column<int>(type: "INTEGER", nullable: false),
                HeadersJson = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "{}"),
                Body = table.Column<string>(type: "TEXT", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ResponseLogs", x => x.Id);
                table.ForeignKey(
                    name: "FK_ResponseLogs_ApiRequests_ApiRequestId",
                    column: x => x.ApiRequestId,
                    principalTable: "ApiRequests",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "HealthResults",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                HealthCheckId = table.Column<Guid>(type: "TEXT", nullable: false),
                Status = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                StatusCode = table.Column<int>(type: "INTEGER", nullable: false),
                DurationMs = table.Column<int>(type: "INTEGER", nullable: false),
                FailureReason = table.Column<string>(type: "TEXT", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HealthResults", x => x.Id);
                table.ForeignKey(
                    name: "FK_HealthResults_HealthChecks_HealthCheckId",
                    column: x => x.HealthCheckId,
                    principalTable: "HealthChecks",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ApiRequests_CollectionId",
            table: "ApiRequests",
            column: "CollectionId");

        migrationBuilder.CreateIndex(
            name: "IX_ApiRequests_EnvironmentId",
            table: "ApiRequests",
            column: "EnvironmentId");

        migrationBuilder.CreateIndex(
            name: "IX_EnvironmentVars_EnvironmentId_Key",
            table: "EnvironmentVars",
            columns: new[] { "EnvironmentId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_HealthChecks_ApiRequestId",
            table: "HealthChecks",
            column: "ApiRequestId");

        migrationBuilder.CreateIndex(
            name: "IX_HealthResults_HealthCheckId",
            table: "HealthResults",
            column: "HealthCheckId");

        migrationBuilder.CreateIndex(
            name: "IX_ResponseLogs_ApiRequestId",
            table: "ResponseLogs",
            column: "ApiRequestId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EnvironmentVars");

        migrationBuilder.DropTable(
            name: "HealthResults");

        migrationBuilder.DropTable(
            name: "ResponseLogs");

        migrationBuilder.DropTable(
            name: "HealthChecks");

        migrationBuilder.DropTable(
            name: "ApiRequests");

        migrationBuilder.DropTable(
            name: "RequestCollections");

        migrationBuilder.DropTable(
            name: "Environments");
    }
}
