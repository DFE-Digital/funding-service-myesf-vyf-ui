using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    public class LayoutManagementPageTests : FundingStreamRegressionTestBase
    {
        public LayoutManagementPageTests()
       : base(adminPageSettingDependentTest: true)
        {
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminLayoutManagementPage_BasicLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            LayoutManagementPage.NavigateToLayoutManagementPage();

            // Assert
            LayoutManagementPage.EnsurePageTitle();
            LayoutManagementPage.EnsurePageHasFilters();
            LayoutManagementPage.EnsurePageHasLayoutTable();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminLayoutManagementPage_FilteringAndPagination_Operations()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            LayoutManagementPage.NavigateToLayoutManagementPage();
            LayoutManagementPage.DoPagination();

            // Assert
            LayoutManagementPage.EnsurePaginationResultsChanged();

            AdminHomePage.NavigateToPage();
            LayoutManagementPage.NavigateToLayoutManagementPage();
            LayoutManagementPage.DoFundingStreamFiltering();

            //Assert
            LayoutManagementPage.EnsureFundingStreamFilterationResultsChanged();

            AdminHomePage.NavigateToPage();
            LayoutManagementPage.NavigateToLayoutManagementPage();
            LayoutManagementPage.DoFundingViewTypeFiltering();
            LayoutManagementPage.DoFundingViewScopeFiltering();

            //Assert
            LayoutManagementPage.EnsureFundingViewTypeFilterationResultsChanged();
            LayoutManagementPage.EnsureFundingViewScopeFilterationResultsChanged();
        }
    }
}