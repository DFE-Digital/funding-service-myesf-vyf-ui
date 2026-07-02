using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Update the Funding Stream name for 16 to 19.
    /// </summary>
    public partial class Update1619FundingStreamName : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.Update1619FundingStreamName.sql";
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

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.Update1619FundingStreamName_ROLLBACK.sql";
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
