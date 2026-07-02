using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class PaymentTypeCodeTests
    {
        #region Public Methods Tests
        [TestMethod, TestCategory("Unit")]
        public void GetNextPaymentDate_ExpectedResult()
        {
            //Arrange
            var nextPayments = GetNextPayment();

            //Act
            var nextPaymentDate = PaymentTypeCode.GetNextPaymentDate(nextPayments, "FS", "AY-1920");

            //Assert
            Assert.IsTrue(nextPaymentDate == new DateTime(2059, 01, 29));
        }

        [TestMethod, TestCategory("Unit")]
        public void GetNextPaymentDate_NextStartDateAsNull()
        {
            //Arrange
            var nextPayments = GetNextPayment();

            //Act
            var nextPaymentDate = PaymentTypeCode.GetNextPaymentDate(nextPayments, "MS", "AY-1920");

            //Assert
            Assert.IsTrue(nextPaymentDate == null);
        }

        [TestMethod, TestCategory("Unit")]
        public void GetNoNextPaymentText_ExpectedResult()
        {
            //Arrange

            //Act
            var noNextPaymentText = PaymentTypeCode.GetNoNextPaymentForTheYearText("FY-1920");

            //Assert
            Assert.IsTrue(noNextPaymentText == "There are no more scheduled payments for financial year 2019 to 2020.");
        }

        #endregion

        #region Private Methods
        private List<NextPayment> GetNextPayment()
        {
            return new List<NextPayment>()
            {
                new NextPayment()
                {
                    Id = 1,
                    NextPaymentDate = new DateTime(2059, 01, 29),
                    NextPaymentTypeCode = "FS",
                    NextPaymentTypeDescription = "test",
                    NextPaymentTypeId = 1,
                    Active = true,
                    FundingPeriodCode = "AY-1920"
                },
                new NextPayment()
                {
                    Id = 2,
                    NextPaymentDate = new DateTime(05, 05, 05),
                    NextPaymentTypeCode = "MS",
                    NextPaymentTypeDescription = "test1",
                    NextPaymentTypeId = 1,
                    Active = true,
                    FundingPeriodCode = "AY-1920"
                },
            };
        }
        #endregion
    }
}