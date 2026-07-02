using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    ///  Updates Funding Periods Codes to NextPayments Table from UpdateDSGNextPaymentFundingPeriodCodes.sql.
    /// </summary>
    /// <seealso cref="Migration" />
    public partial class UpdateDSGNextPaymentDatesPeriodCodeFY2021 : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateDSGNextPaymentFundingPeriodCodes.sql";
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
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateDSGNextPaymentFundingPeriodCodes_ROLLBACK.sql";
            var stream = assembly.GetManifestResourceStream(script);
            if (stream != null)
            {
                using var textStreamReader = new System.IO.StreamReader(stream);
                migrationBuilder.Sql(textStreamReader.ReadToEnd());
            }
        }
    }
}