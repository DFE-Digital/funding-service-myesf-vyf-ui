using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Updates Funding Periods Codes to NextPayments Table from UpdateNextPaymentFundingPeriodCodes.sql.
    /// </summary>
    public partial class UpdateNextPaymentFundingPeriodCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string script = "ViewYourFunding.Repositories.Scripts.UpdateNextPaymentFundingPeriodCodes.sql";
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string script = "ViewYourFunding.Repositories.Scripts.UpdateNextPaymentFundingPeriodCodes_ROLLBACK.sql";
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