using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.LoggedIn;
using PDS.ViewYourFunding.Core.Configuration;
using System.Collections.Generic;
using ViewYourFunding.Automation.Pages.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.LoggedIn
{
    [TestClass]
    [Ignore]
    public class LocalAuthorityRecoupmentDetailPageTests : LoggedInRegressionTestBase
    {
        #region Private fields

        private static string TestLoginExternalUserLaSsfAndMss => "10004801 - External User 103 - LocalAuthoritySsfAndMss";

        private readonly ApplicationConfiguration _applicationConfiguration;

        private string ProviderUserPassword => _applicationConfiguration.TestLoginPassword;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAuthorityRecoupmentDetailPageTests"/> class.
        /// </summary>
        public LocalAuthorityRecoupmentDetailPageTests()
        {
            _applicationConfiguration = Config.ConfigHelper.GetApplicationConfiguration();
        }

        #endregion


        #region Tests

        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void LocalAuthorityRecoupmentDetailPage_BasicLayout()
        {
            // Arrange Act
            LocalAuthorityRecoupmentDetailPage.NavigateToLaRecoupmentPageViaLogin(TestLoginExternalUserLaSsfAndMss, ProviderUserPassword);

            // Assert
            ViewYourFundingBasePage.EnsureBreadcrumbs();
            LocalAuthorityRecoupmentDetailPage.EnsurePrintOrSaveStatement();
            LocalAuthorityRecoupmentDetailPage.EnsureDocumentDownload();
            LocalAuthorityRecoupmentDetailPage.EnsureBreakdownTabs();
            LocalAuthorityRecoupmentDetailPage.EnsureRecoupmentHistory();
        }

        #endregion


        #region Private Helpers

        private static IEnumerable<TableData> GetRecoupmentCalculationsTabTableData()
        {
            yield return new TableData
            {
                Id = "local-authority-funding",
                TableRowData = GetLocalAuthorityFundingRows()
            };
        }

        private static IEnumerable<TableData> GetAnomaliesTabTableData()
        {
            yield return new TableData
            {
                Id = "local-authority-funding",
                TableRowData = GetLocalAuthorityFundingRows()
            };
        }

        private static IEnumerable<TableData> GetPhaseTotalsTabTableData()
        {
            yield return new TableData
            {
                Id = "local-authority-funding",
                TableRowData = GetLocalAuthorityFundingRows()
            };
        }

        private static IEnumerable<TableRowData> GetLocalAuthorityFundingRows()
        {
            yield return new TableRowData
            {
                RowItems = new List<string>
                {
                    "Total recoupment",
                    "£0"
                }
            };
        }

        #endregion
    }
}