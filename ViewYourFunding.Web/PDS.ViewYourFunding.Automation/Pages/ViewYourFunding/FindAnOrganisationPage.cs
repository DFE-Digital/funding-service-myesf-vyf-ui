using FluentAssertions;
using OpenQA.Selenium;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The FindAnOrganisationPage class.
    /// </summary>
    public class FindAnOrganisationPage : ViewYourFundingBasePage
    {
        #region Actions

        /// <summary>
        /// Navigates to page.
        /// </summary>
        public static void NavigateToPage()
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
        }

        /// <summary>
        /// Clicks the provider option button.
        /// </summary>
        public static void ClickProviderOptionButton()
        {
            if (!ProviderSearchArea.Displayed)
            {
                ProviderOption.Click();
            }
        }

        /// <summary>
        /// Clicks the la code option button.
        /// </summary>
        public static void ClickLaCodeOptionButton()
        {
            LaCodeOption.Click();
        }

        /// <summary>
        /// Inputs the local authority search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        public static void InputLocalAuthoritySearchText(string searchText)
        {
            LaCodeOptionInputField.SendKeys(searchText);
        }

        /// <summary>
        /// Submits the local authority search.
        /// </summary>
        public static void SubmitLocalAuthoritySearch()
        {
            LocalAuthoritySearchSubmitButton.MoveAndClick();
        }

        /// <summary>
        /// Inputs the provider text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        public static void InputProviderText(string searchText)
        {
            ProviderOptionInputField.SendKeys(searchText);
        }

        /// <summary>
        /// Submits the provider search.
        /// </summary>
        public static void SubmitProviderSearch()
        {
            ProviderSearchSubmitButton.MoveAndClick();
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the organisations options available.
        /// </summary>
        public static void EnsureOrganisationsOptionsAvailable()
        {
            ProviderOption.Selected.Should().BeFalse();
            LaCodeOption.Selected.Should().BeFalse();
        }

        /// <summary>
        /// Ensures the current page.
        /// </summary>
        public static void EnsureCurrentPage()
        {
            EnsureCurrentPage("View funding at organisation level");
        }

        /// <summary>
        /// Ensures the provider option input field is displayed.
        /// </summary>
        /// <param name="isDisplayed">if set to <c>true</c> [is displayed].</param>
        public static void EnsureProviderOptionInputFieldIsDisplayed(bool isDisplayed)
        {
            ProviderOptionInputField.Displayed.Should().Be(isDisplayed);
        }

        /// <summary>
        /// Ensures the la code option input field is displayed.
        /// </summary>
        /// <param name="isDisplayed">if set to <c>true</c> [is displayed].</param>
        public static void EnsureLaCodeOptionInputFieldIsDisplayed(bool isDisplayed)
        {
            LaCodeOptionInputField.Displayed.Should().Be(isDisplayed);
        }

        /// <summary>
        /// Ensures the error linkis correct.
        /// </summary>
        /// <param name="expectedValidationErrorInputId">The expected validation error input identifier.</param>
        public static void EnsureErrorLinkisCorrect(string expectedValidationErrorInputId)
        {
            ErrorMessageLink.Displayed.Should().BeTrue();
            ErrorMessageLink.Text.Should().Be("Enter a term to search for an organisation");
            ErrorMessageLink.GetAttribute("href").Should().Contain($"#{expectedValidationErrorInputId}");
        }

        /// <summary>
        /// Ensures the error link is hidden.
        /// </summary>
        public static void EnsureErrorLinkIsHidden()
        {
            ErrorMessageLink.Displayed.Should().BeFalse();
        }

        #endregion


        #region Controls

        /// <summary>
        /// Gets the provider option input field.
        /// </summary>
        /// <value>
        /// The provider option input field.
        /// </value>
        protected static IWebElement ProviderOptionInputField =>
            Driver.Instance.WaitToFindElement(By.Id("provider"));

        /// <summary>
        /// Gets the la code option input field.
        /// </summary>
        /// <value>
        /// The la code option input field.
        /// </value>
        protected static IWebElement LaCodeOptionInputField =>
            Driver.Instance.WaitToFindElement(By.Id("laSearch"));

        /// <summary>
        /// Gets the provider option.
        /// </summary>
        /// <value>
        /// The provider option.
        /// </value>
        protected static IWebElement ProviderOption =>
            Driver.Instance.WaitToFindElement(By.Id("radio-1"));

        /// <summary>
        /// Gets the la code option.
        /// </summary>
        /// <value>
        /// The la code option.
        /// </value>
        protected static IWebElement LaCodeOption =>
            Driver.Instance.WaitToFindElement(By.Id("radio-2"));

        /// <summary>
        /// Gets the provider search area.
        /// </summary>
        /// <value>
        /// The provider search area.
        /// </value>
        protected static IWebElement ProviderSearchArea =>
            Driver.Instance.WaitToFindElement(By.Id("Provider"));

        /// <summary>
        /// Gets the provider search submit button.
        /// </summary>
        /// <value>
        /// The provider search submit button.
        /// </value>
        protected static IWebElement ProviderSearchSubmitButton =>
            Driver.Instance.WaitToFindElement(By.Id("school-or-academy-submit"));

        /// <summary>
        /// Gets the local authority search submit button.
        /// </summary>
        /// <value>
        /// The local authority search submit button.
        /// </value>
        protected static IWebElement LocalAuthoritySearchSubmitButton =>
            Driver.Instance.WaitToFindElement(By.Id("la-submit"));

        /// <summary>
        /// Gets the error message link.
        /// </summary>
        /// <value>
        /// The error message link.
        /// </value>
        protected static IWebElement ErrorMessageLink =>
            Driver.Instance.WaitToFindElement(By.Id("errorLink"));

        #endregion
    }
}
