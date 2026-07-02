using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Extensions;

#nullable disable

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// UpdateUIFSMPeriodSettings Migration.
    /// </summary>
    /// <seealso cref="Migration" />
    public partial class UpdateUIFSMPeriodSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateUIFSMPeriodSettings.sql";
            migrationBuilder.ApplyMigrationScript(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateUIFSMPeriodSettings_ROLLBACK.sql";
            migrationBuilder.ApplyMigrationScript(script);
        }
    }
}
