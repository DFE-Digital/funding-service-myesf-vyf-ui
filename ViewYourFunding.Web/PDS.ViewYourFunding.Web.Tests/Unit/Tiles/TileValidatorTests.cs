using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Rules;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Tiles
{
    [TestClass]
    public class TileValidatorTests
    {
        [TestMethod, TestCategory("Unit")]
        public void TileDisplayValidator_Validate_ReturnsFalse_WhenRulesCollectionIsNull()
        {
            //Arrange
            var fakeValidator = new TileDisplayRuleValidator(null);

            //Act
            var actual = fakeValidator.Validate(null);

            //Assert
            actual.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void TileDisplayValidator_Validate_ReturnsFalse_WhenRulesCollectionIsEmpty()
        {
            //Arrange
            var fakeRules = new List<IRule>();
            var fakeValidator = new TileDisplayRuleValidator(fakeRules);

            //Act
            var actual = fakeValidator.Validate(null);

            //Assert
            actual.Should().BeFalse();
        }

        [TestMethod, TestCategory("Unit")]
        public void TileDisplayValidator_Validate_ReturnsTrue_WhenRulesConditionsAreSatisfied()
        {
            //Arrange
            var mockRule = new Mock<IRule>();
            mockRule.Setup(method => method.IsSatisfied(null)).Returns(true);
            var fakeValidator = new TileDisplayRuleValidator(new List<IRule> { mockRule.Object });

            //Act
            var actual = fakeValidator.Validate(null);

            //Assert
            actual.Should().BeTrue();
        }

        [TestMethod, TestCategory("Unit")]
        public void TileDisplayValidator_Validate_ReturnsTrue_WhenRulesConditionsAreNotSatisfied()
        {
            //Arrange
            var mockRule = new Mock<IRule>();
            mockRule.Setup(method => method.IsSatisfied(null)).Returns(false);
            var fakeValidator = new TileDisplayRuleValidator(new List<IRule> { mockRule.Object });

            //Act
            var actual = fakeValidator.Validate(null);

            //Assert
            actual.Should().BeFalse();
        }
    }
}