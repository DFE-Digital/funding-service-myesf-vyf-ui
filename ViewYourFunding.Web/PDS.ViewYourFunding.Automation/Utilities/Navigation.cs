using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace ViewYourFunding.Automation.Utilities
{
    /// <summary>
    /// The Navigation class.
    /// </summary>
    public class Navigation
    {
        #region Main Api

        /// <summary>
        /// Goes to settings.
        /// </summary>
        public static void GoToSettings()
        {
            SettingsLInk.MoveAndClick();
        }

        /// <summary>
        /// Goes to terms and conditions.
        /// </summary>
        public static void GoToTermsAndConditions()
        {
            TermsAndConditionsLink.MoveAndClick();
        }

        /// <summary>
        /// Signouts this instance.
        /// </summary>
        public static void Signout()
        {
            SignoutLink.MoveAndClick();
        }

        /// <summary>
        /// Tries the signout.
        /// </summary>
        public static void TrySignout()
        {
            var matches = Driver.Instance.FindElements(SignoutLinkByLocator);

            if (matches.Count == 0)
            {
                return;
            }

            matches[0].MoveAndClick();
        }

        /// <summary>
        /// Gotoes the breadcrumb.
        /// </summary>
        /// <param name="text">The text.</param>
        public static void GotoBreadcrumb(string text)
        {
            BreadcrumbContainer.FindElement(By.LinkText(text)).MoveAndClick();
        }

        /// <summary>
        /// Goes to support.
        /// </summary>
        public static void GoToSupport()
        {
            SupportLink.MoveAndClick();
        }

        #endregion

        /// <summary>
        /// Ensures the settings link present.
        /// </summary>
        public static void EnsureSettingsLinkPresent()
        {
            Driver.Instance.AssertShouldExist(SettingsLinkByLocator);
        }

        /// <summary>
        /// Ensures the terms and conditions link present.
        /// </summary>
        public static void EnsureTermsAndConditionsLinkPresent()
        {
            Driver.Instance.AssertShouldExist(TermsAndConditionsLinkByLocator);
        }

        /// <summary>
        /// Ensures the privacy and cookies link present.
        /// </summary>
        public static void EnsurePrivacyAndCookiesLinkPresent()
        {
            Driver.Instance.AssertShouldExist(PrivacyAndCookiesLocator);
        }

        /// <summary>
        /// Ensures the support link present.
        /// </summary>
        public static void EnsureSupportLinkPresent()
        {
            Driver.Instance.AssertShouldExist(SupportLinkByLocator);
        }

        /// <summary>
        /// Ensures the support link is not present.
        /// </summary>
        public static void EnsureSupportLinkIsNotPresent()
        {
            Driver.Instance.AssertShouldNotExist(SupportLinkByLocator);
        }

        /// <summary>
        /// Ensures the external details.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="ukprn">The ukprn.</param>
        public static void EnsureExternalDetails(string providerName, string ukprn)
        {
            Assert.AreEqual("Sign out", UserSignOut.Text, "User details not as expected");
        }

        /// <summary>
        /// Ensures the user is logged in.
        /// </summary>
        /// <param name="username">The username.</param>
        public static void EnsureUserIsLoggedIn(string username)
        {
            Assert.AreEqual("Sign out", UserSignOut.Text, "User is not logged in");
        }

        /// <summary>
        /// Ensures the internal details as external.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="ukprn">The ukprn.</param>
        public static void EnsureInternalDetailsAsExternal(string providerName, string ukprn)
        {
            Assert.AreEqual($"Viewing as {providerName} (UKPRN: {ukprn})", UserDetails.Text, "User details not as expected");
        }


        #region Controls

        /// <summary>
        /// Gets the breadcrumb container.
        /// </summary>
        /// <value>
        /// The breadcrumb container.
        /// </value>
        protected static IWebElement BreadcrumbContainer => Driver.Instance.WaitToFindElement(By.Id("global-breadcrumb"));

        /// <summary>
        /// Gets the user sign out.
        /// </summary>
        /// <value>
        /// The user sign out.
        /// </value>
        protected static IWebElement UserSignOut => Driver.Instance.WaitToFindElement(By.LinkText("Sign out"));

        /// <summary>
        /// Gets the user details.
        /// </summary>
        /// <value>
        /// The user details.
        /// </value>
        protected static IWebElement UserDetails => Driver.Instance.WaitToFindElement(By.Id("userdetails"));

        /// <summary>
        /// Gets the settings l ink.
        /// </summary>
        /// <value>
        /// The settings l ink.
        /// </value>
        protected static IWebElement SettingsLInk => Driver.Instance.WaitToFindElement(SettingsLinkByLocator);

        /// <summary>
        /// Gets the settings link by locator.
        /// </summary>
        /// <value>
        /// The settings link by locator.
        /// </value>
        protected static By SettingsLinkByLocator => By.LinkText("Settings");

        /// <summary>
        /// Gets the terms and conditions link.
        /// </summary>
        /// <value>
        /// The terms and conditions link.
        /// </value>
        protected static IWebElement TermsAndConditionsLink => Driver.Instance.WaitToFindElement(TermsAndConditionsLinkByLocator);

        /// <summary>
        /// Gets the terms and conditions link by locator.
        /// </summary>
        /// <value>
        /// The terms and conditions link by locator.
        /// </value>
        protected static By TermsAndConditionsLinkByLocator => By.LinkText("Terms and conditions");

        /// <summary>
        /// Gets the privacy and cookies link.
        /// </summary>
        /// <value>
        /// The privacy and cookies link.
        /// </value>
        protected static IWebElement PrivacyAndCookiesLink => Driver.Instance.WaitToFindElement(PrivacyAndCookiesLocator);

        /// <summary>
        /// Gets the privacy and cookies locator.
        /// </summary>
        /// <value>
        /// The privacy and cookies locator.
        /// </value>
        protected static By PrivacyAndCookiesLocator => By.LinkText("Privacy and cookies");

        /// <summary>
        /// Gets the signout link.
        /// </summary>
        /// <value>
        /// The signout link.
        /// </value>
        protected static IWebElement SignoutLink => Driver.Instance.WaitToFindElement(SignoutLinkByLocator);

        /// <summary>
        /// Gets the signout link by locator.
        /// </summary>
        /// <value>
        /// The signout link by locator.
        /// </value>
        protected static By SignoutLinkByLocator => By.LinkText("Sign out");

        /// <summary>
        /// Gets the support link.
        /// </summary>
        /// <value>
        /// The support link.
        /// </value>
        protected static IWebElement SupportLink => Driver.Instance.WaitToFindElement(SupportLinkByLocator);

        /// <summary>
        /// Gets the support link by locator.
        /// </summary>
        /// <value>
        /// The support link by locator.
        /// </value>
        protected static By SupportLinkByLocator => By.LinkText("Support");

        #endregion
    }
}