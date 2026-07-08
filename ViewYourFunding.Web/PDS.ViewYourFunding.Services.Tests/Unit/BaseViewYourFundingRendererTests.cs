using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    /// <summary>
    /// The BaseViewYourFundingRendererTests class.
    /// </summary>
    /// <seealso cref="BaseViewYourFundingRenderer" />
    [TestClass, TestCategory("Unit")]
    public class BaseViewYourFundingRendererTests : BaseViewYourFundingRenderer
    {
        [TestMethod]
        public void CheckEquality_TrueEquals_ReturnsTrue()
        {
            // Arrange
            var expressionComponent = new EvaluatedExpressionComponent
            {
                OriginalExpression = "0=0",
                EqualsTo = true,
                Evaluated = true,
                LeftHandComponent = "0",
                LeftHandComponentEvaluatedResult = "0",
                Operator = '=',
                ComparisonResult = true,
                RightHandComponent = "0"
            };

            // Act
            var actual = CheckEquality(expressionComponent);

            // Assert
            actual.Should().BeTrue();
        }

        [TestMethod]
        public void CheckEquality_LeftAndRightHandInput_TrueEquals_ReturnsTrue()
        {
            // Arrange
            var expressionComponent = new EvaluatedExpressionComponent
            {
                OriginalExpression = "24.06=24.06",
                EqualsTo = true,
                Evaluated = true,
                LeftHandComponent = "24.06",
                LeftHandComponentEvaluatedResult = "24.06",
                Operator = '=',
                ComparisonResult = true,
                RightHandComponent = "24.06",
                RightHandComponentEvaluatedResult = "24.06"
            };

            // Act
            var actual = CheckEquality(expressionComponent);

            // Assert
            actual.Should().BeTrue();
        }

        [TestMethod]
        public void CheckEquality_LeftAndRightHandInput_FalseEquals_ReturnsTrue()
        {
            // Arrange
            var expressionComponent = new EvaluatedExpressionComponent
            {
                OriginalExpression = "24.06=24.05",
                EqualsTo = true,
                Evaluated = true,
                LeftHandComponent = "24.06",
                LeftHandComponentEvaluatedResult = "24.06",
                Operator = '=',
                ComparisonResult = true,
                RightHandComponent = "24.05",
                RightHandComponentEvaluatedResult = "24.05"
            };

            // Act
            var actual = CheckEquality(expressionComponent);

            // Assert
            actual.Should().BeFalse();
        }

        [TestMethod]
        public void CheckEquality_FalseEquals_ReturnsFalse()
        {
            // Arrange
            var expressionComponent = new EvaluatedExpressionComponent
            {
                OriginalExpression = "0=1",
                EqualsTo = true,
                Evaluated = true,
                LeftHandComponent = "0",
                LeftHandComponentEvaluatedResult = "0",
                Operator = '=',
                ComparisonResult = true,
                RightHandComponent = "1"
            };

            // Act
            var actual = CheckEquality(expressionComponent);

            // Assert
            actual.Should().BeFalse();
        }

        [TestMethod]
        public void CheckEquality_FalseNotEquals_ReturnsFalse()
        {
            // Arrange
            var expressionComponent = new EvaluatedExpressionComponent
            {
                OriginalExpression = "0!=0",
                EqualsTo = false,
                Evaluated = true,
                LeftHandComponent = "0",
                LeftHandComponentEvaluatedResult = "0",
                Operator = '=',
                ComparisonResult = true,
                RightHandComponent = "0"
            };

            // Act
            var actual = CheckEquality(expressionComponent);

            // Assert
            actual.Should().BeFalse();
        }

        [TestMethod]
        public void CheckEquality_TrueNotEquals_ReturnsTrue()
        {
            // Arrange
            var expressionComponent = new EvaluatedExpressionComponent
            {
                OriginalExpression = "0!=1",
                EqualsTo = false,
                Evaluated = true,
                LeftHandComponent = "0",
                LeftHandComponentEvaluatedResult = "0",
                Operator = '=',
                ComparisonResult = true,
                RightHandComponent = "1"
            };

            // Act
            var actual = CheckEquality(expressionComponent);

            // Assert
            actual.Should().BeTrue();
        }

        [TestMethod]
        public void CheckEquality_TrueGreaterThan_ReturnsTrue()
        {
            // Arrange
            var expressionComponent = new EvaluatedExpressionComponent
            {
                OriginalExpression = "2>1",
                Evaluated = true,
                LeftHandComponent = "2",
                LeftHandComponentEvaluatedResult = 2D,
                Operator = '>',
                ComparisonResult = true,
                RightHandComponent = "1"
            };

            // Act
            var actual = CheckEquality(expressionComponent);

            // Assert
            actual.Should().BeTrue();
        }

        [TestMethod]
        public void CheckEquality_FalseGreaterThan_ReturnsFalse()
        {
            // Arrange
            var expressionComponent = new EvaluatedExpressionComponent
            {
                OriginalExpression = "2>2",
                Evaluated = true,
                LeftHandComponent = "2",
                LeftHandComponentEvaluatedResult = 2D,
                Operator = '>',
                ComparisonResult = true,
                RightHandComponent = "2"
            };

            // Act
            var actual = CheckEquality(expressionComponent);

            // Assert
            actual.Should().BeFalse();
        }

        [TestMethod]
        public void CheckEquality_FalseGreaterThanAlternatate_ReturnsFalse()
        {
            // Arrange
            var expressionComponent = new EvaluatedExpressionComponent
            {
                OriginalExpression = "2>3",
                Evaluated = true,
                LeftHandComponent = "2",
                LeftHandComponentEvaluatedResult = 2D,
                Operator = '>',
                ComparisonResult = true,
                RightHandComponent = "3"
            };

            // Act
            var actual = CheckEquality(expressionComponent);

            // Assert
            actual.Should().BeFalse();
        }

        [TestMethod]
        public void PerformRuntimeReplacements_WithComponentVariable_StringReturnedWithReplacementMade()
        {
            // Arrange
            var str = "aa@XYZbbccdd";
            var meta = new ComponentConfiguration();
            var component = new Component(meta)
            {
                Variables = new Dictionary<string, object>
                {
                    { "XYZ", "AAAAAAAA" }
                }
            };

            // Act
            var actual = PerformRuntimeReplacements(str, component, true);

            // Assert
            actual.Should().Be("aaAAAAAAAAbbccdd");
        }

        [TestMethod]
        public void PerformRuntimeReplacements_WithChunks_ChunkCorrect()
        {
            // Arrange
            var str = "CHUNK_8_19_@dataset2";
            var meta = new ComponentConfiguration
            {
                AllDatasetsData = new List<List<IFundingApiSearch>>
                {
                    new List<IFundingApiSearch>(),
                    new List<IFundingApiSearch>
                    {
                        new FundingApiSearchFunding(),
                        new FundingApiSearchFunding(),
                        new FundingApiSearchFunding(),
                    },
                    new List<IFundingApiSearch>()
                }
            };

            var component = new Component(meta);

            // Act
            var actual = PerformRuntimeReplacements(str, component, true);

            // Assert
            actual.Should().NotBeNull();

            var actualList = actual as List<IEnumerable<object>>;
            actualList.Should().NotBeNull();
            actualList.Should().HaveCount(1);
            actualList.First().Should().HaveCount(3);
        }

        [TestMethod]
        public void PerformRuntimeReplacements_WithSmallerChunks_ChunkCorrect()
        {
            // Arrange
            var str = "CHUNK_1_1_@dataset2";
            var meta = new ComponentConfiguration
            {
                AllDatasetsData = new List<List<IFundingApiSearch>>
                {
                    new List<IFundingApiSearch>(),
                    new List<IFundingApiSearch>
                    {
                        new FundingApiSearchFunding(),
                        new FundingApiSearchFunding(),
                        new FundingApiSearchFunding(),
                    },
                    new List<IFundingApiSearch>()
                }
            };

            var component = new Component(meta);

            // Act
            var actual = PerformRuntimeReplacements(str, component, true);

            // Assert
            actual.Should().NotBeNull();

            var actualList = actual as List<IEnumerable<object>>;
            actualList.Should().NotBeNull();
            actualList.Should().HaveCount(3);
            actualList.First().Should().HaveCount(1);
        }

        [TestMethod]
        public void PerformRuntimeReplacements_WithComponentData_StringReturnedWithReplacementMade()
        {
            // Arrange
            var str = "aa@XYZbbccdd";
            var meta = new ComponentConfiguration();
            var component = new Component(meta)
            {
                PageData = new Dictionary<string, object>
                {
                    { "XYZ", "AAAAAAAAZ" }
                }
            };

            // Act
            var actual = PerformRuntimeReplacements(str, component, true);

            // Assert
            actual.Should().Be("aaAAAAAAAAZbbccdd");
        }

        [TestMethod]
        public void PerformRuntimeReplacements_WithMetaVariable_StringReturnedWithReplacementMade()
        {
            // Arrange
            var str = "aa@XYZbbccdd";
            var meta = new ComponentConfiguration
            {
                Variables = new Dictionary<string, object>
                {
                    { "XYZ", "2AAAAAAAA2" }
                }
            };

            var component = new Component(meta);

            // Act
            var actual = PerformRuntimeReplacements(str, component, true);

            // Assert
            actual.Should().Be("aa2AAAAAAAA2bbccdd");
        }

        [TestMethod]
        public void PerformRuntimeReplacements_WithMissingVariable_ExceptionThrown()
        {
            // Arrange
            var str = "aa@XXXbbccdd";
            var meta = new ComponentConfiguration();
            var component = new Component(meta);

            // Act
            Action act = () => PerformRuntimeReplacements(str, component, true);

            // Assert
            act.Should().Throw<Exception>();
        }

        [TestMethod]
        public void PerformRuntimeReplacements_WithMissingVariableButExceptionsDisabled_ExceptionNotThrown()
        {
            // Arrange
            var str = "aa@XXXbbccdd";
            var meta = new ComponentConfiguration();
            var component = new Component(meta);

            // Act
            Action act = () => PerformRuntimeReplacements(str, component, false);

            // Assert
            act.Should().NotThrow<Exception>();
        }

        [TestMethod]
        public void EvaluateWhereClause_FalseEqualsStatement_ReturnsFalse()
        {
            // Arrange
            var meta = new ComponentConfiguration();
            var variables = new Dictionary<string, object>();

            // Act
            var actual = EvaluateWhereClause("a=c", meta, variables);

            // Assert
            actual.Should().BeOfType<bool>();
            actual.As<bool>().Should().BeFalse();
        }

        [TestMethod]
        public void GetEvaluatedExpressionComponents_With2Clauses_Returns2ResultsWithCorrectComponents()
        {
            // Arrange
            var meta = new ComponentConfiguration();
            var variables = new Dictionary<string, object>();

            // Act
            var actual = GetEvaluatedExpressionComponents("a=a|b=b", meta, variables);

            // Assert
            actual.Count.Should().Be(2);

            actual.First().RightHandComponent.Should().Be("a");
            actual.Last().LeftHandComponent.Should().Be("b");
        }

        /// <summary>
        /// Performs the replacements for standard use case data returns correctly replaced text.
        /// </summary>
        [TestMethod]
        public void PerformReplacements_ForStandardUseCaseData_ReturnsCorrectlyReplacedText()
        {
            // Arrange
            var inputText = "@year-1_@year0_@year1_@year2_@thiscolumn_@datanextrow_@dataendrow_@year2short_@thiscellreference_@nextcellreference_@endcellreference";
            var expectedText = "2008_2009_2010_2020_A_2_99_20_A1_A2_A99";

            // Act
            var actualText = PerformReplacements(inputText, new ComponentConfiguration { FundingPeriodCode = "AY-1020" }, CellReference.A(1), 2, 99);

            // Assert
            actualText.Should().Be(expectedText);
        }

        /// <summary>
        /// Performs the replacements for standard use case data returns correctly replaced text.
        /// </summary>
        [TestMethod]
        public void PerformReplacements_ViaChoicePage_ReturnsCorrectlyReplacedText()
        {
            // Arrange
            var inputText = "@year-1_@year0_@year1_@year2_@thiscolumn_@datanextrow_@dataendrow_@year2short_@thiscellreference_@nextcellreference_@endcellreference_@searchTermPart?";
            var expectedText = "2008_2009_2010_2020_A_2_99_20_A1_A2_A99_?viaChoicePage=true";

            // Act
            var actualText = PerformReplacements(inputText, new ComponentConfiguration { FundingPeriodCode = "AY-1020", ViaChoicePage = true }, CellReference.A(1), 2, 99);

            // Assert
            actualText.Should().Be(expectedText);
        }

        /// <summary>
        /// Performs the replacements for standard use case data returns correctly replaced text.
        /// </summary>
        [TestMethod]
        public void PerformReplacements_ViaChoicePageWithSearchTerm_ReturnsCorrectlyReplacedText()
        {
            // Arrange
            var inputText = "@year-1_@year0_@year1_@year2_@thiscolumn_@datanextrow_@dataendrow_@year2short_@thiscellreference_@nextcellreference_@endcellreference_@searchTermPart?";
            var expectedText = "2008_2009_2010_2020_A_2_99_20_A1_A2_A99_?searchTerm=ZAX&viaChoicePage=true";

            // Act
            var actualText = PerformReplacements(inputText, new ComponentConfiguration { SearchTerm = "ZAX", FundingPeriodCode = "AY-1020", ViaChoicePage = true }, CellReference.A(1), 2, 99);

            // Assert
            actualText.Should().Be(expectedText);
        }

        /// <summary>
        /// Gets the col span for none1 colspan returns correct number.
        /// </summary>
        [TestMethod]
        public void GetColSpan_ForNone1Colspan_ReturnsCorrectNumber()
        {
            // Arrange
            var groupWithColspan = new UiModelGroup
            {
                Colspan = 2
            };

            // Act
            var actualColSpan = GetColSpan(groupWithColspan);

            // Assert
            actualColSpan.Should().Be(2);
        }

        /// <summary>
        /// Gets the header styles by class name returns correct style.
        /// </summary>
        [TestMethod]
        public void GetHeaderStyles_ByClassName_ReturnsCorrectStyle()
        {
            // Arrange
            var className = "classABC";

            var group = new UiModelGroup
            {
                ClassName = className
            };

            var classes = new Dictionary<string, UiModelClass>
            {
                {
                    className, new UiModelClass
                    {
                        Style = new UiModelStyle
                        {
                        }
                    }
                }
            };

            // Act
            var headerStyles = GetHeaderStyles(group, classes);

            // Assert
            headerStyles.Count.Should().Be(1);
        }

        /// <summary>
        /// Gets the data styles by class name returns correct datastyle.
        /// </summary>
        [TestMethod]
        public void GetDataStyles_ByClassName_ReturnsCorrectDatastyle()
        {
            // Arrange
            var className = "classABC";

            var group = new UiModelGroup
            {
                ClassName = className
            };

            var classes = new Dictionary<string, UiModelClass>
            {
                {
                    className, new UiModelClass
                    {
                        Datastyle = new UiModelStyle
                        {
                        }
                    }
                }
            };

            var dataset = new UiModelDataset
            {
            };

            // Act
            var dataStyles = GetDataStyles("NOT_RELEVANT", "NOT_RELEVANT", group, classes, dataset);

            // Assert
            dataStyles.Count.Should().Be(1);
        }

        /// <summary>
        /// Check the evaluate visibility condition method.
        /// </summary>
        [TestMethod]
        public void EvaluateVisibilityCondition_NoCondition_ReturnsTrue()
        {
            // Arrange

            // Act
            var selectorMeta = EvaluateVisibilityCondition(new Component(null)
            {
                VisibilityCondition = null
            });

            // Assert
            selectorMeta.Should().Be(true);
        }

        /// <summary>
        /// Check the evaluate visibility condition method.
        /// </summary>
        [TestMethod]
        public void EvaluateVisibilityCondition_1Equals1_ReturnsTrue()
        {
            // Arrange

            // Act
            var selectorMeta = EvaluateVisibilityCondition(new Component(null)
            {
                VisibilityCondition = "1=1"
            });

            // Assert
            selectorMeta.Should().Be(true);
        }

        /// <summary>
        /// Check the evaluate visibility condition method.
        /// </summary>
        [TestMethod]
        public void EvaluateVisibilityCondition_0Equals1_ReturnsFalse()
        {
            // Act
            var selectorMeta = EvaluateVisibilityCondition(new Component(null)
            {
                VisibilityCondition = "0=1"
            });

            // Assert
            selectorMeta.Should().Be(false);
        }

        /// <summary>
        /// Gets the selector meta direct selector returns correct selector and false override.
        /// </summary>
        [TestMethod]
        public void GetSelectorMeta_DirectSelector_ReturnsCorrectSelectorAndFalseOverride()
        {
            // Arrange
            var originalSelector = "ABC";
            var datasetId = "A";

            var group = new UiModelGroup
            {
                Selector = originalSelector
            };

            var dataset = new UiModelDataset
            {
                Id = datasetId
            };

            var expected = new OverrideMeta<string>(originalSelector, false);

            // Act
            var selectorMeta = GetSelectorMeta(group, dataset);

            // Assert
            selectorMeta.Should().Be(expected);
        }

        /// <summary>
        /// Gets the selector meta override by dataset identifier returns correct selector and true override.
        /// </summary>
        [TestMethod]
        public void GetSelectorMeta_OverrideByDatasetId_ReturnsCorrectSelectorAndTrueOverride()
        {
            // Arrange
            var originalSelector = "ABC";
            var datasetId = "A";

            var overrideSelector = "DEF";

            var group = new UiModelGroup
            {
                Selector = originalSelector,
                Overrides = new Dictionary<string, UiModelGroup>
                {
                    {
                        datasetId,
                        new UiModelGroup
                        {
                            Selector = overrideSelector
                        }
                    }
                }
            };

            var dataset = new UiModelDataset
            {
                Id = datasetId
            };

            var expected = new OverrideMeta<string>(overrideSelector, true);

            // Act
            var selectorMeta = GetSelectorMeta(group, dataset);

            // Assert
            selectorMeta.Should().Be(expected);
        }

        /// <summary>
        /// Gets the selector meta override by grouping type returns correct selector and true override.
        /// </summary>
        [TestMethod]
        public void GetSelectorMeta_OverrideByGroupingType_ReturnsCorrectSelectorAndTrueOverride()
        {
            // Arrange
            var originalSelector = "ABC";
            var groupingType = "A";

            var overrideSelector = "DEF";

            var group = new UiModelGroup
            {
                Selector = originalSelector,
                Overrides = new Dictionary<string, UiModelGroup>
                {
                    {
                        groupingType,
                        new UiModelGroup
                        {
                            Selector = overrideSelector
                        }
                    }
                }
            };

            var dataset = new UiModelDataset
            {
                GroupingType = groupingType
            };

            var expected = new OverrideMeta<string>(overrideSelector, true);

            // Act
            var selectorMeta = GetSelectorMeta(group, dataset);

            // Assert
            selectorMeta.Should().Be(expected);
        }

        /// <summary>
        /// Gets the formula meta direct formula returns correct formula and false override.
        /// </summary>
        [TestMethod]
        public void GetFormulaMeta_DirectFormula_ReturnsCorrectFormulaAndFalseOverride()
        {
            // Arrange
            var originalFormula = new UiModelFormula
            {
                Expression = "ABC"
            };
            var datasetId = "A";

            var group = new UiModelGroup
            {
                Formula = originalFormula
            };

            var dataset = new UiModelDataset
            {
                Id = datasetId
            };

            var expected = new OverrideMeta<UiModelFormula>(originalFormula, false);

            // Act
            var formulaMeta = GetFormulaMeta(group, dataset);

            // Assert
            formulaMeta.Should().Be(expected);
        }

        /// <summary>
        /// Gets the formula meta override by dataset identifier returns correct formula and true override.
        /// </summary>
        [TestMethod]
        public void GetFormulaMeta_OverrideByDatasetId_ReturnsCorrectFormulaAndTrueOverride()
        {
            // Arrange
            var originalFormula = new UiModelFormula
            {
                Expression = "ABC"
            };
            var datasetId = "A";

            var overrideFormula = new UiModelFormula
            {
                Expression = "DEF"
            };

            var group = new UiModelGroup
            {
                Formula = originalFormula,
                Overrides = new Dictionary<string, UiModelGroup>
                {
                    {
                        datasetId,
                        new UiModelGroup
                        {
                            Formula = overrideFormula
                        }
                    }
                }
            };

            var dataset = new UiModelDataset
            {
                Id = datasetId
            };

            var expected = new OverrideMeta<UiModelFormula>(overrideFormula, true);

            // Act
            var formulaMeta = GetFormulaMeta(group, dataset);

            // Assert
            formulaMeta.Should().Be(expected);
        }

        /// <summary>
        /// Gets the formula meta override by grouping type returns correct formula and true override.
        /// </summary>
        [TestMethod]
        public void GetFormulaMeta_OverrideByGroupingType_ReturnsCorrectFormulaAndTrueOverride()
        {
            // Arrange
            var originalFormula = new UiModelFormula
            {
                Expression = "ABC"
            };
            var groupingType = "A";

            var overrideFormula = new UiModelFormula
            {
                Expression = "DEF"
            };

            var group = new UiModelGroup
            {
                Formula = originalFormula,
                Overrides = new Dictionary<string, UiModelGroup>
                {
                    {
                        groupingType,
                        new UiModelGroup
                        {
                            Formula = overrideFormula
                        }
                    }
                }
            };

            var dataset = new UiModelDataset
            {
                GroupingType = groupingType
            };

            var expected = new OverrideMeta<UiModelFormula>(overrideFormula, true);

            // Act
            var formulaMeta = GetFormulaMeta(group, dataset);

            // Assert
            formulaMeta.Should().Be(expected);
        }

        /// <summary>
        /// Selects from j object by simple json path returns correct property.
        /// </summary>
        [TestMethod]
        public void Select_FromJObjectBySimpleJsonPath_ReturnsCorrectProperty()
        {
            // Arrange
            var groupCode = "TEST";
            var componentConfiguration = new ComponentConfiguration
            {
                Data = new FundingApiSearchFunding
                {
                    GroupCode = groupCode
                },
                PrimaryIdentifier = groupCode
            };

            // Act
            var value = Select(componentConfiguration, "$..groupCode");

            // Assert
            value.ToString().Should().Be(groupCode);
        }

        /// <summary>
        /// Selects from string by simple json path returns correct property.
        /// </summary>
        [TestMethod]
        public void Select_FromStringBySimpleJsonPath_ReturnsCorrectProperty()
        {
            // Arrange
            var groupCode = "TEST";
            var componentConfiguration = new ComponentConfiguration
            {
                Data = new FundingApiSearchFunding
                {
                    GroupCode = groupCode
                },
                PrimaryIdentifier = groupCode
            };

            // Act
            var value = Select(componentConfiguration, "$..groupCode");

            // Assert
            value.ToString().Should().Be(groupCode);
        }

        [TestMethod]
        public void Select_DistributionPeriod_ReturnsValue()
        {
            // Arrange
            const int expected = 700;

            var fundingLines = new CalculationAndFundingLinesCollection
            {
                Calculations = new Dictionary<int, CalculationNoNesting>(),
                FundingLines = new Dictionary<int, FundingLineNoNesting>()
            };

            fundingLines.FundingLines.Add(1, new FundingLineNoNesting
            {
                Type = "payment",
                DistributionPeriods = new List<DistributionPeriod>
                {
                    new DistributionPeriod
                    {
                        DistributionPeriodId = "FY-2122",
                        Value = 200
                    }
                }
            });

            fundingLines.FundingLines.Add(2, new FundingLineNoNesting
            {
                Type = "payment",
                DistributionPeriods = new List<DistributionPeriod>
                {
                    new DistributionPeriod
                    {
                        DistributionPeriodId = "FY-2122",
                        Value = 500
                    }
                }
            });

            var componentConfiguration = new ComponentConfiguration
            {
                CalculationAndFundingLines = fundingLines
            };

            // Act
            var value = Select(componentConfiguration, "DFY-2122");

            // Assert
            value.Should().Be(expected);
        }

        /// <summary>
        /// Determines whether [is data value uppercase set directly returns true].
        /// </summary>
        [TestMethod]
        public void IsDataValueUppercase_SetDirectly_ReturnsTrue()
        {
            // Arrange
            var group = new UiModelGroup
            {
                Selector = "ABC",
                Datastyle = new UiModelStyle
                {
                    Uppercase = true
                }
            };

            // Act
            var actualUppercase = IsDataValueUppercase(group, new UiModelDataset());

            // Assert
            actualUppercase.Should().Be(true);
        }

        /// <summary>
        /// Determines whether [is data value uppercase overriden by dataset identifier returns true].
        /// </summary>
        [TestMethod]
        public void IsDataValueUppercase_OverridenByDatasetId_ReturnsTrue()
        {
            // Arrange
            var datasetId = "Dataset1";

            var group = new UiModelGroup
            {
                Selector = "ABC",
                Overrides = new Dictionary<string, UiModelGroup>
                {
                    {
                        datasetId,
                        new UiModelGroup
                        {
                            Datastyle = new UiModelStyle
                            {
                                Uppercase = true
                            }
                        }
                    }
                }
            };

            var dataset = new UiModelDataset()
            {
                Id = datasetId
            };

            // Act
            var actualUppercase = IsDataValueUppercase(group, dataset);

            // Assert
            actualUppercase.Should().Be(true);
        }

        /// <summary>
        /// Determines whether [is data value uppercase when not set defaults to false].
        /// </summary>
        [TestMethod]
        public void IsDataValueUppercase_WhenNotSet_DefaultsToFalse()
        {
            // Arrange
            var group = new UiModelGroup
            {
                Selector = "ABC"
            };

            // Act
            var actualUppercase = IsDataValueUppercase(group, new UiModelDataset());

            // Assert
            actualUppercase.Should().Be(false);
        }

        /// <summary>
        /// Gets the funding data for dataset single funding passes through data correctly.
        /// </summary>
        [TestMethod]
        public void GetFundingDataForDataset_SingleFunding_PassesThroughDataCorrectly()
        {
            // Arrange
            var fundingValue = "123.45";

            var dataset = new UiModelDataset();

            var fundings = new List<IFundingApiSearchFunding>
            {
                new FundingApiSearchFunding
                {
                    FundingValue = fundingValue
                }
            };

            // Act
            var data = GetFundingDataForDataset(fundings, dataset, null);

            // Assert
            data.Count.Should().Be(1);
            data[0].FundingValue.Should().Be(fundingValue);
        }

        /// <summary>
        /// Gets the funding data for dataset single funding filtering.
        /// </summary>
        [TestMethod]
        public void GetFundingDataForDataset_SingleFunding_Filtering()
        {
            // Arrange
            var fundingValue = double.Parse("123.45");

            var dataset = new UiModelDataset
            {
                Expression = "TotalAmount=987.65"
            };

            var fundings = new List<IFundingApiSearchFunding>
            {
                new FundingApiSearchFunding
                {
                    TotalAmount = fundingValue
                }
            };

            // Act
            var data = GetFundingDataForDataset(fundings, dataset, null);

            // Assert
            data.Count.Should().Be(0);
        }

        /// <summary>
        /// Gets the funding data for dataset single funding all.
        /// </summary>
        [TestMethod]
        public void GetFundingDataForDataset_SingleFunding_All()
        {
            // Arrange
            var fundingValue = "123.45";

            var dataset = new UiModelDataset
            {
                GroupingType = "All"
            };

            var fundings = new List<IFundingApiSearchFunding>
            {
                new FundingApiSearchFunding
                {
                    FundingValue = fundingValue
                }
            };

            // Act
            var data = GetFundingDataForDataset(fundings, dataset, null);

            // Assert
            data.Count.Should().Be(1);
            data[0].FundingValue.Should().Be(fundingValue);
        }

        /// <summary>
        /// Gets the provider funding data for dataset single funding passes through data correctly.
        /// </summary>
        [TestMethod]
        public void GetProviderFundingDataForDataset_SingleFunding_PassesThroughDataCorrectly()
        {
            // Arrange
            var fundingValue = "123.45";

            var dataset = new UiModelDataset();

            var fundings = new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    FundingValue = fundingValue
                }
            };

            // Act
            var data = GetProviderFundingDataForDataset(fundings, dataset, null);

            // Assert
            data.Count.Should().Be(1);
            data[0].FundingValue.Should().Be(fundingValue);
        }

        /// <summary>
        /// Gets the provider funding data for dataset single funding filtering.
        /// </summary>
        [TestMethod]
        public void GetProviderFundingDataForDataset_SingleFunding_Filtering()
        {
            // Arrange
            var fundingValue = double.Parse("123.45");

            var dataset = new UiModelDataset
            {
                Expression = "TotalAmount=987.65"
            };

            var fundings = new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    TotalAmount = fundingValue
                }
            };

            // Act
            var data = GetProviderFundingDataForDataset(fundings, dataset, null);

            // Assert
            data.Count.Should().Be(0);
        }

        /// <summary>
        /// Gets the provider funding data for dataset single funding all.
        /// </summary>
        [TestMethod]
        public void GetProviderFundingDataForDataset_SingleFunding_All()
        {
            // Arrange
            var fundingValue = "123.45";

            var dataset = new UiModelDataset
            {
                GroupingType = "All"
            };

            var fundings = new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    FundingValue = fundingValue
                }
            };

            // Act
            var data = GetProviderFundingDataForDataset(fundings, dataset, null);

            // Assert
            data.Count.Should().Be(1);
            data[0].FundingValue.Should().Be(fundingValue);
        }

        /// <summary>
        /// Applies the where clause single filter returns filtered result.
        /// </summary>
        [TestMethod]
        public void ApplyWhereClause_SingleFilter_ReturnsFilteredResult()
        {
            // Arrange
            var type = typeof(FundingApiSearchFunding);

            var fundings = new List<IFundingApiSearchFunding>
            {
                new FundingApiSearchFunding
                {
                    GroupingType = "TEST"
                },
                new FundingApiSearchFunding
                {
                    GroupingType = "SOMETHING_ELSE"
                }
            };

            // Act
            var filteredData = ApplyWhereClauseToFunding(type, "GroupingType=TEST", fundings);

            // Assert
            filteredData.Count().Should().Be(1);
        }

        /// <summary>
        /// Gets the json serialized property name field with different name returns correct name back.
        /// </summary>
        [TestMethod]
        public void GetJsonSerializedPropertyName_FieldWithDifferentName_ReturnsCorrectNameBack()
        {
            // Arrange
            var type = typeof(FundingApiSearchFunding);

            // Act
            var propertyName = GetJsonSerializedPropertyName(type, nameof(FundingApiSearchFunding.FundingValue));

            // Assert
            propertyName.Should().Be("fundingValue");
        }

        /// <summary>
        /// Gets the first dataset length with no data returns no results.
        /// </summary>
        [TestMethod]
        public void GetAllDatasetsLength_WithNoData_ReturnsNoResults()
        {
            // Arrange
            var group = new UiModelGroup
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset()
                }
            };

            var fundings = new List<IFundingApiSearchFunding>();

            // Act
            var firstDataset = GetAllDatasetsData(fundings, null, group.Dataset, new UIModelAdditionalFundingStream[0] { }, null);

            // Assert
            firstDataset.Should().NotBeNull().And.HaveCount(1);
            firstDataset.First().Should().HaveCount(0);
        }

        /// <summary>
        /// Gets the first dataset length with2 pieces of data returns correct number.
        /// </summary>
        [TestMethod]
        public void GetAllDatasetsLength_With2PiecesOfData_ReturnsCorrectNumber()
        {
            // Arrange
            var group = new UiModelGroup
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset()
                }
            };

            var fundings = new List<IFundingApiSearchFunding>
            {
                new FundingApiSearchFunding(),
                new FundingApiSearchFunding()
            };

            // Act
            var length = GetAllDatasetsData(fundings, null, group.Dataset, new UIModelAdditionalFundingStream[0] { }, null);

            // Assert
            length.Should().NotBeNull().And.HaveCount(1);
            length.First().Should().HaveCount(2);
        }

        /// <summary>
        /// Gets the first dataset length provider funding returns correct number.
        /// </summary>
        [TestMethod]
        public void GetAllDatasetsLength_ProviderFunding_ReturnsCorrectNumber()
        {
            // Arrange
            var group = new UiModelGroup
            {
                Dataset = new List<UiModelDataset>
                {
                    new UiModelDataset
                    {
                        DatasetName = "ProviderFunding"
                    }
                }
            };

            var providerfunding = new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding(),
                new FundingApiSearchProviderFunding()
            };

            // Act
            var length = GetAllDatasetsData(null, providerfunding, group.Dataset, new UIModelAdditionalFundingStream[0] { }, null);

            // Assert
            length.Should().NotBeNull().And.HaveCount(1);
            length.First().Should().HaveCount(2);
        }

        [TestMethod]
        public void GetFundingDataForDataset_MultipleFundings_Using_Expressions_DateValue_and_AndClause()
        {
            // Arrange
            var fundings = GetFundings();

            List<UiModelDataSetFilter> filter = new List<UiModelDataSetFilter>()
            {
                new UiModelDataSetFilter { PropertyName = "ProviderType", Operation = Operator.Equals, Value = "Academics" },
                new UiModelDataSetFilter { PropertyName = "DateOpened", Operation = Operator.LessThan, Value = "2022-01-11" }
            };

            var dataset = new UiModelDataset
            {
                Expressions = filter,
                ExpressionSeparator = "&"
            };

            // Act
            var data = GetProviderFundingDataForDataset(fundings, dataset, null);

            // Assert
            data.Count.Should().Be(1);
        }

        [TestMethod]
        public void GetFundingDataForDataset_MultipleFundings_Using_Expressions_With_DateValue_and_OrClause()
        {
            // Arrange
            var fundings = GetFundings();

            List<UiModelDataSetFilter> filter = new List<UiModelDataSetFilter>()
            {
                new UiModelDataSetFilter { PropertyName = "ProviderType", Operation = Operator.Equals, Value = "Academics" },
                new UiModelDataSetFilter { PropertyName = "DateOpened", Operation = Operator.LessThan, Value = "2022-01-11" }
            };

            var dataset = new UiModelDataset
            {
                Expressions = filter,
                ExpressionSeparator = "|"
            };

            // Act
            var data = GetProviderFundingDataForDataset(fundings, dataset, null);

            // Assert
            data.Count.Should().Be(4);
        }

        [TestMethod]
        public void GetFundingDataForDataset_MultipleFundings_Using_Expressions_WithMoreThan2Filters_AndClause()
        {
            // Arrange
            List<UiModelDataSetFilter> filter = new List<UiModelDataSetFilter>()
            {
                new UiModelDataSetFilter { PropertyName = "ProviderType", Operation = Operator.Equals, Value = "Academics" },
                new UiModelDataSetFilter { PropertyName = "DateOpened", Operation = Operator.LessThan, Value = "2022-01-11" },
                new UiModelDataSetFilter { PropertyName = "ParentProviderType", Operation = Operator.Equals, Value = "LocalAuthority" },
                new UiModelDataSetFilter { PropertyName = "GroupingReason", Operation = Operator.Equals, Value = "Information" },
                new UiModelDataSetFilter { PropertyName = "PhaseOfEducation", Operation = Operator.Equals, Value = "Primary" }
            };

            var dataset = new UiModelDataset
            {
                Expressions = filter,
                ExpressionSeparator = "&"
            };

            var fundings = GetFundings();

            // Act
            var data = GetProviderFundingDataForDataset(fundings, dataset, null);

            // Assert
            data.Count.Should().Be(1);
        }

        [TestMethod]
        public void GetFundingDataForDataset_WithNoUiModelDataSetFilter_ReturnsExpectedResult()
        {
            // Arrange
            var fundings = GetFundings();

            List<UiModelDataSetFilter> filter = new List<UiModelDataSetFilter>();

            var dataset = new UiModelDataset
            {
                Expressions = filter,
                ExpressionSeparator = "&"
            };

            // Act
            var data = GetProviderFundingDataForDataset(fundings, dataset, null);

            // Assert
            data.Count.Should().Be(fundings.Count);
        }

        private List<IFundingApiSearchProviderFunding> GetFundings()
        {
            return new List<IFundingApiSearchProviderFunding>
            {
                new FundingApiSearchProviderFunding
                {
                    Id = "LAREC-FY-2223-10036998-1_0",
                    ProviderType = "Academics",
                    TotalAmount = double.Parse("23.45"),
                    DateOpened = DateTime.ParseExact("25/01/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    StatusChangedDate = DateTime.Now,
                    ParentProviderType = "LocalAuthority",
                    GroupingReason = "Information",
                    PhaseOfEducation = "Primary"
                },
                new FundingApiSearchProviderFunding
                {
                    Id = "LAREC-FY-2223-10003498-1_0",
                    ProviderType = "Academics",
                    TotalAmount = double.Parse("123.45"),
                    DateOpened = DateTime.ParseExact("25/12/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    StatusChangedDate = DateTime.Now,
                    ParentProviderType = "LocalAuthority",
                    GroupingReason = "Information",
                    PhaseOfEducation = "Primary"
                },
                new FundingApiSearchProviderFunding
                {
                    Id = "LAREC-FY-2223-10004147-1_0",
                    ProviderType = "Mitrovice",
                    TotalAmount = double.Parse("55423.45"),
                    DateOpened = DateTime.ParseExact("25/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    StatusChangedDate = DateTime.Now,
                    ParentProviderType = "LocalAuthority",
                    GroupingReason = "Information",
                    PhaseOfEducation = "Secondary"
                },
                new FundingApiSearchProviderFunding
                {
                    Id = "LAREC-FY-2223-10007901-1_0",
                    ProviderType = "Academics",
                    TotalAmount = double.Parse("12.45"),
                    DateOpened = DateTime.ParseExact("25/03/2022", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    StatusChangedDate = DateTime.Now,
                    ParentProviderType = "LocalAuthority",
                    PhaseOfEducation = "Primary"
                }
            };
        }
    }
}