using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Migration to delete the redundant Use Secure Api global setting.
    /// </summary>
    /// <seealso cref="Migration" />
    public partial class DeleteRedundantUseSecureApiGlobalSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.DeleteRedundantUseSecureApiSetting.sql";
            var stream = assembly.GetManifestResourceStream(script);
            if (stream != null)
            {
                using (var textStreamReader = new System.IO.StreamReader(stream))
                {
                    migrationBuilder.Sql(textStreamReader.ReadToEnd());
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.DeleteRedundantUseSecureApiSetting_ROLLBACK.sql";
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
