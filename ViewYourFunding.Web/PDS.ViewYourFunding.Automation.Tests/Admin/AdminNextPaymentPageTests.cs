using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    public class AdminNextPaymentPageTests : FundingStreamRegressionTestBase
    {
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminNextPayment_CRUD_Operations()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminNextPaymentPage.NavigateToNextPaymentPage();
            AdminNextPaymentPage.AddNextPayment();

            //Assert
            AdminNextPaymentPage.EnsureNextPaymentOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminNextPaymentPage.NavigateToNextPaymentPage();
            AdminNextPaymentPage.EditNextPayment();

            //Assert
            AdminNextPaymentPage.EnsureNextPaymentOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminNextPaymentPage.NavigateToNextPaymentPage();
            AdminNextPaymentPage.DeleteNextPayment();

            //Assert
            AdminNextPaymentPage.EnsureNextPaymentOperationConfirmation();
        }
    }
}