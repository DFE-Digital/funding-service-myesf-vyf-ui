using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Run script to update General setting text.
    /// </summary>
    public partial class UpdateGeneralSettingText : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateGeneralSettingText.sql";
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
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateGeneralSettingText_Rollback.sql";
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
