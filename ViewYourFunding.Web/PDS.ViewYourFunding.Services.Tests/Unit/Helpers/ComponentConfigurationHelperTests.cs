using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Models;
using System;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass, TestCategory("Unit")]
    public class ComponentConfigurationHelperTests
    {
        [TestMethod]
        [DataRow("New Provision", "2021-09-01T00:00:00+00:00", 2021, 2022, true)]
        [DataRow("academy converter", "2021-09-01T00:00:00+00:00", 2021, 2022, true)]
        [DataRow("new opener", "2021-09-01T00:00:00+00:00", 2021, 2022, false)]
        [DataRow("ACADEMY converter", "2021-09-01T00:00:00+00:00", 2021, 2022, true)]
        [DataRow("academy converter", "2024-09-01T00:00:00+00:00", 2021, 2022, false)]
        [DataRow("academy converter", "2021-09-01T00:00:00+00:00", 2020, 2021, false)]
        [DataRow("academy converter", "", 2021, 2022, false)]
        public void IsAcademyConverterOrNewProvisionInYearOpener_ReturnsExpectedResult(string openReason, string inputOpenDate, int yearFrom, int yearTo, bool expectedValue)
        {
            DateTime? openDate = null;

            if (DateTime.TryParse(inputOpenDate, out var dateTime))
            {
                openDate = dateTime;
            }

            // Arrange
            var result = openDate.IsAcademyConverterOrNewProvisionInYearOpener(yearFrom, yearTo, openReason);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [DataRow("2021-09-01T00:00:00+00:00", 2021, 2022, true)]
        [DataRow("2021-09-01T00:00:00+00:00", 2022, 2023, false)]
        [DataRow("", 2021, 2022, false)]
        public void IsAcademicYearInYearOpener_ReturnsExpectedResult(string inputOpenDate, int yearFrom, int yearTo, bool expectedValue)
        {
            DateTime? openDate = null;

            if (DateTime.TryParse(inputOpenDate, out var dateTime))
            {
                openDate = dateTime;
            }

            // Arrange
            var result = openDate.IsAcademicYearInYearOpener(yearFrom, yearTo);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [DataRow("fresh start", true, true, true, false)]
        [DataRow("anything else", true, true, true, true)]
        [DataRow("anything else", true, false, true, true)]
        [DataRow("anything else", true, true, false, true)]
        public void IsInYearOpener_ReturnsExpectedResult(
            string openingReason,
            bool openDaysEqualFullYearDays,
            bool isAcademicYearOpener,
            bool isPreviousYearPostAprilOpener,
            bool expectedValue)
        {
            var input = new InYearOpenerCalculatorSource
            {
                OpeningReason = openingReason,
                IsAcademicYearInYearOpener = isAcademicYearOpener,
                IsOpenDaysEqualToFullYearDays = openDaysEqualFullYearDays,
                IsPreviousYearPostAprilOpener = isPreviousYearPostAprilOpener
            };

            // Arrange
            var result = ComponentConfigurationHelper.IsInYearOpener(input);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [DataRow("1/1/2020", 2020, 2021, VarianceSelectionOption.NoComparison, "The figures in this statement are not being compared to another statement")]
        [DataRow("1/1/2020", 2020, 2021, VarianceSelectionOption.FinalStatementPreviousYear, "This statement is being compared to the 2019 to 2020 statement published on 01 January 2020")]
        [DataRow("1/1/2020", 2020, 2021, VarianceSelectionOption.PreviousStatementCurrentYear, "This statement is being compared to the 2020 to 2021 statement published on 01 January 2020")]
        public void GetVarianceMessage_ReturnsExpectedResult(
            string date,
            int yearFrom,
            int yearTo,
            VarianceSelectionOption option,
            string expectedValue)
        {
            // Arrange
            var result = ComponentConfigurationHelper.GetVarianceMessage(DateTime.Parse(date), yearFrom, yearTo, option);

            // Assert
            result.Should().Be(expectedValue);
        }
    }
}