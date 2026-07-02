using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass]
    public class ExpressionBuilderTests
    {
        [TestMethod, TestCategory("Unit")]
        public void GetExpressoin_ForMatchingFilter_WithAndClause_ReturnsExpectedResult()
        {
            // Arrange
            bool orClause = false;
            List<UiModelDataSetFilter> filter = new List<UiModelDataSetFilter>()
            {
                new UiModelDataSetFilter { PropertyName = "ProviderType", Operation = Operator.Equals, Value = "Academics" },
                new UiModelDataSetFilter { PropertyName = "DateOpened", Operation = Operator.LessThan, Value = "2022-01-28" }
            };

            IFundingApiSearchProviderFunding funding = GetFunding();

            // Act
            var del = ExpressionBuilder.GetExpression<IFundingApiSearchProviderFunding>(filter, orClause).Compile()(funding);

            // Assert
            Assert.IsTrue(del);
        }

        [TestMethod, TestCategory("Unit")]
        public void GetExpressoin_ForMatchingFilter_withOrClause_ReturnsExpectedResult()
        {
            // Arrange
            bool orClause = true;
            List<UiModelDataSetFilter> filter = new List<UiModelDataSetFilter>()
            {
                new UiModelDataSetFilter { PropertyName = "ProviderType", Operation = Operator.Equals, Value = "Academics" },
                new UiModelDataSetFilter { PropertyName = "DateOpened", Operation = Operator.LessThan, Value = "2022-01-11" }
            };

            IFundingApiSearchProviderFunding funding = GetFunding();

            // Act
            var del = ExpressionBuilder.GetExpression<IFundingApiSearchProviderFunding>(filter, orClause).Compile()(funding);

            // Assert
            Assert.IsTrue(del);
        }

        [TestMethod, TestCategory("Unit")]
        public void GetExpressoin_ForNotMatchingFilter_ReturnsExpectedResult()
        {
            // Arrange
            bool orClause = false;
            List<UiModelDataSetFilter> filter = new List<UiModelDataSetFilter>()
            {
                new UiModelDataSetFilter { PropertyName = "ProviderType", Value = "Free" }
            };

            IFundingApiSearchProviderFunding funding = GetFunding();

            // Act
            var del = ExpressionBuilder.GetExpression<IFundingApiSearchProviderFunding>(filter, orClause).Compile()(funding);


            // Assert
            Assert.IsFalse(del);
        }

        [TestMethod, TestCategory("Unit")]
        public void GetExpressoin_ForNoFilter_ReturnsNullDelegate()
        {
            // Arrange
            bool orClause = false;
            List<UiModelDataSetFilter> filter = new List<UiModelDataSetFilter>();


            // Act
            var del = ExpressionBuilder.GetExpression<IFundingApiSearchProviderFunding>(filter, orClause);


            // Assert
            Assert.IsTrue(del == null);
        }

        private IFundingApiSearchProviderFunding GetFunding()
        {
            return new FundingApiSearchProviderFunding
            {
                Id = "LAREC-FY-2223-10036998-1_0",
                ProviderType = "Academics",
                TotalAmount = double.Parse("23.45"),
                DateOpened = DateTime.ParseExact("25/01/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                StatusChangedDate = DateTime.Now,
                ParentProviderType = "LocalAuthority",
                GroupingReason = "Information",
                PhaseOfEducation = "Primary"
            };
        }
    }
}
