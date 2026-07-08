using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.ViewYourFunding
{
    /// <summary>
    /// The NationalFundingAllocationDownloadPageTests class.
    /// </summary>
    /// <seealso cref="FundingStreamRegressionTestBase" />
    [TestClass]
    [Ignore]
    public class NationalFundingAllocationDownloadPageTests : FundingStreamRegressionTestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NationalFundingAllocationDownloadPageTests"/> class.
        /// </summary>
        public NationalFundingAllocationDownloadPageTests() : base(true)
        {
        }

        #region DSG actions

        /// <summary>
        /// NationalFundingAllocationDownloadPageTests tests basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void DSGNationalFundingAllocationDownloadPage_BasicLayout()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.DSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.DSGEnsureCurrentPage(DsgCurrentYearFrom, DsgCurrentYearTo);
            NationalFundingAllocationDownloadPage.EnsureBreadcrumbs();
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void DSGSNationalFundingAllocationDownloadPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.DSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.EnsureAndClickBreadcrumb("Choose how to view funding");
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage has breadcrumb linking to which allocation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void DSGNationalFundingAllocationDownloadPage_HasBreadcrumbLinkingToWhichAllocationPage()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.DSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.EnsureAndClickBreadcrumb("Select a funding type");
            WhichAllocationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage has historic allocations.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void DSGNationalFundingAllocationDownloadPage_HasHistoricAllocations()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.DSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.DSGEnsureCurrentAllocations(DsgCurrentYearFrom - 1, DsgCurrentYearTo - 1);
            NationalFundingAllocationDownloadPage.DSGEnsureHistoricAllocations(DsgCurrentYearFrom - 2, DsgCurrentYearTo - 2);
            NationalFundingAllocationDownloadPage.DSGEnsureHistoricAllocations(DsgCurrentYearFrom - 3, DsgCurrentYearTo - 3);
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage has guidance link.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void DSGNationalFundingAllocationDownloadPage_HasGuidanceLink()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.DSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.DSGEnsureGuidanceLink(DsgCurrentYearFrom, DsgCurrentYearTo);
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage has funding block resource links.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void DSGNationalFundingAllocationDownloadPage_HasFundingBlockResourceLinks()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.DSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.DSGEnsureFundingBlockResourceLinks(DsgCurrentYearFrom, DsgCurrentYearTo);
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage has spreadsheet links.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void DSGNationalFundingAllocationDownloadPage_HasSpreadsheetLinks()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.DSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.DSGEnsureSpreadsheetLinks(DsgCurrentYearFrom, DsgCurrentYearTo);
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage has format request email links.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void DSGNationalFundingAllocationDownloadPage_HasFormatRequestEmailLinks()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.DSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.DSGEnsureFormatRequestLinks(DsgCurrentYearFrom, DsgCurrentYearTo);
        }

        #endregion


        #region PSG Actions

        /// <summary>
        /// NationalFundingAllocationDownloadPage for latest year basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void PSGNationalFundingAllocationDownloadPage_ForLatestYear_BasicLayout()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.PSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.PSGEnsureCurrentPage(PsgCurrentYearFrom, PsgCurrentYearTo, true);
            NationalFundingAllocationDownloadPage.EnsureBreadcrumbs();
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage for previous year basic layout.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void PSGNationalFundingAllocationDownloadPage_ForPreviousYear_BasicLayout()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.PSGNavigateToPage();
            NationalFundingAllocationDownloadPage.PSGClickLinkToLatestOrHistoricAllocations(PsgCurrentYearFrom - 1, PsgCurrentYearTo - 1);

            // Assert
            NationalFundingAllocationDownloadPage.PSGEnsureCurrentPage(PsgCurrentYearFrom - 1, PsgCurrentYearTo - 1, false);
            NationalFundingAllocationDownloadPage.EnsureBreadcrumbs();
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage has breadcrumb linking to viewing choice page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void PSGNationalFundingAllocationDownloadPage_HasBreadcrumbLinkingToViewingChoicePage()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.PSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.EnsureAndClickBreadcrumb(ChooseHowToViewFundingLinkText);
            ViewingChoicePage.EnsureCurrentPage();
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage has breadcrumb linking to which allocation page.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void PSGNationalFundingAllocationDownloadPage_HasBreadcrumbLinkingToWhichAllocationPage()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.PSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.EnsureAndClickBreadcrumb("Select a funding type");
            WhichAllocationPage.EnsureCurrentPage();
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage for latest year has historic allocations.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void PSGNationalFundingAllocationDownloadPage_ForLatestYear_HasHistoricAllocations()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.PSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.PSGEnsureLinkToLatestOrHistoricAllocations(PsgCurrentYearFrom - 1, PsgCurrentYearTo - 1, false);
            NationalFundingAllocationDownloadPage.PSGEnsureLinkToLatestOrHistoricAllocations(PsgCurrentYearFrom - 2, PsgCurrentYearTo - 2, false);
            NationalFundingAllocationDownloadPage.PSGEnsureLinkToLatestOrHistoricAllocations(PsgCurrentYearFrom - 3, PsgCurrentYearTo - 3, false);
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage for previous year has latest and historic allocations.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void PSGNationalFundingAllocationDownloadPage_ForPreviousYear_HasLatestAndHistoricAllocations()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.PSGNavigateToPage();
            NationalFundingAllocationDownloadPage.PSGClickLinkToLatestOrHistoricAllocations(PsgCurrentYearFrom - 1, PsgCurrentYearTo - 1);

            // Assert
            NationalFundingAllocationDownloadPage.PSGEnsureLinkToLatestOrHistoricAllocations(PsgCurrentYearFrom, PsgCurrentYearTo, true);
            NationalFundingAllocationDownloadPage.PSGEnsureLinkToLatestOrHistoricAllocations(PsgCurrentYearFrom - 2, PsgCurrentYearTo - 2, false);
            NationalFundingAllocationDownloadPage.PSGEnsureLinkToLatestOrHistoricAllocations(PsgCurrentYearFrom - 3, PsgCurrentYearTo - 3, false);
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage for latest year has spreadsheet links.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void PSGNationalFundingAllocationDownloadPage_ForLatestYear_HasSpreadsheetLinks()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.PSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.PSGEnsureSpreadsheetLinks(PsgCurrentYearFrom, PsgCurrentYearTo, true);
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage for previous year has spreadsheet links.
        /// </summary>
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void PSGNationalFundingAllocationDownloadPage_ForPreviousYear_HasSpreadsheetLinks()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.PSGNavigateToPage();
            NationalFundingAllocationDownloadPage.PSGClickLinkToLatestOrHistoricAllocations(PsgCurrentYearFrom - 1, PsgCurrentYearTo - 1);

            // Assert
            NationalFundingAllocationDownloadPage.PSGEnsureSpreadsheetLinks(PsgCurrentYearFrom - 1, PsgCurrentYearTo - 1, false);
        }

        /// <summary>
        /// NationalFundingAllocationDownloadPage has format request email links.
        /// </summary>
        [TestMethod, TestCategory("Regression")]
        public void PSGNationalFundingAllocationDownloadPage_HasFormatRequestEmailLinks()
        {
            // Arrange Act
            NationalFundingAllocationDownloadPage.PSGNavigateToPage();

            // Assert
            NationalFundingAllocationDownloadPage.PSGEnsureFormatRequestLinks(PsgCurrentYearFrom, PsgCurrentYearTo);
        }

        #endregion
    }
}