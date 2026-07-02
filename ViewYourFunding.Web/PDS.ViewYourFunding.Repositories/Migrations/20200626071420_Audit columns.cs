using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// A migration to run to alter the DB, to add audit columns.
    /// </summary>
    public partial class Auditcolumns : Migration
    {
        /// <summary>
        /// A migration to run to alter the DB, to to add audit columns.
        /// </summary>
        /// <param name="migrationBuilder">The migration build to use.</param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PublicationLayouts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "PublicationLayouts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "PublicationLayouts",
                nullable: true);
        }

        /// <summary>
        /// Revert the migration.
        /// </summary>
        /// <param name="migrationBuilder">The migration build to use.</param>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PublicationLayouts");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "PublicationLayouts");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "PublicationLayouts");
        }
    }
}
