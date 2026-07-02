using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    public partial class AddNextPaymentsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SettingValues_FundingStreams_FundingStreamId",
                table: "SettingValues");

            migrationBuilder.DropForeignKey(
                name: "FK_SettingValues_Settings_SettingId",
                table: "SettingValues");

            migrationBuilder.AlterColumn<int>(
                name: "SettingId",
                table: "SettingValues",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FundingStreamId",
                table: "SettingValues",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "NextPaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeCode = table.Column<string>(maxLength: 32, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FundingStreamId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NextPaymentTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NextPaymentTypes_FundingStreams_FundingStreamId",
                        column: x => x.FundingStreamId,
                        principalTable: "FundingStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "NextPayments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NextPaymentDate = table.Column<DateTime>(type: "Date", nullable: false),
                    NextPaymentTypeId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    FundingStreamId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NextPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NextPayments_FundingStreams_FundingStreamId",
                        column: x => x.FundingStreamId,
                        principalTable: "FundingStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_NextPayments_NextPaymentTypes_NextPaymentTypeId",
                        column: x => x.NextPaymentTypeId,
                        principalTable: "NextPaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NextPayments_FundingStreamId",
                table: "NextPayments",
                column: "FundingStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_NextPayments_NextPaymentTypeId",
                table: "NextPayments",
                column: "NextPaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NextPaymentTypes_FundingStreamId",
                table: "NextPaymentTypes",
                column: "FundingStreamId");

            migrationBuilder.AddForeignKey(
                name: "FK_SettingValues_FundingStreams_FundingStreamId",
                table: "SettingValues",
                column: "FundingStreamId",
                principalTable: "FundingStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SettingValues_Settings_SettingId",
                table: "SettingValues",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SettingValues_FundingStreams_FundingStreamId",
                table: "SettingValues");

            migrationBuilder.DropForeignKey(
                name: "FK_SettingValues_Settings_SettingId",
                table: "SettingValues");

            migrationBuilder.DropTable(
                name: "NextPayments");

            migrationBuilder.DropTable(
                name: "NextPaymentTypes");

            migrationBuilder.AlterColumn<int>(
                name: "SettingId",
                table: "SettingValues",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FundingStreamId",
                table: "SettingValues",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SettingValues_FundingStreams_FundingStreamId",
                table: "SettingValues",
                column: "FundingStreamId",
                principalTable: "FundingStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SettingValues_Settings_SettingId",
                table: "SettingValues",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
