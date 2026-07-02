using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    /// <summary>
    /// The FilenameHelperTests class.
    /// </summary>
    [TestClass]
    public class FilenameHelperTests
    {
        /// <summary>
        /// Gets the components from filename valid national file name correctly decodes.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GetComponentsFromFilename_ValidNationalFileName_CorrectlyDecodes()
        {
            // Arrange
            var filename = "PSG_AY-1920_20200101_120000.ods";

            // Act
            var actual = FilenameHelper.GetComponentsFromFilename(filename);

            // Assert
            actual.Should().BeEquivalentTo(new FileNameComponents
            {
                Extension = "ods",
                PublicationDate = new DateTime(2020, 01, 01),
                FundingPeriodCode = "AY-1920",
                FundingStreamCode = "PSG"
            });
        }

        /// <summary>
        /// Builds the output spreadsheet filename filname in input format returns name in UI format.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void BuildOutputSpreadsheetFilename_FilnameInInputFormat_ReturnsNameInUiFormat()
        {
            // Arrange
            var filename = "PSG_AY-1920_20200101_120000_Organisation_303.ods";
            var date = new DateTime(2020, 1, 1);
            var fundingStreams = new List<FundingStream>
            {
                new FundingStream
                {
                    FundingStreamCode = "PSG",
                    FundingStreamName = "ABC_de-f"
                }
            };

            // Act
            var actual = FilenameHelper.BuildOutputSpreadsheetFilename(filename, date, fundingStreams);

            // Assert
            actual.Should().BeEquivalentTo("abc-de-f_2019-to-2020_published-01-01-2020.ods");
        }

        /// <summary>
        /// Gets the components from filename organisation file name correctly decodes.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GetComponentsFromFilename_OrganisationFileName_CorrectlyDecodes()
        {
            // Arrange
            var filename = "PSG_AY-1920_20200101_120000_Organisation_303.ods";

            // Act
            var actual = FilenameHelper.GetComponentsFromFilename(filename);

            // Assert
            actual.Should().BeEquivalentTo(new FileNameComponents
            {
                Extension = "ods",
                PublicationDate = new DateTime(2020, 01, 01),
                FundingPeriodCode = "AY-1920",
                FundingStreamCode = "PSG"
            });
        }

        /// <summary>
        /// Gets the components from filename invalid national file name returns null.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void GetComponentsFromFilename_InvalidNationalFileName_ReturnsNull()
        {
            // Arrange
            var filename = "PSG_AY-1920_AA200101_120000.ods";

            // Act
            var actual = FilenameHelper.GetComponentsFromFilename(filename);

            // Assert
            actual.Should().BeEquivalentTo((FileNameComponents)null);
        }
    }
}