using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class IndicativeProviderSummaryPage1619Tests : LoggedInRegressionTestBase
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        private const string ProviderUserNameIndicative = "10060613 - External User 109 - Academy Trust 1619";

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        #region Constructor

        public IndicativeProviderSummaryPage1619Tests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void Indicative_ProviderPage_BasicLayout()
        {
            // Arrange Act
            ProviderPage.NavigateToPageViaLogin(ProviderUserNameIndicative, ProviderUserPassword);

            // Assert
            ProviderPage.EnsureCurrentProviderPage();
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            ProviderPage.Expand1619();
            ProviderPage.Ensure1619FundingBreakdownLink();
            ProviderPage.EnsureAllocationHistory();
            ProviderPage.EnsureExploreTopic();
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