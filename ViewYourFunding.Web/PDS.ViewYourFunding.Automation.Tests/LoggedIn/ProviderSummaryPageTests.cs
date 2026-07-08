using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class ProviderSummaryPageTests : LoggedInRegressionTestBase
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserName_JustGag => _applicationConfiguration.TestLoginExternalUsernamePrimaryInYearZeroStartUpFunding;

        private string ProviderUserName_Both => _applicationConfiguration.TestLoginExternalUsernameAllThrough;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        /// <summary>
        /// Gets or sets the test login external user for local authority ssf.
        /// </summary>
        private string TestLoginExternalUserLaSsf { get; set; } = "10004801 - External User 103 - LocalAuthoritySsfAndMss";

        /// <summary>
        /// Gets or sets the test login external user for NMSS.
        /// </summary>
        private string TestLoginExternalUserNMSS { get; set; } = "10015031 - External User 114 - NMSS";

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderSummaryPageTests"/> class.
        /// </summary>
        public ProviderSummaryPageTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPage_BasicLayout()
        {
            // Arrange Act
            ProviderPage.NavigateToPageViaLogin(ProviderUserName_Both, ProviderUserPassword);

            // Assert
            ProviderPage.EnsureCurrentProviderPage();
            ProviderPage.EnsureBreadcrumbs();
            ProviderPage.EnsureHasExpectedNumberOfSections(5);
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPage_CheckAuthorise()
        {
            // Arrange Act
            ProviderPage.NavigateToPageWithoutLogin();

            // Assert
            ProviderPage.EnsureNavigatedToLogin();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPage_CheckGAG()
        {
            // Arrange Act
            ProviderPage.NavigateToPageViaLogin(ProviderUserName_JustGag, ProviderUserPassword);

            // Assert
            ProviderPage.EnsureCurrentProviderPage();
            ProviderPage.ExpandGAG();

            ProviderPage.EnsureGAGMinimumFundingGuaranteeDisplays();

            ProviderPage.EnsureAllocationHistory();
            ProviderPage.EnsureExploreTopic();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPage_CheckPSG()
        {
            // Arrange Act
            ProviderPage.NavigateToPageViaLogin(ProviderUserName_Both, ProviderUserPassword);

            // Assert
            ProviderPage.EnsureCurrentProviderPage();
            ProviderPage.ExpandPSG();

            ProviderPage.EnsurePSGTableDisplays();
            ProviderPage.EnsurePSGSpreadsheetLink();
            ProviderPage.EnsurePaymentDates();
            ProviderPage.EnsureAllocationHistory();
            ProviderPage.EnsureResources();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPage_Check1619()
        {
            // Arrange Act
            ProviderPage.NavigateToPageViaLogin(ProviderUserName_Both, ProviderUserPassword);

            // Assert
            ProviderPage.EnsureCurrentProviderPage();
            ProviderPage.Expand1619();
            ProviderPage.Ensure1619FundingBreakdownLink();
            ProviderPage.EnsureAllocationHistory();
            ProviderPage.EnsureExploreTopic();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPage_Check1619LocalAuthoritySsf()
        {
            // Arrange Act
            ProviderPage.NavigateToPageViaLogin(TestLoginExternalUserLaSsf, ProviderUserPassword);

            // Assert
            ProviderPage.EnsureCurrentProviderPage();
            ProviderPage.ExpandLaSsf();

            ProviderPage.EnsureLaSsfFundingBreakdownLink();
            ProviderPage.EnsureLaSsfNewDownloadLink();
            ProviderPage.EnsureAllocationHistory();
            ProviderPage.EnsureExploreTopic();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPage_Check1416()
        {
            // Arrange Act
            ProviderPage.NavigateToPageViaLogin(ProviderUserName_Both, ProviderUserPassword);

            // Assert
            ProviderPage.EnsureCurrentProviderPage();
            ProviderPage.Expand1416();

            ProviderPage.Ensure1416FundingBreakdownLinks();
            ProviderPage.EnsureAllocationHistory();
            ProviderPage.EnsureExploreTopic();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPage_CheckNMSS()
        {
            // Arrange Act
            ProviderPage.NavigateToPageViaLogin(TestLoginExternalUserNMSS, ProviderUserPassword);

            // Assert
            ProviderPage.EnsureCurrentProviderPage();
            ProviderPage.ExpandNMSS();

            ProviderPage.EnsureNMSSFundingBreakdownLinks();
            ProviderPage.EnsureAllocationHistory();
            ProviderPage.EnsureExploreTopic();
        }
    }
}