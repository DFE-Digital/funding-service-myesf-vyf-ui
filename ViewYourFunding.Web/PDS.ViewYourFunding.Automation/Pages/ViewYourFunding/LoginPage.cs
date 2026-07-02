using OpenQA.Selenium;
using System.Linq;
using System.Threading;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    public class LoginPage : ViewYourFundingBasePage
    {
        public static void Open()
        {
            Goto("view-latest-funding/login");
        }

        public static void Logout()
        {
            Goto("view-latest-funding/logout");
            Thread.Sleep(5000);
        }

        public static void LogoutIfLoggedIn()
        {
            if (AccountDetails.Any())
            {
                Goto("view-latest-funding/logout");
            }
        }

        public static void Login(string username, string password)
        {
            Signin
                .WithUsername(username)
                .WithPassword(password)
                .Proceed();

            Thread.Sleep(5000);
        }


        public static LoginCommand Signin
        {
            get
            {
                EnsureCurrentPage();
                return new LoginCommand();
            }
        }

        public static void EnsureCurrentPage()
        {
            EnsureCurrentPage("Login");
        }

        protected static IWebElement UsernameInputBox => Driver.Instance.WaitToFindElement(By.Id("Username"));

        protected static IWebElement PasswordInputBox => Driver.Instance.WaitToFindElement(By.Id("Password"));

        protected static IWebElement LoginButton => Driver.Instance.WaitToFindElement(By.CssSelector("button[value='login']"));

        protected static IWebElement OrganisationSelector =>
            Driver.Instance.FindElements(By.CssSelector("input[name='selected-organisation']")).FirstOrDefault();

        protected static IWebElement ContinueButton =>
            Driver.Instance.WaitToFindElement(By.CssSelector("input[type='submit']"));

        public class LoginCommand
        {
            #region Fields

            private string _password;
            private string _username;

            #endregion

            public LoginCommand WithPassword(string password)
            {
                _password = password;
                return this;
            }

            public LoginCommand WithUsername(string username)
            {
                _username = username;
                return this;
            }

            public void Proceed()
            {
                UsernameInputBox
                    .SendKeys(_username);

                PasswordInputBox
                    .SendKeys(_password);

                LoginButton
                    .MoveAndClick();
            }
        }
    }
}