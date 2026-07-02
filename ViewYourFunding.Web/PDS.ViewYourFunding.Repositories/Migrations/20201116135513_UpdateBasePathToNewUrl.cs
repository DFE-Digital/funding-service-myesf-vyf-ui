using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    ///  Updates BasePath to use the new "view-latest-funding" url from UpdateBasePathToNewUrl.sql.
    /// </summary>
    /// <seealso cref="Migration" />
    public partial class UpdateBasePathToNewUrl : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateBasePathToNewUrl.sql";
            var stream = assembly.GetManifestResourceStream(script);
            if (stream != null)
            {
                using var textStreamReader = new System.IO.StreamReader(stream);
                migrationBuilder.Sql(textStreamReader.ReadToEnd());
            }
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateBasePathToNewUrl_ROLLBACK.sql";
            var stream = assembly.GetManifestResourceStream(script);
            if (stream != null)
            {
                using var textStreamReader = new System.IO.StreamReader(stream);
                migrationBuilder.Sql(textStreamReader.ReadToEnd());
            }
        }
    }
}