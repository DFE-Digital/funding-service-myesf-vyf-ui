using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    [TestClass]
    [Ignore]
    public class LoginTests : FundingStreamRegressionTestBase
    {
        private readonly string username;
        private readonly string password;

        public LoginTests() : base(true)
        {
            var config = Config.ConfigHelper.GetApplicationConfiguration();

            username = config.TestLoginUsername;
            password = config.TestLoginPassword;
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void Login_CanLogin()
        {
            // Arrange Act
            LoginPage.Open();
            LoginPage.Login(username, password);

            // Assert
            StartPage.EnsureCurrentPage("View latest funding");
            StartPage.EnsureLoggedIn();
        }

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void Login_CanLoginAndLogout()
        {
            LoginPage.LogoutIfLoggedIn();

            // Arrange Act
            StartPage.Open();
            LoginPage.Open();

            LoginPage.Login(username, password);

            // Assert
            StartPage.EnsureCurrentPage("View latest funding");
            StartPage.EnsureLoggedIn();

            LoginPage.Logout();

            StartPage.EnsureCurrentPage("View latest funding");
            StartPage.EnsureLoggedOut();
        }
    }
}