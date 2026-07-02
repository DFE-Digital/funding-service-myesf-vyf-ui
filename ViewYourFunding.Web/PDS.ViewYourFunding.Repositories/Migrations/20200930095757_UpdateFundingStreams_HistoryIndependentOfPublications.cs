using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Migration to set default values for the HistoryIndependentOfPublications column in FundingStreams table.
    /// </summary>
    public partial class UpdateFundingStreams_HistoryIndependentOfPublications : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.SetHistoryIndependentOfPublications.sql";
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
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.SetHistoryIndependentOfPublications_ROLLBACK.sql";
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
