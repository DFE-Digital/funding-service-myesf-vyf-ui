using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;
using System.Diagnostics.CodeAnalysis;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    ///  Updates Academic and Financial settings types from UpdateAcademicFinancialYearDataTypes.sql.
    /// </summary>
    /// <seealso cref="Migration" />
    [ExcludeFromCodeCoverage]
    public partial class UpdateAcademicFinancialYearDataTypes : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateAcademicFinancialYearDataTypes.sql";
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
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateAcademicFinancialYearDataTypes_ROLLBACK.sql";
            var stream = assembly.GetManifestResourceStream(script);
            if (stream != null)
            {
                using var textStreamReader = new System.IO.StreamReader(stream);
                migrationBuilder.Sql(textStreamReader.ReadToEnd());
            }
        }
    }
}
