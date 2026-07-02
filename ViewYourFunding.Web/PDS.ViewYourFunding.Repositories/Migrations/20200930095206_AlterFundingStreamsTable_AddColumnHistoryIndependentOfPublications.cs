using Microsoft.EntityFrameworkCore.Migrations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// DB migration to add a new column HistoryIndependentOfPublications in the FundingStreams table.
    /// </summary>
    public partial class AlterFundingStreamsTable_AddColumnHistoryIndependentOfPublications : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HistoryIndependentOfPublications",
                table: "FundingStreams",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HistoryIndependentOfPublications",
                table: "FundingStreams");
        }
    }
}
