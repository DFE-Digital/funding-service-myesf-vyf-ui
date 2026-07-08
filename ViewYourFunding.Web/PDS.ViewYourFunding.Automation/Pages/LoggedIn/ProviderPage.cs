using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using ViewYourFunding.Automation.Pages.ViewYourFunding;
using ViewYourFunding.Automation.Utilities;

namespace PDS.ViewYourFunding.Automation.Pages.LoggedIn
{
    /// <summary>
    /// The provider page class.
    /// </summary>
    public class ProviderPage : ViewYourFundingBasePage
    {
        /// <summary>
        /// Navigates to page.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToPageViaLogin(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            LoginPage.Open();
            LoginPage.Login(username, password);

            Goto("view-latest-funding/pre-16-16-19-statements");
        }

        /// <summary>
        /// Navigate to LA Recoupment page.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToRecoupmentPageViaLogin(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            LoginPage.Open();
            LoginPage.Login(username, password);

            Goto("view-latest-funding/recoupment-reports");
        }

        /// <summary>
        /// Navigate to LA Recoupment history page.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void NavigateToRecoupmentHistoryPageViaLogin(string username, string password)
        {
            LoginPage.LogoutIfLoggedIn();
            LoginPage.Open();
            LoginPage.Login(username, password);

            var ukPrn = username.Substring(0, username.IndexOf(" - ", StringComparison.Ordinal));

            Goto($"view-latest-funding/recoupment-reports/recoupment-history?ukprn={ukPrn}&fundingStreamNamePathPart=la-recoupment");
        }

        /// <summary>
        /// Navigates to page.
        /// </summary>
        public static void NavigateToPageWithoutLogin()
        {
            LoginPage.LogoutIfLoggedIn();
            StartPage.Open();

            Goto("view-latest-funding/pre-16-16-19-statements");
        }

        /// <summary>
        /// Navigates to page.
        /// </summary>
        public static void NavigateToPageDirectly()
        {
            Goto("view-latest-funding/pre-16-16-19-statements");
        }

        /// <summary>
        /// Click on the School share budget tab link.
        /// </summary>
        public static void ClickOnSchoolShareBudgetTabLink()
        {
            SchoolShareTabLink.MoveAndClick();
        }

        /// <summary>
        /// Click on the High needs tab link.
        /// </summary>
        public static void ClickOnHighNeedsTabLink()
        {
            HighNeedsTabLink.MoveAndClick();
        }

        /// <summary>
        /// Click on the view funding breakdown link.
        /// </summary>
        public static void ClickOnViewFundingBreakdownLink()
        {
            Sixteen19FundingBreakdownLink.MoveAndClick();
        }

        /// <summary>
        /// Click on the view history link.
        /// </summary>
        public static void ClickOnViewHistoryLink()
        {
            ViewHistoryOfAllocatedFundingLink.MoveAndClick();
        }


        /// <summary>
        /// Click on the view funding breakdown link.
        /// </summary>
        public static void ClickOnLAFundingBreakdownLink()
        {
            LaSsfBreakdownLink.Click();
        }

        /// <summary>
        /// Click on the view funding breakdown link.
        /// </summary>
        public static void ClickOnViewFourteen16FundingBreakdownLink()
        {
            Fourteen16FundingBreakdownLink.MoveAndClick();
        }

        #region Assertions

        public static void EnsureNavigatedToLogin()
        {
            EnsureCurrentPage("Login");
        }

        /// <summary>
        /// Ensures the current provider page.
        /// </summary>
        public static void EnsureCurrentProviderPage()
        {
            EnsureCurrentPage("Allocation statements");
        }

        /// <summary>
        /// Ensure the page has the expected number of sections.
        /// </summary>
        /// <param name="expectedCount">The expected number of sections.</param>
        public static void EnsureHasExpectedNumberOfSections(int expectedCount)
        {
            Sections.Count.Should().BeGreaterOrEqualTo(expectedCount);
        }

        /// <summary>
        /// Expands the GAG section.
        /// </summary>
        public static void ExpandGAG()
        {
            if (!GAGSectionDiv.Displayed)
            {
                GAGSectionButton.Click();
            }
        }

        /// <summary>
        /// Click the GAG download link.
        /// </summary>
        public static void ClickGAGDownloadLink()
        {
            GAGDownloadADocument.Click();
        }

        /// <summary>
        /// Expands the PSG section.
        /// </summary>
        public static void ExpandPSG()
        {
            if (!PSGSectionDiv.Displayed)
            {
                PSGSectionButton.Click();
            }
        }

        /// <summary>
        /// Expands the 16 to 19 section.
        /// </summary>
        public static void Expand1619()
        {
            if (!Sixteen19SectionDiv.Displayed)
            {
                Sixteen19SectionButton.Click();
            }
        }

        /// <summary>
        /// Expands the 14 to 16 section.
        /// </summary>
        public static void Expand1416()
        {
            if (!Fourteen16SectionDiv.Displayed)
            {
                Fourteen16SectionButton.Click();
            }
        }

        /// <summary>
        /// Expands the La Ssf section.
        /// </summary>
        public static void ExpandLaSsf()
        {
            if (!LaSsfSectionDiv.Displayed)
            {
                LaSsfSectionButton.Click();
            }
        }

        /// <summary>
        /// Expands the NMSS section.
        /// </summary>
        public static void ExpandNMSS()
        {
            if (!NmssSectionDiv.Displayed)
            {
                NmssSectionButton.Click();
            }
        }

        /// <summary>
        /// Ensure resources.
        /// </summary>
        public static void EnsureResources()
        {
            var found = false;

            foreach (var h3 in H3Titles)
            {
                if (h3.Text.Contains("resources"))
                {
                    found = true;
                    break;
                }
            }

            found.Should().BeTrue();
        }

        /// <summary>
        /// Ensure explore topic.
        /// </summary>
        public static void EnsureExploreTopic()
        {
            var found = false;

            foreach (var h3 in H3Titles)
            {
                if (h3.Text.Contains("Explore the topic"))
                {
                    found = true;
                    break;
                }
            }

            found.Should().BeTrue();
        }

        /// <summary>
        /// Ensure allocation history.
        /// </summary>
        public static void EnsureAllocationHistory()
        {
            var found = false;

            foreach (var h3 in H3Titles)
            {
                if (h3.Text.Equals("Allocation history"))
                {
                    found = true;
                    break;
                }
            }

            found.Should().BeTrue();
        }

        public static void EnsureGAGMinimumFundingGuaranteeDisplays()
        {
            GAGMinimumFundingGuarantee.Displayed.Should().BeTrue();
        }

        public static void Ensure1619FundingBreakdownLink()
        {
            Sixteen19FundingBreakdownLink.Displayed.Should().BeTrue();
        }

        public static void Ensure1416FundingBreakdownLinks()
        {
            Fourteen16FundingBreakdownCoreProgrammeLink.Displayed.Should().BeTrue();
            Fourteen16FundingBreakdownPupilPremiumLink.Displayed.Should().BeTrue();
        }

        public static void EnsureLaSsfFundingBreakdownLink()
        {
            LaSsfBreakdownLink.Displayed.Should().BeTrue();
        }

        public static void EnsureNMSSFundingBreakdownLinks()
        {
            Nmss1619BursaryFundLink.Displayed.Should().BeTrue();
            NmssHighNeedsFundingLink.Displayed.Should().BeTrue();
        }

        public static void ClickOnNMSSHighNeedsLink()
        {
            NmssHighNeedsFundingLink.MoveAndClick();
        }

        public static void ClickOnNMSSDiscretionaryBursaryFundLink()
        {
            Nmss1619BursaryFundLink.MoveAndClick();
        }

        public static void EnsurePSGTableDisplays()
        {
            PSGTable.Displayed.Should().BeTrue();
        }

        public static void EnsurePaymentDates()
        {
            var found = false;

            foreach (var h3 in H3Titles)
            {
                if (h3.Text.Equals("Payment dates"))
                {
                    found = true;
                    break;
                }
            }

            found.Should().BeTrue();
        }

        public static void EnsureGAGSpreadsheetLink()
        {
            GAGDownloadADocument.Displayed.Should().BeTrue();
        }

        public static void EnsureLaSsfNewDownloadLink()
        {
            LASsfDownloadADocument.Displayed.Should().BeTrue();
        }

        public static void Ensure1416DownloadLink()
        {
            Fourteen16DownloadADocument.Displayed.Should().BeTrue();
        }

        public static void EnsurePSGSpreadsheetLink()
        {
            PSGDownloadADocument.Displayed.Should().BeTrue();
        }

        #endregion


        #region Page Elements

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> Sections => Driver.Instance.FindElements(By.ClassName("govuk-accordion__section-header"));

        /// <summary>
        /// Gets the section1 button.
        /// </summary>
        /// <value>
        /// The section1 button.
        /// </value>
        protected static IWebElement Sixteen19SectionButton => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-1619 .govuk-accordion__section-button"));

        /// <summary>
        /// Gets the 14 to 16 section button.
        /// </summary>
        protected static IWebElement Fourteen16SectionButton => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-1416 .govuk-accordion__section-button"));

        /// <summary>
        /// Gets the local authority SSF button.
        /// </summary>
        /// <value>
        /// The local authority SSF button.
        /// </value>
        protected static IWebElement LaSsfSectionButton => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-funding-1619 .govuk-accordion__section-button"));

        /// <summary>
        /// Gets the section1 div.
        /// </summary>
        /// <value>
        /// The section1 div.
        /// </value>
        protected static IWebElement Sixteen19SectionDiv => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-1619 .govuk-accordion__section-content"));

        /// <summary>
        /// Gets the 14 to 16 summary section.
        /// </summary>
        protected static IWebElement Fourteen16SectionDiv => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-1416 .govuk-accordion__section-content"));

        /// <summary>
        /// Gets the local authority SSF div.
        /// </summary>
        /// <value>
        /// The local authority SSF div.
        /// </value>
        protected static IWebElement LaSsfSectionDiv => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-funding-1619 .govuk-accordion__section-content"));

        /// <summary>
        /// Gets the section2 button.
        /// </summary>
        /// <value>
        /// The section2 button.
        /// </value>
        protected static IWebElement GAGSectionButton => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-GAG .govuk-accordion__section-button"));

        /// <summary>
        /// Gets the section2 div.
        /// </summary>
        /// <value>
        /// The section2 div.
        /// </value>
        protected static IWebElement GAGSectionDiv => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-GAG .govuk-accordion__section-content"));

        /// <summary>
        /// Gets the section3 button.
        /// </summary>
        /// <value>
        /// The section3 button.
        /// </value>
        protected static IWebElement PSGSectionButton => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-PSG .govuk-accordion__section-button"));

        /// <summary>
        /// Gets the section3 div.
        /// </summary>
        /// <value>
        /// The section3 div.
        /// </value>
        protected static IWebElement PSGSectionDiv => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-PSG .govuk-accordion__section-content"));

        /// <summary>
        /// Gets the NMSS summary section button.
        /// </summary>
        /// <value>
        /// The the NMSS summary section button.
        /// </value>
        protected static IWebElement NmssSectionButton => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-NMSS .govuk-accordion__section-button"));

        /// <summary>
        /// Gets the NMSS summary section div.
        /// </summary>
        /// <value>
        /// The NMSS summary section div.
        /// </value>
        protected static IWebElement NmssSectionDiv => Driver.Instance.FindElement(By.CssSelector(".govuk-accordion__section.for-NMSS .govuk-accordion__section-content"));

        /// <summary>
        /// Gets the h3 titles.
        /// </summary>
        /// <value>
        /// The h3 titles.
        /// </value>
        protected static ReadOnlyCollection<IWebElement> H3Titles => Driver.Instance.FindElements(By.TagName("h3"));

        /// <summary>
        /// Gets the download a document block.
        /// </summary>
        /// <value>
        /// The download a document block.
        /// </value>
        protected static IWebElement PSGDownloadADocument => PSGSectionDiv.FindElement(By.ClassName("download-a-document"));

        /// <summary>
        /// Gets the download a document block.
        /// </summary>
        /// <value>
        /// The download a document block.
        /// </value>
        protected static IWebElement GAGDownloadADocument => GAGSectionDiv.FindElement(By.ClassName("download-a-document-new")).FindElement(By.TagName("a"));

        /// <summary>
        /// Gets the download a document block for 16 to 19 funding.
        /// </summary>
        /// <value>
        /// The download a document block.
        /// </value>
        protected static IWebElement DownloadADocumentNew => Sixteen19SectionDiv.FindElement(By.ClassName("download-a-document-new"));

        /// <summary>
        /// Gets the download a document block for 16 to 19 funding.
        /// </summary>
        /// <value>
        /// The download a document block.
        /// </value>
        protected static IWebElement LASsfDownloadADocument => LaSsfSectionDiv.FindElement(By.ClassName("download-a-document-new"));

        /// <summary>
        /// Gets the 14 to 16 download document element.
        /// </summary>
        protected static IWebElement Fourteen16DownloadADocument => Fourteen16SectionDiv.FindElement(By.ClassName("download-a-document-new"));

        /// <summary>
        /// Gets the psg table.
        /// </summary>
        /// <value>
        /// The psg table.
        /// </value>
        protected static IWebElement PSGTable => Driver.Instance.FindElement(By.ClassName("funding-breakdown-pesg"));

        /// <summary>
        /// Gets the gag minimum funding guarantee.
        /// </summary>
        /// <value>
        /// The gag minimum funding guarantee.
        /// </value>
        protected static IWebElement GAGMinimumFundingGuarantee => GAGSectionDiv.FindElement(By.LinkText("Minimum funding guarantee"));

        /// <summary>
        /// Gets the 16 to 19 breakdown link.
        /// </summary>
        protected static IWebElement Sixteen19FundingBreakdownLink => Sixteen19SectionDiv.FindElement(By.CssSelector(".govuk-heading-l a"));

        /// <summary>
        /// Gets the 14 to 16 breakdown link.
        /// </summary>
        protected static IWebElement Fourteen16FundingBreakdownLink => Fourteen16SectionDiv.FindElement(By.LinkText("Core programme"));

        /// <summary>
        /// Gets the 14 to 16 core programme link.
        /// </summary>
        protected static IWebElement Fourteen16FundingBreakdownCoreProgrammeLink => Fourteen16SectionDiv.FindElement(By.LinkText("Core programme"));

        /// <summary>
        /// Gets the 14 to 16 additional funding link.
        /// </summary>
        protected static IWebElement Fourteen16FundingBreakdownPupilPremiumLink => Fourteen16SectionDiv.FindElement(By.LinkText("Pupil premium"));

        /// <summary>
        /// Gets the NMSS 1619 discretionary bursary funding breakdown link.
        /// </summary>
        protected static IWebElement Nmss1619BursaryFundLink => NmssSectionDiv.FindElement(By.LinkText("16 to 19 discretionary bursary fund"));

        /// <summary>
        /// Gets the NMSS high needs funding breakdown link.
        /// </summary>
        protected static IWebElement NmssHighNeedsFundingLink => NmssSectionDiv.FindElement(By.LinkText("High needs funding"));

        /// <summary>
        /// Gets the la ssf breakdown link.
        /// </summary>
        protected static IWebElement LaSsfBreakdownLink => LaSsfSectionDiv.FindElement(By.LinkText("View schools' individual allocations"));

        /// <summary>
        /// Gets the school share tab link.
        /// </summary>
        /// <value>
        /// The school share tab link.
        /// </value>
        protected static IWebElement SchoolShareTabLink => Driver.Instance.FindElement(By.LinkText("School budget share"));

        /// <summary>
        /// Gets the high needs tab link.
        /// </summary>
        /// <value>
        /// The high needs tab link.
        /// </value>
        protected static IWebElement HighNeedsTabLink => Driver.Instance.FindElement(By.LinkText("High needs"));

        /// <summary>
        /// Gets the view history of allocated funding link.
        /// </summary>
        /// <value>
        /// The view history of allocated funding link.
        /// </value>
        protected static IWebElement ViewHistoryOfAllocatedFundingLink => Driver.Instance.FindElement(By.LinkText("View history of allocated funding"));


        #endregion
    }
}