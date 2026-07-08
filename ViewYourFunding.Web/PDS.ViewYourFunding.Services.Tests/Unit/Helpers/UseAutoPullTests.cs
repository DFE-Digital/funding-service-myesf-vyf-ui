using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass, TestCategory("Unit")]
    public class UseAutoPullTests
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("True")]
        [DataRow("False")]
        public void UseAutoPull(string autoPullValue)
        {
            // Arrange
            var fundingStream = new FundingStream();
            if (autoPullValue != null)
            {
                fundingStream = new FundingStream
                {
                    SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            SettingId = 1,
                            CreatedAt = new DateTime(2020, 10, 1),
                            Value = autoPullValue,
                            Setting = new SettingType
                            {
                                SettingName = "UseAutoPull",
                                SettingDescription = "SettingDescription1",
                                ValueDataType = SettingValueDataType.Bool,
                                ValuesAreEditable = false,
                            }
                        }
                    }
                };
            }

            var expectedResult = (autoPullValue == "True") ? true : false;

            // Act
            var act = fundingStream.UseAutoPull();

            // Assert
            act.Should().Be(expectedResult);
        }
    }
}
