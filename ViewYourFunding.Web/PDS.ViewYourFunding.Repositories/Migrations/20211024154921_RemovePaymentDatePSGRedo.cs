using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Delete the next payment date for PSG AY-2122.
    /// Previous migration wasn't successful on few environmets.
    /// </summary>
    public partial class RemovePaymentDatePSGRedo : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateNextPaymentPSG.sql";
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
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateNextPaymentPSG_ROLLBACK.sql";
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
