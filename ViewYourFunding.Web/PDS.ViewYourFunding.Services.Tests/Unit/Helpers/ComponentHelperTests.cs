using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Tests.Unit.Helpers
{
    [TestClass]
    public class ComponentHelperTests
    {
        [TestMethod, TestCategory("Unit")]
        public void GetAndIncreaseInstanceNumber_X_Y()
        {
            // Arrange
            var context = new Mock<HttpContext>();
            context.Setup(c => c.Items).Returns(new Dictionary<object, object>());

            var name = "ABCD";
            var othername = "EFGHI";

            // Act
            var firstActual = ComponentHelper.GetAndIncreaseInstanceNumber(name, context.Object);
            var secondActual = ComponentHelper.GetAndIncreaseInstanceNumber(name, context.Object);
            var thirdActual = ComponentHelper.GetAndIncreaseInstanceNumber(othername, context.Object);

            // Assert
            firstActual.Should().Be(1);
            secondActual.Should().Be(2);
            thirdActual.Should().Be(1);
        }

        [TestMethod, TestCategory("Unit")]
        public void GetLevel_NestedConditional_ReturnsCorrectLevel()
        {
            // Arrange
            var group = new UiModelGroup
            {
                Type = "Conditional/Condition",
                Expression = "#"
            };

            var heirarchy = new UiModelGroup
            {
                Type = "TopLevel",
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Type = "Conditional/Condition",
                        Expression = "#",
                        Groups = new List<UiModelGroup>
                        {
                            new UiModelGroup
                            {
                                Type = "Conditional/TrueOutput",
                                Groups = new List<UiModelGroup>
                                {
                                    new UiModelGroup
                                    {
                                        Type = "Conditional/Condition",
                                        Expression = "#",
                                        Groups = new List<UiModelGroup>
                                        {
                                            new UiModelGroup
                                            {
                                                Type = "Conditional/TrueOutput",
                                                Groups = new List<UiModelGroup>
                                                {
                                                    group
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var expectedLevel = 2;

            // Act
            var actualLevel = ComponentHelper.GetLevel(group, heirarchy, "Conditional/Condition");

            // Assert
            actualLevel.Should().Be(expectedLevel);
        }

        [TestMethod, TestCategory("Unit")]
        public void GetVisibilityConditionLevel_NestedConditional_ReturnsCorrectLevel()
        {
            // Arrange
            var group = new UiModelGroup
            {
                Type = "General/Literal",
                VisibilityCondition = "ABC"
            };

            var heirarchy = new UiModelGroup
            {
                Type = "TopLevel",
                Groups = new List<UiModelGroup>
                {
                    new UiModelGroup
                    {
                        Type = "General/Literal",
                        VisibilityCondition = "ABC",
                        Groups = new List<UiModelGroup>
                        {
                            new UiModelGroup
                            {
                                Type = "General/Literal",
                                VisibilityCondition = "ABC",
                                Groups = new List<UiModelGroup>
                                {
                                    new UiModelGroup
                                    {
                                        Type = "General/Literal",
                                        VisibilityCondition = "ABC",
                                        Groups = new List<UiModelGroup>
                                        {
                                            new UiModelGroup
                                            {
                                                Type = "General/Literal",
                                                VisibilityCondition = "ABC",
                                                Groups = new List<UiModelGroup>
                                                {
                                                    group
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var expectedLevel = 4;

            // Act
            var actualLevel = ComponentHelper.GetVisibilityConditionLevel(group, heirarchy);

            // Assert
            actualLevel.Should().Be(expectedLevel);
        }
    }
}