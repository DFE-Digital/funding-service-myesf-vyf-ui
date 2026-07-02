using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Helper;
using System;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    /// <summary>
    /// The FundingPeriodHelperTests class.
    /// </summary>
    [TestClass]
    public class FundingPeriodHelperTests
    {
        /// <summary>
        /// Gets the years from code valid funding period code should evaluate correct.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GetYearsFromCode_ValidFundingPeriodCode_ShouldEvaluateCorrect()
        {
            // Arrange
            var expectedYear1 = 2019;
            var expectedYear2 = 2020;

            // Act
            var actual = FundingPeriodHelper.GetYearsFromCode("FY-1920");

            // Assert
            actual.Should().BeEquivalentTo((expectedYear1, expectedYear2));
        }

        /// <summary>
        /// Gets the years from code invalid funding period code should throw error.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GetYearsFromCode_InvalidFundingPeriodCode_ShouldThrowError()
        {
            // Act
            Action act = () => FundingPeriodHelper.GetYearsFromCode("FY-A920");

            // Assert
            act.Should().Throw<Exception>();
        }

        /// <summary>
        /// Gets the year type name from code for code returns expected value.
        /// </summary>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="expected">The expected.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow("FY", "financial year")]
        [DataRow("FY-", "financial year")]
        [DataRow("FY-0", "financial year")]
        [DataRow("FY-2021", "financial year")]
        [DataRow("AY", "academic year")]
        [DataRow("AY-", "academic year")]
        [DataRow("AY-0", "academic year")]
        [DataRow("AY-2021", "academic year")]
        public void GetYearTypeNameFromCode_ForCode_ReturnsExpectedValue(string fundingPeriodCode, string expected)
        {
            // Act
            var actual = FundingPeriodHelper.GetYearTypeNameFromCode(fundingPeriodCode);

            // Assert
            actual.Should().Be(expected);
        }

        /// <summary>
        /// Gets the code from years for years and year type returns expected value.
        /// </summary>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="yearTypeCode">The year type code.</param>
        /// <param name="expected">The expected.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow(2019, 2020, YearTypeCode.AcademicYear, "AY-1920")]
        [DataRow(2018, 2019, YearTypeCode.AcademicYear, "AY-1819")]
        [DataRow(2098, 2099, YearTypeCode.AcademicYear, "AY-9899")]
        [DataRow(2019, 2020, YearTypeCode.FinancialYear, "FY-1920")]
        [DataRow(2018, 2019, YearTypeCode.FinancialYear, "FY-1819")]
        [DataRow(2098, 2099, YearTypeCode.FinancialYear, "FY-9899")]
        public void GetCodeFromYears_ForYearsAndYearType_ReturnsExpectedValue(int yearFrom, int yearTo, string yearTypeCode, string expected)
        {
            // Act
            var actual = FundingPeriodHelper.GetCodeFromYears(yearFrom, yearTo, yearTypeCode);

            // Assert
            actual.Should().Be(expected);
        }

        /// <summary>
        /// Gets the funding period and type from period code and returns expected value.
        /// </summary>
        /// <param name="code">The funding period code.</param>
        /// <param name="expected">The expected return.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow("AY-1920", "2019/20 academic year")]
        [DataRow("AY-1819", "2018/19 academic year")]
        [DataRow("AY-9899", "2098/99 academic year")]
        [DataRow("FY-1920", "2019/20 financial year")]
        [DataRow("FY-1819", "2018/19 financial year")]
        [DataRow("FY-9899", "2098/99 financial year")]
        public void GetYearAndTypeNameFromCode_ReturnsExpectedValue(string code, string expected)
        {
            // Act
            var actual = FundingPeriodHelper.GetYearAndTypeNameFromCode(code);

            // Assert
            actual.Should().Be(expected);
        }

        [TestMethod, TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(null)]
        [DataRow("")]
        public void GetYearAndTypeNameFromCode_ArgumentNullException(string code)
        {
            // Act
            var actual = FundingPeriodHelper.GetYearAndTypeNameFromCode(code);
        }

        [TestMethod, TestCategory("Unit")]
        [ExpectedException(typeof(FormatException))]
        [DataRow("A-1920")]
        [DataRow("AYY-1920")]
        [DataRow("AY1920")]
        [DataRow("AY-192")]
        [DataRow("AY-19201")]
        [DataRow("AY-192O")]
        public void GetYearAndTypeNameFromCode_FormatException(string code)
        {
            // Act
            var actual = FundingPeriodHelper.GetYearAndTypeNameFromCode(code);
        }
    }
}