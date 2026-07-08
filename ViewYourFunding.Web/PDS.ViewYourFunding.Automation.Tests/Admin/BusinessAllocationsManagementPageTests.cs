using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    [TestCategory("Regression"), TestCategory("CoreRegression")]
    public class BusinessAllocationsManagementPageTests : FundingStreamRegressionTestBase
    {
        private static string TestBusinessFundingstreamUser => "Internal User39 AllocationsAdministrator - NMSS 14-16 16-19";

        public BusinessAllocationsManagementPageTests()
        : base(adminPageSettingDependentTest: true)
        {
        }

        [TestMethod]
        public void BusinessAllocationsManagementHomePage_BasicLayout()
        {
            // Arrange Act
            BusinessAllocationsManagementPage.NavigateToPage(AdminUserName, AdminUserPassword);

            // Assert
            BusinessAllocationsManagementPage.EnsurePageTitle("Manage allocations data");
            BusinessAllocationsManagementPage.EnsurePageHasAllocationsManagementActions();
        }

        [TestMethod]
        public void BusinessAllocationsManagementRequestDataPage_BasicLayout()
        {
            // Arrange Act
            BusinessAllocationsManagementPage.NavigateToPage(TestBusinessFundingstreamUser, AdminUserPassword);
            BusinessAllocationsManagementPage.NavigateToRequestDataPage();

            // Assert
            BusinessAllocationsManagementPage.EnsurePageTitle("Request allocations data");

            BusinessAllocationsManagementPage.EnsureBreadcrumbs();
            BusinessAllocationsManagementPage.EnsureRunFeedReaderPageElements();
        }

        [TestMethod]
        public void BusinessAllocationsManagementRequestDataStatusPageFromRequestData_BasicLayout()
        {
            // Arrange Act
            BusinessAllocationsManagementPage.NavigateToPage(TestBusinessFundingstreamUser, AdminUserPassword);
            BusinessAllocationsManagementPage.NavigateToRequestDataPage();

            BusinessAllocationsManagementPage.NavigateToRunStatusPageFromRequestData();

            // Assert
            BusinessAllocationsManagementPage.EnsurePageTitle("Confirm data request");
        }

        [TestMethod]
        public void BusinessAllocationsManagementRequestDataStatusPageFromHome_BasicLayout()
        {
            // Arrange Act
            BusinessAllocationsManagementPage.NavigateToPage(TestBusinessFundingstreamUser, AdminUserPassword);

            BusinessAllocationsManagementPage.NavigateToRunStatusPageFromHome();

            // Assert
            BusinessAllocationsManagementPage.EnsurePageTitle("Check status");
        }

        [TestMethod]
        public void RunPDFComparisonLayout()
        {
            // Arrange Act
            BusinessAllocationsManagementPage.NavigateToPage(TestBusinessFundingstreamUser, AdminUserPassword);
            BusinessAllocationsManagementPage.NavigateToRunPdfComparisonPage();

            // BusinessAllocationsManagementPage
            BusinessAllocationsManagementPage.EnsurePageTitle("Compare statements");
            BusinessAllocationsManagementPage.EnsureBreadcrumbs();
            BusinessAllocationsManagementPage.EnsureRunPdfComparisonPageElements();
        }
    }
}