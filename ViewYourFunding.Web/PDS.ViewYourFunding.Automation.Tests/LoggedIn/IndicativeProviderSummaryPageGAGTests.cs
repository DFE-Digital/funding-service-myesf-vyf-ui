using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class IndicativeProviderSummaryPageGAGTests : LoggedInRegressionTestBase
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        private const string ProviderUserNameSpecial = "10087061 - External User 112 - Indicative Primary Special";

        private const string ProviderUserNameMainstream = "10086776 - External User 113 - Indicative Secondary Mainstream";

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        #region Constructor

        public IndicativeProviderSummaryPageGAGTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void Special_ProviderPage_BasicLayout()
        {
            // Arrange Act
            IndicativeProviderPage.NavigateToPageViaLogin(ProviderUserNameSpecial, ProviderUserPassword);

            // Assert
            IndicativeProviderPage.EnsureCurrentProviderPage();
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            IndicativeProviderPage.EnsureHasExpectedNumberOfSections(5);
            IndicativeProviderPage.ExpandGAG();
            IndicativeProviderPage.EnsureGAGSchoolBudgetShareDisplays();
            IndicativeProviderPage.EnsureHighNeedsDisplays();
            IndicativeProviderPage.EnsureGAGPostOpeningGrantDisplays();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void MainStream_ProviderPage_BasicLayout()
        {
            // Arrange Act
            IndicativeProviderPage.NavigateToPageViaLogin(ProviderUserNameMainstream, ProviderUserPassword);

            // Assert
            IndicativeProviderPage.EnsureCurrentProviderPage();
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            IndicativeProviderPage.EnsureHasExpectedNumberOfSections(5);
            IndicativeProviderPage.ExpandGAG();
            IndicativeProviderPage.EnsureGAGMinimumFundingGuaranteeDisplays();
            IndicativeProviderPage.EnsureGAGSchoolBudgetShareDisplays();
            IndicativeProviderPage.EnsureGAGStartUpGrantDisplays();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderPage_CheckAuthorise()
        {
            // Arrange Act
            IndicativeProviderPage.NavigateToPageWithoutLogin();

            // Assert
            IndicativeProviderPage.EnsureNavigatedToLogin();
        }
    }
}