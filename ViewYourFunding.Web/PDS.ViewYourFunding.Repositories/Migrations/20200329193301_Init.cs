using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// The Initial schema migration.
    /// </summary>
    /// <seealso cref="Migration" />
    public partial class Init : Migration
    {
        /// <summary>
        /// <para>
        /// Builds the operations that will migrate the database 'up'.
        /// </para>
        /// <para>
        /// That is, builds the operations that will take the database from the state left in by the
        /// previous migration so that it is up-to-date with regard to this migration.
        /// </para>
        /// <para>
        /// This method must be overridden in each class the inherits from <see cref="T:Microsoft.EntityFrameworkCore.Migrations.Migration" />.
        /// </para>
        /// </summary>
        /// <param name="migrationBuilder">The <see cref="T:Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder" /> that will build the operations.</param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FundingStreams",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FundingStreamCode = table.Column<string>(nullable: true),
                    FundingStreamName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundingStreams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingName = table.Column<string>(nullable: true),
                    SettingDescription = table.Column<string>(nullable: true),
                    ValueDataType = table.Column<int>(),
                    ValuesAreEditable = table.Column<bool>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publications",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FundingStreamId = table.Column<int>(),
                    PublishedDate = table.Column<DateTime>(),
                    FundingPeriodCode = table.Column<string>(nullable: true),
                    CutOffDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(),
                    UIModelVersion = table.Column<int>(nullable: true),
                    SpreadsheetModelVersion = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(),
                    LastUpdatedAt = table.Column<DateTime>(),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publications_FundingStreams_FundingStreamId",
                        column: x => x.FundingStreamId,
                        principalTable: "FundingStreams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SettingValues",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FundingStreamId = table.Column<int>(nullable: true),
                    SettingId = table.Column<int>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(),
                    LastUpdatedAt = table.Column<DateTime>(),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingValues_FundingStreams_FundingStreamId",
                        column: x => x.FundingStreamId,
                        principalTable: "FundingStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SettingValues_Settings_SettingId",
                        column: x => x.SettingId,
                        principalTable: "Settings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publications_FundingStreamId",
                table: "Publications",
                column: "FundingStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_SettingValues_FundingStreamId",
                table: "SettingValues",
                column: "FundingStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_SettingValues_SettingId",
                table: "SettingValues",
                column: "SettingId");
        }

        /// <summary>
        /// <para>
        /// Builds the operations that will migrate the database 'down'.
        /// </para>
        /// <para>
        /// That is, builds the operations that will take the database from the state left in by
        /// this migration so that it returns to the state that it was in before this migration was applied.
        /// </para>
        /// <para>
        /// This method must be overridden in each class the inherits from <see cref="T:Microsoft.EntityFrameworkCore.Migrations.Migration" /> if
        /// both 'up' and 'down' migrations are to be supported. If it is not overridden, then calling it
        /// will throw and it will not be possible to migrate in the 'down' direction.
        /// </para>
        /// </summary>
        /// <param name="migrationBuilder">The <see cref="T:Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder" /> that will build the operations.</param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Publications");

            migrationBuilder.DropTable(
                name: "SettingValues");

            migrationBuilder.DropTable(
                name: "FundingStreams");

            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
