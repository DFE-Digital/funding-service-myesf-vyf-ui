using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;
using System.Reflection;

namespace PDS.ViewYourFunding.Repositories.Extensions
{
    /// <summary>
    /// Extensions for applying migrations.
    /// </summary>
    public static class MigrationExtensions
    {
        /// <summary>
        /// Applies the migration script.
        /// </summary>
        /// <param name="migrationBuilder">The migration builder.</param>
        /// <param name="scriptToApply">The script to apply.</param>
        public static void ApplyMigrationScript(this MigrationBuilder migrationBuilder, string scriptToApply)
        {
            var assembly = Assembly.GetAssembly(typeof(Context));
            if (assembly == null)
            {
                return;
            }

            var stream = assembly.GetManifestResourceStream(scriptToApply);
            if (stream == null)
            {
                return;
            }

            using var textStreamReader = new System.IO.StreamReader(stream);
            migrationBuilder.Sql(textStreamReader.ReadToEnd());
        }
    }
}