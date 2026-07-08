using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    [TestCategory("Regression"), TestCategory("CoreRegression")]
    public class SearchProviderDataPageTests : FundingStreamRegressionTestBase
    {
        private static string ThreeFundingStreamUser => "Internal User39 AllocationsAdministrator - NMSS 14-16 16-19";

        private static string OneFundingStreamUser => "Internal User38 AllocationsAdministrator - PPG";

        public SearchProviderDataPageTests() : base(adminPageSettingDependentTest: true)
        {
        }

        [TestMethod]
        public void SearchProviderDataPage_SelectAFundingStreamPage_MoreThanOneFundingStream()
        {
            // Arrange Act
            BusinessAllocationsManagementPage.NavigateToPage(ThreeFundingStreamUser, AdminUserPassword);
            BusinessAllocationsManagementPage.NavigateToSearchProviderData();

            // Assert
            BusinessAllocationsManagementPage.EnsureBreadcrumbs();
            BusinessAllocationsManagementPage.EnsureSearchProviderDataPageElements();
        }

        [TestMethod]
        public void SearchProviderDataPage_SearchByProviderAndYearPage_MoreThanOneFundingStream()
        {
            // Arrange Act
            BusinessAllocationsManagementPage.NavigateToPage(ThreeFundingStreamUser, AdminUserPassword);
            BusinessAllocationsManagementPage.NavigateToSearchProviderData();
            BusinessAllocationsManagementPage.NavigateToSearchByProviderAndYear();

            // Assert
            BusinessAllocationsManagementPage.EnsureBreadcrumbs();
            BusinessAllocationsManagementPage.EnsureSearchByProviderAndYearPageElements();
        }

        [TestMethod]
        public void SearchProviderDataPage_SearchByProviderAndYearDetailsPage_MoreThanOneFundingStream()
        {
            // Arrange Act
            BusinessAllocationsManagementPage.NavigateToPage(ThreeFundingStreamUser, AdminUserPassword);
            BusinessAllocationsManagementPage.NavigateToSearchProviderData();
            BusinessAllocationsManagementPage.NavigateToSearchByProviderAndYear();
            BusinessAllocationsManagementPage.NavigateToSearchByProviderAndYearDetails();

            // Assert
            BusinessAllocationsManagementPage.EnsureBreadcrumbs();
            BusinessAllocationsManagementPage.EnsureSearchByProviderAndYearDetailsPageElements();
        }

        [TestMethod]
        public void SearchProviderDataPage_SelectAFundingStreamPage_OneFundingStream()
        {
            // Arrange Act
            BusinessAllocationsManagementPage.NavigateToPage(OneFundingStreamUser, AdminUserPassword);
            BusinessAllocationsManagementPage.NavigateToSearchProviderData();

            // Assert
            BusinessAllocationsManagementPage.EnsureBreadcrumbs();
            BusinessAllocationsManagementPage.EnsureSearchByProviderAndYearPageElements();
        }

        [TestMethod]
        public void SearchProviderDataPage_SearchByProviderAndYearDetailsPage_OneFundingStream()
        {
            // Arrange Act
            BusinessAllocationsManagementPage.NavigateToPage(OneFundingStreamUser, AdminUserPassword);
            BusinessAllocationsManagementPage.NavigateToSearchProviderData();
            BusinessAllocationsManagementPage.NavigateToSearchByProviderAndYearDetails();

            // Assert
            BusinessAllocationsManagementPage.EnsureBreadcrumbs();
            BusinessAllocationsManagementPage.EnsureSearchByProviderAndYearDetailsPageElements();
        }
    }
}
