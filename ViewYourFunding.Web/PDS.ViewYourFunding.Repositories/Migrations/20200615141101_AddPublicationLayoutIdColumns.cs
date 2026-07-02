using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Migration to Add the new layoutId columns on the Publications table.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Migrations.Migration" />
    public partial class AddPublicationLayoutIdColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LocalAuthorityFundingBreakdownLayoutId",
                table: "Publications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocalAuthorityHistoryLayoutId",
                table: "Publications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocalAuthoritySpreadsheetLayoutId",
                table: "Publications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocalAuthoritySummaryLayoutId",
                table: "Publications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderFundingBreakdownLayoutId",
                table: "Publications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderHistoryLayoutId",
                table: "Publications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderSpreadsheetLayoutId",
                table: "Publications",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProviderSummaryLayoutId",
                table: "Publications",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
