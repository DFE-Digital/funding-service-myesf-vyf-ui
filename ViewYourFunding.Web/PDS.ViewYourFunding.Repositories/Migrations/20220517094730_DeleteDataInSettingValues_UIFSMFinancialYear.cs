using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Extensions;

#nullable disable

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// DeleteDataInSettingValues_UIFSMFinancialYear Migration.
    /// </summary>
    /// <seealso cref="Migration" />
    public partial class DeleteDataInSettingValues_UIFSMFinancialYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.DeleteDataInSettingValues_UIFSMFinancialYear.sql";
            migrationBuilder.ApplyMigrationScript(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.DeleteDataInSettingValues_UIFSMFinancialYear_ROLLBACK.sql";
            migrationBuilder.ApplyMigrationScript(script);
        }
    }
}
