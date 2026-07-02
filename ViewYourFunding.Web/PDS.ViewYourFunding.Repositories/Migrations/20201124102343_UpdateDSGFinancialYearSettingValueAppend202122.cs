using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Appends '202122' to the financial year setting for DSG.
    /// </summary>
    /// <seealso cref="Migration"/>
    public partial class UpdateDSGFinancialYearSettingValueAppend202122 : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateDSGFinancialYearSettingValueAppend202122.sql";
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
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateDSGFinancialYearSettingValueAppend202122_ROLLBACK.sql";
            var stream = assembly.GetManifestResourceStream(script);
            if (stream != null)
            {
                using var textStreamReader = new System.IO.StreamReader(stream);
                migrationBuilder.Sql(textStreamReader.ReadToEnd());
            }
        }
    }
}