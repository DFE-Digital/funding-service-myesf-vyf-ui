using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    public class AdminHomePageTests : FundingStreamRegressionTestBase
    {
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminHomePage_BasicLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);

            // Assert
            AdminHomePage.EnsureAdminTiles();
        }
    }
}