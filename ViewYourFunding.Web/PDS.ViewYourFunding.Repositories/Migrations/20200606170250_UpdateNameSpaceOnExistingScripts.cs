using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// Updating Name Space on Script Migrations.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Migrations.Migration" />
    public partial class UpdateNameSpaceOnExistingScripts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string deleteRedundantFundingStreamSettingsScript = "PDS.ViewYourFunding.Repositories.Scripts.DeleteRedundantFundingStreamSettings.sql";
            var deleteRedundantFundingStreamSettingsStream = assembly.GetManifestResourceStream(deleteRedundantFundingStreamSettingsScript);
            if (deleteRedundantFundingStreamSettingsStream != null)
            {
                using (var textStreamReader = new System.IO.StreamReader(deleteRedundantFundingStreamSettingsStream))
                {
                    migrationBuilder.Sql(textStreamReader.ReadToEnd());
                }
            }

            const string updateNextPaymentFundingPeriodCodesScript = "PDS.ViewYourFunding.Repositories.Scripts.UpdateNextPaymentFundingPeriodCodes.sql";
            var stream = assembly.GetManifestResourceStream(updateNextPaymentFundingPeriodCodesScript);
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
            const string deleteRedundantFundingStreamSettingsRollbackScript = "PDS.ViewYourFunding.Repositories.Scripts.DeleteRedundantFundingStreamSettings_ROLLBACK.sql";
            var deleteRedundantFundingStreamSettingsRollbackStream = assembly.GetManifestResourceStream(deleteRedundantFundingStreamSettingsRollbackScript);
            if (deleteRedundantFundingStreamSettingsRollbackStream != null)
            {
                using (var textStreamReader = new System.IO.StreamReader(deleteRedundantFundingStreamSettingsRollbackStream))
                {
                    migrationBuilder.Sql(textStreamReader.ReadToEnd());
                }
            }

            const string updateNextPaymentFundingPeriodCodesRollbackScript = "PDS.ViewYourFunding.Repositories.Scripts.UpdateNextPaymentFundingPeriodCodes_ROLLBACK.sql";
            var stream = assembly.GetManifestResourceStream(updateNextPaymentFundingPeriodCodesRollbackScript);
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
