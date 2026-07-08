using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    public class AdminSettingTypeListPageTests : FundingStreamRegressionTestBase
    {
        public AdminSettingTypeListPageTests()
        : base(adminPageSettingDependentTest: true)
        {
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminSettingTypeListPage_BasicLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminHomePage.ClickOnTile("Setting Types");

            // Assert
            AdminSettingTypeListPage.EnsureSettingsTable();
            AdminSettingTypeListPage.EnsureSettingTypesHeader();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminSettingTypesPage_CRUD_Operations_On_Settings()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminHomePage.ClickOnTile("Setting Types");

            AdminSettingTypeListPage.AddSetting();

            // Assert
            AdminFundingStreamSettingsPage.EnsureSettingOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminHomePage.ClickOnTile("Setting Types");
            AdminSettingTypeListPage.EditSetting();

            // Assert
            AdminSettingTypeListPage.EnsureSettingOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminHomePage.ClickOnTile("Setting Types");
            AdminSettingTypeListPage.DeleteSetting();

            // Assert
            AdminSettingTypeListPage.EnsureSettingOperationConfirmation();
        }
    }
}