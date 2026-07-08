using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class ProviderHistoryPageTests : LoggedInRegressionTestBase
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserName => _applicationConfiguration.TestLoginExternalUsernamePrimary;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        private string ProviderWithMultipleGagAllocationHistory => _applicationConfiguration.TestLoginExternalUserMutipleGagAllocationHistory;

        private string Provider1619UserName => _applicationConfiguration.TestLoginExternalUser1619;

        private string ProviderWithMultiple1619AllocationHistory => _applicationConfiguration.TestLoginExternalUserMutiple1619AllocationHistory;

        private string Provider1416UserName => _applicationConfiguration.TestLoginExternalUser1416;

        private string ProviderNMSSUserName => "10015031 - External User 114 - NMSS";

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderHistoryPageTests"/> class.
        /// </summary>
        public ProviderHistoryPageTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Layout

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_BasicLayout()
        {
            // Arrange Act
            ProviderHistoryPage.NavigateToPageViaLogin_GAG(ProviderUserName, ProviderUserPassword);

            // Assert
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_GAG();
            ProviderHistoryPage.EnsureBreadcrumbs();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_CheckAuthorise()
        {
            // Arrange Act
            ProviderHistoryPage.NavigateToPageWithoutLogin_GAG();

            // Assert
            ProviderHistoryPage.EnsureNavigatedToLogin();
        }

        #endregion Layout


        #region General Annual Grant

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_Layout_GAG()
        {
            ProviderHistoryPage.NavigateToPageViaLogin_GAG(ProviderUserName, ProviderUserPassword);
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_GAG();
            ProviderHistoryPage.EnsureFundingBreakdownLinkDisplayed_GAG();
            ProviderHistoryPage.EnsureInformationExchangeLinkDisplayed();


            ProviderHistoryPage.ClickOnFundingBreakdownWebLink_GAG();
            ProviderHistoryPage.EnsureFundingBreakdownPage_GAG();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_GAGMultipleAllocationHistory()
        {
            ProviderHistoryPage.NavigateToPageViaLogin_GAG(ProviderWithMultipleGagAllocationHistory, ProviderUserPassword);
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_GAG();
            ProviderHistoryPage.EnsureMultipleFundingBreakdownLinksDisplayed_GAG();
            ProviderHistoryPage.EnsureInformationExchangeLinkDisplayed();

            ProviderHistoryPage.ClickOnLatestFundingBreakdownLink_GAG();
            ProviderHistoryPage.EnsureLatestFundingBreakdownPageDisplayed_GAG();

            ProviderHistoryPage.ClickOnViewAllocationHistoryLink();
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_GAG();

            ProviderHistoryPage.ClickOnHistoricFundingBreakdownWebLink_GAG();
            ProviderHistoryPage.EnsureHistoricFundingBreakdownPageDisplayed_GAG();

            ProviderHistoryPage.ClickOnViewAllocationHistoryLink();
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_GAG();

            ProviderHistoryPage.ClickOnFinalFundingBreakdownLink_GAG();
            ProviderHistoryPage.EnsureFinalFundingBreakdownPageDisplayed_GAG();

            ProviderHistoryPage.ClickOnViewAllocationHistoryLink();
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_GAG();

            ProviderHistoryPage.ClickOnHistoricFromPreviousYearFundingBreakdownWebLink_GAG();
            ProviderHistoryPage.EnsurePreviousYearsHistoricFundingBreakdownPageDisplayed_GAG();
        }
        #endregion General Annual Grant


        #region PE and Sport Premium

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_Layout_PSG()
        {
            ProviderHistoryPage.NavigateToPageViaLogin_PSG(ProviderUserName, ProviderUserPassword);
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_PSG();
            ProviderHistoryPage.EnsureFundingBreakdownLinkDisplayed_PSG();
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_PSG();
        }

        #endregion PE and Sport Premium


        #region 16 to 19 Funding

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_CheckAuthorise_1619()
        {
            // Arrange Act
            ProviderHistoryPage.NavigateToPageWithoutLogin_1619();

            // Assert
            ProviderHistoryPage.EnsureNavigatedToLogin();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_Layout_1619()
        {
            ProviderHistoryPage.NavigateToPageViaLogin_1619(Provider1619UserName, ProviderUserPassword);
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_1619();
            ProviderHistoryPage.EnsureBreadcrumbs();
            ProviderHistoryPage.EnsureInformationExchangeLinkDisplayed();
            ProviderHistoryPage.EnsureFundingBreakdownLinkDisplayed_1619();

            ProviderHistoryPage.ClickOnFundingBreakdownWebLink_1619();
            ProviderHistoryPage.EnsureFundingBreakdownPage_1619();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_1619MultipleAllocationHistory()
        {
            ProviderHistoryPage.NavigateToPageViaLogin_1619(ProviderWithMultiple1619AllocationHistory, ProviderUserPassword);
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_1619();
            ProviderHistoryPage.EnsureMultipleFundingBreakdownLinksDisplayed_1619();
            ProviderHistoryPage.EnsureInformationExchangeLinkDisplayed();

            ProviderHistoryPage.ClickOnLatestFundingBreakdownLink_1619();
            ProviderHistoryPage.EnsureLatestFundingBreakdownPageDisplayed_1619();

            ProviderHistoryPage.ClickOnViewAllocationHistoryLink();
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_1619();

            ProviderHistoryPage.ClickOnHistoricFundingBreakdownWebLink_1619();
            ProviderHistoryPage.EnsureHistoricFundingBreakdownPageDisplayed_1619();

            ProviderHistoryPage.ClickOnViewAllocationHistoryLink();
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_1619();

            ProviderHistoryPage.ClickOnFinalFundingBreakdownLink_1619();
            ProviderHistoryPage.EnsureFinalFundingBreakdownPageDisplayed_1619();

            ProviderHistoryPage.ClickOnViewAllocationHistoryLink();
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_1619();

            ProviderHistoryPage.ClickOnHistoricFromPreviousYearFundingBreakdownWebLink_1619();
            ProviderHistoryPage.EnsurePreviousYearsHistoricFundingBreakdownPageDisplayed_1619();
        }

        #endregion


        #region 14 to 16 Funding

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_CheckAuthorise_1416()
        {
            // Arrange Act
            ProviderHistoryPage.NavigateToPageWithoutLogin_1416();

            // Assert
            ProviderHistoryPage.EnsureNavigatedToLogin();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_Layout_1416()
        {
            ProviderHistoryPage.NavigateToPageViaLogin_1416(Provider1416UserName, ProviderUserPassword);
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_1416();
            ProviderHistoryPage.EnsureBreadcrumbs();
            ProviderHistoryPage.EnsureInformationExchangeLinkDisplayed();
            ProviderHistoryPage.EnsureFundingBreakdownLinkDisplayed_1416();

            ProviderHistoryPage.ClickOnFundingBreakdownWebLink_1416();
            ProviderHistoryPage.EnsureFundingBreakdownPage_1416();
        }

        #endregion


        #region NMSS Funding

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_CheckAuthorise_NMSS()
        {
            // Arrange Act
            ProviderHistoryPage.NavigateToPageWithoutLogin_NMSS();

            // Assert
            ProviderHistoryPage.EnsureNavigatedToLogin();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderHistoryPage_Layout_NMSS()
        {
            ProviderHistoryPage.NavigateToPageViaLogin_NMSS(ProviderNMSSUserName, ProviderUserPassword);
            ProviderHistoryPage.EnsureCurrentProviderHistoryPage_NMSS();
            ProviderHistoryPage.EnsureBreadcrumbs();
            ProviderHistoryPage.EnsureDocumentExchangeLinkDisplayed();
            ProviderHistoryPage.EnsureFundingBreakdownLinkDisplayed_NMSS();
        }

        #endregion
    }
}