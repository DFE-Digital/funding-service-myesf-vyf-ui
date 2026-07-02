using FluentAssertions;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The LocalAuthorityPSGFundingBreakdownPage class.
    /// </summary>
    public class LocalAuthorityPSGFundingBreakdownPage : ViewYourFundingBasePage
    {
        #region Private Fields

        /// <summary>
        /// The one match locator.
        /// </summary>
        private static readonly By OneMatchLocator = By.ClassName("one-match");

        /// <summary>
        /// The multiple match locator.
        /// </summary>
        private static readonly By MultipleMatchLocator = By.ClassName("more-than-one-match");

        /// <summary>
        /// The back to top link locator.
        /// </summary>
        private static readonly By BackToTopLinkLocator = By.LinkText("Back to top");

        #endregion


        #region Actions

        /// <summary>
        /// Navigates to page via did you mean page.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="localAuthorityName">Name of the local authority.</param>
        public static void NavigateToPageViaDidYouMeanPage(string searchTerm, string localAuthorityName)
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.InputLocalAuthoritySearchText(searchTerm);
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();
            LocalAuthorityDidYouMeanPage.ClickLocalAuthorityLink(localAuthorityName);
            LocalAuthorityStatementPage.ClickPSGFundingBreakdownLink();
        }

        /// <summary>
        /// Navigates to page using exact search.
        /// </summary>
        /// <param name="localAuthorityCode">The local authority code.</param>
        public static void NavigateToPageUsingExactSearch(string localAuthorityCode)
        {
            StartPage.Open();
            StartPage.ClickStartButton();
            ViewingChoicePage.SelectOrganisationOption();
            ViewingChoicePage.ClickContinueButton();
            FindAnOrganisationPage.ClickLaCodeOptionButton();
            FindAnOrganisationPage.InputLocalAuthoritySearchText(localAuthorityCode);
            FindAnOrganisationPage.SubmitLocalAuthoritySearch();
            LocalAuthorityStatementPage.ClickPSGFundingBreakdownLink();
        }

        /// <summary>
        /// Applies the last establishment filter.
        /// </summary>
        public static void ApplyLastEstablishmentFilter()
        {
            EstablishmentOptions.Last().Click();
        }

        /// <summary>
        /// Applies the first establishment filter.
        /// </summary>
        public static void ApplyFirstEstablishmentFilter()
        {
            EstablishmentOptions.First().Click();
        }

        /// <summary>
        /// Gets the provider total allocation amount.
        /// </summary>
        /// <returns>The provider total allocation amount.</returns>
        public static string GetProviderTotalAllocationAmount()
        {
            return ProviderFundingTotalAllocationAmount.Text;
        }

        #endregion


        #region Assertions

        /// <summary>
        /// Ensures the filters.
        /// </summary>
        public static void EnsureFilters()
        {
            FilterSection.Should().NotBeNull();
            FilterSection.Displayed.Should().BeTrue();
            var fieldsets = FilterSection.FindElements(By.TagName("fieldset"));
            fieldsets.Should().HaveCount(1);
            fieldsets[0].GetAttribute("data-filterkey").Should().Be("EstablishmentType");
        }

        /// <summary>
        /// Ensures the total allocation.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        public static void EnsureTotalAllocation(int yearFrom, int yearTo)
        {
            TotalAllocationSection.Should().NotBeNull();
            TotalAllocationSection.Displayed.Should().BeTrue();
            TotalAllocationSection.Text.Should().Match($"Total allocation for academic year {yearFrom} to {yearTo}*");

            var totalAllocationAmount = TotalAllocationSection.FindElement(By.TagName("p"));
            totalAllocationAmount.Should().NotBeNull();
            totalAllocationAmount.Displayed.Should().BeTrue();
            totalAllocationAmount.Text.Should().Match("£*").And.NotMatch("*.00");
        }

        /// <summary>
        /// Ensures the document link.
        /// </summary>
        public static void EnsureDocumentLink()
        {
            DocumentDownloadContainer.Should().NotBeNull();
            DocumentDownloadContainer.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the breakdown table.
        /// </summary>
        public static void EnsureBreakdownTable()
        {
            var thElements = TableHeader.FindElements(By.TagName("th"));
            thElements.Should().HaveCount(5);
            thElements[0].Text.Should().Be("Local authority code");
            thElements[1].Text.Should().Be("Local authority name");
            thElements[2].Text.Should().Match("Local authority establishment number (at *)");
            thElements[3].Text.Should().Match("School name (at *)");
            thElements[4].Text.Should().Be("Total allocation");

            TableRows.Should().HaveCountGreaterThan(0);
        }

        /// <summary>
        /// Ensures the back to top link.
        /// </summary>
        public static void EnsureBackToTopLink()
        {
            BackToTopLink.Should().NotBeNull();
            BackToTopLink.Displayed.Should().BeTrue();
            BackToTopLink.GetAttribute("href").Should().EndWith("#content");
        }

        /// <summary>
        /// Ensures the back to top link is not displayed.
        /// </summary>
        public static void EnsureBackToTopLinkIsNotDisplayed()
        {
            Driver.Instance.WaitToFindElementInvisibility(BackToTopLinkLocator).Should().BeTrue();
        }

        /// <summary>
        /// Ensures the allocation history link.
        /// </summary>
        public static void EnsureAllocationHistoryLink()
        {
            AllocationHistory.Should().NotBeNull();
            AllocationHistory.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the general resources section.
        /// </summary>
        public static void EnsureGeneralResourcesSection()
        {
            GeneralResources.Should().NotBeNull();
            GeneralResources.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the filter multiple match text is displayed.
        /// </summary>
        /// <param name="isDisplayed">if set to <c>true</c> [is displayed].</param>
        public static void EnsureFilterMultipleMatchTextIsDisplayed(bool isDisplayed)
        {
            if (isDisplayed)
            {
                var multipleMatchText = Driver.Instance.ReturnIfVisible(MultipleMatchLocator);
                multipleMatchText.Displayed.Should().BeTrue();
            }
            else
            {
                Driver.Instance.WaitToFindElementInvisibility(MultipleMatchLocator).Should().BeTrue();
            }
        }

        /// <summary>
        /// Ensures the filter single match text is displayed.
        /// </summary>
        /// <param name="isDisplayed">if set to <c>true</c> [is displayed].</param>
        public static void EnsureFilterSingleMatchTextIsDisplayed(bool isDisplayed)
        {
            if (isDisplayed)
            {
                var singleMatchText = Driver.Instance.ReturnIfVisible(OneMatchLocator);
                singleMatchText.Displayed.Should().BeTrue();
            }
            else
            {
                Driver.Instance.WaitToFindElementInvisibility(OneMatchLocator).Should().BeTrue();
            }
        }

        /// <summary>
        /// Ensures the provider funding total allocation amount is updated.
        /// </summary>
        /// <param name="originalAmount">The original amount.</param>
        public static void EnsureProviderFundingTotalAllocationAmountIsUpdated(string originalAmount)
        {
            Driver.Instance.WaitToFindElementContentHasChangedFromOriginalContent(ProviderFundingTotalAllocationAmount, originalAmount);
        }

        #endregion


        #region Page Elements

        /// <summary>
        /// Gets the filter section.
        /// </summary>
        /// <value>
        /// The filter section.
        /// </value>
        protected static IWebElement FilterSection
            => Driver.Instance.WaitToFindElement(By.Id("filterContent"));

        /// <summary>
        /// Gets the total allocation section.
        /// </summary>
        /// <value>
        /// The total allocation section.
        /// </value>
        protected static IWebElement TotalAllocationSection
            => Driver.Instance.WaitToFindElement(By.Id("psg-total-allocation"));

        /// <summary>
        /// Gets the document download container.
        /// </summary>
        /// <value>
        /// The document download container.
        /// </value>
        protected static IWebElement DocumentDownloadContainer
            => Driver.Instance.WaitToFindElement(By.ClassName("download-a-document-container"));

        /// <summary>
        /// Gets the table header.
        /// </summary>
        /// <value>
        /// The table header.
        /// </value>
        protected static IWebElement TableHeader
            => Driver.Instance.WaitToFindElement(By.TagName("thead"));

        /// <summary>
        /// Gets the table rows.
        /// </summary>
        /// <value>
        /// The table rows.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> TableRows
            => Driver.Instance.WaitToFindElements(By.ClassName("filterable"));

        /// <summary>
        /// Gets the back to top link.
        /// </summary>
        /// <value>
        /// The back to top link.
        /// </value>
        protected static IWebElement BackToTopLink
            => Driver.Instance.WaitToFindElement(BackToTopLinkLocator);

        /// <summary>
        /// Gets the allocation history.
        /// </summary>
        /// <value>
        /// The allocation history.
        /// </value>
        protected static IWebElement AllocationHistory
            => Driver.Instance.WaitToFindElement(By.Id("psg-allocation-history"));

        /// <summary>
        /// Gets the general resources.
        /// </summary>
        /// <value>
        /// The general resources.
        /// </value>
        protected static IWebElement GeneralResources
            => Driver.Instance.WaitToFindElement(By.Id("psg-general-resources"));

        /// <summary>
        /// Gets the establishment options.
        /// </summary>
        /// <value>
        /// The establishment options.
        /// </value>
        protected static IEnumerable<IWebElement> EstablishmentOptions
            => Driver.Instance.WaitToFindElements(By.Name("QueryFilter.Filters.EstablishmentType"));

        /// <summary>
        /// Gets the provider funding total allocation amount.
        /// </summary>
        /// <value>
        /// The provider funding total allocation amount.
        /// </value>
        protected static IWebElement ProviderFundingTotalAllocationAmount
            => Driver.Instance.WaitToFindElement(By.ClassName("total-funding-amount"));

        #endregion
    }
}