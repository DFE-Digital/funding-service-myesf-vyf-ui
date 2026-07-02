using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// A migration to run to alter the DB, to drop layout columns and move them to their own table.
    /// </summary>
    public partial class AddPublicationLayoutsTable : Migration
    {
        /// <summary>
        /// A migration to run to alter the DB, to drop layout columns and move them to their own table.
        /// </summary>
        /// <param name="migrationBuilder">The migration build to use.</param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalAuthorityFundingBreakdownLayoutId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "LocalAuthorityHistoryLayoutId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "LocalAuthoritySpreadsheetLayoutId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "LocalAuthoritySummaryLayoutId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "ProviderFundingBreakdownLayoutId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "ProviderHistoryLayoutId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "ProviderSpreadsheetLayoutId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "ProviderSummaryLayoutId",
                table: "Publications");

            migrationBuilder.CreateTable(
                name: "PublicationLayouts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicationId = table.Column<int>(nullable: false),
                    FundingViewType = table.Column<int>(nullable: false),
                    FundingViewScope = table.Column<int>(nullable: false),
                    LayoutId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicationLayouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicationLayouts_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PublicationLayouts_PublicationId",
                table: "PublicationLayouts",
                column: "PublicationId");
        }

        /// <summary>
        /// Revert the migration.
        /// </summary>
        /// <param name="migrationBuilder">The migration build to use.</param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicationLayouts");

            migrationBuilder.AddColumn<Guid>(
                name: "LocalAuthorityFundingBreakdownLayoutId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocalAuthorityHistoryLayoutId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocalAuthoritySpreadsheetLayoutId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocalAuthoritySummaryLayoutId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderFundingBreakdownLayoutId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderHistoryLayoutId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderSpreadsheetLayoutId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderSummaryLayoutId",
                table: "Publications",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}