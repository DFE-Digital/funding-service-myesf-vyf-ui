using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using ViewYourFunding.Automation.Utilities;

namespace ViewYourFunding.Automation.Pages.ViewYourFunding
{
    /// <summary>
    /// The ViewYourFundingBasePage class.
    /// </summary>
    public abstract class ViewYourFundingBasePage : BasePage
    {
        /// <summary>
        /// Ensures the error components.
        /// </summary>
        /// <param name="showErrors">if set to <c>true</c> [show errors].</param>
        public static void EnsureErrorComponents(bool showErrors)
        {
            var formGroupError = Driver.Instance.ReturnIfVisible(By.ClassName("form-group-error"));
            var formGroupNoError = Driver.Instance.ReturnIfVisible(By.ClassName("form-group-no-error"));
            var afterErrorSummary = Driver.Instance.ReturnIfVisible(By.ClassName("after-error-summary"));
            var afterHiddenErrorSummary = Driver.Instance.ReturnIfVisible(By.ClassName("after-hidden-error-summary"));
            var errorSummary = Driver.Instance.ReturnIfVisible(By.ClassName("error-summary"));
            var errorMessage = Driver.Instance.ReturnIfVisible(By.ClassName("error-message"));

            if (showErrors)
            {
                formGroupError.Should().NotBeNull();
                afterErrorSummary.Should().NotBeNull();
                errorSummary.Should().NotBeNull();
                errorMessage.Should().NotBeNull();

                formGroupNoError.Should().BeNull();
                afterHiddenErrorSummary.Should().BeNull();
            }
            else
            {
                formGroupError.Should().BeNull();
                afterErrorSummary.Should().BeNull();
                errorSummary.Should().BeNull();
                errorMessage.Should().BeNull();

                formGroupNoError.Should().NotBeNull();
                afterHiddenErrorSummary.Should().NotBeNull();
            }
        }

        /// <summary>
        /// Ensures the breadcrumbs.
        /// </summary>
        public static void EnsureBreadcrumbs()
        {
            Breadcrumbs.Displayed.Should().BeTrue();
        }

        /// <summary>
        /// Ensures the and click breadcrumb.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        public static void EnsureAndClickBreadcrumb(string linkText)
        {
            var breadcrumbLink = Breadcrumbs.FindElementOrDefault(By.LinkText(linkText));
            breadcrumbLink.Should().NotBeNull();
            breadcrumbLink.MoveAndClick();
        }

        /// <summary>
        /// Ensures the breadcrumb not shown.
        /// </summary>
        /// <param name="linkText">The link text.</param>
        public static void EnsureBreadcrumbNotShown(string linkText)
        {
            var breadcrumbLink = Breadcrumbs.FindElementOrDefault(By.LinkText(linkText));
            breadcrumbLink.Should().BeNull();
        }

        /// <summary>
        /// Ensures the related link.
        /// </summary>
        /// <param name="expectedText">The expected text.</param>
        /// <param name="expectedUrl">The expected URL.</param>
        /// <param name="expectedHeader">The expected header.</param>
        public static void EnsureRelatedLink(string expectedText, string expectedUrl, string expectedHeader)
        {
            var sectionHeaders = RelatedLinksSection.FindElements(By.TagName("h2"));
            var sectionHeader = sectionHeaders.SingleOrDefault(h => h.Text.Equals(expectedHeader, StringComparison.OrdinalIgnoreCase));

            sectionHeader.Should().NotBeNull();
            sectionHeader.Displayed.Should().BeTrue();

            var sectionHeaderId = sectionHeader.GetAttribute("id");
            var navBlocks = RelatedLinksSection.FindElements(By.TagName("nav"));
            var navBlock = navBlocks.SingleOrDefault(n => n.GetAttribute("aria-labelledby") == sectionHeaderId);

            navBlock.Should().NotBeNull();
            navBlock.Displayed.Should().BeTrue();

            var link = navBlock.FindElement(By.LinkText(expectedText));

            link.Should().NotBeNull();
            link.Displayed.Should().BeTrue();

            var targetUrl = link.GetAttribute("href");

            targetUrl.Should().Contain(expectedUrl);
        }

        /// <summary>
        /// Checks the downloaded csv has same data as csv in the test resource location.
        /// </summary>
        /// <param name="fileName">The file name to match on.</param>
        public static void EnsureCSVContentsAreSame(string fileName)
        {
            var actualFilePath = $"{Driver.DownloadDirectory}\\{fileName}";
            for (var i = 0; i < 10; i++)
            {
                if (File.Exists(actualFilePath))
                {
                    break;
                }

                Thread.Sleep(1000);
            }

            var expectedRecords = CSVReaderHelper.GetTestRecords(fileName);
            var actualRecords = CSVReaderHelper.GetActualRecords(actualFilePath);

            actualRecords.Should().BeEquivalentTo(expectedRecords);
        }

        /// <summary>
        /// Gets the breadcrumbs.
        /// </summary>
        /// <value>
        /// The breadcrumbs.
        /// </value>
        protected static IWebElement Breadcrumbs => Driver.Instance.WaitToFindElement(By.Id("global-breadcrumb"));

        /// <summary>
        /// Gets the related links section.
        /// </summary>
        /// <value>
        /// The related links section.
        /// </value>
        protected static IWebElement RelatedLinksSection => Driver.Instance.WaitToFindElement(By.Id("related"));
    }
}
