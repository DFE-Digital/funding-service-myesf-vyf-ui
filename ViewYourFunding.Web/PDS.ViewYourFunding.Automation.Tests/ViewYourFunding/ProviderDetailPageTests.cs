using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using System;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The ProviderDetailPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class ProviderDetailPageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// The multiple results search term.
        /// </summary>
        private const string MultipleResultsSearchTerm = "St Mary's";

        /// <summary>
        /// ProviderDetailsPage basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderDetailsPage_BasicLayout()
        {
            // Arrange Act
            ProviderDetailPage.NavigateToPage();

            // Assert
            ProviderDetailPage.EnsureCurrentProviderPage();
            ProviderDetailPage.EnsureBreadcrumbs();
            ProviderDetailPage.EnsureAlternativeFundings();
        }

        /// <summary>
        /// ProviderDetailsPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderDetailPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            ProviderDetailPage.NavigateToPage();

            // Assert
            ProviderDetailPage.EnsureAndClickBreadcrumb(ChooseHowToViewFundingLinkText);
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderDetailsPage check available payment date maintained.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderDetailPage_CheckAvailablePaymentDate_Maintained()
        {
            // Arrange Act
            ProviderDetailPage.NavigateToPage();

            // Assert
            ProviderDetailPage.EnsureCurrentPage(MultipleResultsSearchTerm, true);
            ProviderDetailPage.ExpandPSG();
            DateTime nextPaymentDate = FundingStreamRegressionTestBase.PsgNextPaymentDateMaintainedParsed;
            if (nextPaymentDate == DateTime.MinValue)
            {
                ProviderDetailPage.EnsureNoNextAvailablePaymentDateText(FundingStreamRegressionTestBase.NoPsgNextPaymentDateText);
            }
            else
            {
                ProviderDetailPage.EnsureNextAvailablePaymentDate(FundingStreamRegressionTestBase.PsgNextPaymentDateMaintainedParsed.ToDateDisplay());
            }
        }

        /// <summary>
        /// ProviderDetailsPage check available payment date academies.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderDetailPage_CheckAvailablePaymentDate_Academies()
        {
            // Arrange Act
            ProviderDetailPage.NavigateToPage("Belsize");

            // Assert
            ProviderDetailPage.ExpandPSG();
            DateTime nextPaymentdate = FundingStreamRegressionTestBase.PsgNextPaymentDateAcademiesParsed;
            if (nextPaymentdate == DateTime.MinValue)
            {
                ProviderDetailPage.EnsureNoNextAvailablePaymentDateText(FundingStreamRegressionTestBase.NoPsgNextPaymentDateText);
            }
            else
            {
                ProviderDetailPage.EnsureNextAvailablePaymentDate(FundingStreamRegressionTestBase.PsgNextPaymentDateAcademiesParsed.ToDateDisplay());
            }
        }

        /// <summary>
        /// ProviderDetailsPage check available payment date NMSS.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderDetailPage_CheckAvailablePaymentDate_NMSS()
        {
            // Arrange Act
            ProviderDetailPage.NavigateToPage("Breckenbrough");

            // Assert
            ProviderDetailPage.ExpandPSG();
            DateTime nextPaymentdate = FundingStreamRegressionTestBase.PsgNextPaymentDateNmssParsed;
            if (nextPaymentdate == DateTime.MinValue)
            {
                ProviderDetailPage.EnsureNoNextAvailablePaymentDateText(FundingStreamRegressionTestBase.NoPsgNextPaymentDateText);
            }
            else
            {
                ProviderDetailPage.EnsureNextAvailablePaymentDate(FundingStreamRegressionTestBase.PsgNextPaymentDateNmssParsed.ToDateDisplay());
            }
        }

        /// <summary>
        /// ProviderDetailsPage has breadcrumb linking to find an organisation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderDetailPage_HasBreadcrumbLinkingToFindAnOrganisationPage()
        {
            // Arrange Act
            ProviderDetailPage.NavigateToPage();

            // Assert
            ProviderDetailPage.EnsureAndClickBreadcrumb(ViewFundingAtOrganisationLinkText);
            FindAnOrganisationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// ProviderDetailsPage via did you mean has breadcrumb linking provider did you mean page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void ProviderDetailPage_ViaDidYouMean_HasBreadcrumbLinkingProviderDidYouMeanPage()
        {
            // Arrange Act
            ProviderDetailPage.NavigateToViaDidYouMeanPage(MultipleResultsSearchTerm);

            // Assert
            ProviderDetailPage.EnsureAndClickBreadcrumb(SearchResultsLinkText);
            ProviderResultsPage.EnsureBreadcrumbs();
            ProviderResultsPage.EnsureResultsText(MultipleResultsSearchTerm);
        }

        /// <summary>
        /// ProviderDetailsPage all sections expanded.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void ProviderDetailPage_AllSectionsExpanded()
        {
            // Arrange Act
            ProviderDetailPage.NavigateToPage();
            ProviderDetailPage.ToggleMainContent();
            ProviderDetailPage.ToggleAlternativeFunding();

            // Assert
            ProviderDetailPage.EnsureCurrentProviderPage();
            ProviderDetailPage.EnsureMainContent(true);
            ProviderDetailPage.EnsureAlternativeFundings(true);
        }
    }
}