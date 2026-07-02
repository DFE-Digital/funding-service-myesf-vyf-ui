using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Web.Helpers;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Helpers
{
    /// <summary>
    /// The DisplayHelperTests class.
    /// </summary>
    [TestClass]
    public class DisplayHelperTests
    {
        [TestMethod, TestCategory("Unit")]
        public void UpdateAllocationHistoryListToDisplay_WhenListLengthLessThanFour_ShouldnotUpdate()
        {
            // Arrange
            var futureYears = new List<(int, int)>();
            var currentYearsBeforeThisYear = new List<(int, int)>() { (2019, 2020) };
            var historicYears = new List<(int, int)>() { (2018, 2019), (2017, 2018) };

            // Act
            DisplayHelper.UpdateAllocationHistoryListToDisplay(futureYears, currentYearsBeforeThisYear, historicYears);

            // Assert
            futureYears.Should().BeEquivalentTo(new List<(int, int)>());
            currentYearsBeforeThisYear.Should().BeEquivalentTo(new List<(int, int)>() { (2019, 2020) });
            historicYears.Should().BeEquivalentTo(new List<(int, int)>() { (2018, 2019), (2017, 2018) });
        }

        [TestMethod, TestCategory("Unit")]
        public void UpdateAllocationHistoryListToDisplay_WhenListLengthMoreThanFour_ShouldUpdate()
        {
            // Arrange
            var futureYears = new List<(int, int)>() { (2020, 2021) };
            var currentYearsBeforeThisYear = new List<(int, int)>() { (2019, 2020) };
            var historicYears = new List<(int, int)>() { (2018, 2019), (2017, 2018) };

            // Act
            DisplayHelper.UpdateAllocationHistoryListToDisplay(futureYears, currentYearsBeforeThisYear, historicYears);

            // Assert
            futureYears.Should().BeEquivalentTo(new List<(int, int)>() { (2020, 2021) });
            currentYearsBeforeThisYear.Should().BeEquivalentTo(new List<(int, int)>() { (2019, 2020) });
            historicYears.Should().BeEquivalentTo(new List<(int, int)>() { (2018, 2019) });
        }

        [TestMethod, TestCategory("Unit")]
        public void UpdateAllocationHistoryListToDisplay_WhenListLengthMoreThanFour_ShouldUpdateCorrectList()
        {
            // Arrange
            var futureYears = new List<(int, int)>() { (2020, 2021) };
            var currentYearsBeforeThisYear = new List<(int, int)>() { (2019, 2020), (2018, 2019), (2017, 2018) };
            var historicYears = new List<(int, int)>();

            // Act
            DisplayHelper.UpdateAllocationHistoryListToDisplay(futureYears, currentYearsBeforeThisYear, historicYears);

            // Assert
            futureYears.Should().BeEquivalentTo(new List<(int, int)>() { (2020, 2021) });
            currentYearsBeforeThisYear.Should().BeEquivalentTo(new List<(int, int)>() { (2019, 2020), (2018, 2019) });
            historicYears.Should().BeEquivalentTo(new List<(int, int)>());
        }
    }
}