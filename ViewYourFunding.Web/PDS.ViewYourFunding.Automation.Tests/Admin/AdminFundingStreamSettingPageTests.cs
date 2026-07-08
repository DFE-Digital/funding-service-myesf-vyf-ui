using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    public class AdminFundingStreamSettingPageTests : FundingStreamRegressionTestBase
    {
        public AdminFundingStreamSettingPageTests()
        : base(adminPageSettingDependentTest: true)
        {
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminFundingStreamSettingsPage_BasicLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminFundingStreamSettingsPage.NavigateToFundingStreamSettingsPage();

            // Assert
            AdminFundingStreamSettingsPage.EnsureNextPaymentTypesOption();
            AdminFundingStreamSettingsPage.EnsureNextPaymentsOption();
            AdminFundingStreamSettingsPage.EnsurePublicationsOption();
            AdminFundingStreamSettingsPage.EnsureFundingStreamSettingsTable();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminFundingStreamSettingsPage_CRUD_Operations_On_FundingStreamSettings()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminFundingStreamSettingsPage.NavigateToFundingStreamSettingsPage();

            AdminFundingStreamSettingsPage.AddFundingStreamSetting();

            // Assert
            AdminFundingStreamSettingsPage.EnsureSettingOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminFundingStreamSettingsPage.NavigateToFundingStreamSettingsPage();
            AdminFundingStreamSettingsPage.EditFundingStreamSetting();

            // Assert
            AdminFundingStreamSettingsPage.EnsureSettingOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminFundingStreamSettingsPage.NavigateToFundingStreamSettingsPage();
            AdminFundingStreamSettingsPage.DeleteFundingStreamSetting();

            // Assert
            AdminFundingStreamSettingsPage.EnsureSettingOperationConfirmation();
        }
    }
}