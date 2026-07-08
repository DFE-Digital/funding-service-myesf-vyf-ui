using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    public class AdminGeneralSettingListPageTests : FundingStreamRegressionTestBase
    {
        public AdminGeneralSettingListPageTests()
        : base(adminPageSettingDependentTest: true)
        {
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminGeneralSettingListPage_BasicLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminGeneralSettingsPage.NavigateToGeneralSettingsPage();

            //Assert
            AdminGeneralSettingsPage.EnsurePageTitle();
            AdminGeneralSettingsPage.EnsurePageHasGeneralSettingsTable();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminGeneralSettingListPage_EditOperation()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminGeneralSettingsPage.NavigateToGeneralSettingsPage();
            AdminGeneralSettingsPage.EditGeneralSetting();

            //Assert
            AdminGeneralSettingsPage.EnsureGeneralSettingsOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminGeneralSettingsPage.NavigateToGeneralSettingsPage();
            AdminGeneralSettingsPage.RevertGeneralSetting();

            //Assert
            AdminGeneralSettingsPage.EnsureGeneralSettingsOperationConfirmation();
        }
    }
}
