using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    /// <summary>
    /// The CurrentAndHistoricFundingStreamPeriodsHelperTests class.
    /// </summary>
    [TestClass]
    public class CurrentAndHistoricFundingStreamPeriodsHelperTests
    {
        /// <summary>
        /// Get the current funding stream period for 1619 returns expected value.
        /// </summary>
        /// <param name="fundingPeriod">The year from.</param>
        /// <param name="fundingStreamCode">The year to.</param>
        /// <param name="digitalIYOStatementsGoLiveDate">The year type code.</param>
        /// <param name="fundingStreamPeriod">The expected.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow("AS-2425", "1619", null, "1619-AS-2425")]
        [DataRow("AS-2526", "1619", null, "1619-AS-2526")]
        [DataRow("AS-2627", "1619", null, "1619-AS-2627")]
        public void GetCurrentAndHistoricFundingStreamPeriodsFor1619_ReturnsSigleFundingPeriod(string fundingPeriod, string fundingStreamCode, DateTime digitalIYOStatementsGoLiveDate, string fundingStreamPeriod)
        {
            // Act
            var actual = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalIYOStatementsGoLiveDate, fundingStreamPeriod);

            // Assert
            actual.Should().Equal(fundingStreamPeriod);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("AC-2324", "GAG", "AC-GAG-2324")]
        public void GetCurrentFundingStreamPeriodForGAGLiveDate2023_ReturnsSigleFundingPeriod(string fundingPeriod, string fundingStreamCode, string fundingStreamPeriod)
        {
            //Arrange
            var digitalIYOStatementsGoLiveDate = new DateTime(2023, 01, 01);

            // Expected
            var fundingStreamAndPeriods = new List<string>()
            {
                { "GAG-AC-2324" },
            };

            // Act
            var actual = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalIYOStatementsGoLiveDate, fundingStreamPeriod);

            // Assert
            actual.Should().Equal(fundingStreamAndPeriods);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("AC-2425", "GAG", "AC-GAG-2425")]
        public void GetCurrentAndHistoricFundingStreamPeriodsForGAGLiveDate2023_ReturnsMultipleFundingPeriod(string fundingPeriod, string fundingStreamCode, string fundingStreamPeriod)
        {
            //Arrange
            var digitalIYOStatementsGoLiveDate = new DateTime(2023, 01, 01);

            // Expected
            var fundingStreamAndPeriods = new List<string>()
            {
                { "GAG-AC-2425" },
                { "GAG-AC-2324" },
            };

            // Act
            var actual = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalIYOStatementsGoLiveDate, fundingStreamPeriod);

            // Assert
            actual.Should().Equal(fundingStreamAndPeriods);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("AC-2223", "GAG", "GAG-AC-2223")]
        public void GetDefaultFundingStreamPeriodForGAGLiveDate2023_ReturnDefaultFundingPeriod(string fundingPeriod, string fundingStreamCode, string fundingStreamPeriod)
        {
            //Arrange
            var digitalIYOStatementsGoLiveDate = new DateTime(2023, 01, 01);

            // Expected
            var fundingStreamAndPeriods = new List<string>()
            {
                { "GAG-AC-2223" },
            };

            // Act
            var actual = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalIYOStatementsGoLiveDate, fundingStreamPeriod);

            // Assert
            actual.Should().Equal(fundingStreamAndPeriods);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("AC-2425", "GAG", "AC-GAG-2425")]
        public void GetCurrentFundingStreamPeriodForGAGLiveDate2024_ReturnsSigleFundingPeriod(string fundingPeriod, string fundingStreamCode, string fundingStreamPeriod)
        {
            //Arrange
            var digitalIYOStatementsGoLiveDate = new DateTime(2024, 01, 01);

            // Expected
            var fundingStreamAndPeriods = new List<string>()
            {
                { "GAG-AC-2425" },
            };

            // Act
            var actual = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalIYOStatementsGoLiveDate, fundingStreamPeriod);

            // Assert
            actual.Should().Equal(fundingStreamAndPeriods);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("AC-2526", "GAG", "AC-GAG-2526")]
        public void GetCurrentAndHistoricFundingStreamPeriodsForGAGLiveDate2024_ReturnsMultipleFundingPeriod(string fundingPeriod, string fundingStreamCode, string fundingStreamPeriod)
        {
            //Arrange
            var digitalIYOStatementsGoLiveDate = new DateTime(2024, 01, 01);

            // Expected
            var fundingStreamAndPeriods = new List<string>()
            {
                { "GAG-AC-2526" },
                { "GAG-AC-2425" },
            };

            // Act
            var actual = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalIYOStatementsGoLiveDate, fundingStreamPeriod);

            // Assert
            actual.Should().Equal(fundingStreamAndPeriods);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("AC-2324", "GAG", "GAG-AC-2324")]
        public void GetDefaultFundingStreamPeriodForGAGLiveDate2024_ReturnDefaultFundingPeriod(string fundingPeriod, string fundingStreamCode, string fundingStreamPeriod)
        {
            //Arrange
            var digitalIYOStatementsGoLiveDate = new DateTime(2024, 01, 01);

            // Expected
            var fundingStreamAndPeriods = new List<string>()
            {
                { "GAG-AC-2324" },
            };

            // Act
            var actual = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalIYOStatementsGoLiveDate, fundingStreamPeriod);

            // Assert
            actual.Should().Equal(fundingStreamAndPeriods);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("AC-2526", "GAG", "AC-GAG-2526")]
        public void GetCurrentFundingStreamPeriodForGAGLiveDate2025_ReturnsSigleFundingPeriod(string fundingPeriod, string fundingStreamCode, string fundingStreamPeriod)
        {
            //Arrange
            var digitalIYOStatementsGoLiveDate = new DateTime(2025, 01, 01);

            // Expected
            var fundingStreamAndPeriods = new List<string>()
            {
                { "GAG-AC-2526" },
            };

            // Act
            var actual = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalIYOStatementsGoLiveDate, fundingStreamPeriod);

            // Assert
            actual.Should().Equal(fundingStreamAndPeriods);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("AC-2627", "GAG", "AC-GAG-2627")]
        public void GetCurrentAndHistoricFundingStreamPeriodsForGAGLiveDate2025_ReturnsMultipleFundingPeriod(string fundingPeriod, string fundingStreamCode, string fundingStreamPeriod)
        {
            //Arrange
            var digitalIYOStatementsGoLiveDate = new DateTime(2025, 01, 01);

            // Expected
            var fundingStreamAndPeriods = new List<string>()
            {
                { "GAG-AC-2627" },
                { "GAG-AC-2526" },
            };

            // Act
            var actual = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalIYOStatementsGoLiveDate, fundingStreamPeriod);

            // Assert
            actual.Should().Equal(fundingStreamAndPeriods);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow("AC-2425", "GAG", "GAG-AC-2425")]
        public void GetDefaultFundingStreamPeriodForGAGLiveDate2025_ReturnDefaultFundingPeriod(string fundingPeriod, string fundingStreamCode, string fundingStreamPeriod)
        {
            //Arrange
            var digitalIYOStatementsGoLiveDate = new DateTime(2025, 01, 01);

            // Expected
            var fundingStreamAndPeriods = new List<string>()
            {
                { "GAG-AC-2425" },
            };

            // Act
            var actual = CurrentAndHistoricFundingStreamPeriodsHelper.GetCurrentAndHistoricFundingStreamPeriods(fundingPeriod, fundingStreamCode, digitalIYOStatementsGoLiveDate, fundingStreamPeriod);

            // Assert
            actual.Should().Equal(fundingStreamAndPeriods);
        }
    }
}