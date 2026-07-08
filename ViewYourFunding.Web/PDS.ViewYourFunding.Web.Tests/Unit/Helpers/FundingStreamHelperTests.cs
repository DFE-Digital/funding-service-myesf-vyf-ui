using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Helpers;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Helpers
{
    [TestClass, TestCategory("Unit")]
    public class FundingStreamHelperTests
    {
        [TestMethod]
        public void GetEarliestPublication_NoPublications()
        {
            // Arrange
            var fundingStream = new FundingStream();

            // Act
            var actualResult = fundingStream.GetEarliestPublication(false);

            // Assert
            actualResult.Should().BeNull();
        }

        [TestMethod]
        [DataRow(2022, 10, 25, 2021, 10, 25, 2020, 10, 25, 2020, 10, 25)]
        [DataRow(2022, 12, 25, 2022, 10, 25, 2022, 11, 25, 2022, 10, 25)]
        [DataRow(2022, 10, 24, 2022, 10, 25, 2022, 10, 26, 2022, 10, 24)]
        public void GetEarliestPublication_ExpectedResult(int year1, int month1, int day1, int year2, int month2, int day2, int year3, int month3, int day3, int expectedYear, int expectedMonth, int expectedDay)
        {
            // Arrange
            var fundingStream = new FundingStream();
            var publication1 = GetNewPublication(year1, month1, day1);
            var publication2 = GetNewPublication(year2, month2, day2);
            var publication3 = GetNewPublication(year3, month3, day3);

            fundingStream.Publications = new List<Publication>
            {
                publication1, publication2, publication3
            };

            var expectedResult = GetNewPublication(expectedYear, expectedMonth, expectedDay);

            // Act
            var actualResult = fundingStream.GetEarliestPublication(false);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void GetLatestPublication_NoPublications()
        {
            // Arrange
            var fundingStream = new FundingStream();

            // Act
            var actualResult = fundingStream.GetLatestPublication(false);

            // Assert
            actualResult.Should().BeNull();
        }

        [TestMethod]
        [DataRow(2020, 10, 25, 2021, 10, 25, 2022, 10, 25, 2022, 10, 25)]
        [DataRow(2022, 11, 25, 2022, 12, 25, 2022, 10, 25, 2022, 12, 25)]
        [DataRow(2022, 10, 26, 2022, 10, 25, 2022, 10, 24, 2022, 10, 26)]
        public void GetLatestPublication_ExpectedResult(int year1, int month1, int day1, int year2, int month2, int day2, int year3, int month3, int day3, int expectedYear, int expectedMonth, int expectedDay)
        {
            // Arrange
            var fundingStream = new FundingStream();
            var publication1 = GetNewPublication(year1, month1, day1);
            var publication2 = GetNewPublication(year2, month2, day2);
            var publication3 = GetNewPublication(year3, month3, day3);

            fundingStream.Publications = new List<Publication>
            {
                publication1, publication2, publication3
            };

            var expectedResult = GetNewPublication(expectedYear, expectedMonth, expectedDay);

            // Act
            var actualResult = fundingStream.GetLatestPublication(false);

            // Assert
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private Publication GetNewPublication(int year, int month, int day)
        {
            var newPublication = new Publication();
            newPublication.PublishedDate = new DateTime(year, month, day);
            newPublication.Status = PublicationStatus.Published;

            return newPublication;
        }
    }
}
