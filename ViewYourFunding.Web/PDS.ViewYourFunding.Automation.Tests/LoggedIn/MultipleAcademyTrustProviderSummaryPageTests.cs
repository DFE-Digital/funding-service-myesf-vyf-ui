using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class MultipleAcademyTrustProviderSummaryPageTests : LoggedInRegressionTestBase
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserName => _applicationConfiguration.TestLoginExternalUserMat;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAcademyTrustProviderSummaryPageTests"/> class.
        /// </summary>
        public MultipleAcademyTrustProviderSummaryPageTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void MultipleAcademyTrustStatementPage_BasicLayout()
        {
            // Arrange Act
            MultipleAcademyTrustStatementPage.NavigateToPageViaLogin(ProviderUserName, ProviderUserPassword);

            // Assert
            MultipleAcademyTrustStatementPage.EnsureCurrentProviderPage();
            MultipleAcademyTrustStatementPage.EnsureBreadcrumbs();
            MultipleAcademyTrustStatementPage.EnsureHasSections();
            MultipleAcademyTrustStatementPage.EnsureHasFilters();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void MultipleAcademyTrustStatementPage_CheckAuthorise()
        {
            // Arrange Act
            MultipleAcademyTrustStatementPage.NavigateToPageWithoutLogin();

            // Assert
            MultipleAcademyTrustStatementPage.EnsureNavigatedToLogin();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void MultipleAcademyTrustStatementPage_Check1619()
        {
            // Arrange Act
            MultipleAcademyTrustStatementPage.NavigateToPageViaLogin(ProviderUserName, ProviderUserPassword);
            MultipleAcademyTrustStatementPage.ExpandSixteen19();

            // Assert
            MultipleAcademyTrustStatementPage.EnsureCurrentProviderPage();

            MultipleAcademyTrustStatementPage.EnsureAllocationHistory();
            MultipleAcademyTrustStatementPage.EnsureExploreTopic();
            MultipleAcademyTrustStatementPage.EnsureSixteen19BreakdownTextDisplays();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void MultipleAcademyTrustStatementPage_CheckGAG()
        {
            // Arrange Act
            MultipleAcademyTrustStatementPage.NavigateToPageViaLogin(ProviderUserName, ProviderUserPassword);

            // Assert
            MultipleAcademyTrustStatementPage.EnsureCurrentProviderPage();
            MultipleAcademyTrustStatementPage.ExpandGAG();

            MultipleAcademyTrustStatementPage.EnsureAllocationHistory();
            MultipleAcademyTrustStatementPage.EnsureExploreTopic();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void MultipleAcademyTrustStatementPage_CheckPSG()
        {
            // Arrange Act
            MultipleAcademyTrustStatementPage.NavigateToPageViaLogin(ProviderUserName, ProviderUserPassword);
            MultipleAcademyTrustStatementPage.ExpandPSG();

            // Assert
            MultipleAcademyTrustStatementPage.EnsureAllocationHistory();
            MultipleAcademyTrustStatementPage.EnsurePSGFundingBreakdownLinkDisplays();
            MultipleAcademyTrustStatementPage.EnsureAllocationHistory();
            MultipleAcademyTrustStatementPage.EnsureResources();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void MultipleAcademyTrustStatementPage_FilterByAcademyName()
        {
            // Arrange Act
            MultipleAcademyTrustStatementPage.NavigateToPageViaLogin(ProviderUserName, ProviderUserPassword);
            MultipleAcademyTrustStatementPage.FilterStatementsByAcademyName("St Aidan's Church of England High School");

            // Assert
            MultipleAcademyTrustStatementPage.EnsureCurrentProviderPage();
            MultipleAcademyTrustStatementPage.EnsureBreadcrumbs();
            MultipleAcademyTrustStatementPage.EnsureHasFilters();
            MultipleAcademyTrustStatementPage.EnsureHasAcademySection("St Aidan's Church of England High School");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void MultipleAcademyTrustStatementPage_FilterByFundingType()
        {
            // Arrange Act
            MultipleAcademyTrustStatementPage.NavigateToPageViaLogin(ProviderUserName, ProviderUserPassword);
            MultipleAcademyTrustStatementPage.FilterStatementsByFundingType("PSG");

            // Assert
            MultipleAcademyTrustStatementPage.EnsureCurrentProviderPage();
            MultipleAcademyTrustStatementPage.EnsureBreadcrumbs();
            MultipleAcademyTrustStatementPage.EnsureHasFilters();
            MultipleAcademyTrustStatementPage.EnsureHasFundingTypeSection("PSG");
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void MultipleAcademyTrustStatementPage_FilterByLocalAuthority()
        {
            // Arrange Act
            MultipleAcademyTrustStatementPage.NavigateToPageViaLogin(ProviderUserName, ProviderUserPassword);
            MultipleAcademyTrustStatementPage.FilterStatementsByLocalAuthority("Southwark");

            // Assert
            MultipleAcademyTrustStatementPage.EnsureCurrentProviderPage();
            MultipleAcademyTrustStatementPage.EnsureBreadcrumbs();
            MultipleAcademyTrustStatementPage.EnsureHasFilters();
            MultipleAcademyTrustStatementPage.EnsureHasLocalAuthoritySection("Southwark");
        }
    }
}