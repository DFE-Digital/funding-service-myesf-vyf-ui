using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Add new column.
    /// </summary>
    public partial class AddFundingStreamBusinessAllocationNameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FundingStreamBusinessAllocationName",
                table: "FundingStreams",
                nullable: true);

            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string seed = "PDS.ViewYourFunding.Repositories.Scripts.SeedFundingStreamBusinessAllocationName.sql";

            var seedStream = assembly.GetManifestResourceStream(seed);
            if (seedStream != null)
            {
                using (var textStreamReader = new System.IO.StreamReader(seedStream))
                {
                    migrationBuilder.Sql(textStreamReader.ReadToEnd());
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FundingStreamBusinessAllocationName",
                table: "FundingStreams");
        }
    }
}
