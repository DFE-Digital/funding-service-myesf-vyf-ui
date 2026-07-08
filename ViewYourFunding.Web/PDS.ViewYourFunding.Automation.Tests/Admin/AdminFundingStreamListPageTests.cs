using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    public class AdminFundingStreamListPageTests : FundingStreamRegressionTestBase
    {
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminFundingStreamListPage_BasicLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminHomePage.ClickOnTile("Funding streams settings");

            // Assert
            AdminFundingStreamListPage.EnsureFundingStreamTableExists();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminFundingStreamListPage_CrudOperations()
        {
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminHomePage.ClickOnTile("Funding streams settings");
            AdminFundingStreamListPage.AddFundingStream();

            // Assert
            AdminFundingStreamListPage.EnsureFundingStreamOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminHomePage.ClickOnTile("Funding streams settings");
            AdminFundingStreamListPage.EditFundingStream();

            // Assert
            AdminFundingStreamListPage.EnsureFundingStreamOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminHomePage.ClickOnTile("Funding streams settings");
            AdminFundingStreamListPage.DeleteFundingStream();

            // Assert
            AdminFundingStreamListPage.EnsureFundingStreamOperationConfirmation();
        }
    }
}