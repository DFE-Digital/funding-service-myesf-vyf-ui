using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Automation.Tests.Constants;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Implementations;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ViewYourFunding.Automation.Testing;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The FundingStreamRegressionTestBase class.
    /// </summary>
    /// <seealso cref="BaseRegressionTest" />
    public class FundingStreamRegressionTestBase : BaseRegressionTest
    {
        #region Shared constants

        /// <summary>
        /// The PSG current year from.
        /// </summary>
        public const int PsgCurrentYearFrom = 2019, PsgCurrentYearTo = 2020;

        /// <summary>
        /// The DSG current year from.
        /// </summary>
        public const int DsgCurrentYearFrom = 2021, DsgCurrentYearTo = 2022;

        /// <summary>
        /// The PSG next payment date maintained parsed.
        /// </summary>
        public static DateTime PsgNextPaymentDateMaintainedParsed;

        /// <summary>
        /// The PSG next payment date academies parsed.
        /// </summary>
        public static DateTime PsgNextPaymentDateAcademiesParsed;

        /// <summary>
        /// The PSG next payment date NMSS parsed.
        /// </summary>
        public static DateTime PsgNextPaymentDateNmssParsed;

        /// <summary>
        /// The DSG next payment date parsed.
        /// </summary>
        public static List<(string periodCode, DateTime paymentDate)> DsgNextPaymentDateFsParsed = new List<(string periodCode, DateTime paymentDate)>();

        /// <summary>
        /// The no PSG next payment date text.
        /// </summary>
        public static string NoPsgNextPaymentDateText;

        /// <summary>
        /// The no DSG next payment date text.
        /// </summary>
        public static string NoDsgNextPaymentDateText;

        /// <summary>
        /// The no next payment date texts for each financial year for DSG.
        /// </summary>
        public IDictionary<(int, int), string> DsgNoNextPaymentDateTexts = new Dictionary<(int, int), string>();

        /// <summary>
        /// The search results link text.
        /// </summary>
        public const string SearchResultsLinkText = "Search results";

        /// <summary>
        /// The view funding at organisation link text.
        /// </summary>
        public const string ViewFundingAtOrganisationLinkText = "View funding at organisation level";

        /// <summary>
        /// The choose how to view funding link text.
        /// </summary>
        public const string ChooseHowToViewFundingLinkText = "Choose how to view funding";

        /// <summary>
        /// The provider details page main provider name.
        /// </summary>
        public const string ProviderDetailsPageMainProviderName = "St Mary's Kilburn Church of England Primary School";

        /// <summary>
        /// The acadamic year.
        /// </summary>
        public const string AcadamicYear = "AY-1920";

        /// <summary>
        /// The financial year.
        /// </summary>
        public const string FinancialYear = "FY-2122";

        #endregion


        #region Private Fields

        /// <summary>
        /// The document dependent test.
        /// </summary>
        private readonly bool _documentDependentTest;

        /// <summary>
        /// The admin funding setting dependent test.
        /// </summary>
        private readonly bool _adminPageSettingDependentTest;

        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly ApplicationConfiguration _applicationConfiguration;

        private int _psgFundingStreamId,
            _dsgFundingStreamId,
            _psgYearSettingId,
            _dsgYearSettingId;

        private string _psgOriginalYearSetting,
            _dsgOriginalYearSetting;

        /// <summary>
        /// The PSG next payment date maintained.
        /// </summary>
        private string _psgNextPaymentDateMaintained;

        /// <summary>
        /// The PSG next payment date academies.
        /// </summary>
        private string _psgNextPaymentDateAcademies;

        /// <summary>
        /// The PSG next payment date NMSS.
        /// </summary>
        private string _psgNextPaymentDateNmss;

        /// <summary>
        /// The settings service.
        /// </summary>
        private IUserJourneyService _userJourneyService;

        /// <summary>
        /// The automation testing regression settings service.
        /// </summary>
        private IAutomationTestingSettingValueService _automationTestingSettingValueService;

        /// <summary>
        /// The automation testing setting type service.
        /// </summary>
        private IAutomationTestingSettingTypeService _automationTestingSettingTypeService;

        /// <summary>
        /// The document filename PSG.
        /// </summary>
        private string _documentFilenamePsg, _documentFilenamePreviousYearPsg;

        /// <summary>
        /// The document filename DSG.
        /// </summary>
        private string _documentFilenameDsg, _documentFilenamePreviousYearDsg;

        /// <summary>
        /// The document service.
        /// </summary>
        private IFundingDocumentStorageService _documentService;

        /// <summary>
        /// Next Payments for Funding stream PSG.
        /// </summary>
        private IList<NextPayment> _pSgNextPayments;

        /// <summary>
        /// Next Payments for Funding stream DSG.
        /// </summary>
        private IList<NextPayment> _dSgNextPayments;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamRegressionTestBase"/> class.
        /// </summary>
        /// <param name="documentDependentTest">if set to <c>true</c> [document dependent test].</param>
        /// <param name="adminPageSettingDependentTest">if set to <c>true</c> [admin page setting dependent test].</param>
        public FundingStreamRegressionTestBase(bool documentDependentTest = false, bool adminPageSettingDependentTest = false)
        {
            _documentDependentTest = documentDependentTest;
            _adminPageSettingDependentTest = adminPageSettingDependentTest;
            DisplayViewYourFunding = true;
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests Overrides

        /// <summary>
        /// Sets up.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [TestInitialize]
        public new async Task SetUp()
        {
            await SetUpEachTest();
            base.SetUp();
        }

        /// <summary>
        /// Tears down.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [TestCleanup]
        public new async Task TearDown()
        {
            await TearDownEachTest();
            base.TearDown();
        }

        /// <summary>
        /// Sets up each test.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        protected virtual async Task SetUpEachTest()
        {
            var context = GetContext();
            var fundingStreamRepository = GetFundingStreamRepository(context);

            _userJourneyService = new UserJourneyService(
                new MemoryCacheService(null, 0),
                GetMapper(),
                null,
                fundingStreamRepository);

            _automationTestingSettingValueService = new AutomationTestingSettingValueService(
                context,
                null);

            _automationTestingSettingTypeService = new AutomationTestingSettingTypeService(
                context,
                null);

            await UpdateSettings();
            await GetNextPaymentDates();
            if (_documentDependentTest)
            {
                await UploadDocuments();
            }

            if (_adminPageSettingDependentTest)
            {
                await _automationTestingSettingValueService.DeletePsgRegressionSettingValue(_psgFundingStreamId);
                await _automationTestingSettingTypeService.AddRegressionSettingValueTestSettingTypeAsync();
                await _automationTestingSettingTypeService.DeleteRegressionTestSettingTypeAsync();
            }
        }

        /// <summary>
        /// Tears down each test.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        protected virtual async Task TearDownEachTest()
        {
            if (_documentDependentTest)
            {
                await RemoveDocuments();
            }

            if (_adminPageSettingDependentTest)
            {
                await _automationTestingSettingTypeService.DeleteRegressionSettingValueTestSettingTypeAsync();
                await _automationTestingSettingTypeService.DeleteRegressionTestSettingTypeAsync();
            }

            await ResetSettings();
        }

        #endregion


        #region Public Properties

        public string AdminUserName => _applicationConfiguration.TestLoginUsername;

        public string AdminUserPassword => _applicationConfiguration.TestLoginPassword;

        public readonly (int, int)[] DsgActiveYears = new[]
        {
            (2021, 2022),
            (2020, 2021)
        };

        #endregion


        #region Helper Methods

        /// <summary>
        /// Gets the next payment date.
        /// </summary>
        /// <param name="nextPayments">The next payments.</param>
        /// <param name="nextPaymentDateTypeCode">The next payment date type code.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <returns>The next payment date.</returns>
        public DateTime? GetNextPaymentDate(
            IEnumerable<NextPayment> nextPayments,
            string nextPaymentDateTypeCode,
            string fundingPeriodCode)
        {
            var nextPaymentsList = nextPayments?.ToList();
            if (nextPaymentsList?.Any() == true)
            {
                return nextPaymentsList
                    .Where(x => x.Active)
                    .OrderBy(x => x.NextPaymentDate)
                    .FirstOrDefault(x =>
                        fundingPeriodCode.Equals(x.FundingPeriodCode, StringComparison.InvariantCultureIgnoreCase) &&
                        nextPaymentDateTypeCode.Equals(x.NextPaymentTypeCode, StringComparison.InvariantCultureIgnoreCase) &&
                        x.NextPaymentDate.Date >= DateTime.Now.Date)?.NextPaymentDate;
            }

            return null;
        }

        private static IMapper GetMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureWebMappings();
            return new Mapper(config);
        }

        private Context GetContext()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseSqlServer(_applicationConfiguration.ConnectionStrings.Vyf)
                .Options;

            return new Context(options);
        }

        private IFundingStreamRepository GetFundingStreamRepository(Context context)
        {
            return new FundingStreamRepository(context, null);
        }

        private async Task UpdateSettings()
        {
            var fundingStreams = await _userJourneyService.GetFundingStreams();

            // PSG
            _psgFundingStreamId = fundingStreams.Single(s => s.FundingStreamCode == FundingStreamCode.PEAndSport).Id;

            var psgSettings = await _userJourneyService.GetSettings(FundingStreamCode.PEAndSport);

            var psgYearSetting = psgSettings.Single(s => s.Setting.SettingName.Equals(SettingName.AcademicYear, StringComparison.OrdinalIgnoreCase));
            _psgYearSettingId = psgYearSetting.Id;
            _psgOriginalYearSetting = psgYearSetting.Value;

            await _automationTestingSettingValueService.UpdateSettingValueAsync(_psgFundingStreamId, _psgYearSettingId, $"{PsgCurrentYearFrom}{PsgCurrentYearTo % 100}");

            // DSG
            _dsgFundingStreamId = fundingStreams.Single(s => s.FundingStreamCode == FundingStreamCode.DSG).Id;

            var dsgSettings = await _userJourneyService.GetSettings(FundingStreamCode.DSG);

            var dsgYearSetting = dsgSettings.Single(s => s.Setting.SettingName.Equals(SettingName.FinancialYear, StringComparison.OrdinalIgnoreCase));
            _dsgYearSettingId = dsgYearSetting.Id;
            _dsgOriginalYearSetting = dsgYearSetting.Value;

            var dsgActiveYearsSettingValue = string.Join(",", DsgActiveYears.Select(years => $"{years.Item1}{years.Item2 % 100}"));

            await _automationTestingSettingValueService.UpdateSettingValueAsync(_dsgFundingStreamId, _dsgYearSettingId, dsgActiveYearsSettingValue);
        }

        private async Task ResetSettings()
        {
            // PSG
            await _automationTestingSettingValueService.UpdateSettingValueAsync(_psgFundingStreamId, _psgYearSettingId, _psgOriginalYearSetting);

            // DSG
            await _automationTestingSettingValueService.UpdateSettingValueAsync(_dsgFundingStreamId, _dsgYearSettingId, _dsgOriginalYearSetting);
        }

        private async Task GetNextPaymentDates()
        {
            var fundingStreams = await _userJourneyService.GetFundingStreams();

            //PSG
            _pSgNextPayments = fundingStreams.FirstOrDefault(s => s.FundingStreamCode == FundingStreamCode.PEAndSport).NextPayments.ToList();

            _psgNextPaymentDateMaintained = GetNextPaymentDate(_pSgNextPayments, "MS", AcadamicYear)?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            _psgNextPaymentDateAcademies = GetNextPaymentDate(_pSgNextPayments, "AD", AcadamicYear)?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            _psgNextPaymentDateNmss = GetNextPaymentDate(_pSgNextPayments, "NMSS", AcadamicYear)?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            DateTime.TryParseExact(_psgNextPaymentDateMaintained, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out PsgNextPaymentDateMaintainedParsed);
            DateTime.TryParseExact(_psgNextPaymentDateAcademies, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out PsgNextPaymentDateAcademiesParsed);
            DateTime.TryParseExact(_psgNextPaymentDateNmss, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out PsgNextPaymentDateNmssParsed);

            NoPsgNextPaymentDateText = PaymentTypeCode.GetNoNextPaymentForTheYearText(AcadamicYear);

            //DSG
            _dSgNextPayments = fundingStreams.FirstOrDefault(s => s.FundingStreamCode == FundingStreamCode.DSG).NextPayments.ToList();

            foreach (var (yearFrom, yearTo) in DsgActiveYears)
            {
                var activeYearFundingPeriodCode = $"FY-{yearFrom - 2000}{yearTo - 2000}";
                var nextPaymentDate = GetNextPaymentDate(_dSgNextPayments, "FS", activeYearFundingPeriodCode);

                DsgNextPaymentDateFsParsed.Add((activeYearFundingPeriodCode, nextPaymentDate.GetValueOrDefault()));
                DsgNoNextPaymentDateTexts.Add((yearFrom, yearTo), PaymentTypeCode.GetNoNextPaymentForTheYearText(activeYearFundingPeriodCode));
            }

            NoDsgNextPaymentDateText = PaymentTypeCode.GetNoNextPaymentForTheYearText(FinancialYear);
        }

        private async Task UploadDocuments()
        {
            _documentService = new AzureBlobStorageFundingDocumentService(
                _applicationConfiguration.BlobStorage.ServiceName,
                _applicationConfiguration.BlobStorage.Key,
                _applicationConfiguration.BlobStorage.ContainerName,
                new LoggerAdapter<AzureBlobStorageFundingDocumentService>(null));

            var documentFormat = FundingDocumentFileType.Spreadsheet_OpenFormat;

            // PSG
            var fundingPeriodCodePsg =
                $"{YearTypeCode.AcademicYear}-{PsgCurrentYearFrom % 100}{PsgCurrentYearTo % 100}";
            _documentFilenamePsg =
                $"{FundingStreamCode.PEAndSport}_{fundingPeriodCodePsg}_{PsgCurrentYearTo}0102_000000.{documentFormat}";

            var fundingPeriodCodePreviousYearPsg =
                $"{YearTypeCode.AcademicYear}-{(PsgCurrentYearFrom - 1) % 100}{(PsgCurrentYearTo - 1) % 100}";
            _documentFilenamePreviousYearPsg =
                $"{FundingStreamCode.PEAndSport}_{fundingPeriodCodePreviousYearPsg}_{PsgCurrentYearTo - 1}0102_000000.{documentFormat}";

            // DSG
            var fundingPeriodCodeDsg =
                $"{YearTypeCode.FinancialYear}-{DsgCurrentYearFrom % 100}{DsgCurrentYearTo % 100}";
            _documentFilenameDsg =
                $"{FundingStreamCode.DSG}_{fundingPeriodCodeDsg}_{DsgCurrentYearTo}0102_000000.{documentFormat}";

            var fundingPeriodCodePreviousYearDsg =
                $"{YearTypeCode.FinancialYear}-{(DsgCurrentYearFrom - 1) % 100}{(DsgCurrentYearTo - 1) % 100}";
            _documentFilenamePreviousYearDsg =
                $"{FundingStreamCode.DSG}_{fundingPeriodCodePreviousYearDsg}_{DsgCurrentYearTo - 1}0102_000000.{documentFormat}";

            var cutoffDate = new DateTime(2030, 1, 1);

            var currentYearPsgUpload = _documentService.Upload(
                    _documentFilenamePsg,
                    new byte[1024],
                    new DateTime(2019, 10, 28),
                    cutoffDate,
                    documentFormat,
                    fundingPeriodCodePsg,
                    FundingStreamCode.PEAndSport);

            var previousYearPsgUpload = _documentService.Upload(
                    _documentFilenamePreviousYearPsg,
                    new byte[1024],
                    new DateTime(PsgCurrentYearTo - 1, 1, 1),
                    cutoffDate,
                    documentFormat,
                    fundingPeriodCodePreviousYearPsg,
                    FundingStreamCode.PEAndSport);

            var currentYearDsgUpload = _documentService.Upload(
                    _documentFilenameDsg,
                    new byte[1024],
                    new DateTime(2019, 12, 19),
                    cutoffDate,
                    documentFormat,
                    fundingPeriodCodeDsg,
                    FundingStreamCode.DSG);

            var previousYearDsgUpload = _documentService.Upload(
                    _documentFilenamePreviousYearDsg,
                    new byte[1024],
                    new DateTime(DsgCurrentYearTo - 1, 1, 1),
                    cutoffDate,
                    documentFormat,
                    fundingPeriodCodePreviousYearDsg,
                    FundingStreamCode.DSG);

            await Task.WhenAll(currentYearPsgUpload, previousYearPsgUpload, currentYearDsgUpload, previousYearDsgUpload);
        }

        /// <summary>
        /// Removes the documents.
        /// </summary>
        private async Task RemoveDocuments()
        {
            await Task.WhenAll(new List<Task>
            {
                _documentService.Delete(_documentFilenamePsg),
                _documentService.Delete(_documentFilenamePreviousYearPsg),
                _documentService.Delete(_documentFilenameDsg),
                _documentService.Delete(_documentFilenamePreviousYearDsg)
            });
        }

        #endregion
    }
}