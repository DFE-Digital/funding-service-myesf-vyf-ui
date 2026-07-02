using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Migration to add DeletedOn and DeleteAt columns to the FundingStreams table.
    /// </summary>
    public partial class UpdateFundingStreamDeleteColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "FundingStreams");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "FundingStreams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "FundingStreams");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "FundingStreams",
                type: "datetime2",
                nullable: true);
        }
    }
}
