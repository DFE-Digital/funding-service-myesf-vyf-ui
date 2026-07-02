using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Extensions;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    ///  The UpdateLARECShowOnOrganisationLoggedInView Migration.
    /// </summary>
    /// <seealso cref="Migration" />
    public partial class UpdateLARECShowOnOrganisationLoggedInView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateLARECShowOnOrganisationLoggedInView.sql";
            migrationBuilder.ApplyMigrationScript(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.UpdateLARECShowOnOrganisationLoggedInView_ROLLBACK.sql";
            migrationBuilder.ApplyMigrationScript(script);
        }
    }
}
