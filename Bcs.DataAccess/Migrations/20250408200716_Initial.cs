using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bcs.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ClubStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClubTag = table.Column<string>(type: "character varying(20)", nullable: false),
                    Trophies = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubStats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    Tag = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RequiredTrophies = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    BadgeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LatestStatsId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.Tag);
                    table.ForeignKey(
                        name: "FK_Clubs_ClubStats_LatestStatsId",
                        column: x => x.LatestStatsId,
                        principalTable: "ClubStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerTag = table.Column<string>(type: "text", nullable: false),
                    PlayerSynchronizationHistoryId = table.Column<int>(type: "integer", nullable: false),
                    Trophies = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerStats_PlayerSynchronizationHistory_PlayerSynchronizat~",
                        column: x => x.PlayerSynchronizationHistoryId,
                        principalTable: "PlayerSynchronizationHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Tag = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClubTag = table.Column<string>(type: "character varying(20)", nullable: true),
                    LatestStatsId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Tag);
                    table.ForeignKey(
                        name: "FK_Players_Clubs_ClubTag",
                        column: x => x.ClubTag,
                        principalTable: "Clubs",
                        principalColumn: "Tag",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Players_PlayerStats_LatestStatsId",
                        column: x => x.LatestStatsId,
                        principalTable: "PlayerStats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClubStats_ClubTag",
                table: "ClubStats",
                column: "ClubTag");

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_LatestStatsId",
                table: "Clubs",
                column: "LatestStatsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clubs_Tag",
                table: "Clubs",
                column: "Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_PlayerSynchronizationHistoryId",
                table: "PlayerStats",
                column: "PlayerSynchronizationHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_PlayerTag",
                table: "PlayerStats",
                column: "PlayerTag");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ClubTag",
                table: "Players",
                column: "ClubTag");

            migrationBuilder.CreateIndex(
                name: "IX_Players_LatestStatsId",
                table: "Players",
                column: "LatestStatsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_Tag",
                table: "Players",
                column: "Tag",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClubStats_Clubs_ClubTag",
                table: "ClubStats",
                column: "ClubTag",
                principalTable: "Clubs",
                principalColumn: "Tag",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Players_PlayerTag",
                table: "PlayerStats",
                column: "PlayerTag",
                principalTable: "Players",
                principalColumn: "Tag",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubStats_Clubs_ClubTag",
                table: "ClubStats");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Clubs_ClubTag",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_PlayerSynchronizationHistory_PlayerSynchronizat~",
                table: "PlayerStats");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Players_PlayerTag",
                table: "PlayerStats");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "ClubStats");

            migrationBuilder.DropTable(
                name: "PlayerSynchronizationHistory");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "PlayerStats");
        }
    }
}
