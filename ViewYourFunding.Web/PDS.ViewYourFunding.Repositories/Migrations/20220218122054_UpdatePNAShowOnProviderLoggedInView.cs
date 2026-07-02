using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// UpdatePNAShowOnProviderLoggedInView Migration.
    /// </summary>
    /// <seealso cref="Migration" />
    public partial class UpdatePNAShowOnProviderLoggedInView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdatePNAShowOnProviderLoggedInView.sql";
            ApplyMigrationScript(migrationBuilder, script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdatePNAShowOnProviderLoggedInView_ROLLBACK.sql";
            ApplyMigrationScript(migrationBuilder, script);
        }

        private void ApplyMigrationScript(MigrationBuilder migrationBuilder, string scriptToApply)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            var stream = assembly.GetManifestResourceStream(scriptToApply);
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
