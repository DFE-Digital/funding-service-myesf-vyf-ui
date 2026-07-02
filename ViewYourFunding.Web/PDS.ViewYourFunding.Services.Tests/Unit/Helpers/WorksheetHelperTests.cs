using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass, TestCategory("Unit")]
    public class WorksheetHelperTests
    {
        [TestMethod]
        [DataRow(null, null, 5, 10, null)]
        [DataRow(5, 12, 5, 10, "x")]
        [DataRow(6, 12, 8, 20, "x")]
        [DataRow(1, 10, 3, 10, "x")]
        [DataRow(0, 5, 0, 5, null)]
        [DataRow(1, 4, 5, 10, null)]
        public void GetNotApplicableRangeValueEvaluation_ReturnsExpectedResult(int? minimum, int? maximum, double valueToCompare, double valueToCompareTwo, string expectedValue)
        {
            var valuesToCompareTo = new[] { valueToCompare, valueToCompareTwo };
            var uiModel = new UiModelGroup()
            {
                NotApplicableForValueWithinRange = new UiModelNotApplicableForValueWithinRange
                {
                    Minimum = minimum,
                    Maximum = maximum
                }
            };

            // Arrange/Act
            var result = uiModel.GetNotApplicableWithinRangeValueEvaluation(valuesToCompareTo);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [DataRow(0, 6, 5, "x")]
        [DataRow(0, 6, 1, "x")]
        [DataRow(0, 6, 0, null)]
        [DataRow(0, 6, 6, null)]
        public void GetNotApplicableRangeValueEvaluation_WithOnevalueToCompare_ReturnsExpectedResult(int? minimum, int? maximum, double valueToCompareOne, string expectedValue)
        {
            var valuesToCompareTo = new[] { valueToCompareOne };
            var uiModel = new UiModelGroup()
            {
                NotApplicableForValueWithinRange = new UiModelNotApplicableForValueWithinRange
                {
                    Minimum = minimum,
                    Maximum = maximum
                }
            };

            // Arrange/Act
            var result = uiModel.GetNotApplicableWithinRangeValueEvaluation(valuesToCompareTo);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [DataRow(0, 6, 0, 1, 5, "x")]
        [DataRow(0, 6, 0, 1, 6, "x")]
        [DataRow(0, 6, 1, 5, 6, "x")]
        [DataRow(0, 6, 5, 6, 10, "x")]
        [DataRow(0, 6, 0, 6, 15, null)]
        public void GetNotApplicableRangeValueEvaluation_WithThreeValueToCompare_ReturnsExpectedResult(int? minimum, int? maximum, double valueToCompareOne, double valueToCompareTwo, double valueToCompareThree, string expectedValue)
        {
            var valuesToCompareTo = new[] { valueToCompareOne, valueToCompareTwo, valueToCompareThree };
            var uiModel = new UiModelGroup()
            {
                NotApplicableForValueWithinRange = new UiModelNotApplicableForValueWithinRange
                {
                    Minimum = minimum,
                    Maximum = maximum
                }
            };

            // Arrange/Act
            var result = uiModel.GetNotApplicableWithinRangeValueEvaluation(valuesToCompareTo);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [DataRow(0, 6, 0, 0, -1, null)]
        [DataRow(0, 6, 2, -2, 0, "x")]
        [DataRow(0, 6, 6, 0, -3, null)]
        [DataRow(0, 6, -4, 3, 0, "x")]
        [DataRow(0, 6, 4, 5, -4, "x")]
        [DataRow(0, 6, 6, 2, 0, "x")]
        [DataRow(0, 6, -5, 7, 0, null)]
        [DataRow(0, 6, 3, 8, 0, "x")]
        [DataRow(0, 6, 6, 8, 0, null)]
        [DataRow(0, 6, 0, -1, 1, "x")]
        [DataRow(0, 6, 3, 0, 2, "x")]
        [DataRow(0, 6, 9, -2, 3, "x")]
        [DataRow(0, 6, 0, 1, 4, "x")]
        [DataRow(0, 6, 4, 3, 5, "x")]
        [DataRow(0, 6, 10, 2, 1, "x")]
        [DataRow(0, 6, 0, 8, 2, "x")]
        [DataRow(0, 6, 2, 7, 3, "x")]
        [DataRow(0, 6, 11, 6, 4, "x")]
        [DataRow(0, 6, 0, -4, 12, null)]
        [DataRow(0, 6, 3, -2, 13, "x")]
        [DataRow(0, 6, 23, 0, 14, null)]
        [DataRow(0, 6, 0, 3, 15, "x")]
        [DataRow(0, 6, 3, 5, 16, "x")]
        [DataRow(0, 6, 6, 2, 17, "x")]
        [DataRow(0, 6, 0, 10, 18, null)]
        [DataRow(0, 6, 5, 9, 19, "x")]
        [DataRow(0, 6, 9, 8, 20, null)]
        public void GetNotApplicableRangeValueEvaluation_WithMoreCombinations_ReturnsExpectedResult(int? minimum, int? maximum, double valueToCompareOne, double valueToCompareTwo, double valueToCompareThree, string expectedValue)
        {
            var valuesToCompareTo = new[] { valueToCompareOne, valueToCompareTwo, valueToCompareThree };
            var uiModel = new UiModelGroup()
            {
                NotApplicableForValueWithinRange = new UiModelNotApplicableForValueWithinRange
                {
                    Minimum = minimum,
                    Maximum = maximum
                }
            };

            // Arrange/Act
            var result = uiModel.GetNotApplicableWithinRangeValueEvaluation(valuesToCompareTo);

            // Assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        [DataRow(0, 6, 0, 0, null)]
        [DataRow(0, 6, 5, 5, "x")]
        [DataRow(0, 6, 6, 6, null)]
        [DataRow(0, 6, 1, 1, "x")]
        [DataRow(0, 6, 1, 5, "x")]
        [DataRow(0, 6, 1, 6, "x")]
        [DataRow(0, 6, 0, 1, "x")]
        [DataRow(0, 6, 0, 5, "x")]
        [DataRow(0, 6, 0, 6, null)]
        public void GetNotApplicableRangeValueEvaluation_WithTwoValueToCompare_ReturnsExpectedResult(int? minimum, int? maximum, double valueToCompareOne, double valueToCompareTwo, string expectedValue)
        {
            var valuesToCompareTo = new[] { valueToCompareOne, valueToCompareTwo };
            var uiModel = new UiModelGroup()
            {
                NotApplicableForValueWithinRange = new UiModelNotApplicableForValueWithinRange
                {
                    Minimum = minimum,
                    Maximum = maximum
                }
            };

            // Arrange/Act
            var result = uiModel.GetNotApplicableWithinRangeValueEvaluation(valuesToCompareTo);

            // Assert
            result.Should().Be(expectedValue);
        }
    }
}
