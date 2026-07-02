using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Add Pupil premium friendly name for business allocation page.
    /// </summary>
    public partial class AddPupilPremiumFriendlyNameForBusinessAllocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string seed = "PDS.ViewYourFunding.Repositories.Scripts.SeedFundingStreamBusinessAllocationNamePP.sql";

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
        }
    }
}
