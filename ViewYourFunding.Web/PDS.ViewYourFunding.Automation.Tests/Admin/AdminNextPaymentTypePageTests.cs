using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Automation.Pages.Admin;
using PDS.ViewYourFunding.Automation.Tests.ViewYourFunding;

namespace PDS.ViewYourFunding.Automation.Tests.Admin
{
    [TestClass]
    [Ignore]
    public class AdminNextPaymentTypePageTests : FundingStreamRegressionTestBase
    {
        [TestMethod, TestCategory("Regression"), TestCategory("CoreRegression")]
        public void AdminNextPaymentType_CRUD_Operations()
        {
            // Arrange Act
            AdminHomePage.NavigateToPageViaLogin(AdminUserName, AdminUserPassword);
            AdminNextPaymentTypePage.NavigateToNextPaymentTypePage();
            AdminNextPaymentTypePage.AddNextPaymentType();

            //Assert
            AdminNextPaymentTypePage.EnsureNextPaymentTypeOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminNextPaymentTypePage.NavigateToNextPaymentTypePage();
            AdminNextPaymentTypePage.EditNextPaymentType();

            //Assert
            AdminNextPaymentTypePage.EnsureNextPaymentTypeOperationConfirmation();

            AdminHomePage.NavigateToPage();
            AdminNextPaymentTypePage.NavigateToNextPaymentTypePage();
            AdminNextPaymentTypePage.DeleteNextPaymentType();

            //Assert
            AdminNextPaymentTypePage.EnsureNextPaymentTypeOperationConfirmation();
        }
    }
}