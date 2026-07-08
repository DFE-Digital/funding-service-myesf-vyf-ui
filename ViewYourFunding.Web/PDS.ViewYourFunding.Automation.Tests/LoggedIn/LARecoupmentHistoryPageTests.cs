using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class LARecoupmentHistoryPageTests : LoggedInRegressionTestBase
    {
        #region Private fields

        private static string TestLoginExternalUserLaSsfAndMss => "10004801 - External User 103 - LocalAuthoritySsfAndMss";

        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LARecoupmentHistoryPageTests"/> class.
        /// </summary>
        public LARecoupmentHistoryPageTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LARecoupmentHistoryPage_BasicLayout()
        {
            // Arrange Act
            LARecoupmentHistoryPage.NavigateToLaRecoupmentHistoryPageViaLogin(TestLoginExternalUserLaSsfAndMss, ProviderUserPassword);

            // Assert
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            LARecoupmentHistoryPage.EnsureCurrentPage("Recoupment history", true);
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LARecoupmentHistoryPage_ExpandedAccordion_BasicLayout()
        {
            // Arrange Act
            LARecoupmentHistoryPage.NavigateToLaRecoupmentHistoryPageViaLogin(TestLoginExternalUserLaSsfAndMss, ProviderUserPassword);

            // Assert
            LARecoupmentHistoryPage.EnsureReportDataLinksDisplayed();
            LARecoupmentHistoryPage.EnsureRecoupmentReportDataPublicationLinks();
            LARecoupmentHistoryPage.EnsureRecoupmentReportDataPublicationLinkTexts();
            LARecoupmentHistoryPage.EnsureRecoupmentReportDataPublicationTotalRecoupmentValue();
            LARecoupmentHistoryPage.EnsureTabElements();
            LARecoupmentHistoryPage.EnsureRecoupmentReportHeadingAccordionSections();
            LARecoupmentHistoryPage.EnsureRecoupmentReportTableSections();
        }

        #endregion
    }
}
