using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using System;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Extensions
{
    /// <summary>
    /// The DateTimeExtensionTests class.
    /// </summary>
    [TestClass]
    public class DateTimeExtensionTests
    {
        /// <summary>
        /// Converts to filenamestring_withdaygreaterthanten_ensureformatcorrect.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ToFilenameString_WithDayGreaterThanTen_EnsureFormatCorrect()
        {
            // Arrange
            var testDate = new DateTime(2019, 10, 12);

            // Act
            var actual = testDate.ToFilenameString();

            // Assert
            actual.Should().Be("12-10-2019");
        }

        /// <summary>
        /// Converts to filenamestring_withdaylessthanten_ensureleadingzerodisplayed.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ToFilenameString_WithDayLessThanTen_EnsureLeadingZeroDisplayed()
        {
            // Arrange
            var testDate = new DateTime(2019, 10, 1);

            // Act
            var actual = testDate.ToFilenameString();

            // Assert
            actual.Should().Be("01-10-2019");
        }

        /// <summary>
        /// Converts to datedisplay_withdaygreaterthanten_ensureformatcorrect.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ToDateDisplay_WithDayGreaterThanTen_EnsureFormatCorrect()
        {
            // Arrange
            var testDate = new DateTime(2019, 10, 12);

            // Act
            var actual = testDate.ToDateDisplay();

            // Assert
            actual.Should().Be("12 October 2019");
        }

        /// <summary>
        /// Converts to datedisplay_withdaylessthanten_ensureleadingzeronotdisplayed.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void ToDateDisplay_WithDayLessThanTen_EnsureLeadingZeroNotDisplayed()
        {
            // Arrange
            var testDate = new DateTime(2019, 10, 1);

            // Act
            var actual = testDate.ToDateDisplay();

            // Assert
            actual.Should().Be("1 October 2019");
        }

        /// <summary>
        /// Converts to routeparameterstring_returnsexpectedvalue.
        /// </summary>
        /// <param name="day">The day.</param>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <param name="expected">The expected.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow(1, 1, 2019, "1-1-2019")]
        [DataRow(2, 1, 2019, "2-1-2019")]
        [DataRow(1, 2, 2019, "1-2-2019")]
        [DataRow(1, 12, 2019, "1-12-2019")]
        [DataRow(12, 1, 2019, "12-1-2019")]
        public void ToRouteParameterString_ReturnsExpectedValue(int day, int month, int year, string expected)
        {
            // Arrange
            var date = new DateTime(year, month, day);

            // Act
            var actual = date.ToRouteParameterString();

            // Assert
            actual.Should().Be(expected);
        }

        /// <summary>
        /// Converts to routeparameterdate_returnsexpectedvalue.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="expectedDay">The expected day.</param>
        /// <param name="expectedMonth">The expected month.</param>
        /// <param name="expectedYear">The expected year.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow("1-1-2019", 1, 1, 2019)]
        [DataRow("2-1-2019", 2, 1, 2019)]
        [DataRow("1-2-2019", 1, 2, 2019)]
        [DataRow("1-12-2019", 1, 12, 2019)]
        [DataRow("12-1-2019", 12, 1, 2019)]
        public void ToRouteParameterDate_ReturnsExpectedValue(string date, int expectedDay, int expectedMonth, int expectedYear)
        {
            // Arrange
            var expected = new DateTime(expectedYear, expectedMonth, expectedDay);

            // Act
            var actual = date.ToRouteParameterDate();

            // Assert
            actual.Should().Be(expected);
        }

        /// <summary>
        /// Converts to routeparameterdate_whenformatdoesnotmatch_throwsexception.
        /// </summary>
        /// <param name="date">The date.</param>
        [TestMethod, TestCategory("Unit")]
        [DataRow("")]
        [DataRow("A")]
        [DataRow("1")]
        [DataRow("2019-1-1")]
        [DataRow("1/1/2019")]
        [DataRow("1.1.2019")]
        [DataRow("1-13-2019")]
        public void ToRouteParameterDate_WhenFormatDoesNotMatch_ThrowsException(string date)
        {
            // Arrange Act
            Action act = () => date.ToRouteParameterDate();

            // Assert
            act.Should().Throw<Exception>().WithMessage($"{date} is not in the expected format*");
        }
    }
}
