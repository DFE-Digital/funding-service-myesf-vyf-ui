using Microsoft.EntityFrameworkCore.Migrations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Adds column FundingPeriodCode to NextPayments Table.
    /// </summary>
    public partial class AddFundingPeriodCodeOnNextPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FundingPeriodCode",
                table: "NextPayments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FundingPeriodCode",
                table: "NextPayments");
        }
    }
}