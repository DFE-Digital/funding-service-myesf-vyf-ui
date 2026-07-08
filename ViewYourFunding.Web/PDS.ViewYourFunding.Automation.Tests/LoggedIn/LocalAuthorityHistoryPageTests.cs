using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class LocalAuthorityHistoryPageTests : LoggedInRegressionTestBase
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        private string LocalAuthorityUserPassword => _applicationConfiguration.TestLoginPassword;

        private string LocalAuthority1619UserName => "10004801 - External User 103 - LocalAuthoritySsfAndMss";

        private string LocalAuthorityWithMultiple1619AllocationHistory => "10004801 - External User 103 - LocalAuthoritySsfAndMss";

        private string LatestFundingBreakDownDate => "10 January 2021";

        private string PreviousYearsFundingBreakDownDate => "10 January 2020";


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAuthorityHistoryPageTests"/> class.
        /// </summary>
        public LocalAuthorityHistoryPageTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Layout
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityHistoryPage_Layout_SchoolSixthForm()
        {
            //Arrange Act
            LocalAuthorityHistoryPage.NavigateToPageViaLogin_SchoolSixthForm(LocalAuthority1619UserName, LocalAuthorityUserPassword);

            //Assert
            LocalAuthorityHistoryPage.EnsureCurrentLocalAuthorityHistoryPage_SchoolSixthForm();
            LocalAuthorityHistoryPage.EnsureBreadcrumbs();
            LocalAuthorityHistoryPage.EnsureInformationExchangeLinkDisplayed();
            LocalAuthorityHistoryPage.EnsureMultipleFundingBreakdownLinksDisplayed_SchoolSixthForm(new string[] { LatestFundingBreakDownDate, PreviousYearsFundingBreakDownDate });
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityHistoryPage_CheckAuthorise()
        {
            // Arrange Act
            LocalAuthorityHistoryPage.NavigateToPageWithoutLogin_SchoolSixthForm();

            // Assert
            LocalAuthorityHistoryPage.EnsureNavigatedToLogin();
        }

        #endregion Layout


        #region School sixth form funding

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityHistoryPage_SchoolsixthFormMultipleAllocationHistory()
        {
            LocalAuthorityHistoryPage.NavigateToPageViaLogin_SchoolSixthForm(LocalAuthorityWithMultiple1619AllocationHistory, LocalAuthorityUserPassword);
            LocalAuthorityHistoryPage.EnsureCurrentLocalAuthorityHistoryPage_SchoolSixthForm();
            LocalAuthorityHistoryPage.EnsureMultipleFundingBreakdownLinksDisplayed_SchoolSixthForm(new string[] { LatestFundingBreakDownDate, PreviousYearsFundingBreakDownDate });
            LocalAuthorityHistoryPage.EnsureInformationExchangeLinkDisplayed();

            LocalAuthorityHistoryPage.ClickOnFundingBreakdownLink_ForChosenDate(LatestFundingBreakDownDate);
            LocalAuthorityHistoryPage.EnsureLatestFundingBreakdownPageDisplayed_SchoolSixthForm();

            LocalAuthorityHistoryPage.ClickOnViewAllocationHistoryLink();
            LocalAuthorityHistoryPage.EnsureCurrentLocalAuthorityHistoryPage_SchoolSixthForm();

            LocalAuthorityHistoryPage.ClickOnFundingBreakdownLink_ForChosenDate(PreviousYearsFundingBreakDownDate);
            LocalAuthorityHistoryPage.EnsurePreviousYearsHistoricFundingBreakdownPageDisplayed_SchoolSixthForm();

            LocalAuthorityHistoryPage.ClickOnViewAllocationHistoryLink();
            LocalAuthorityHistoryPage.EnsureCurrentLocalAuthorityHistoryPage_SchoolSixthForm();
        }

        #endregion

    }
}