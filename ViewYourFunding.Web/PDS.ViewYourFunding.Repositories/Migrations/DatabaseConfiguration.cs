using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Repositories.Extensions;
using PDS.ViewYourFunding.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PDS.ViewYourFunding.Repositories.Migrations
{
    /// <summary>
    /// The Database configuration.
    /// </summary>
    public static class DatabaseConfiguration
    {
        /// <summary>
        /// The PSG funding stream code.
        /// </summary>
        private const string PsgFundingStreamCode = "PSG";

        /// <summary>
        /// The PSG funding stream name.
        /// </summary>
        private const string PsgFundingStreamName = "PE and sport premium";

        /// <summary>
        /// The PSG funding stream business allocation name.
        /// </summary>
        private const string PsgFundingStreamBusinessAllocationName = "PE and sport premium grant (PSG)";

        /// <summary>
        /// The DSG funding stream code.
        /// </summary>
        private const string DsgFundingStreamCode = "DSG";

        /// <summary>
        /// The DSG funding stream name.
        /// </summary>
        private const string DsgFundingStreamName = "Dedicated schools grant";

        /// <summary>
        /// The DSG function stream name, sentence cased.
        /// </summary>
        private const string DsgFundingStreamNameSentenceCased = "dedicated schools grant";

        /// <summary>
        /// The DSG funding stream business allocation name.
        /// </summary>
        private const string DsgFundingStreamBusinessAllocationName = "Dedicated schools grant (DSG)";

        /// <summary>
        /// The GAG funding stream code.
        /// </summary>
        private const string GagFundingStreamCode = "GAG";

        /// <summary>
        /// The GAG funding stream name.
        /// </summary>
        private const string GagFundingStreamName = "General annual grant";

        /// <summary>
        /// The GAG funding stream name, sentence cased.
        /// </summary>
        private const string GagFundingStreamNameSentenceCased = "general annual grant";

        /// <summary>
        /// The GAG funding stream business allocation name.
        /// </summary>
        private const string GagFundingStreamBusinessAllocationName = "General annual grant (GAG)";

        /// <summary>
        /// The NMSS funding stream name, sentence cased.
        /// </summary>
        private const string NMSSFundingStreamNameSentenceCased = "non maintained special school";

        /// <summary>
        /// The NMSS funding stream name and code.
        /// </summary>
        private const string NMSSFundingStreamCode = "NMSS";

        /// <summary>
        /// The NMSS funding stream name.
        /// </summary>
        private const string NMSSFundingStreamName = "Non maintained special school funding";

        /// <summary>
        /// The NMSS funding stream business allocation name.
        /// </summary>
        private const string NMSSFundingStreamBusinessAllocationName = "Non-maintained special schools (NMSS)";

        /// <summary>
        /// The 16to19 funding stream name and code.
        /// </summary>
        private const string SixteenToNineteenFundingStreamNameAndCode = "1619";

        /// <summary>
        /// 16 to 19 funding stream business allocation name.
        /// </summary>
        private const string SixteenToNineteenFundingStreamBusinessAllocationName = "16 to 19 funding (16-19)";

        /// <summary>
        /// The Pupil premium funding stream code.
        /// </summary>
        private const string PpgFundingStreamCode = "PP";

        /// <summary>
        /// The Pupil premium funding stream name.
        /// </summary>
        private const string PpgFundingStreamName = "Pupil premium";

        /// <summary>
        /// The PPG funding stream business allocation name.
        /// </summary>
        private const string PpgFundingStreamBusinessAllocationName = "Pupil premium grant (PPG)";

        /// <summary>
        /// The LA recoupment funding stream code.
        /// </summary>
        private const string LARecoupmentFundingStreamCode = "LAREC";

        /// <summary>
        /// The LA recoupment funding stream name.
        /// </summary>
        private const string LARecoupmentFundingStreamName = "LA recoupment";

        /// <summary>
        /// The LA recoupment funding stream business allocation name.
        /// </summary>
        private const string LARecoupmentFundingStreamBusinessAllocationName = "LA recoupment (LAREC)";

        /// <summary>
        /// Uri to generate funding document.
        /// </summary>
        private const string GenerateDocumentUrlFormat = "/view-latest-funding/api/funding/GenerateFundingDocument?fundingStreamCode={0}&fundingPeriodCode={1}&cutoffDate={2}&publicationDate={3}&modelVersion={4}&waitForIndexBuild=true";

        /// <summary>
        /// System User Name.
        /// </summary>
        private const string SystemUserName = "System";

        /// <summary>
        /// Type Code for Non Maintained Special School.
        /// </summary>
        private const string NonMaintainedSpecialSchoolTypeCode = "NMSS";

        /// <summary>
        /// Type code for Maintained School.
        /// </summary>
        private const string MaintainedSchoolTypeCode = "MS";

        /// <summary>
        /// Type code for Academies.
        /// </summary>
        private const string AcademiesTypeCode = "AD";

        /// <summary>
        /// Type code for Funding Specific.
        /// </summary>
        private const string FundingSpecificTypeCode = "FS";

        /// <summary>
        /// The setting type id that represents the URL for the logged-in admin view.
        /// </summary>
        private const int UrlForLoggedInViewAdminTypeId = 1;

        /// <summary>
        /// The setting type id that is used to determine whether the View Your Funding area is available when admin is logged in.
        /// </summary>
        private const int DisplayViewYourFundingLoggedInTypeId = 2;

        /// <summary>
        /// The setting type id that represents the URL for the logged-in provider view.
        /// </summary>
        private const int UrlForLoggedInViewProviderTypeId = 3;

        /// <summary>
        /// The setting type id that is used to determine whether the View Your Funding area is available when provider is logged in.
        /// </summary>
        private const int DisplayViewYourFundingLoggedInProviderViewTypeId = 4;

        /// <summary>
        /// The setting type id that represents the URL for the external view.
        /// </summary>
        private const int UrlForExternalViewTypeId = 5;

        /// <summary>
        /// The setting type id used to determine whether the External View Your Funding area is available.
        /// </summary>
        private const int DisplayViewYourFundingExternalTypeId = 6;

        /// <summary>
        /// The setting type id that represents the URL for the logged-in MAT view.
        /// </summary>
        private const int UrlForLoggedInViewMultipleAcademyTrustTypeId = 7;

        /// <summary>
        /// The setting type id that is used to determine whether the View Your Funding area is available when MAT is logged in.
        /// </summary>
        private const int DisplayLoggedInMultipleAcademyTrustViewTypeId = 8;

        /// <summary>
        /// The setting type id that is used to determine whether the different views will show selectors or not (only for testing purpose).
        /// </summary>
        private const int DisplaySelectorsTypeId = 9;

        /// <summary>
        /// The setting type id that is used to determine whether the different views will show the statement specification (only for testing purpose).
        /// </summary>
        private const int DisplayStatementSpecificationTypeId = 13;

        /// <summary>
        /// The setting type id that is used to determine whether the different views will show data (only ever false for testing purpose).
        /// </summary>
        private const int ShowDataTypeId = 14;

        /// <summary>
        /// The DSG Year 2021 - 2022 funding period code.
        /// </summary>
        private const string DSGFY2122FundingPeriodCode = "FY-2122";

        /// <summary>
        /// The DSG Year 2022 - 2023 funding period code.
        /// </summary>
        private const string DSGFY2223FundingPeriodCode = "FY-2223";

        /// <summary>
        /// The DSG Year 2023 - 2024 funding period code.
        /// </summary>
        private const string DSGFY2324FundingPeriodCode = "FY-2324";

        /// <summary>
        /// The DSG Year 2024 - 2025 funding period code.
        /// </summary>
        private const string DSGFY2425FundingPeriodCode = "FY-2425";

        /// <summary>
        /// The DSG Year 2025 - 2026 funding period code.
        /// </summary>
        private const string DSGFY2526FundingPeriodCode = "FY-2526";

        /// <summary>
        /// The DSG Year 2026 - 2027 funding period code.
        /// </summary>
        private const string DSGFY2627FundingPeriodCode = "FY-2627";

        /// <summary>
        /// The PSG Year 2021 - 2022 funding period code.
        /// </summary>
        private const string PSGAY2122FundingPeriodCode = "AY-2122";

        /// <summary>
        /// The PSG Year 2022 - 2023 funding period code.
        /// </summary>
        private const string PSGAY2223FundingPeriodCode = "AY-2223";

        /// <summary>
        /// The PSG Year 2023 - 2024 funding period code.
        /// </summary>
        private const string PSGAY2324FundingPeriodCode = "AY-2324";

        /// <summary>
        /// The PSG Year 2024 - 2025 funding period code.
        /// </summary>
        private const string PSGAY2425FundingPeriodCode = "AY-2425";

        /// <summary>
        /// The PSG Year 2025 - 2026 funding period code.
        /// </summary>
        private const string PSGAY2526FundingPeriodCode = "AY-2526";

        /// <summary>
        /// Type Code for Main PNA.
        /// </summary>
        private const string MainPnaTypeCode = "PNA";

        /// <summary>
        /// The Pupil premium funding stream name.
        /// </summary>
        private const string MainPnaFundingStreamName = "Main pupil number adjustment";

        /// <summary>
        /// The PPG funding stream business allocation name.
        /// </summary>
        private const string MainPnaFundingStreamBusinessAllocationName = "Main pupil number adjustment (PNA)";

        /// <summary>
        /// The UIFSM funding stream code.
        /// </summary>
        private const string UIFSMFundingStreamCode = "UIFSM";

        /// <summary>
        /// The UIFSM funding stream name.
        /// </summary>
        private const string UIFSMFundingStreamName = "Universal infant free school meals";

        /// <summary>
        /// The UIFSM funding stream business allocation name.
        /// </summary>
        private const string UIFSMFundingStreamBusinessAllocationName = "Universal infant free school meals (UIFSM)";

        /// <summary>
        /// Initializes the specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<Context>();

                // auto migration
                context.Database.Migrate();

                // Seed the database.
                SeedData(context);
            }
        }

        /// <summary>
        /// Seed VYF settings, setting values, publications and global settings.
        /// </summary>
        /// <param name="context">The DbContext to use.</param>
        public static void SeedData(Context context)
        {
            SeedDataViewYourFundingData(context, DateTime.Now);
            SeedGlobalSettings(context, DateTime.Now);
        }

        /// <summary>
        /// Seed Data.
        /// </summary>
        /// <param name="context">The DbContext to use.</param>
        /// <param name="now">Date to use in audit.</param>
        public static void SeedGlobalSettings(Context context, DateTime now)
        {
            var anyChanges = false;

            if (!context.GlobalSettings.Any(s => s.Type == UrlForLoggedInViewAdminTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = UrlForLoggedInViewAdminTypeId,
                    Description = "URL for internal admin view",
                    EditType = GlobalSetting.SettingEditType.String,
                    ReadOnly = false,
                    Value = "/view-latest-funding/admin/home",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }

            if (!context.GlobalSettings.Any(s => s.Type == DisplayViewYourFundingLoggedInTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = DisplayViewYourFundingLoggedInTypeId,
                    Description = "Is view your funding internal admin view available?",
                    EditType = GlobalSetting.SettingEditType.Bool,
                    ReadOnly = false,
                    Value = "FALSE",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }

            if (!context.GlobalSettings.Any(s => s.Type == UrlForLoggedInViewProviderTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = UrlForLoggedInViewProviderTypeId,
                    Description = "Url for logged-in provider view",
                    EditType = GlobalSetting.SettingEditType.String,
                    ReadOnly = false,
                    Value = "/view-latest-funding/pre-16-16-19-statements",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }

            if (!context.GlobalSettings.Any(s => s.Type == DisplayViewYourFundingLoggedInProviderViewTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = DisplayViewYourFundingLoggedInProviderViewTypeId,
                    Description = "Is view your funding logged-in provider view available?",
                    EditType = GlobalSetting.SettingEditType.Bool,
                    ReadOnly = false,
                    Value = "FALSE",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }


            if (!context.GlobalSettings.Any(s => s.Type == UrlForExternalViewTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = UrlForExternalViewTypeId,
                    Description = "URL for public view",
                    EditType = GlobalSetting.SettingEditType.String,
                    ReadOnly = false,
                    Value = "/view-latest-funding",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }

            if (!context.GlobalSettings.Any(s => s.Type == DisplayViewYourFundingExternalTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = DisplayViewYourFundingExternalTypeId,
                    Description = "Is view your funding public view available?",
                    EditType = GlobalSetting.SettingEditType.Bool,
                    ReadOnly = false,
                    Value = "FALSE",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }

            if (!context.GlobalSettings.Any(s => s.Type == UrlForLoggedInViewMultipleAcademyTrustTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = UrlForLoggedInViewMultipleAcademyTrustTypeId,
                    Description = "Url for logged-in MAT view",
                    EditType = GlobalSetting.SettingEditType.String,
                    ReadOnly = false,
                    Value = "/view-latest-funding/pre-16-16-19-statements/parent",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }

            if (!context.GlobalSettings.Any(s => s.Type == DisplayLoggedInMultipleAcademyTrustViewTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = DisplayLoggedInMultipleAcademyTrustViewTypeId,
                    Description = "Is view your funding logged-in MAT view available?",
                    EditType = GlobalSetting.SettingEditType.Bool,
                    ReadOnly = false,
                    Value = "FALSE",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }

            if (!context.GlobalSettings.Any(s => s.Type == DisplaySelectorsTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = DisplaySelectorsTypeId,
                    Description = "Are selectors to be displayed for testing purpose?",
                    EditType = GlobalSetting.SettingEditType.Bool,
                    ReadOnly = false,
                    Value = "FALSE",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }

            if (!context.GlobalSettings.Any(s => s.Type == DisplayStatementSpecificationTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = DisplayStatementSpecificationTypeId,
                    Description = "Is the statement specification to be displayed for testing purpose?",
                    EditType = GlobalSetting.SettingEditType.Bool,
                    ReadOnly = false,
                    Value = "FALSE",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }

            if (!context.GlobalSettings.Any(s => s.Type == ShowDataTypeId))
            {
                context.GlobalSettings.Add(new GlobalSetting()
                {
                    Type = ShowDataTypeId,
                    Description = "Is data to be displayed (only set to false for testing purposes)?",
                    EditType = GlobalSetting.SettingEditType.Bool,
                    ReadOnly = false,
                    Value = "TRUE",
                    CreatedAt = now,
                    UpdatedAt = now
                });

                anyChanges = true;
            }

            if (anyChanges)
            {
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Seed VYF settings, setting values and publication data.
        /// </summary>
        /// <param name="context">The DbContext to use.</param>
        /// <param name="datetimeToSet">Date to use in audit.</param>
        private static void SeedDataViewYourFundingData(Context context, DateTime datetimeToSet)
        {
            var (fundingStreams, anyChanges) = SeedFundingStreams(context);

            if (SeedSettingsAndSettingValues(context, fundingStreams, datetimeToSet))
            {
                anyChanges = true;
            }

            if (SeedPublications(context, fundingStreams, datetimeToSet))
            {
                anyChanges = true;
            }

            if (SeedNextPaymentTypes(context, fundingStreams, datetimeToSet))
            {
                anyChanges = true;
            }

            if (anyChanges)
            {
                context.SaveChanges();
            }
        }

        private static bool SeedPublications(Context context, IEnumerable<FundingStream> fundingStreams, DateTime datetimeToSet)
        {
            var anyChanges = false;

            if (SeedPublication(context, fundingStreams, datetimeToSet, DsgFundingStreamCode, new DateTime(2019, 12, 19), "FY-2021", "<abbr title=\"Education and Skills Funding Agency\">ESFA</abbr> announces 2020 to 2021 schools block, <abbr title=\"central school services block\">CSSB</abbr>, early years block, high needs block, and details of the charges for national copyright licences issued to local authorities."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, PsgFundingStreamCode, new DateTime(2020, 04, 23), "AY-1920", "New allocations published for the academic year 2019 to 2020."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, PsgFundingStreamCode, new DateTime(2020, 02, 25), "AY-1920", "New allocations published for the academic year 2019 to 2020."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, PsgFundingStreamCode, new DateTime(2019, 10, 28), "AY-1920", "New allocations published for the academic year 2019 to 2020."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, PsgFundingStreamCode, new DateTime(2019, 4, 24), "AY-1819", "New allocations published for the academic year 2018 to 2019."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, PsgFundingStreamCode, new DateTime(2019, 2, 21), "AY-1819", "New allocations published for the academic year 2018 to 2019."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, PsgFundingStreamCode, new DateTime(2018, 10, 25), "AY-1819", "New allocations published for the academic year 2018 to 2019."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, DsgFundingStreamCode, new DateTime(2019, 12, 19), "FY-2021", "<abbr title=\"Education and Skills Funding Agency\">ESFA</abbr> announces 2020 to 2021 schools block, <abbr title=\"central school services block\">CSSB</abbr>, early years block, high needs block, and details of the charges for national copyright licences issued to local authorities."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, GagFundingStreamCode, new DateTime(2021, 6, 1), "AC-2122", string.Empty))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, PpgFundingStreamCode, new DateTime(2022, 10, 25), "FY-2223", "New allocations published for the academic year 2021 to 2022."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, LARecoupmentFundingStreamCode, new DateTime(2020, 10, 25), "FY-2021", "New allocations published for the academic year 2020 to 2021."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, LARecoupmentFundingStreamCode, new DateTime(2020, 11, 11), "FY-2021", "New allocations published for the academic year 2020 to 2021."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, LARecoupmentFundingStreamCode, new DateTime(2021, 10, 25), "FY-2122", "New allocations published for the academic year 2021 to 2022."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, LARecoupmentFundingStreamCode, new DateTime(2021, 11, 11), "FY-2122", "New allocations published for the academic year 2021 to 2022."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, LARecoupmentFundingStreamCode, new DateTime(2022, 10, 25), "FY-2223", "New allocations published for the academic year 2022 to 2023."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, LARecoupmentFundingStreamCode, new DateTime(2022, 11, 11), "FY-2223", "New allocations published for the academic year 2022 to 2023."))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, MainPnaTypeCode, new DateTime(2021, 6, 1), "AC-2122", string.Empty))
            {
                anyChanges = true;
            }

            if (SeedPublication(context, fundingStreams, datetimeToSet, UIFSMFundingStreamCode, new DateTime(2022, 06, 01), "AY-2223", "New allocations published for the academic year 2022 to 2023."))
            {
                anyChanges = true;
            }

            return anyChanges;
        }

        private static bool SeedPublication(Context context, IEnumerable<FundingStream> fundingStreams, DateTime datetimeToSet, string fundingStreamCode, DateTime publishedDate, string fundingPeriodCode, string description)
        {
            var fundingStream = fundingStreams.Single(s => s.FundingStreamCode == fundingStreamCode);
            var publications = context.Publications.Where(p => p.FundingStreamId == fundingStream.Id).ToList();
            var anyChanges = false;

            if (publications.All(p => p.PublishedDate != publishedDate))
            {
                context.Publications.Add(new Publication()
                {
                    FundingStream = fundingStream,
                    PublishedDate = publishedDate,
                    FundingPeriodCode = fundingPeriodCode,
                    Description = description,
                    Status = PublicationStatus.Published,
                    CreatedAt = datetimeToSet,
                    LastUpdatedAt = datetimeToSet,
                    LastUpdatedBy = "System",
                    PublicationLayouts = new List<PublicationLayout>()
                });

                anyChanges = true;
            }

            return anyChanges;
        }

        private static bool SeedSettingsAndSettingValues(Context context, IEnumerable<FundingStream> fundingStreams, DateTime datetimeToSet)
        {
            var dsg = fundingStreams.Where(s => s.FundingStreamCode == DsgFundingStreamCode);
            var psg = fundingStreams.Where(s => s.FundingStreamCode == PsgFundingStreamCode);
            var pp = fundingStreams.Where(s => s.FundingStreamCode == PpgFundingStreamCode);
            var gag = fundingStreams.Where(s => s.FundingStreamCode == GagFundingStreamCode);
            var nmss = fundingStreams.Where(s => s.FundingStreamCode == NMSSFundingStreamCode);
            var sixteenToNineteen = fundingStreams.Where(s => s.FundingStreamCode == SixteenToNineteenFundingStreamNameAndCode);
            var larecoupment = fundingStreams.Where(s => s.FundingStreamCode == LARecoupmentFundingStreamCode);
            var pna = fundingStreams.Where(s => s.FundingStreamCode == MainPnaTypeCode);
            var uifsm = fundingStreams.Where(s => s.FundingStreamCode == UIFSMFundingStreamCode);

            var anyChanges = false;

            if (SeedSetting(context, larecoupment, datetimeToSet, "FinancialYear", "The financial year of the allocations that should be shown, e.g. 202122.", SettingValueDataType.String, true, "202223", false, new KeyValuePair<string, string>(LARecoupmentFundingStreamCode, "202223")))
            {
                anyChanges = true;
                context.SaveChanges();
            }

            if (SeedSetting(context, psg, datetimeToSet, "AcademicYear", "The academic year of the allocations that should be shown, e.g. 201819.", SettingValueDataType.String, true, "201920", false, new KeyValuePair<string, string>(PsgFundingStreamCode, "201920")))
            {
                anyChanges = true;
                context.SaveChanges();
            }

            if (SeedSetting(context, pp, datetimeToSet, "FinancialYear", "The financial year of the allocations that should be shown, e.g. 202122.", SettingValueDataType.String, true, "202223", false, new KeyValuePair<string, string>(PpgFundingStreamCode, "202223")))
            {
                anyChanges = true;
                context.SaveChanges();
            }

            if (SeedSetting(context, dsg, datetimeToSet, "FinancialYear", "The financial year of the allocations that should be shown, e.g. 201819.", SettingValueDataType.String, true, "202021", false))
            {
                anyChanges = true;
                context.SaveChanges();
            }

            RemoveSetting(context, gag, "AcademicYear");

            if (SeedSetting(context, gag, datetimeToSet, "AcademyAcademicYear", "The academy academic year of the allocations that should be shown, e.g. 201819.", SettingValueDataType.String, true, "202122", false, new KeyValuePair<string, string>(GagFundingStreamCode, "202122")))
            {
                anyChanges = true;
            }

            RemoveSetting(context, sixteenToNineteen, "AcademicYear");

            if (SeedSetting(context, sixteenToNineteen, datetimeToSet, "AcademyAndSchoolAcademicYear", "The academy academic year of the allocations that should be shown, e.g. 201819.", SettingValueDataType.String, true, "202122", false, new KeyValuePair<string, string>(SixteenToNineteenFundingStreamNameAndCode, "202122")))
            {
                anyChanges = true;
            }

            RemoveSetting(context, nmss, "AcademyAndSchoolAcademicYear");

            if (SeedSetting(context, nmss, datetimeToSet, "AcademicYear", "The academic year of the allocations that should be shown, e.g. 201819.", SettingValueDataType.String, true, "202122", false, new KeyValuePair<string, string>(NMSSFundingStreamCode, "202122")))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, gag, datetimeToSet, "ParentProviderType", "The parent provider type, e.g. LocalAuthority.", SettingValueDataType.String, true, "AcademyTrust", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, sixteenToNineteen, datetimeToSet, "ParentProviderType", "The parent provider type, e.g. LocalAuthority.", SettingValueDataType.String, true, "LocalAuthority", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, sixteenToNineteen, datetimeToSet, "UseStaticData", "Determines whether to use hard coded static data as a data source.", SettingValueDataType.Bool, true, "true", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, nmss, datetimeToSet, "ParentProviderType", "The parent provider type, e.g. LocalAuthority.", SettingValueDataType.String, true, "LocalAuthority", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, gag, datetimeToSet, "FundingDocumentFileType", "The funding document type", SettingValueDataType.String, true, "csv", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, fundingStreams, datetimeToSet, "UpdateSpreadsheetUrl", "The url to request to trigger this funding stream's spreadsheet(s) to be generated.", SettingValueDataType.String, false, GenerateDocumentUrlFormat, true))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, dsg, datetimeToSet, "LAGroupingReason", "The grouping reason to use for LAs.", SettingValueDataType.String, false, "Payment", true))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, dsg, datetimeToSet, "HistoricAllocationsAreExternalYear", "The year histroic allocations are just external links.", SettingValueDataType.Int, true, "2019", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, psg, datetimeToSet, "NextPaymentDateTypeCode", "The next payment date type code.", SettingValueDataType.String, true, "MS", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, psg, datetimeToSet, "OrganisationDownloadSizeInBytes", "The download size (in bytes) for the organisation spreadsheet.", SettingValueDataType.Int, true, "7000", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, dsg, datetimeToSet, "OrganisationDownloadSizeInBytes", "The download size (in bytes) for the organisation spreadsheet.", SettingValueDataType.Int, true, "28000", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, larecoupment, datetimeToSet, "OrganisationDownloadSizeInBytes", "The download size (in bytes) for the organisation spreadsheet.", SettingValueDataType.Int, true, "1024", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, psg, datetimeToSet, "ProviderDownloadSizeInBytes", "The download size (in bytes) for the provider spreadsheet.", SettingValueDataType.Int, true, "5000", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, dsg, datetimeToSet, "ProviderDownloadSizeInBytes", "The download size (in bytes) for the provider spreadsheet.", SettingValueDataType.Int, true, "5000", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, gag, datetimeToSet, "ProviderDownloadSizeInBytes", "The download size (in bytes) for the provider spreadsheet.", SettingValueDataType.Int, true, "206000", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, nmss, datetimeToSet, "UseStaticData", "Determines whether to use hard coded static data as a data source.", SettingValueDataType.Bool, true, "true", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, pp, datetimeToSet, "UseStaticData", "Determines whether to use hard coded static data as a data source.", SettingValueDataType.Bool, true, "true", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, pp, datetimeToSet, "UseLatestFundingData", "Determines whether to use latest funding data to build results.", SettingValueDataType.Bool, true, "true", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, pp, datetimeToSet, "ProviderDownloadSizeInBytes", "The download size (in bytes) for the provider spreadsheet.", SettingValueDataType.Int, true, "5000", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, gag, datetimeToSet, "UseStaticData", "Determines whether to use hard coded static data as a data source.", SettingValueDataType.Bool, true, "true", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, pp, datetimeToSet, "ParentGroupTypeFilter", "The parent provider grouping type to filter when getting provider funding for funding stream.", SettingValueDataType.String, true, "LocalAuthority", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, pp, datetimeToSet, "GroupingReasonFilter", "The grouping reason to filter when getting provider funding for funding stream.", SettingValueDataType.String, true, "Information", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, pna, datetimeToSet, "AcademyAcademicYear", "The academy academic year of the allocations that should be shown, e.g. 201819.", SettingValueDataType.String, true, "202122", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, pna, datetimeToSet, "ParentProviderType", "	The parent provider type, e.g. LocalAuthority.", SettingValueDataType.String, true, "AcademyTrust", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, pna, datetimeToSet, "FundingDocumentFileType", "The funding document type.", SettingValueDataType.String, true, "csv", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, pna, datetimeToSet, "ProviderDownloadSizeInBytes", "The download size (in bytes) for the provider spreadsheet.", SettingValueDataType.String, true, "206000", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, pna, datetimeToSet, "UseStaticData", "Determines whether to use hard coded static data as a data source.", SettingValueDataType.String, true, "true", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, uifsm, datetimeToSet, "AcademicYear", "The academic year of the allocations that should be shown, e.g. 202122.", SettingValueDataType.String, true, "202223", false, new KeyValuePair<string, string>(UIFSMFundingStreamCode, "202223")))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, uifsm, datetimeToSet, "FundingDocumentFileType", "The funding document type.", SettingValueDataType.String, true, "ods", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, uifsm, datetimeToSet, "ProviderDownloadSizeInBytes", "The download size (in bytes) for the provider spreadsheet.", SettingValueDataType.String, true, "206000", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, uifsm, datetimeToSet, "UseStaticData", "Determines whether to use hard coded static data as a data source.", SettingValueDataType.String, true, "true", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, uifsm, datetimeToSet, "ParentGroupTypeFilter", "The parent provider grouping type to filter when getting provider funding for funding stream.", SettingValueDataType.String, true, "LocalAuthority", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, uifsm, datetimeToSet, "UseLatestFundingData", "Determines whether to use latest funding data to build results.", SettingValueDataType.Bool, true, "true", false))
            {
                anyChanges = true;
            }

            var pdfDocumentGenerationEnabledFundingStreams = gag.Concat(sixteenToNineteen).Concat(nmss).Concat(pna);

            if (SeedSetting(context, pdfDocumentGenerationEnabledFundingStreams, datetimeToSet, "PDFDocumentGenerationEnabled", "Determines whether to use a funding stream code for pdf document generation.", SettingValueDataType.Bool, true, "true", false))
            {
                anyChanges = true;
            }

            var fundingReportEnabledFundingStreams = sixteenToNineteen.Concat(larecoupment);

            if (SeedSetting(context, fundingReportEnabledFundingStreams, datetimeToSet, "FundingReportEnabled", "Determines whether to use a funding stream code for funding report generation.", SettingValueDataType.Bool, true, "true", false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, gag, datetimeToSet, "EmailEnabledFundingPeriods", "Comma separated Funding Period Ids (Eg: FY-2425 or AC-2425,AC-2526) for which Email needs to be sent. If the setting is not added or empty, no email will be sent for the given funding streams.", SettingValueDataType.String, true, string.Empty, false))
            {
                anyChanges = true;
            }

            if (SeedSetting(context, gag, datetimeToSet, "DigitalStatementsGoLiveDate", "Effective from the launch date of Digital Statements, emails will be dispatched and alerts (both new and updated) will be displayed exclusively for statements issued by CFS subsequent to this date.", SettingValueDataType.Date, true, string.Empty, false))
            {
                anyChanges = true;
            }

            return anyChanges;
        }

        private static void RemoveSetting(Context context, IEnumerable<FundingStream> fundingStreams, string settingName)
        {
            var setting = context.Settings.Include(s => s.SettingValues).FirstOrDefault(s => s.SettingName == settingName);

            foreach (var fundingStream in fundingStreams)
            {
                var settingValue = setting.SettingValues.FirstOrDefault(sv =>
                    sv.Setting.Id == setting.Id && sv.FundingStream?.Id == fundingStream.Id);

                if (settingValue == null)
                {
                    continue;
                }

                setting.SettingValues.Remove(settingValue);
                context.SaveChanges();
            }
        }

        private static bool SeedSetting(
            Context context,
            IEnumerable<FundingStream> fundingStreams,
            DateTime datetimeToSet,
            string settingName,
            string settingDescription,
            SettingValueDataType valueDataType,
            bool valuesAreEditable,
            string settingDefaultValue,
            bool forceUpdate,
            params KeyValuePair<string, string>[] settingValueOverridesForStreamCodes)
        {
            var setting = context.Settings.Include(s => s.SettingValues).FirstOrDefault(s => s.SettingName == settingName);
            var anyChange = setting == null;

            if (setting == null)
            {
                setting = new Setting()
                {
                    SettingName = settingName,
                    SettingDescription = settingDescription,
                    ValueDataType = valueDataType,
                    ValuesAreEditable = valuesAreEditable,
                    SettingValues = new Collection<SettingValue>(),
                    CreatedAt = datetimeToSet,
                    LastUpdatedAt = datetimeToSet,
                    LastUpdatedBy = "System"
                };
            }
            else
            {
                if (setting.SettingName != settingName)
                {
                    setting.SettingName = settingName;
                    anyChange = true;
                }

                if (setting.SettingDescription != settingDescription)
                {
                    setting.SettingDescription = settingDescription;
                    anyChange = true;
                }

                if (setting.ValueDataType != valueDataType)
                {
                    setting.ValueDataType = valueDataType;
                    anyChange = true;
                }

                if (setting.ValuesAreEditable != valuesAreEditable)
                {
                    setting.ValuesAreEditable = valuesAreEditable;
                    anyChange = true;
                }
            }

            foreach (var fundingStream in fundingStreams)
            {
                var settingValue =
                        (settingValueOverridesForStreamCodes?.FirstOrDefault(o => o.Key == fundingStream.FundingStreamCode))?.Value
                        ?? settingDefaultValue;

                var exists = setting.SettingValues.Any(sv =>
                    sv.Setting.Id == setting.Id && sv.FundingStream?.Id == fundingStream.Id);

                if (fundingStream.Id == 0 || !exists)
                {
                    var newSettingValue = new SettingValue()
                    {
                        FundingStream = fundingStream,
                        Setting = setting,
                        Value = settingValue,
                        CreatedAt = datetimeToSet,
                        LastUpdatedAt = datetimeToSet,
                        LastUpdatedBy = "System"
                    };

                    setting.SettingValues.Add(newSettingValue);
                    anyChange = true;
                }
                else if (forceUpdate)
                {
                    var settingExistingValue = setting.SettingValues.Single(v => v.FundingStream?.Id == fundingStream.Id);

                    if (settingExistingValue.Value != settingValue)
                    {
                        settingExistingValue.Value = settingValue;
                        settingExistingValue.LastUpdatedAt = datetimeToSet;
                        settingExistingValue.LastUpdatedBy = "System";

                        anyChange = true;
                    }
                }
            }

            if (anyChange)
            {
                context.Settings.AddOrUpdate(setting, s => s.SettingName);
                context.SaveChanges();
            }

            return anyChange;
        }


        private static bool SeedNextPaymentTypes(Context context, IEnumerable<FundingStream> fundingStreams, DateTime now)
        {
            var anyChanges = false;

            if (SeedNextPaymentType(context, fundingStreams, now, DsgFundingStreamCode, FundingSpecificTypeCode, "Funding Specific"))
            {
                anyChanges = true;
            }

            if (SeedNextPaymentType(context, fundingStreams, now, PsgFundingStreamCode, NonMaintainedSpecialSchoolTypeCode, "Non maintained schools"))
            {
                anyChanges = true;
            }

            if (SeedNextPaymentType(context, fundingStreams, now, PsgFundingStreamCode, AcademiesTypeCode, "Academies"))
            {
                anyChanges = true;
            }

            if (SeedNextPaymentType(context, fundingStreams, now, PsgFundingStreamCode, MaintainedSchoolTypeCode, "Maintained schools"))
            {
                anyChanges = true;
            }

            if (SeedNextPaymentType(context, fundingStreams, now, PpgFundingStreamCode, MaintainedSchoolTypeCode, "Maintained schools"))
            {
                anyChanges = true;
            }

            return anyChanges;
        }

        /// <summary>
        /// Seeds the type of the view your funding next payment type.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="fundingStreams">The funding streams.</param>
        /// <param name="now">The now.</param>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="typeCode">The type code.</param>
        /// <param name="description">The description.</param>
        private static bool SeedNextPaymentType(
            Context context,
            IEnumerable<FundingStream> fundingStreams,
            DateTime now,
            string fundingStreamCode,
            string typeCode,
            string description)
        {
            var fundingStream = fundingStreams.Single(s => s.FundingStreamCode == fundingStreamCode);
            var nextPaymentTypes = context.NextPaymentTypes.Where(npt => npt.FundingStreamId == fundingStream.Id).ToList();
            var nextPayments = context.NextPayments.Where(np => np.FundingStreamId == fundingStream.Id).ToList();
            var anyChanges = false;

            if (nextPaymentTypes.All(p => p.TypeCode != typeCode))
            {
                var newPaymentType = new NextPaymentType
                {
                    TypeCode = typeCode,
                    Description = description,
                    FundingStream = fundingStream,
                    CreatedAt = now,
                    LastUpdatedAt = now,
                    LastUpdatedBy = SystemUserName
                };

                context.NextPaymentTypes.Add(newPaymentType);
                anyChanges = true;
                SeedNextPayments(context, fundingStreamCode, typeCode, fundingStream, newPaymentType, now, nextPayments);
            }
            else
            {
                var nextPaymentType = context.NextPaymentTypes.First(npt => npt.FundingStreamId == fundingStream.Id && npt.TypeCode == typeCode);
                anyChanges = SeedNextPayments(context, fundingStreamCode, typeCode, fundingStream, nextPaymentType, now, nextPayments);
            }

            return anyChanges;
        }

        private static bool SeedNextPayments(Context context, string fundingStreamCode, string typeCode, FundingStream fundingStream, NextPaymentType nextPaymentType, DateTime now, List<NextPayment> nextPayments)
        {
            var anyChanges = false;
            switch (fundingStreamCode)
            {
                case PsgFundingStreamCode:
                    switch (typeCode)
                    {
                        case NonMaintainedSpecialSchoolTypeCode:
                            SeedPSGNMSSNextPayments(context, fundingStream, nextPaymentType, now, nextPayments);
                            anyChanges = true;
                            break;
                        case AcademiesTypeCode:
                            SeedPSGAcademiesNextPayments(context, fundingStream, nextPaymentType, now, nextPayments);
                            anyChanges = true;
                            break;
                        case MaintainedSchoolTypeCode:
                            SeedPSGMSNextPayments(context, fundingStream, nextPaymentType, now, nextPayments);
                            anyChanges = true;
                            break;
                    }

                    break;

                case DsgFundingStreamCode:
                    SeedDSGNextPayments(context, fundingStream, nextPaymentType, now, nextPayments);
                    anyChanges = true;
                    break;
            }

            return anyChanges;
        }

        private static void SeedDSGNextPayments(Context context, FundingStream fundingStream, NextPaymentType nextPaymentType, DateTime now, List<NextPayment> nextPayments)
        {
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 04, 3), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 04, 16), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 04, 30), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 05, 12), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 05, 22), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 06, 3), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 06, 22), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 07, 3), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 07, 22), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 08, 5), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 08, 21), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 09, 3), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 09, 22), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 10, 5), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 10, 22), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 11, 4), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 11, 20), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 12, 3), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 12, 22), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 01, 6), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 01, 22), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 02, 3), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 02, 22), nextPaymentType, nextPayments);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 03, 3), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 03, 22), nextPaymentType, nextPayments);

            // FY-2122 Payment Dates
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 04, 7), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 04, 16), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 04, 30), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 05, 12), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 05, 21), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 06, 3), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 06, 22), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 07, 5), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 07, 22), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 08, 4), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 08, 20), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 09, 3), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 09, 22), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 10, 5), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 10, 22), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 11, 3), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 11, 22), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 12, 3), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 12, 22), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 01, 6), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 01, 21), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 02, 3), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 02, 22), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 03, 3), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 03, 22), nextPaymentType, nextPayments, DSGFY2122FundingPeriodCode);

            // FY-2223 Payment Dates
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 04, 5), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 04, 14), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 04, 29), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 05, 11), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 05, 20), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 06, 7), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 06, 22), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 07, 5), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 07, 22), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 08, 3), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 08, 22), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 09, 5), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 09, 22), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 10, 3), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 10, 21), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 11, 3), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 11, 22), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 12, 5), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 12, 22), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 01, 5), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 01, 20), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 02, 3), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 02, 22), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 03, 3), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 03, 22), nextPaymentType, nextPayments, DSGFY2223FundingPeriodCode);

            // FY-2324 Payment Dates
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 04, 5), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 05, 4), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 06, 5), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 07, 5), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 08, 3), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 09, 5), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 10, 4), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 11, 3), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 12, 5), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 01, 4), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 02, 5), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 03, 5), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 03, 22), nextPaymentType, nextPayments, DSGFY2324FundingPeriodCode);

            // FY-2425 Payment Dates
            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 04, 04), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 05, 03), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 06, 05), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 07, 03), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 08, 05), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 09, 04), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 10, 03), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 11, 05), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 12, 04), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 01, 06), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 02, 05), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 03, 05), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 03, 24), nextPaymentType, nextPayments, DSGFY2425FundingPeriodCode);

            // FY-2526 Payment Dates
            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 04, 03), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 05, 06), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 06, 04), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 07, 03), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 08, 05), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 09, 03), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 10, 03), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 11, 05), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 12, 03), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 01, 05), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 02, 04), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 03, 04), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 03, 23), nextPaymentType, nextPayments, DSGFY2526FundingPeriodCode);

            // FY-2627 Payment Dates
            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 04, 07), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 05, 06), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 06, 03), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 07, 03), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 08, 05), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 09, 03), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 10, 05), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 11, 04), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 12, 03), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2027, 01, 06), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2027, 02, 03), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2027, 03, 03), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);

            SeedNextPayment(context, fundingStream, now, new DateTime(2027, 03, 22), nextPaymentType, nextPayments, DSGFY2627FundingPeriodCode);
        }

        private static void SeedPSGNMSSNextPayments(Context context, FundingStream fundingStream, NextPaymentType nextPaymentType, DateTime now, List<NextPayment> nextPayments)
        {
            SeedNextPayment(context, fundingStream, now, new DateTime(2019, 11, 1), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 03, 1), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 05, 1), nextPaymentType, nextPayments);

            // AY-2122 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 11, 2), nextPaymentType, nextPayments, PSGAY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 5, 4), nextPaymentType, nextPayments, PSGAY2122FundingPeriodCode);

            // AY-2223 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 12, 19), nextPaymentType, nextPayments, PSGAY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 6, 19), nextPaymentType, nextPayments, PSGAY2223FundingPeriodCode);

            // AY-2324 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 12, 18), nextPaymentType, nextPayments, PSGAY2324FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 4, 18), nextPaymentType, nextPayments, PSGAY2324FundingPeriodCode);

            // AY-2425 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 12, 18), nextPaymentType, nextPayments, PSGAY2425FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 4, 17), nextPaymentType, nextPayments, PSGAY2425FundingPeriodCode);

            // AY-2526 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 12, 18), nextPaymentType, nextPayments, PSGAY2526FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 4, 17), nextPaymentType, nextPayments, PSGAY2526FundingPeriodCode);
        }

        private static void SeedPSGAcademiesNextPayments(Context context, FundingStream fundingStream, NextPaymentType nextPaymentType, DateTime now, List<NextPayment> nextPayments)
        {
            SeedNextPayment(context, fundingStream, now, new DateTime(2019, 11, 1), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 03, 1), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 05, 1), nextPaymentType, nextPayments);

            // AY-2122 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 11, 1), nextPaymentType, nextPayments, PSGAY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 3, 1), nextPaymentType, nextPayments, PSGAY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 5, 3), nextPaymentType, nextPayments, PSGAY2122FundingPeriodCode);

            // AY-2223 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 11, 8), nextPaymentType, nextPayments, PSGAY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 3, 8), nextPaymentType, nextPayments, PSGAY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 5, 9), nextPaymentType, nextPayments, PSGAY2223FundingPeriodCode);

            // AY-2324 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 11, 8), nextPaymentType, nextPayments, PSGAY2324FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 3, 8), nextPaymentType, nextPayments, PSGAY2324FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 5, 8), nextPaymentType, nextPayments, PSGAY2324FundingPeriodCode);

            // AY-2425 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 11, 8), nextPaymentType, nextPayments, PSGAY2425FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 3, 10), nextPaymentType, nextPayments, PSGAY2425FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 5, 9), nextPaymentType, nextPayments, PSGAY2425FundingPeriodCode);

            // AY-2526 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 11, 10), nextPaymentType, nextPayments, PSGAY2526FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 3, 9), nextPaymentType, nextPayments, PSGAY2526FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 5, 11), nextPaymentType, nextPayments, PSGAY2526FundingPeriodCode);
        }

        private static void SeedPSGMSNextPayments(Context context, FundingStream fundingStream, NextPaymentType nextPaymentType, DateTime now, List<NextPayment> nextPayments)
        {
            SeedNextPayment(context, fundingStream, now, new DateTime(2019, 10, 30), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 02, 28), nextPaymentType, nextPayments);
            SeedNextPayment(context, fundingStream, now, new DateTime(2020, 04, 30), nextPaymentType, nextPayments);

            // AY-2122 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2021, 10, 29), nextPaymentType, nextPayments, PSGAY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 2, 28), nextPaymentType, nextPayments, PSGAY2122FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 4, 29), nextPaymentType, nextPayments, PSGAY2122FundingPeriodCode);

            // AY-2223 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2022, 10, 31), nextPaymentType, nextPayments, PSGAY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 2, 28), nextPaymentType, nextPayments, PSGAY2223FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 4, 28), nextPaymentType, nextPayments, PSGAY2223FundingPeriodCode);

            // AY-2324 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2023, 10, 31), nextPaymentType, nextPayments, PSGAY2324FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 2, 29), nextPaymentType, nextPayments, PSGAY2324FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 4, 30), nextPaymentType, nextPayments, PSGAY2324FundingPeriodCode);

            // AY-2425 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2024, 10, 31), nextPaymentType, nextPayments, PSGAY2425FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 2, 28), nextPaymentType, nextPayments, PSGAY2425FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 4, 30), nextPaymentType, nextPayments, PSGAY2425FundingPeriodCode);

            // AY-2526 Next payments
            SeedNextPayment(context, fundingStream, now, new DateTime(2025, 10, 31), nextPaymentType, nextPayments, PSGAY2526FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 2, 27), nextPaymentType, nextPayments, PSGAY2526FundingPeriodCode);
            SeedNextPayment(context, fundingStream, now, new DateTime(2026, 4, 30), nextPaymentType, nextPayments, PSGAY2526FundingPeriodCode);
        }

        /// <summary>
        /// Seeds the view your funding next payment.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="fundingStream">The funding streams.</param>
        /// <param name="now">The now.</param>
        /// <param name="nextPaymentDate">The next payment date.</param>
        /// <param name="nextPaymentType">Type of the next payment.</param>
        /// <param name="nextPayments">The next payment.</param>
        /// <param name="paymentFundingPeriodCode">The payment funding period code.</param>
        private static void SeedNextPayment(
            Context context,
            FundingStream fundingStream,
            DateTime now,
            DateTime nextPaymentDate,
            NextPaymentType nextPaymentType,
            List<NextPayment> nextPayments,
            string paymentFundingPeriodCode = null)
        {
            var fundingPeriodCode = fundingStream.FundingStreamCode switch
            {
                DsgFundingStreamCode => "FY-2021",
                PsgFundingStreamCode => "AY-1920",
                _ => null
            };

            fundingPeriodCode = paymentFundingPeriodCode ?? fundingPeriodCode;

            if (!nextPayments.Any(p =>
                p.NextPaymentDate == nextPaymentDate &&
                p.NextPaymentTypeId == nextPaymentType.Id &&
                p.FundingPeriodCode == fundingPeriodCode))
            {
                context.NextPayments.Add(new NextPayment
                {
                    FundingStream = fundingStream,
                    NextPaymentDate = nextPaymentDate,
                    NextPaymentType = nextPaymentType,
                    FundingPeriodCode = fundingPeriodCode,
                    Active = true,
                    CreatedAt = now,
                    LastUpdatedAt = now,
                    LastUpdatedBy = SystemUserName
                });
            }
        }

        private static (IEnumerable<FundingStream>, bool) SeedFundingStreams(Context context)
        {
            var (peSportsFundingStream, anyChanges1) = SeedFundingStream(context, PsgFundingStreamCode, PsgFundingStreamName, PsgFundingStreamBusinessAllocationName, false, true, true, true, true, true, false);
            var (dsgFundingStream, anyChanges2) = SeedFundingStream(context, DsgFundingStreamCode, DsgFundingStreamName, DsgFundingStreamBusinessAllocationName, true, false, false, false, true, true, false, DsgFundingStreamNameSentenceCased);
            var (gagFundingStream, anyChanges3) = SeedFundingStream(context, GagFundingStreamCode, GagFundingStreamName, GagFundingStreamBusinessAllocationName, false, true, false, false, false, false, true, GagFundingStreamNameSentenceCased);
            var (sixteenToNineteenFundingStream, anyChanges4) = SeedFundingStream(context, SixteenToNineteenFundingStreamNameAndCode, SixteenToNineteenFundingStreamNameAndCode, SixteenToNineteenFundingStreamBusinessAllocationName, false, false, false, false, false, false, false, SixteenToNineteenFundingStreamNameAndCode);
            var (nmssFundingStream, anyChanges5) = SeedFundingStream(context, NMSSFundingStreamCode, NMSSFundingStreamName, NMSSFundingStreamBusinessAllocationName, false, true, false, false, false, false, true, NMSSFundingStreamNameSentenceCased);
            var (ppgFundingStream, anyChanges6) = SeedFundingStream(context, PpgFundingStreamCode, PpgFundingStreamName, PpgFundingStreamBusinessAllocationName, false, false, false, false, false, false, false);
            var (larecoupmentFundingStream, anyChanges7) = SeedFundingStream(context, LARecoupmentFundingStreamCode, LARecoupmentFundingStreamName, LARecoupmentFundingStreamBusinessAllocationName, false, false, false, true, false, false, false);
            var (pnaFundingStream, anyChanges8) = SeedFundingStream(context, MainPnaTypeCode, MainPnaFundingStreamName, MainPnaFundingStreamBusinessAllocationName, false, true, false, false, false, false, true);
            var (uifsmFundingStream, anyChanges9) = SeedFundingStream(context, UIFSMFundingStreamCode, UIFSMFundingStreamName, UIFSMFundingStreamBusinessAllocationName, false, false, false, false, false, false, false);

            var anyChanges = anyChanges1 || anyChanges2 || anyChanges3 || anyChanges4 || anyChanges5 || anyChanges6 || anyChanges7 || anyChanges8 || anyChanges9;

            return (new List<FundingStream>
            {
                peSportsFundingStream,
                dsgFundingStream,
                gagFundingStream,
                sixteenToNineteenFundingStream,
                nmssFundingStream,
                ppgFundingStream,
                larecoupmentFundingStream,
                pnaFundingStream,
                uifsmFundingStream
            }, anyChanges);
        }

        private static (FundingStream, bool) SeedFundingStream(
            Context context,
            string fundingStreamCode,
            string fundingStreamName,
            string fundingStreamBusinessAllocationName,
            bool fundingStreamCodePubliclyKnown,
            bool relevantForProviders_LoggedIn,
            bool relevantForProviders_Public,
            bool relevantForOrganisations_LoggedIn,
            bool relevantForOrganisations_Public,
            bool relevantForNational,
            bool historyIndependentOfPublications,
            string fundingStreamNameWithinSentence = null)
        {
            var fundingStream = context.FundingStreams.FirstOrDefault(s => s.FundingStreamCode == fundingStreamCode);

            if (fundingStream != null)
            {
                return (fundingStream, false);
            }

            fundingStream = new FundingStream
            {
                FundingStreamCode = fundingStreamCode,
                FundingStreamName = fundingStreamName,
                FundingStreamNameWithinSentence = fundingStreamNameWithinSentence,
                FundingStreamBusinessAllocationName = fundingStreamBusinessAllocationName,
                FundingStreamCodePubliclyKnown = fundingStreamCodePubliclyKnown,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                LastUpdatedBy = SystemUserName,
                Active = true,
                RelevantForProviders_LoggedIn = relevantForProviders_LoggedIn,
                RelevantForProviders_Public = relevantForProviders_Public,
                RelevantForOrganisations_LoggedIn = relevantForOrganisations_LoggedIn,
                RelevantForOrganisations_Public = relevantForOrganisations_Public,
                RelevantForNational = relevantForNational,
                HistoryIndependentOfPublications = historyIndependentOfPublications
            };

            context.FundingStreams.AddOrUpdate(fundingStream, fs => fs.FundingStreamCode);
            return (fundingStream, true);
        }
    }
}