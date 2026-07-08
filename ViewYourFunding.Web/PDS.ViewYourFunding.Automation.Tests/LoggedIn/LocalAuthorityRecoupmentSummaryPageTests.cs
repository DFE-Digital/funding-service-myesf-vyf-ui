using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class LocalAuthorityRecoupmentSummaryPageTests : LoggedInRegressionTestBase
    {
        #region Private fields

        private static string TestLoginExternalUserLaSsfAndMss => "10004801 - External User 103 - LocalAuthoritySsfAndMss";

        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAuthorityRecoupmentSummaryPageTests"/> class.
        /// </summary>
        public LocalAuthorityRecoupmentSummaryPageTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityRecoupmentSummaryPage_BasicLayout()
        {
            // Arrange Act
            LocalAuthorityRecoupmentSummaryPage.NavigateToLaRecoupmentSummaryPageViaLogin(TestLoginExternalUserLaSsfAndMss, ProviderUserPassword);

            // Assert
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            LocalAuthorityRecoupmentSummaryPage.EnsureCurrentPage("Recoupment reports");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityRecoupmentSummaryPage_ExpandedAccordion_BasicLayout()
        {
            // Arrange Act
            LocalAuthorityRecoupmentSummaryPage.NavigateToLaRecoupmentSummaryPageViaLogin(TestLoginExternalUserLaSsfAndMss, ProviderUserPassword);
            LocalAuthorityRecoupmentSummaryPage.ExpandAccordion();

            // Assert
            LocalAuthorityRecoupmentSummaryPage.EnsureRecoupmentHistory();
            LocalAuthorityRecoupmentSummaryPage.EnsureTabLinks();
        }

        #endregion

    }
}
