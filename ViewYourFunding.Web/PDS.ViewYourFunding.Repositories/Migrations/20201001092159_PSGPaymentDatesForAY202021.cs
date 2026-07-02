using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Run script to add PSG payment dates for academic year 2020 to 2021.
    /// </summary>
    public partial class PSGPaymentDatesForAY202021 : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string seed = "PDS.ViewYourFunding.Repositories.Scripts.SeedNextPaymentTypesAndFundingStreams.sql";
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.AddPSGPaymentDatesForAY202021.sql";

            var seedStream = assembly.GetManifestResourceStream(seed);
            if (seedStream != null)
            {
                using (var textStreamReader = new System.IO.StreamReader(seedStream))
                {
                    migrationBuilder.Sql(textStreamReader.ReadToEnd());
                }
            }

            var stream = assembly.GetManifestResourceStream(script);
            if (stream != null)
            {
                using (var textStreamReader = new System.IO.StreamReader(stream))
                {
                    migrationBuilder.Sql(textStreamReader.ReadToEnd());
                }
            }
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string seed = "PDS.ViewYourFunding.Repositories.Scripts.SeedNextPaymentTypesAndFundingStreams_ROLLBACK.sql";
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.AddPSGPaymentDatesForAY202021_ROLLBACK.sql";

            var seedStream = assembly.GetManifestResourceStream(seed);
            if (seedStream != null)
            {
                using (var textStreamReader = new System.IO.StreamReader(seedStream))
                {
                    migrationBuilder.Sql(textStreamReader.ReadToEnd());
                }
            }

            var stream = assembly.GetManifestResourceStream(script);
            if (stream != null)
            {
                using (var textStreamReader = new System.IO.StreamReader(stream))
                {
                    migrationBuilder.Sql(textStreamReader.ReadToEnd());
                }
            }
        }
    }
}