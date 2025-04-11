using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bcs.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ClubSynchronizationHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clubs_ClubStats_LatestStatsId",
                table: "Clubs");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_PlayerSynchronizationHistory_PlayerSynchronizat~",
                table: "PlayerStats");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_PlayerStats_LatestStatsId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "ClubStats");

            migrationBuilder.DropTable(
                name: "PlayerSynchronizationHistory");

            migrationBuilder.DropIndex(
                name: "IX_Players_LatestStatsId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Clubs_LatestStatsId",
                table: "Clubs");

            migrationBuilder.DropColumn(
                name: "LatestStatsId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "LatestStatsId",
                table: "Clubs");

            migrationBuilder.RenameColumn(
                name: "PlayerSynchronizationHistoryId",
                table: "PlayerStats",
                newName: "ClubSynchronizationHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerStats_PlayerSynchronizationHistoryId",
                table: "PlayerStats",
                newName: "IX_PlayerStats_ClubSynchronizationHistoryId");

            migrationBuilder.AddColumn<int>(
                name: "Trophies",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PlayerStats",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Trophies",
                table: "Clubs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClubSynchronizationHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Trophies = table.Column<int>(type: "integer", nullable: false),
                    ClubTag = table.Column<string>(type: "character varying(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubSynchronizationHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClubSynchronizationHistory_Clubs_ClubTag",
                        column: x => x.ClubTag,
                        principalTable: "Clubs",
                        principalColumn: "Tag",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubSynchronizationHistory_ClubTag",
                table: "ClubSynchronizationHistory",
                column: "ClubTag");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_ClubSynchronizationHistory_ClubSynchronizationH~",
                table: "PlayerStats",
                column: "ClubSynchronizationHistoryId",
                principalTable: "ClubSynchronizationHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_ClubSynchronizationHistory_ClubSynchronizationH~",
                table: "PlayerStats");

            migrationBuilder.DropTable(
                name: "ClubSynchronizationHistory");

            migrationBuilder.DropColumn(
                name: "Trophies",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "Trophies",
                table: "Clubs");

            migrationBuilder.RenameColumn(
                name: "ClubSynchronizationHistoryId",
                table: "PlayerStats",
                newName: "PlayerSynchronizationHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerStats_ClubSynchronizationHistoryId",
                table: "PlayerStats",
                newName: "IX_PlayerStats_PlayerSynchronizationHistoryId");

            migrationBuilder.AddColumn<int>(
                name: "LatestStatsId",
                table: "Players",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LatestStatsId",
                table: "Clubs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClubStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClubTag = table.Column<string>(type: "character varying(20)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Trophies = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClubStats_Clubs_ClubTag",
                        column: x => x.ClubTag,
                        principalTable: "Clubs",
                        principalColumn: "Tag",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerSynchronizationHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSynchronizationHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_LatestStatsId",
                table: "Players",
                column: "LatestStatsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_LatestStatsId",
                table: "Clubs",
                column: "LatestStatsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClubStats_ClubTag",
                table: "ClubStats",
                column: "ClubTag");

            migrationBuilder.AddForeignKey(
                name: "FK_Clubs_ClubStats_LatestStatsId",
                table: "Clubs",
                column: "LatestStatsId",
                principalTable: "ClubStats",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_PlayerSynchronizationHistory_PlayerSynchronizat~",
                table: "PlayerStats",
                column: "PlayerSynchronizationHistoryId",
                principalTable: "PlayerSynchronizationHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_PlayerStats_LatestStatsId",
                table: "Players",
                column: "LatestStatsId",
                principalTable: "PlayerStats",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
