using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    [TestCategory("Regression"), TestCategory("CoreRegression")]
    public class PdfGenerationActionsPageTests : FundingStreamRegressionTestBase
    {
        public PdfGenerationActionsPageTests()
        : base(adminPageSettingDependentTest: true)
        {
        }

        [TestMethod]
        public void AdminGeneralSettingListPage_BasicLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            PdfGenerationActionsPage.NavigateToPdfGenerationActionsPage();

            // Assert
            PdfGenerationActionsPage.EnsurePageTitle("Document Generation Actions List");
            PdfGenerationActionsPage.EnsurePageHasGeneralSettingsTable();
        }

        [TestMethod]
        public void RunFeedReaderLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            PdfGenerationActionsPage.NavigateToPdfGenerationActionsPage();
            PdfGenerationActionsPage.NavigateToRunFeedReaderProceedPage();

            // Assert
            PdfGenerationActionsPage.EnsurePageTitle("Run Feed Reader");
            PdfGenerationActionsPage.EnsureRunFeedReaderPageElements();
        }

        [TestMethod]
        public void RunPDFComparisonLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            PdfGenerationActionsPage.NavigateToPdfGenerationActionsPage();
            PdfGenerationActionsPage.NavigateToRunPdfComparisonPage();

            // Assert
            PdfGenerationActionsPage.EnsurePageTitle("Run PDF Comparison");
            PdfGenerationActionsPage.EnsureRunPdfComparisonPageElements();
        }

        [TestMethod]
        public void AdminGeneralSettingListPage_GenerateSinglePdf()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            PdfGenerationActionsPage.NavigateToPdfGenerationActionsPage();
            PdfGenerationActionsPage.NavigateToGenerateSinglePdfPage();

            // Assert
            PdfGenerationActionsPage.EnsurePageTitle("Generate Single Document");
            PdfGenerationActionsPage.EnsureGenerateSinglePdfPageElements();
        }

        [TestMethod]
        public void GenerateFundingReportsLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            PdfGenerationActionsPage.NavigateToPdfGenerationActionsPage();
            PdfGenerationActionsPage.NavigateToGenerateFundingReportPage();

            // Assert
            PdfGenerationActionsPage.EnsurePageTitle("Generate Funding Report");
            PdfGenerationActionsPage.EnsureGenerateFundingReportsPageElements();
        }

        [TestMethod]
        public void AdminGeneralSettingListPage_LastFeedReaderRunLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            PdfGenerationActionsPage.NavigateToPdfGenerationActionsPage();
            PdfGenerationActionsPage.NavigateToFeedReaderLastFunPage();

            // Assert
            PdfGenerationActionsPage.EnsurePageTitle("Latest Feed Reader Status");
            PdfGenerationActionsPage.EnsurePageHasLastRunTableElements();
        }

        [TestMethod]
        public void ReRunPdfGenerationLayout()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            PdfGenerationActionsPage.NavigateToPdfGenerationActionsPage();
            PdfGenerationActionsPage.NavigateToRerunPdfGenerationPage();

            // Assert
            PdfGenerationActionsPage.EnsurePageTitle("ReRun Document Generation on batch");
            PdfGenerationActionsPage.EnsureReRunPdfGenerationPageElements();
        }
    }
}