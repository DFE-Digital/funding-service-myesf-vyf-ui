using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    /// <summary>
    /// The FundingPublicationDateHelperTests class.
    /// </summary>
    [TestClass]
    public class FundingPublicationDateHelperTests
    {
        /// <summary>
        /// The september.
        /// </summary>
        private const string September = "September", April = "April";

        /// <summary>
        /// Gets as of month and year returns expected value.
        /// </summary>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="yearFrom">The year from.</param>
        /// <param name="yearTo">The year to.</param>
        /// <param name="expected">The expected.</param>
        [TestMethod, TestCategory("Unit")]
        [DynamicData(nameof(GetAsOfMonthAndYearSource))]
        public void GetAsOfMonthAndYear_ReturnsExpectedValue(
            DateTime publicationDate,
            int yearFrom,
            int yearTo,
            (string, string) expected)
        {
            // Act
            var actual = FundingPublicationDateHelper.GetAcademicAsOfData(publicationDate, yearFrom, yearTo);

            // Assert
            actual.Should().Be(expected);
        }

        /// <summary>
        /// Gets the get as of month and year source.
        /// </summary>
        /// <value>
        /// The get as of month and year source.
        /// </value>
        private static IEnumerable<object[]> GetAsOfMonthAndYearSource =>
            new List<object[]>
            {
                new object[]
                {
                    new DateTime(2019, 10, 1),
                    2019,
                    2020,
                    (September, "2019")
                },
                new object[]
                {
                    new DateTime(2020, 02, 1),
                    2019,
                    2020,
                    (September, "2019")
                },
                new object[]
                {
                    new DateTime(2020, 06, 1),
                    2019,
                    2020,
                    (April, "2020")
                },
                new object[]
                {
                    new DateTime(2020, 08, 1),
                    2019,
                    2020,
                    (April, "2020")
                },
                new object[]
                {
                    new DateTime(2019, 02, 1),
                    2018,
                    2019,
                    (September, "2018")
                },
                new object[]
                {
                    new DateTime(2019, 06, 1),
                    2018,
                    2019,
                    (April, "2019")
                },
                new object[]
                {
                    new DateTime(2021, 08, 1),
                    2020,
                    2021,
                    (April, "2021")
                },
                new object[]
                {
                    new DateTime(2021, 3, 10),
                    2020,
                    2021,
                    (September, "2020")
                },
                new object[]
                {
                    new DateTime(2021, 4, 10),
                    2020,
                    2021,
                    (April, "2021")
                },
                new object[]
                {
                    default(DateTime),
                    2019,
                    2020,
                    ((string)null, (string)null)
                },
                new object[]
                {
                    default(DateTime),
                    2019,
                    2020,
                    ((string)null, (string)null)
                },
            };
    }
}
