using Microsoft.EntityFrameworkCore.Migrations;
using PDS.ViewYourFunding.Repositories.Extensions;
using System;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// The Update1416ParentProviderTypeAndAcademicSettings migration.
    /// </summary>
    /// <seealso cref=Migration" />
    public partial class Update1416ParentProviderTypeAndAcademicSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.Update1416ParentProviderTypeAndAcademicSettings.sql";
            migrationBuilder.ApplyMigrationScript(script);

            // Auto generated
            migrationBuilder.AlterColumn<int>(
               name: "FundingStreamId",
               table: "Publications",
               type: "int",
               nullable: false,
               oldClrType: typeof(int),
               oldType: "int")
               .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "TypeCode",
                table: "NextPaymentTypes",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32)
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "FundingStreamId",
                table: "NextPaymentTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "NextPaymentTypeId",
                table: "NextPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NextPaymentDate",
                table: "NextPayments",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "FundingStreamId",
                table: "NextPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            const string script = "PDS.ViewYourFunding.Repositories.Scripts.Update1416ParentProviderTypeAndAcademicSettings_ROLLBACK.sql";
            migrationBuilder.ApplyMigrationScript(script);

            // Auto generated
            migrationBuilder.AlterColumn<int>(
               name: "FundingStreamId",
               table: "Publications",
               type: "int",
               nullable: false,
               oldClrType: typeof(int),
               oldType: "int")
               .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<string>(
                name: "TypeCode",
                table: "NextPaymentTypes",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "FundingStreamId",
                table: "NextPaymentTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "NextPaymentTypeId",
                table: "NextPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NextPaymentDate",
                table: "NextPayments",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "FundingStreamId",
                table: "NextPayments",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 2);
        }
    }
}