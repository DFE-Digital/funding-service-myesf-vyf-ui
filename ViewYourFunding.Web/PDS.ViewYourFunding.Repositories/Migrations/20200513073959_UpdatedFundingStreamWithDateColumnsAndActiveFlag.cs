using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Updates FundingStream Table With Created date,last updated at,deleted date, last updated By and Active status from UpdatedFundingStreamWithDateColumnsAndActiveFlag.sql.
    /// </summary>
    /// <seealso cref="Migration" />
    public partial class UpdatedFundingStreamWithDateColumnsAndActiveFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "FundingStreams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FundingStreams",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "FundingStreams",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "FundingStreams",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "FundingStreams",
                maxLength: 128,
                nullable: false,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "FundingStreams");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FundingStreams");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "FundingStreams");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "FundingStreams");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "FundingStreams");
        }
    }
}
