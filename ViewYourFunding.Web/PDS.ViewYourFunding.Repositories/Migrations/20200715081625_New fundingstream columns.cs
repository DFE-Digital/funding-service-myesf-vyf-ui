using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Implementations;

/// <summary>
/// A.
/// </summary>
namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// B.
    /// </summary>
    public partial class Newfundingstreamcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FundingStreamCodePubliclyKnown",
                table: "FundingStreams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FundingStreamNameWithinSentence",
                table: "FundingStreams",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RelevantForNational",
                table: "FundingStreams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RelevantForOrganisations_LoggedIn",
                table: "FundingStreams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RelevantForOrganisations_Public",
                table: "FundingStreams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RelevantForProviders_LoggedIn",
                table: "FundingStreams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RelevantForProviders_Public",
                table: "FundingStreams",
                nullable: false,
                defaultValue: false);

            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.SetRelevantForFields.sql";
            var stream = assembly.GetManifestResourceStream(script);
            if (stream != null)
            {
                using (var textStreamReader = new System.IO.StreamReader(stream))
                {
                    migrationBuilder.Sql(textStreamReader.ReadToEnd());
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FundingStreamCodePubliclyKnown",
                table: "FundingStreams");

            migrationBuilder.DropColumn(
                name: "FundingStreamNameWithinSentence",
                table: "FundingStreams");

            migrationBuilder.DropColumn(
                name: "RelevantForNational",
                table: "FundingStreams");

            migrationBuilder.DropColumn(
                name: "RelevantForOrganisations_LoggedIn",
                table: "FundingStreams");

            migrationBuilder.DropColumn(
                name: "RelevantForOrganisations_Public",
                table: "FundingStreams");

            migrationBuilder.DropColumn(
                name: "RelevantForProviders_LoggedIn",
                table: "FundingStreams");

            migrationBuilder.DropColumn(
                name: "RelevantForProviders_Public",
                table: "FundingStreams");

            var assembly = System.Reflection.Assembly.GetAssembly(typeof(Context));
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.SetRelevantForFields_ROLLBACK.sql";
            var stream = assembly.GetManifestResourceStream(script);
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