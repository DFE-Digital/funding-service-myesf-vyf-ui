using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// The Delete Redundant Funding stream settings migration class.
    /// </summary>
    /// <seealso cref="Migration" />
    public partial class DeleteRedundantFundingStreamSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string script = "ViewYourFunding.Repositories.Scripts.DeleteRedundantFundingStreamSettings.sql";
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            var stream = assembly.GetManifestResourceStream(script);
            if (stream == null)
            {
                return;
            }

            using (var textStreamReader = new System.IO.StreamReader(stream))
            {
                migrationBuilder.Sql(textStreamReader.ReadToEnd());
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string script = "ViewYourFunding.Repositories.Scripts.DeleteRedundantFundingStreamSettings_ROLLBACK.sql";
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            var stream = assembly.GetManifestResourceStream(script);
            if (stream == null)
            {
                return;
            }

            using (var textStreamReader = new System.IO.StreamReader(stream))
            {
                migrationBuilder.Sql(textStreamReader.ReadToEnd());
            }
        }
    }
}
