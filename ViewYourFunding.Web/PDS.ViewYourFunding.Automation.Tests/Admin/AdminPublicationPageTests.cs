using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    public class AdminPublicationPageTests : FundingStreamRegressionTestBase
    {
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminPublicationPage_CRUD_Operations_On_Publications()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminPublicationPage.NavigateToFundingStreamSettingsPage();

            AdminPublicationPage.AddPublication();

            // Assert
            AdminPublicationPage.EnsureSettingOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminPublicationPage.NavigateToFundingStreamSettingsPage();
            AdminPublicationPage.EditPublication();

            // Assert
            AdminPublicationPage.EnsureSettingOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminPublicationPage.NavigateToFundingStreamSettingsPage();
            AdminPublicationPage.DeletePublication();

            // Assert
            AdminPublicationPage.EnsureSettingOperationConfirmation();
        }
    }
}