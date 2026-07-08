using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations.FundingView;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using YearTypeCode = PDS.ViewYourFunding.Services.Constants.YearTypeCode;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// Base renderer for View Your Funding data.
    /// </summary>
    public abstract class BaseViewYourFundingRenderer
    {
        private const string FundingValueContextSuffix = ".FundingValue)";
        private const string ContextSchemaVersionSuffix = ".SchemaVersion)";
        private const char SubQueryOrCharacter = '¬';
        private const char OrCharacter = '|';
        private const char AndCharacter = '&';
        private const char EqualsCharacter = '=';
        private const char GreaterThanCharacter = '>';
        private const char LessThanCharacter = '<';
        private const char NotCharacter = '!';
        private const string DatasetCountPrefix = "$dataset";
        private const string DatasetCountSuffix = ".Count";
        private static readonly Regex FundingPeriodCodeRegex = new Regex(@"^[A-Za-z]{2}(-)[0-Z]{4}$", RegexOptions.Compiled);

        /// <summary>
        /// Check if the left hand and right hand sides of an expression are to be considered equal.
        /// </summary>
        /// <param name="clause">The parts that make up an expression.</param>
        /// <returns>True if equal, false if not.</returns>
        public static bool CheckEquality(EvaluatedExpressionComponent clause)
        {
            var actualValue = clause.LeftHandComponentEvaluatedResult is bool tempValueBool ?
                tempValueBool.ToString().ToLower()
                : clause.LeftHandComponentEvaluatedResult?.ToString();

            if (clause.Operator == EqualsCharacter)
            {
                if (actualValue is null)
                {
                    return clause.EqualsTo == true && "nullValue".Equals(clause.RightHandComponent, StringComparison.InvariantCultureIgnoreCase);
                }

                if (IsANumber(clause.LeftHandComponentEvaluatedResult) && IsANumber(clause.RightHandComponentEvaluatedResult))
                {
                    var leftHandDouble = Convert.ToDouble(clause.LeftHandComponentEvaluatedResult);
                    var rightHandDouble = Convert.ToDouble(clause.RightHandComponentEvaluatedResult);

                    return leftHandDouble == rightHandDouble;
                }

                return (clause.EqualsTo == true && string.Equals(actualValue, clause.RightHandComponent, StringComparison.InvariantCultureIgnoreCase))
                    || (!clause.EqualsTo == true && !string.Equals(actualValue, clause.RightHandComponent, StringComparison.InvariantCultureIgnoreCase));
            }
            else if (clause.Operator == GreaterThanCharacter)
            {
                var actualDouble = IsANumber(clause.LeftHandComponentEvaluatedResult) ? Convert.ToDouble(clause.LeftHandComponentEvaluatedResult) : 0d;

                if (clause.RightHandEvaluated && clause.RightHandComponentEvaluatedResult != null)
                {
                    double.TryParse(clause.RightHandComponentEvaluatedResult.ToString(), out var expectedDouble);

                    return actualDouble > expectedDouble;
                }

                if (clause.RightHandComponent != null && clause.RightHandComponent is string)
                {
                    double.TryParse(clause.RightHandComponent, out var expectedDouble);

                    return actualDouble > expectedDouble;
                }
            }
            else if (clause.Operator == LessThanCharacter)
            {
                var actualDouble = IsANumber(clause.LeftHandComponentEvaluatedResult) ? Convert.ToDouble(clause.LeftHandComponentEvaluatedResult) : 0d;

                if (clause.RightHandEvaluated && clause.RightHandComponentEvaluatedResult != null)
                {
                    double.TryParse(clause.RightHandComponentEvaluatedResult.ToString(), out var expectedDouble);

                    return actualDouble < expectedDouble;
                }

                if (clause.RightHandComponent != null && clause.RightHandComponent is string)
                {
                    double.TryParse(clause.RightHandComponent, out var expectedDouble);

                    return actualDouble < expectedDouble;
                }
            }

            throw new NotImplementedException(clause.Operator.ToString());
        }

        /// <summary>
        /// Evaluate values to a list of objects.
        /// </summary>
        /// <param name="values">The unevaluated values.</param>
        /// <param name="component">The component to get data from.</param>
        /// <returns>A list of objects.</returns>
        public static List<object> EvaluateValues(IReadOnlyCollection<object> values, Component component)
        {
            if (values == null || !values.Any())
            {
                return null;
            }

            var evaluatedValues = values.Where(value =>
                    !(value is string)
                    && !(value is EvaluatedExpressionComponent)
                    && !(value is EvaluatedExpressionComponentCollection))
                .ToList();

            foreach (var value in values.Where(loopValue => loopValue is string))
            {
                var replacedValue = PerformRuntimeReplacements(value?.ToString(), component);
                evaluatedValues.Add(replacedValue);
            }

            foreach (var value in values.Where(loopValue =>
                loopValue is EvaluatedExpressionComponent
                || loopValue is EvaluatedExpressionComponentCollection))
            {
                var evaluationExpressionList = new EvaluatedExpressionComponentCollection();

                if (value is EvaluatedExpressionComponentCollection valueList)
                {
                    evaluationExpressionList = valueList;
                }
                else if (value is EvaluatedExpressionComponent valueExpression)
                {
                    evaluationExpressionList.Items.Add(valueExpression);
                }

                var loopItemResults = new List<object>();

                foreach (var evaluationExpressionItem in evaluationExpressionList.Items)
                {
                    if (evaluationExpressionItem.Evaluated)
                    {
                        var result = CheckEquality(evaluationExpressionItem);
                        loopItemResults.Add(result);

                        continue;
                    }

                    var evaluationExpression = evaluationExpressionItem.Clone(); // Clone it so we don't alter the original

                    var containsVariable = evaluationExpression.LeftHandComponent is string
                        && evaluationExpression.LeftHandComponent?.Contains("@") == true;

                    if (evaluationExpression.IsSelector)
                    {
                        evaluationExpression.Evaluated = true;

                        var config = component.ComponentConfiguration;
                        var selector = evaluationExpression.LeftHandComponent;

                        if (component.Context != null)
                        {
                            if (component.Context is string contextString)
                            {
                                double.TryParse(Convert.ToString(component.ContextSchemaVersion), out var schemaVersion);
                                var contextKey = $"{contextString}-{schemaVersion}";
                                if (config.ContextConfigurationCache.ContainsKey(contextKey))
                                {
                                    config = config.ContextConfigurationCache[contextKey];
                                }
                                else
                                {
                                    var originalConfig = config;

                                    schemaVersion = schemaVersion > 0
                                        ? schemaVersion
                                        : component.ComponentConfiguration.SchemaVersion;

                                    config = new ComponentConfiguration
                                    {
                                        Data = new FundingApiSearchFunding
                                        {
                                            FundingValue = contextString
                                        },
                                        ConstantDefinedVariables = config.ConstantDefinedVariables,
                                        SchemaVersion = schemaVersion
                                    };

                                    originalConfig.ContextConfigurationCache.Add(contextKey, config);
                                }
                            }
                            else
                            {
                                config = new ComponentConfiguration
                                {
                                    Data = new FundingApiSearchFunding(),
                                    ConstantDefinedVariables = config.ConstantDefinedVariables,
                                    SchemaVersion = component.ComponentConfiguration.SchemaVersion
                                };
                            }
                        }

                        selector = PerformReplacements(selector, config, canShowSelector: false);
                        var selectorResult = Select(config, selector);

                        if (!ComponentService.IsNumber(selectorResult))
                        {
                            loopItemResults.Add(selectorResult);
                            continue;
                        }

                        loopItemResults.Add(ComponentService.StyleNumber(
                            selectorResult,
                            component.OriginalGroup?.Absolute ?? false,
                            component.OriginalGroup?.Datastyle?.NumberFormat,
                            component.OriginalGroup?.Scale));

                        continue;
                    }

                    var replacedValue = PerformRuntimeReplacements(evaluationExpression.LeftHandComponent, component, neverShowSelectors: true);

                    if (replacedValue is string)
                    {
                        evaluationExpression.LeftHandComponent = replacedValue?.ToString();

                        if (containsVariable)
                        {
                            evaluationExpression.LeftHandComponentEvaluatedResult = evaluationExpression.LeftHandComponent;
                        }
                    }
                    else
                    {
                        evaluationExpression.LeftHandComponentEvaluatedResult = replacedValue;
                    }

                    if (evaluationExpression.Operator.HasValue)
                    {
                        if (evaluationExpression.RightHandComponent.Contains("@"))
                        {
                            evaluationExpression.RightHandComponent = PerformRuntimeReplacements(evaluationExpression.RightHandComponent, component, neverShowSelectors: true)?.ToString();
                        }

                        evaluationExpression.ComparisonResult = CheckEquality(evaluationExpression);

                        if (!evaluationExpression.ComparisonResult.HasValue)
                        {
                            throw new Exception("Expression is evaluated but has no value");
                        }

                        loopItemResults.Add(evaluationExpression.ComparisonResult.Value);
                    }
                    else
                    {
                        loopItemResults.Add(replacedValue);
                    }

                    evaluationExpression.Evaluated = true;
                }

                if (loopItemResults.Count > 1)
                {
                    var result = EvaluateAndOrOrToBool(loopItemResults, evaluationExpressionList.IsOrQuery, component);
                    evaluatedValues.Add(result);

                    continue;
                }

                evaluatedValues.Add(loopItemResults.FirstOrDefault());
            }

            return evaluatedValues;
        }

        /// <summary>
        /// Perform runtime replacements of variables.
        /// </summary>
        /// <param name="inputString">The input string to look for variables in.</param>
        /// <param name="component">The component to look at for data.</param>
        /// <param name="errorIfVariablesRemaining">Whether to throw an error if there are still variables left at the end - defaults to true.</param>
        /// <param name="neverShowSelectors">Explicitly never show selectors.</param>
        /// <returns>A string with the replacements filled in.</returns>
        public static object PerformRuntimeReplacements(
            string inputString,
            Component component,
            bool errorIfVariablesRemaining = true,
            bool neverShowSelectors = false)
        {
            var chunks = CheckForAndHandleChunkVariable(inputString, component, errorIfVariablesRemaining, neverShowSelectors);
            if (chunks != null)
            {
                return chunks;
            }

            if (inputString?.Contains("@") != true)
            {
                return inputString?.ReplaceVYFAtSymbolWithAt();
            }

            var configuration = component.ComponentConfiguration;
            var isHtml = configuration.IsHtml;

            if (inputString.StartsWith("@"))
            {
                var evalatedComponents = GetEvaluatedExpressionComponents(inputString, configuration, component.PageData);
                var result = evalatedComponents?.FirstOrDefault()?.LeftHandComponentEvaluatedResult;
                var stillNeedsReplacement = result?.ToString().Contains("@") == true;

                if (result != null && !stillNeedsReplacement)
                {
                    if (!neverShowSelectors && configuration?.ShowSelectors == true && result is string resultString && resultString.Length < 100)
                    {
                        if (configuration?.ShowData == false)
                        {
                            resultString = string.Empty;
                        }

                        if (isHtml)
                        {
                            resultString += $" (<i><strong>{evalatedComponents?.FirstOrDefault()?.OriginalExpression?.Replace("@", "&#64;")}</strong></i>)";
                        }
                        else
                        {
                            resultString += $" ({evalatedComponents?.FirstOrDefault()?.OriginalExpression?.Replace("@", "&#64;")})";
                        }

                        return resultString;
                    }

                    return result;
                }
            }

            var leftHandReplaced = inputString;

            leftHandReplaced = ReplaceVariableStrings(leftHandReplaced, component.PageData);
            leftHandReplaced = ReplaceVariableStrings(leftHandReplaced, component.Variables);
            leftHandReplaced = ReplaceVariableStrings(leftHandReplaced, configuration?.Variables);

            if (component.Context is string contextString
            && !string.IsNullOrWhiteSpace(component.VisibilityCondition))
            {
                if (contextString.Contains(FundingValueContextSuffix))
                {
                    component.Context =
                        PerformRuntimeReplacements(
                            contextString,
                            component,
                            neverShowSelectors: true);

                    contextString = Convert.ToString(component.Context);

                    component.ContextSchemaVersion =
                        PerformRuntimeReplacements(
                            Convert.ToString(component.OriginalGroup.Context)
                                .Replace(FundingValueContextSuffix, ContextSchemaVersionSuffix),
                            component,
                            neverShowSelectors: true);
                    double.TryParse(Convert.ToString(component.ContextSchemaVersion), out var schemaVersion);

                    schemaVersion = schemaVersion > 0
                        ? schemaVersion
                        : configuration.SchemaVersion;

                    configuration = new ComponentConfiguration
                    {
                        Data = new FundingApiSearchFunding
                        {
                            FundingValue = contextString
                        },
                        ConstantDefinedVariables = configuration.ConstantDefinedVariables,
                        SchemaVersion = schemaVersion
                    };
                }
                else
                {
                    double.TryParse(Convert.ToString(component.ContextSchemaVersion), out var schemaVersion);

                    schemaVersion = schemaVersion > 0
                        ? schemaVersion
                        : configuration.SchemaVersion;
                    configuration = new ComponentConfiguration
                    {
                        Data = new FundingApiSearchFunding
                        {
                            FundingValue = contextString
                        },
                        ConstantDefinedVariables = configuration.ConstantDefinedVariables,
                        SchemaVersion = schemaVersion
                    };
                }
            }

            leftHandReplaced = PerformReplacements(leftHandReplaced, configuration);

            var anyVariablesRemaining = leftHandReplaced?.Contains("@") ?? false;

            if (errorIfVariablesRemaining && anyVariablesRemaining)
            {
                throw new Exception($"One or more variables could not be replaced in '{leftHandReplaced}'");
            }

            return leftHandReplaced?.ReplaceVYFAtSymbolWithAt();
        }

        /// <summary>
        /// Get a list of the evaluated expressions components that comprise a where clause.
        /// </summary>
        /// <param name="whereClause">The where clause to evaluate.</param>
        /// <param name="componentConfiguration">The meta data about the funding stream.</param>
        /// <param name="pageData">Variables to be used as replacements in left or right hand components.</param>
        /// <returns>A list of the evaluated expressions components that comprise a where clause.</returns>
        public static List<EvaluatedExpressionComponent> GetEvaluatedExpressionComponents(
            string whereClause,
            ComponentConfiguration componentConfiguration,
            Dictionary<string, object> pageData)
        {
            var isOrQuery = whereClause.Contains(OrCharacter);
            var clauses = whereClause.Split(isOrQuery ? OrCharacter : AndCharacter);

            var returnValue = new List<EvaluatedExpressionComponent>();
            var variables = Merge(componentConfiguration?.Variables, pageData);
            variables = Merge(variables, componentConfiguration?.ConstantDefinedVariables);

            foreach (var clause in clauses)
            {
                var clausePosition = GetClausePosition(clause);

                var whereClauseKey = clause;
                var whereClauseParts = new[] { whereClauseKey };
                bool? equalsTo = null;

                if (clausePosition.HasValue)
                {
                    whereClauseParts = new[] { clause.Substring(0, clausePosition.Value), clause.Substring(clausePosition.Value + 1) };

                    whereClauseKey = whereClauseParts.First();
                    equalsTo = !whereClauseKey.Contains(NotCharacter);
                    whereClauseKey = whereClauseKey.Replace(NotCharacter.ToString(), string.Empty);
                }

                var expectedValue = whereClauseParts.Length > 1 ? whereClauseParts.Last() : null;

                var returnItem = new EvaluatedExpressionComponent
                {
                    OriginalExpression = clause,
                    LeftHandComponent = whereClauseKey,
                    RightHandComponent = expectedValue,
                    Operator = clausePosition.HasValue ? clause[clausePosition.Value] : (char?)null,
                    EqualsTo = equalsTo
                };

                object actualValue = null;

                if (returnItem.LeftHandComponent.StartsWith("@"))
                {
                    string enclosedSection = null;

                    if (returnItem.LeftHandComponent.StartsWith("@("))
                    {
                        enclosedSection = returnItem.LeftHandComponent.Substring(2, returnItem.LeftHandComponent.IndexOf(")") - 2);

                        if (returnItem.LeftHandComponent != $"@({enclosedSection})")
                        {
                            returnValue.Add(returnItem);
                            continue;
                        }
                    }

                    var leftHandWithoutWrapping = returnItem.LeftHandComponent.Substring(1).Replace("(", string.Empty).Replace(")", string.Empty);
                    var leftHandWithoutWrappingFirstProperty = leftHandWithoutWrapping.Split('.')[0].Split('[')[0];
                    var continueLoop = false;

                    switch (leftHandWithoutWrapping)
                    {
                        case "DistributionPeriodMonthNumber":
                            actualValue = componentConfiguration?.PublishedDate.Month;
                            break;
                        case "PublicationUiModelVersion":
                            actualValue = componentConfiguration?.PublicationUiModelVersion;
                            break;
                        case nameof(componentConfiguration.InYearOpener):
                            actualValue = componentConfiguration?.InYearOpener;
                            break;
                        case nameof(componentConfiguration.IsCurrentYearOpener):
                            actualValue = componentConfiguration?.IsCurrentYearOpener;
                            break;
                        case nameof(componentConfiguration.OpeningDay):
                            actualValue = componentConfiguration?.OpeningDay;
                            break;
                        case nameof(componentConfiguration.OpeningMonth):
                            actualValue = componentConfiguration?.OpeningMonth;
                            break;
                        case nameof(componentConfiguration.OpeningYear):
                            actualValue = componentConfiguration?.OpeningYear;
                            break;
                        case nameof(componentConfiguration.IsMainstreamAcademy):
                            actualValue = componentConfiguration?.IsMainstreamAcademy;
                            break;
                        case nameof(componentConfiguration.IsMainstreamAcademySponsored):
                            actualValue = componentConfiguration?.IsMainstreamAcademySponsored;
                            break;
                        case nameof(componentConfiguration.IsMainstreamFreeSchool):
                            actualValue = componentConfiguration?.IsMainstreamFreeSchool;
                            break;
                        case nameof(componentConfiguration.IsSpecialAcademy):
                            actualValue = componentConfiguration?.IsSpecialAcademy;
                            break;
                        case nameof(componentConfiguration.IsSpecialFreeSchool):
                            actualValue = componentConfiguration?.IsSpecialFreeSchool;
                            break;
                        case "providerIsPrimaryInstitution":
                        case nameof(componentConfiguration.ProviderIsPrimaryInstitution):
                            actualValue = componentConfiguration?.ProviderIsPrimaryInstitution;
                            break;
                        case "providerIsSecondaryInstitution":
                        case nameof(componentConfiguration.ProviderIsSecondaryInstitution):
                            actualValue = componentConfiguration?.ProviderIsSecondaryInstitution;
                            break;
                        case "providerIsAllThroughInstitution":
                        case nameof(componentConfiguration.ProviderIsAllThroughInstitution):
                            actualValue = componentConfiguration?.ProviderIsAllThroughInstitution;
                            break;
                        default:
                            if (variables?.ContainsKey(leftHandWithoutWrappingFirstProperty) != true)
                            {
                                returnValue.Add(returnItem);
                                continueLoop = true;

                                break;
                            }

                            actualValue = variables[leftHandWithoutWrappingFirstProperty];

                            if (enclosedSection != null)
                            {
                                var propertiesStrings = enclosedSection
                                    .Replace(".", "¬")
                                    .Replace("[", "¬")
                                    .Replace("]", string.Empty)
                                    .Split('¬')
                                    .Skip(1)
                                    .ToArray();

                                actualValue = GetValueFromNestedProperties(propertiesStrings, actualValue);
                            }

                            break;
                    }

                    if (continueLoop)
                    {
                        continue;
                    }

                    returnItem.LeftHandComponent = leftHandWithoutWrappingFirstProperty;
                }
                else if (returnItem.LeftHandComponent.StartsWith("C") && returnItem.Operator.HasValue)
                {
                    int.TryParse(returnItem.LeftHandComponent.Substring(1), out var templateCalculationId);
                    actualValue = componentConfiguration?.CalculationAndFundingLines?.Calculations?.FirstOrDefault(c => c.Key == templateCalculationId).Value?.Value;
                }
                else if (returnItem.LeftHandComponent.StartsWith("L") && returnItem.Operator.HasValue)
                {
                    int.TryParse(returnItem.LeftHandComponent.Substring(1), out var templateLineId);
                    actualValue = componentConfiguration?.CalculationAndFundingLines?.FundingLines?.FirstOrDefault(c => c.Key == templateLineId).Value?.Value;
                }
                else
                {
                    actualValue = returnItem.LeftHandComponent;
                }

                if (returnItem.RightHandComponent?.StartsWith("@") == true)
                {
                    var rightHandWithoutWrapping = returnItem.RightHandComponent.Substring(1).Replace("(", string.Empty).Replace(")", string.Empty);
                    var rightHandWithoutWrappingFirstProperty = rightHandWithoutWrapping.Split('.')[0];

                    object actualRightHandValue;
                    switch (rightHandWithoutWrapping)
                    {
                        case "DistributionPeriodMonthNumber":
                            actualRightHandValue = componentConfiguration?.PublishedDate.Month;
                            break;
                        case "PublicationUiModelVersion":
                            actualRightHandValue = componentConfiguration?.PublicationUiModelVersion;
                            break;
                        case nameof(componentConfiguration.InYearOpener):
                            actualRightHandValue = componentConfiguration?.InYearOpener;
                            break;
                        case nameof(componentConfiguration.IsCurrentYearOpener):
                            actualRightHandValue = componentConfiguration?.IsCurrentYearOpener;
                            break;
                        case nameof(componentConfiguration.OpeningDay):
                            actualRightHandValue = componentConfiguration?.OpeningDay;
                            break;
                        case nameof(componentConfiguration.OpeningMonth):
                            actualRightHandValue = componentConfiguration?.OpeningMonth;
                            break;
                        case nameof(componentConfiguration.OpeningYear):
                            actualRightHandValue = componentConfiguration?.OpeningYear;
                            break;
                        case nameof(componentConfiguration.IsMainstreamAcademy):
                            actualRightHandValue = componentConfiguration?.IsMainstreamAcademy;
                            break;
                        case nameof(componentConfiguration.IsMainstreamAcademySponsored):
                            actualRightHandValue = componentConfiguration?.IsMainstreamAcademySponsored;
                            break;
                        case nameof(componentConfiguration.IsMainstreamFreeSchool):
                            actualRightHandValue = componentConfiguration?.IsMainstreamFreeSchool;
                            break;
                        case nameof(componentConfiguration.IsSpecialAcademy):
                            actualRightHandValue = componentConfiguration?.IsSpecialAcademy;
                            break;
                        case nameof(componentConfiguration.IsSpecialFreeSchool):
                            actualRightHandValue = componentConfiguration?.IsSpecialFreeSchool;
                            break;
                        case "providerIsPrimaryInstitution":
                        case nameof(componentConfiguration.ProviderIsPrimaryInstitution):
                            actualRightHandValue = componentConfiguration?.ProviderIsPrimaryInstitution;
                            break;
                        case "providerIsSecondaryInstitution":
                        case nameof(componentConfiguration.ProviderIsSecondaryInstitution):
                            actualRightHandValue = componentConfiguration?.ProviderIsSecondaryInstitution;
                            break;
                        case "providerIsAllThroughInstitution":
                        case nameof(componentConfiguration.ProviderIsAllThroughInstitution):
                            actualRightHandValue = componentConfiguration?.ProviderIsAllThroughInstitution;
                            break;
                        default:
                            actualRightHandValue = variables[rightHandWithoutWrappingFirstProperty];
                            break;
                    }

                    returnItem.RightHandComponentEvaluatedResult = actualRightHandValue;
                    returnItem.RightHandEvaluated = true;
                }

                returnItem.LeftHandComponentEvaluatedResult = actualValue;
                returnItem.Evaluated = true;

                returnValue.Add(returnItem);
            }

            return returnValue;
        }

        /// <summary>
        /// Determine if a component is visible or not.
        /// </summary>
        /// <param name="component">The component to use.</param>
        /// <returns>True if visible (also in the case there is no condition), false if not.</returns>
        public static bool EvaluateVisibilityCondition(Component component)
        {
            if (string.IsNullOrWhiteSpace(component.VisibilityCondition))
            {
                return true;
            }

            var configuration = component.ComponentConfiguration;

            if (configuration?.ShowStatementSpecification == true)
            {
                return true;
            }

            var visibilityExpresionUnevaluated = EvaluateWhereClause(component.VisibilityCondition, configuration, component.Variables);
            var visibilityEvaluated = EvaluateValues(new List<object> { visibilityExpresionUnevaluated }, component);
            var visiblityString = visibilityEvaluated?.FirstOrDefault()?.ToString();

            bool.TryParse(visiblityString, out var visibility);
            return visibility;
        }

        /// <summary>
        /// Get a dictionary containing a combination of the fixed variables, and the dynamic ones.
        /// </summary>
        /// <param name="configuration">The component configuration.</param>
        /// <param name="rowBookmark">The current row bookmark (cell reference) - applicable for spreadsheets only.</param>
        /// <param name="nextRowNumber">The next row number (the current row plus 1) - applicable for spreadsheets only.</param>
        /// <param name="dataEndRow">The last data row number - applicable for spreadsheets only.</param>
        /// <returns>A merged dictionary.</returns>
        public static Dictionary<string, object> GetCombinedVariables(
            ComponentConfiguration configuration,
            CellReference rowBookmark = null,
            int nextRowNumber = -1,
            int? dataEndRow = -1)
        {
            var variables = configuration.Variables;

            if (variables == null)
            {
                variables = new Dictionary<string, object>();
            }

            var rowVariables = GetRowSpecificVariables(rowBookmark, nextRowNumber, dataEndRow);
            return Merge(configuration.ConstantDefinedVariables, Merge(rowVariables, variables));
        }

        /// <summary>
        /// Is the type a generic list?.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>True if is of type List.</returns>
        public static bool IsGenericList(object obj)
        {
            var oType = obj.GetType();
            return oType.GetTypeInfo().IsGenericType && oType.GetGenericTypeDefinition() == typeof(List<>);
        }

        /// <summary>
        /// Get a dictionary containing dynamic row variables.
        /// </summary>
        /// <param name="rowBookmark">The current row bookmark (cell reference) - applicable for spreadsheets only.</param>
        /// <param name="nextRowNumber">The next row number (the current row plus 1) - applicable for spreadsheets only.</param>
        /// <param name="dataEndRow">The last data row number - applicable for spreadsheets only.</param>
        /// <returns>A dictionary.</returns>
        public static Dictionary<string, object> GetRowSpecificVariables(
            CellReference rowBookmark = null,
            int nextRowNumber = -1,
            int? dataEndRow = -1)
        {
            var endCellReference = rowBookmark != null
                ? new CellReference(rowBookmark?.ToString())
                {
                    RowNumber = dataEndRow ?? 1
                }
                : null;

            var variables = new Dictionary<string, object>
            {
                // Spreadsheets
                { "thisplustwocolumn", rowBookmark?.Add(2).Letters() },
                { "thisplusthreecolumn", rowBookmark?.Add(3).Letters() },
                { "thisplusfourcolumn", rowBookmark?.Add(4).Letters() },
                { "thisplusfivecolumn", rowBookmark?.Add(5).Letters() },
                { "thiscolumn", rowBookmark?.Letters() },
                { "nextcolumn", rowBookmark?.Add(1).Letters() },
                { "previouscolumn", rowBookmark?.Add(-1).Letters() },
                { "thiscellreference", rowBookmark?.ToString() },
                { "nextcellreference", rowBookmark?.NextRow().ToString() },
                { "endcellreference", endCellReference?.ToString() },
                { "datathisrow", (nextRowNumber - 1).ToString() },
                { "datanextrow", nextRowNumber.ToString() },
                { "dataendrow", dataEndRow.ToString() }
            };

            return variables;
        }

        /// <summary>
        /// Get a dictionary containing a combination of the fixed variables, and the dynamic ones.
        /// </summary>
        /// <param name="configuration">The component configuration.</param>
        /// <returns>A dictionary.</returns>
        public static Dictionary<string, object> GetConstantDefinedVariables(ComponentConfiguration configuration)
        {
            var periodStartYear = -1;
            var periodEndYear = -1;

            if (!string.IsNullOrEmpty(configuration.FundingPeriodCode))
            {
                (periodStartYear, periodEndYear) = FundingPeriodHelper.GetYearsFromCode(configuration.FundingPeriodCode);
            }

            var financialYear1 = FundingPeriodHelper.GetCodeFromYears(periodStartYear, periodEndYear, YearTypeCode.FinancialYear);
            var financialYear2 = FundingPeriodHelper.GetCodeFromYears(periodEndYear, periodEndYear + 1, YearTypeCode.FinancialYear);
            var yearTypeName = FundingPeriodHelper.GetYearTypeNameFromCode(configuration.FundingPeriodCode);
            var yearTypeCode = FundingPeriodHelper.GetCodeFromYearType(yearTypeName);
            var year3Short = (periodEndYear + 1).ToString().Length > 2 ? (periodEndYear + 1).ToString().Substring(2) : null;
            var year2Short = periodEndYear.ToString().Length > 2 ? periodEndYear.ToString().Substring(2) : null;
            var year1Short = periodStartYear.ToString().Length > 2 ? periodStartYear.ToString().Substring(2) : null;
            var year0 = periodStartYear - 1;
            var year0Short = year0.ToString().Length > 2 ? year0.ToString().Substring(2) : null;
            var yearMinus1Short = (periodStartYear - 2).ToString().Length > 2 ? (periodStartYear - 2).ToString().Substring(2) : null;
            var yearMinus2Short = (periodStartYear - 3).ToString().Length > 2 ? (periodStartYear - 3).ToString().Substring(2) : null;

            const string viaChoicePageParameter = "viaChoicePage=true";
            var fundingStreamNamePathPart = configuration.FundingStreamName?.ToUIPathComponent();
            var searchTermPartQuestionMark = GetSearchTermParameter(configuration.SearchTerm, "?", configuration.ViaChoicePage, viaChoicePageParameter);
            var searchTermPartAmpersand = GetSearchTermParameter(configuration.SearchTerm, "&", configuration.ViaChoicePage, viaChoicePageParameter);

            var localAuthorityFundingBreakdownPath = $"{configuration.BasePath}/{fundingStreamNamePathPart}/funding-breakdown/" +
                $"{periodStartYear}-to-{periodEndYear}/{configuration.LocalAuthorityCode}/{configuration.PublishedDate.ToString("dd-M-yyyy")}{searchTermPartQuestionMark}";

            var localAuthorityFundingBreakdownPathTabs = localAuthorityFundingBreakdownPath
                + (localAuthorityFundingBreakdownPath.Contains("?") ? "&tab=" : "?tab=");

            var providerFundingBreakdownPath = $"{configuration.BasePath}/{fundingStreamNamePathPart}/provider-funding-breakdown/" +
                $"{configuration.PublishedDate:dd-M-yyyy}/{configuration.PrimaryIdentifier}-{periodStartYear}-to-{periodEndYear}/{searchTermPartQuestionMark}";

            var loggedInProviderFundingBreakdownPath =
                $"{configuration.UrlForLoggedInView}/{configuration.PrimaryIdentifier}/{fundingStreamNamePathPart}/{configuration.StatusChangedDate?.ToString("dd-M-yyyy")}/{periodStartYear}-to-{periodEndYear}";

            var loggedInLocalAuthorityFundingBreakdownPath =
                $"{configuration.UrlForLoggedInView}/{configuration.EntityGroupUkprn}/{fundingStreamNamePathPart}/{configuration.StatusChangedDate?.ToString("dd-M-yyyy")}/{periodStartYear}-to-{periodEndYear}/la";

            if (configuration.ViaChoicePage)
            {
                loggedInProviderFundingBreakdownPath += $"?{viaChoicePageParameter}";
                loggedInLocalAuthorityFundingBreakdownPath += $"?{viaChoicePageParameter}";
            }

            var loggedInProviderFundingBreakdownPathTabs = $"{loggedInProviderFundingBreakdownPath}{(loggedInProviderFundingBreakdownPath.Contains("?") ? "&" : "?")}tab=";

            //TODO:hardcoded UKPRN as it is null for LAREC, This needs to remove once resolved
            var laRecoupmentDetailsPath = $"/view-latest-funding/recoupment-reports/{configuration.EntityGroupUkprn ?? "10007063"}/{configuration.FundingStreamCode}/{configuration.PublishedDate.ToString("dd-M-yyyy")}/{periodStartYear}-to-{periodEndYear}/la";
            var laRecoupmentDetailsPathTabs = $"{laRecoupmentDetailsPath}{(laRecoupmentDetailsPath.Contains("?") ? "&" : "?")}tab=";

            var laNameTitleCase =
                string.IsNullOrWhiteSpace(configuration.LocalAuthorityName?.ReplaceAtSymbolInName()) ?
                string.Empty :
                System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(configuration.LocalAuthorityName.ReplaceAtSymbolInName().ToLower());


            var variables = new Dictionary<string, object>
            {
                // General
                { "bullet", "•" },
                { "britishCurrencySymbol", "£" },
                { "regionName", configuration.RegionName },
                { "basePath", configuration.BasePath },

                // Dates
                { "year-2short", yearMinus2Short },
                { "year-2", (periodStartYear - 3).ToString() },
                { "year-1short", yearMinus1Short },
                { "year-1", (periodStartYear - 2).ToString() },
                { "year0short", year0Short },
                { "year0", (periodStartYear - 1).ToString() },
                { "year1short", year1Short },
                { "year1", periodStartYear.ToString() },
                { "year2short", year2Short },
                { "year2", periodEndYear.ToString() },
                { "year3short", year3Short },
                { "year3", (periodEndYear + 1).ToString() },
                { "FY1", financialYear1 },
                { "FY2", financialYear2 },
                { "yearTypeName", yearTypeName },
                { "yearTypeCode", yearTypeCode },
                { "asofmonth", configuration.AsOfMonth },
                { "asofyear", configuration.AsOfYear },
                { "publicationdate-dd_MMMM_yyyy", configuration.PublishedDate.ToString("dd MMMM yyyy") },
                { "publicationDate_dd-M-yyyy", configuration.PublishedDate.ToString("dd-M-yyyy") },
                { "publicationdate-d_MMMM_yyyy", configuration.PublishedDate.ToString("d MMMM yyyy") },
                { "publicationdate", configuration.PublishedDate.ToString("dddd, dd MMM, yyyy") },
                { "isCurrentYear", configuration.IsCurrentYear },
                { "currentDate", DateTime.Now.ToString("dd MMMM yyyy") },
                { "currentMonth", DateTime.Now.ToString("MMMM yyyy") },

                // Funding stream
                { "fundingStreamCodeLowercase", configuration.FundingStreamCode?.ToLower() },
                { "fundingStreamCodeOrName", configuration.FundingStreamCodeOrName },
                { "fundingStreamCode", configuration.FundingStreamCode },
                { "fundingStreamNamePathPart", configuration.FundingStreamName?.ToUIPathComponent() },
                { "fundingStreamNameLowercase", configuration.FundingStreamName?.ToLower() },
                { "fundingStreamNameWithinSentence", configuration.FundingStreamNameWithinSentence },
                { "fundingStreamName", configuration.FundingStreamName },
                { "shortFundingStreamNameHtml", configuration.ShortFundingStreamNameHtml },
                { "expandedFundingStreamNameHtml", configuration.ExpandedFundingStreamNameHtml },

                // LA
                { "localAuthorityNameOverride", configuration.LocalAuthorityNameOverride?.ReplaceAtSymbolInName() },
                { "localAuthorityName", configuration.LocalAuthorityName?.ReplaceAtSymbolInName() },
                { nameof(laNameTitleCase), laNameTitleCase },
                { "providerLocalAuthorityName", configuration.ProviderLocalAuthorityName },
                { "laCode", configuration.LocalAuthorityCode },

                // Provider
                { "providerName", configuration.ProviderName?.ReplaceAtSymbolInName() },
                { "providerUkprn", configuration.PrimaryIdentifier },
                { "providerEstablishmentNumber", configuration.ProviderEstablishmentNumber },
                { "providerIsPrimaryInstitution", configuration.ProviderIsPrimaryInstitution },
                { "providerIsSecondaryInstitution", configuration.ProviderIsSecondaryInstitution },
                { "providerIsAllThroughInstitution", configuration.ProviderIsAllThroughInstitution },
                { nameof(configuration.InYearOpener), configuration.InYearOpener },
                { nameof(configuration.IsCurrentYearOpener), configuration.IsCurrentYearOpener },
                { nameof(configuration.IsSecondYearInYearOpener), configuration.IsSecondYearInYearOpener },
                { nameof(configuration.IsAcademyConverterOrNewProvisionInYearOpener), configuration.IsAcademyConverterOrNewProvisionInYearOpener },
                { nameof(configuration.IsIndicativeFunding), configuration.IsIndicativeFunding },
                { nameof(configuration.IsPostAprilOpener), configuration.IsPostAprilOpener },
                { nameof(configuration.OpeningDay), configuration.OpeningDay },
                { nameof(configuration.OpeningMonth), configuration.OpeningMonth },
                { nameof(configuration.OpeningYear), configuration.OpeningYear },
                { nameof(configuration.IsMainstreamAcademy), configuration.IsMainstreamAcademy },
                { nameof(configuration.IsMainstreamFreeSchool), configuration.IsMainstreamFreeSchool },
                { nameof(configuration.IsSpecialAcademy), configuration.IsSpecialAcademy },
                { nameof(configuration.IsSpecialFreeSchool), configuration.IsSpecialFreeSchool },
                { nameof(configuration.IsLocalAuthorityProviderType), configuration.IsLocalAuthorityProviderType },
                { nameof(configuration.IsAcademyProviderType), configuration.IsAcademyProviderType },
                { nameof(configuration.IsSpecialPost16ProviderType), configuration.IsSpecialPost16ProviderType },
                { nameof(configuration.IsSchoolProviderType), configuration.IsSchoolProviderType },
                { nameof(configuration.IsFurtherEducationProviderType), configuration.IsFurtherEducationProviderType },
                { nameof(configuration.IsNonProgrammeFundedProviderType), configuration.IsNonProgrammeFundedProviderType },
                { "providerOpenDateddMMMMyyyy", configuration.ProviderOpenDate?.ToString("dd MMMM yyyy") },
                { "providerOpenDaysStartDateddMMMMyyyy", configuration.ProviderOpenDaysStartDate?.ToString("dd MMMM yyyy") },
                { "providerOpenDate", configuration.ProviderOpenDate },
                { "providerUpin", configuration.ProviderUpin },
                { "providerUrn", configuration.ProviderUrn },
                { "providerType", configuration.ProviderType },
                { "providerSubType", configuration.ProviderSubType },
                { "varianceMessage", configuration.VarianceMessage },

                // Page paths
                { "providerFundingBreakdownPath", providerFundingBreakdownPath },
                { "localAuthorityFundingBreakdownPathTabs", localAuthorityFundingBreakdownPathTabs },
                { "localAuthorityFundingBreakdownPath", localAuthorityFundingBreakdownPath },
                { "searchTermPart?", searchTermPartQuestionMark },
                { "searchTermPart&", searchTermPartAmpersand },

                // Logged in provider paths
                { "loggedInProviderFundingBreakdownPathTabs", loggedInProviderFundingBreakdownPathTabs },
                { "loggedInProviderFundingBreakdownPath", loggedInProviderFundingBreakdownPath },
                { "loggedInLocalAuthorityFundingBreakdownPath", loggedInLocalAuthorityFundingBreakdownPath },
                { "statusChangedDateUiFormatted", configuration.StatusChangedDate.ToDateDisplay() },
                { "statusChangedDate", configuration.StatusChangedDate },

                // LA recoupment paths
                { "laRecoupmentDetailsPathTabs", laRecoupmentDetailsPathTabs },

                // LoggedIn Home Link
                { "LoggedInProviderHomeLink", ComponentHelper.ApplicationConfiguration?.LoggedInProviderHomeLink?.Trim()?.TrimEnd('/') ?? "/" },
            };

            if (configuration.AllDatasetsData != null)
            {
                const int MAX_NUMBER_OF_DATASET_VARIABLES = 10; // This number is largely arbitrary.

                for (var idx = 1; idx <= MAX_NUMBER_OF_DATASET_VARIABLES; idx++)
                {
                    variables.Add(
                        $"dataset{idx}",
                        configuration.AllDatasetsData.Count >= idx ? configuration.AllDatasetsData[idx - 1] : null);
                }
            }

            return variables;
        }

        /// <summary>
        /// Gets the first dataset definition.
        /// </summary>
        /// <param name="groups">List of groups.</param>
        /// <param name="fallback">Fallback dataset.</param>
        /// <returns>The UI model dataset.</returns>
        public static List<UiModelDataset> GetAllDatasetDefinitions(List<UiModelGroup> groups, List<UiModelDataset> fallback = null)
        {
            var datasets = new List<UiModelDataset>();

            if (fallback != null)
            {
                datasets.AddRange(fallback);
            }

            if (groups != null)
            {
                groups.ForEach(group => GetAllDatasets(group, datasets));
            }

            return datasets;
        }

        /// <summary>
        /// Evaluate a where clause to true, false or an object to be evaluated later.
        /// </summary>
        /// <param name="whereClause">The where clause to evaluate.</param>
        /// <param name="componentConfiguration">The meta data about the funding stream.</param>
        /// <param name="variables">Variables to be used as replacements in left or right hand components.</param>
        /// <returns>True, false or an object to be evaluated later.</returns>
        protected static object EvaluateWhereClause(
            string whereClause,
            ComponentConfiguration componentConfiguration,
            Dictionary<string, object> variables)
        {
            var clauses = GetEvaluatedExpressionComponents(whereClause, componentConfiguration, variables);
            var isOrQuery = whereClause.Contains(OrCharacter);

            if (clauses.Any(clause => !clause.Evaluated))
            {
                return new EvaluatedExpressionComponentCollection
                {
                    Items = clauses,
                    IsOrQuery = isOrQuery
                };
            }

            if (clauses.Count == 1 && clauses.First().Operator == null)
            {
                return clauses.First().LeftHandComponentEvaluatedResult;
            }

            return EvaluateAndOrOrToBool(
                clauses.Select(clause => (object)clause).ToList(),
                isOrQuery,
                new Component(componentConfiguration));
        }

        /// <summary>
        /// Given a string with replacement parts in it (e.g. 'Hello it is @year1'), swap the replacements for the relevant data.
        /// </summary>
        /// <param name="inputObject">The title of the worksheet (the none replaced text).</param>
        /// <param name="configuration">The component configuration.</param>
        /// <param name="rowBookmark">The current row bookmark (cell reference) - applicable for spreadsheets only.</param>
        /// <param name="nextRowNumber">The next row number (the current row plus 1) - applicable for spreadsheets only.</param>
        /// <param name="dataEndRow">The last data row number - applicable for spreadsheets only.</param>
        /// <param name="canShowSelector">Can a selector be shown.</param>
        /// <returns>The string with the replacements made.</returns>
        protected static string PerformReplacements(
            object inputObject,
            ComponentConfiguration configuration,
            CellReference rowBookmark = null,
            int nextRowNumber = -1,
            int? dataEndRow = -1,
            bool canShowSelector = true)
        {
            var text = inputObject is JArray titleArray ?
                string.Join(string.Empty, titleArray.ToObject<string[]>()) : inputObject?.ToString();

            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var responseString = text ?? string.Empty;

            if (configuration.Data != null)
            {
                const string SELECTOR_PATH_VARIABLE_START = "@[$";
                const string SELECTOR_PATH_VARIABLE_END = "]";

                while (responseString.Contains(SELECTOR_PATH_VARIABLE_START))
                {
                    var startPosition = responseString.IndexOf(SELECTOR_PATH_VARIABLE_START);
                    var endPosition = responseString.Substring(startPosition).LastIndexOf(SELECTOR_PATH_VARIABLE_END);

                    var replacementSnippet = responseString.Substring(startPosition, endPosition) + SELECTOR_PATH_VARIABLE_END;

                    var selectorPath = replacementSnippet.Substring(
                        SELECTOR_PATH_VARIABLE_START.Length - 1,
                        (replacementSnippet.Length - (SELECTOR_PATH_VARIABLE_START.Length + SELECTOR_PATH_VARIABLE_END.Length)) + 1);

                    var jsonPathResult = Select(configuration, selectorPath)?.ToString();
                    responseString = responseString.Replace(replacementSnippet, jsonPathResult);
                }
            }

            if (responseString.Contains("@") != true)
            {
                return responseString;
            }

            var isHtml = configuration.IsHtml;
            var variables = GetCombinedVariables(configuration, rowBookmark, nextRowNumber, dataEndRow);

            foreach (var variable in variables)
            {
                var key = $"@{variable.Key}";

                if (responseString.Contains(key))
                {
                    var replaceWith = variable.Value?.ToString();

                    if (configuration.ShowSelectors)
                    {
                        var originalVariable = configuration?.OriginalModel?.Variables?.FirstOrDefault(v => v.Name == variable.Key);
                        var showSelector = canShowSelector && originalVariable?.NeverShowSelector != true;

                        if (showSelector)
                        {
                            if (configuration?.ShowData == false)
                            {
                                replaceWith = string.Empty;
                            }

                            var simpliedString = " (";

                            if (isHtml)
                            {
                                simpliedString += "<i><strong>&#64;";
                            }
                            else
                            {
                                simpliedString += "@";
                            }

                            if (originalVariable != null)
                            {
                                var selector = originalVariable.Selector ?? originalVariable.Expression;
                                simpliedString += $"{variable.Key} ({ComponentHelper.SimplifySelector(selector, isHtml)})";
                            }
                            else
                            {
                                simpliedString += ComponentHelper.SimplifySelector(variable.Key, isHtml);
                            }

                            if (isHtml)
                            {
                                simpliedString += "</strong></i>";
                            }

                            simpliedString += ")";
                            replaceWith += simpliedString;
                        }
                    }

                    responseString = responseString.Replace(key, replaceWith);
                }
            }

            return responseString;
        }

        /// <summary>
        /// Get the number of columns a single column should span.
        /// </summary>
        /// <param name="group">Information about the cell/group, including other cells/groups that may be underneath it.</param>
        /// <returns>A number, 1 being the default (and minimum).</returns>
        protected static int GetColSpan(UiModelGroup group)
        {
            if (group.Colspan.HasValue)
            {
                return group.Colspan.Value;
            }

            if (group.Groups == null)
            {
                return 1;
            }

            var result = 0;

            for (int idx = 0, len = group.Groups.Count; idx < len; idx++)
            {
                var subGroup = group.Groups[idx];
                result += GetColSpan(subGroup);
            }

            return result;
        }

        /// <summary>
        /// Merge 2 dictionaries.
        /// </summary>
        /// <param name="first">The first dictionary.</param>
        /// <param name="second">The second dictionary.</param>
        /// <returns>A merged dictionary.</returns>
        protected static Dictionary<string, object> Merge(Dictionary<string, object> first, Dictionary<string, object> second)
        {
            if (first == null || second == null)
            {
                return first ?? second;
            }

            var returnDictionary = new Dictionary<string, object>(first);

            foreach (var item in second)
            {
                returnDictionary[item.Key] = item.Value;
            }

            return returnDictionary;
        }

        /// <summary>
        /// Does the group have a formula or selector specified, either directly or in overrides?.
        /// </summary>
        /// <param name="uiModelGroup">The current group to process.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <param name="checkOverrides">Whether or not to check the group's overrides for a formula/selector.</param>
        /// <returns>True if the group has a formula or selector specified, otherwise false.</returns>
        protected static bool HasFormulaOrSelector(UiModelGroup uiModelGroup, UiModelDataset dataset, bool checkOverrides = true)
        {
            if (uiModelGroup?.Selector != null)
            {
                return true;
            }

            if (uiModelGroup?.Formula?.Expression != null)
            {
                return true;
            }

            if (!checkOverrides)
            {
                return false;
            }

            var overrideGroup = GetGroupingTypeOverrideGroup(uiModelGroup, dataset);
            if (HasFormulaOrSelector(overrideGroup, null, false))
            {
                return true;
            }

            overrideGroup = GetDatasetIdOverrideGroup(uiModelGroup, dataset);
            return HasFormulaOrSelector(overrideGroup, null, false);
        }

        /// <summary>
        /// Get the relevant UI model formula (it may have been overridden).
        /// </summary>
        /// <param name="uiModelGroup">The current group to process.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <returns>An <see cref="OverrideMeta{T}"/> object containing the UI model formula and a boolean indicating whether or not it is an override.</returns>
        protected static OverrideMeta<UiModelFormula> GetFormulaMeta(UiModelGroup uiModelGroup, UiModelDataset dataset)
        {
            var overrideGroup = GetDatasetIdOverrideGroup(uiModelGroup, dataset);
            if (overrideGroup?.Formula?.Expression != null)
            {
                return new OverrideMeta<UiModelFormula>(overrideGroup.Formula, true);
            }

            overrideGroup = GetGroupingTypeOverrideGroup(uiModelGroup, dataset);
            if (overrideGroup?.Formula?.Expression != null)
            {
                return new OverrideMeta<UiModelFormula>(overrideGroup.Formula, true);
            }

            return uiModelGroup.Formula?.Expression != null
                ? new OverrideMeta<UiModelFormula>(uiModelGroup.Formula, false)
                : new OverrideMeta<UiModelFormula>(null, false);
        }

        /// <summary>
        /// Get the relevant JsonPath selector (it may have been overridden).
        /// </summary>
        /// <param name="uiModelGroup">The current group to process.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <returns>An <see cref="OverrideMeta{T}"/> object containing the JsonPath selector and a boolean indicating whether or not it is an override.</returns>
        protected static OverrideMeta<string> GetSelectorMeta(UiModelGroup uiModelGroup, UiModelDataset dataset)
        {
            var overrideGroup = GetDatasetIdOverrideGroup(uiModelGroup, dataset);
            if (overrideGroup?.Selector != null)
            {
                return new OverrideMeta<string>(overrideGroup.Selector, true);
            }

            overrideGroup = GetGroupingTypeOverrideGroup(uiModelGroup, dataset);
            if (overrideGroup?.Selector != null)
            {
                return new OverrideMeta<string>(overrideGroup.Selector, true);
            }

            return new OverrideMeta<string>(uiModelGroup.Selector, false);
        }

        /// <summary>
        /// Apply the JsonPath to the Json object to get a value.
        /// </summary>
        /// <param name="componentConfiguration">The component configuration.</param>
        /// <param name="selectorPath">The JsonPath to apply.</param>
        /// <param name="returnZeroForNoValue">Return a 0 if the line or calc if found, but it has no value property (or the value is null).</param>
        /// <returns>A JToken.</returns>
        protected static object Select(ComponentConfiguration componentConfiguration, string selectorPath, bool returnZeroForNoValue = true)
        {
            if (string.IsNullOrEmpty(selectorPath))
            {
                return null;
            }

            var noValueResult = returnZeroForNoValue ? 0D : (double?)null;

            try
            {
                var simplifiedId = ComponentHelper.SimplifySelector(selectorPath, componentConfiguration.IsHtml);
                var isNumberWithPreecedingLetter = int.TryParse(simplifiedId?.Substring(1), out var id) == true;
                var isDistributionPeriodIdSelector = simplifiedId?.StartsWith("D") == true &&
                                           FundingPeriodCodeRegex.IsMatch(simplifiedId.Substring(1));

                if (!isNumberWithPreecedingLetter && !isDistributionPeriodIdSelector)
                {
                    switch (selectorPath.Replace("$..", "$."))
                    {
                        case "$.totalValue":
                            return componentConfiguration.CalculationAndFundingLines.TotalValue;
                        case "$.groupCode":
                            return componentConfiguration.PrimaryIdentifier;
                        case "$.providerType":
                            return componentConfiguration.ProviderType;
                        case "$.providerSubType":
                            return componentConfiguration.ProviderType;
                    }

                    if (selectorPath.StartsWith(DatasetCountPrefix, StringComparison.InvariantCultureIgnoreCase)
                        && selectorPath.EndsWith(DatasetCountSuffix, StringComparison.InvariantCultureIgnoreCase)
                        && componentConfiguration.AllDatasetsData.Any())
                    {
                        return GetDataSetCount(componentConfiguration, selectorPath);
                    }

                    if (componentConfiguration.Data is IFundingApiSearchProviderFunding providerFunding)
                    {
                        switch (selectorPath.Replace("$..", "$."))
                        {
                            case "$.openingShortDateFormat":
                                return componentConfiguration.ProviderOpenDate?.ToString("dd/MM/yyyy");
                            case "$.organisationDfeNumber":
                                return providerFunding.OrganisationDfeNumber;
                            case "$.parentPrimaryIdentifier":
                                return providerFunding.ParentPrimaryIdentifier;
                            case "$.parentName":
                                return providerFunding.ParentName;
                            case "$.localAuthorityName":
                                return providerFunding.LocalAuthorityName;
                            case "$.localAuthorityNameOverride":
                                return componentConfiguration.LocalAuthorityNameOverride;
                            case "$.parliamentaryConstituencyName":
                                return providerFunding.ParliamentaryConstituencyName;
                            case "$.organisationName":
                                return providerFunding.OrganisationName;
                            case "$.organisationUkprn":
                                return providerFunding.OrganisationUkprn;
                            case "$.fundingValue.totalValue":
                                return providerFunding.TotalAmount;
                            case "$.providerUrn":
                                return providerFunding.ProviderUrn;
                            case "$.providerSubTypeValue":
                                return providerFunding.ProviderSubType;
                            case "$.estabNumber":
                                return providerFunding.OrganisationDfeNumber != null && providerFunding.ParentPrimaryIdentifier != null ?
                                    providerFunding.OrganisationDfeNumber.Substring(providerFunding.ParentPrimaryIdentifier.Length)
                                    : string.Empty;
                            case "$.inYearAcademyConversions":
                                return GetDaysOpenForInYearAcademyConversions(componentConfiguration);
                        }
                    }

                    if (componentConfiguration.Data is IFundingApiSearchFunding funding)
                    {
                        switch (selectorPath.Replace("$..", "$."))
                        {
                            case "$.notApplicableText":
                                return "N/A";
                            case "$.totalText":
                                return "Total";
                            case "$.totalRecoupable":
                                return "Total of all current recoupable academies";
                            case "$.country":
                                return "England";
                            case "$.groupName":
                                return funding.GroupName;
                            case "$.schoolType":
                                return funding.SchoolType;
                            case "$.governmentOfficeRegion":
                                return funding.GovernmentOfficeRegion;
                            case "$.fundingValue.totalValue":
                                return funding.TotalAmount;
                        }
                    }

                    var result = componentConfiguration?.FundingValueJObject?.SelectTokens(selectorPath)?.FirstOrDefault();
                    var resultValue = result?.Value<JToken>();

                    if (resultValue == null && returnZeroForNoValue)
                    {
                        return 0D;
                    }

                    return resultValue?.ToObject<object>();
                }

                if (simplifiedId.StartsWith("C"))
                {
                    var calculations = componentConfiguration.CalculationAndFundingLines.Calculations;

                    if (!calculations.ContainsKey(id))
                    {
                        return 0D;
                    }

                    return calculations[id].Value ?? noValueResult;
                }
                else if (simplifiedId.StartsWith("L"))
                {
                    var fundingLines = componentConfiguration.CalculationAndFundingLines.FundingLines;

                    if (!fundingLines.ContainsKey(id))
                    {
                        return 0D;
                    }

                    return fundingLines[id].Value ?? noValueResult;
                }
                else if (simplifiedId.StartsWith("D"))
                {
                    simplifiedId = simplifiedId.Substring(1);
                    if (!componentConfiguration.FundingDistributionPeriods.ContainsKey(simplifiedId))
                    {
                        return 0D;
                    }

                    componentConfiguration.FundingDistributionPeriods.TryGetValue(simplifiedId, out var result);

                    return result;
                }

                throw new Exception($"Selector {selectorPath} not matched");
            }
            catch (Exception exception)
            {
                throw new Exception($"Error evaluating selector path '{selectorPath}' - {exception.Message}", exception);
            }
        }

        /// <summary>
        /// Should the cell text be made uppercase?.
        /// </summary>
        /// <param name="group">Cell info.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <returns>True if it should be made uppercase, false if not.</returns>
        protected static bool IsDataValueUppercase(UiModelGroup group, UiModelDataset dataset)
        {
            if (dataset.GroupingType != null && (group.Overrides?.ContainsKey(dataset.GroupingType) ?? false))
            {
                if (group.Overrides[dataset.GroupingType].Datastyle?.Uppercase ?? false)
                {
                    return true;
                }
            }

            if (dataset.Id != null && (group.Overrides?.ContainsKey(dataset.Id) ?? false))
            {
                if (group.Overrides[dataset.Id].Datastyle?.Uppercase ?? false)
                {
                    return true;
                }
            }

            return group.Datastyle?.Uppercase ?? false;
        }

        /// <summary>
        /// Apply the where clause to the data.
        /// </summary>
        /// <param name="parentType">The type of the containing class.</param>
        /// <param name="whereClause">The where clause, in predefined format.</param>
        /// <param name="data">The data to look at.</param>
        /// <returns>The filtered data.</returns>
        protected static IEnumerable<IFundingApiSearchFunding> ApplyWhereClauseToFunding(
            Type parentType,
            string whereClause,
            IEnumerable<IFundingApiSearchFunding> data)
        {
            var subPartOrs = whereClause.Replace("(", string.Empty).Replace(")", string.Empty).Split(SubQueryOrCharacter);
            var returnList = new List<IFundingApiSearchFunding>();

            foreach (var subPartOr in subPartOrs)
            {
                var whereClauseParts = subPartOr.Split('=');
                var whereClauseKey = whereClauseParts[0];
                var whereClauseValue = whereClauseParts[1];

                var notEqualsTo = whereClauseKey.Contains("!");
                whereClauseKey = whereClauseKey.Replace("!", string.Empty);

                var whereClauseOperand = PropertyInfoHelper.GetProperty(parentType, whereClauseKey);

                returnList.AddRange(
                    notEqualsTo
                        ? data.Where(funding => whereClauseOperand.GetValue(funding, null)?.ToString() != whereClauseValue).ToList()
                        : data.Where(funding => whereClauseOperand.GetValue(funding, null)?.ToString() == whereClauseValue).ToList());
            }

            return returnList.GroupBy(item => item.Id).Select(items => items.First());
        }

        /// <summary>
        /// Apply the where clause to the data.
        /// </summary>
        /// <param name="parentType">The type of the containing class.</param>
        /// <param name="whereClause">The where clause, in predefined format.</param>
        /// <param name="data">The data to look at.</param>
        /// <returns>The filtered data.</returns>
        protected static IEnumerable<IFundingApiSearchProviderFunding> ApplyWhereClauseToProviderFunding(
            Type parentType,
            string whereClause,
            IEnumerable<IFundingApiSearchProviderFunding> data)
        {
            var subPartOrs = whereClause.Replace("(", string.Empty).Replace(")", string.Empty).Split(SubQueryOrCharacter);
            var returnList = new List<IFundingApiSearchProviderFunding>();

            foreach (var subPartOr in subPartOrs)
            {
                var whereClauseParts = subPartOr.Split('=');
                var whereClauseKey = whereClauseParts[0];
                var whereClauseValue = whereClauseParts[1];

                var notEqualsTo = whereClauseKey.Contains("!");
                whereClauseKey = whereClauseKey.Replace("!", string.Empty);

                var whereClauseOperand = PropertyInfoHelper.GetProperty(parentType, whereClauseKey);

                returnList.AddRange(
                    notEqualsTo
                        ? data.Where(providerFunding => whereClauseOperand.GetValue(providerFunding, null)?.ToString() != whereClauseValue).ToList()
                        : data.Where(providerFunding => whereClauseOperand.GetValue(providerFunding, null)?.ToString() == whereClauseValue).ToList());
            }

            return returnList.GroupBy(item => item.Id + item.ParentId).Select(items => items.First());
        }

        /// <summary>
        /// Get the Json Serialised property name of a type (its likely camel cased).
        /// </summary>
        /// <param name="parentPropertyType">The property type.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The Json name of the property.</returns>
        protected static string GetJsonSerializedPropertyName(Type parentPropertyType, string propertyName)
        {
            var propertyInfo = PropertyInfoHelper.GetProperty(parentPropertyType, propertyName);

            if (propertyInfo == null)
            {
                throw new Exception("Property does not exist");
            }

            return propertyInfo.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? propertyName;
        }

        /// <summary>
        /// Get the relevant header styles for the group.
        /// </summary>
        /// <param name="uiModelGroup">The data for the group.</param>
        /// <param name="classes">The classes available.</param>
        /// <returns>A list of styles that should be applied.</returns>
        protected List<UiModelStyle> GetHeaderStyles(UiModelGroup uiModelGroup, Dictionary<string, UiModelClass> classes)
        {
            var returnList = new List<UiModelStyle>();

            if (!string.IsNullOrEmpty(uiModelGroup.ClassName))
            {
                foreach (var className in uiModelGroup.ClassName.Split(' '))
                {
                    if (classes?.ContainsKey(className) == true && classes[className].Style != null)
                    {
                        returnList.Add(classes[className].Style);
                    }
                }
            }

            if (uiModelGroup.Style != null)
            {
                returnList.Add(uiModelGroup.Style);
            }

            return returnList;
        }

        /// <summary>
        /// Get the relevant datastyles for a group.
        /// </summary>
        /// <param name="rowId">The id of the row (applicable only for prepended and appended rows).</param>
        /// <param name="rowClasses">The classes attached to the row.</param>
        /// <param name="uiModelGroup">The data for the group.</param>
        /// <param name="classes">The classes available.</param>
        /// <param name="dataset">The dataset we are applying.</param>
        /// <returns>>A list of data styles that should be applied.</returns>
        protected List<UiModelStyle> GetDataStyles(string rowId, string rowClasses, UiModelGroup uiModelGroup, Dictionary<string, UiModelClass> classes, UiModelDataset dataset)
        {
            var returnList = new List<UiModelStyle>();

            // Specific row id override
            if (!string.IsNullOrEmpty(rowId)
                && uiModelGroup?.Overrides?.ContainsKey(rowId) == true
                && (uiModelGroup.Overrides[rowId].Datastyle != null || !string.IsNullOrEmpty(uiModelGroup.Overrides[rowId].ClassName)))
            {
                var overrideItem = uiModelGroup.Overrides[rowId];

                if (!string.IsNullOrEmpty(overrideItem.ClassName))
                {
                    foreach (var className in overrideItem.ClassName.Split(' '))
                    {
                        if (classes?.ContainsKey(className) == true
                            && classes?[className].Datastyle != null)
                        {
                            returnList.Add(classes[className].Datastyle);
                        }
                    }
                }
            }
            else
            {
                // Class name for the group
                if (!string.IsNullOrEmpty(uiModelGroup?.ClassName))
                {
                    foreach (var className in uiModelGroup.ClassName.Split(' '))
                    {
                        if (classes?.ContainsKey(className) == true && classes?[className].Datastyle != null)
                        {
                            returnList.Add(classes[className].Datastyle);
                        }
                    }
                }

                // Class name for the row
                if (!string.IsNullOrEmpty(rowClasses))
                {
                    foreach (var className in rowClasses.Split(' '))
                    {
                        if (classes?.ContainsKey(className) == true && classes?[className].Datastyle != null)
                        {
                            returnList.Add(classes[className].Datastyle);
                        }
                    }
                }

                // Group specific data style
                if (uiModelGroup?.Datastyle != null)
                {
                    returnList.Add(uiModelGroup.Datastyle);
                }

                // Styles for the dataset grouping type
                if (!string.IsNullOrEmpty(dataset?.GroupingType)
                    && uiModelGroup.Overrides?.ContainsKey(dataset.GroupingType) == true
                    && (uiModelGroup.Overrides[dataset.GroupingType].Datastyle != null || !string.IsNullOrEmpty(uiModelGroup.Overrides[dataset.GroupingType].ClassName)))
                {
                    var overrideItem = uiModelGroup.Overrides[dataset.GroupingType];

                    if (!string.IsNullOrEmpty(overrideItem.ClassName))
                    {
                        foreach (var className in overrideItem.ClassName.Split(' '))
                        {
                            if (classes?.ContainsKey(className) == true
                                && classes?[className].Datastyle != null)
                            {
                                returnList.Add(classes[className].Datastyle);
                            }
                        }
                    }

                    if (overrideItem.Datastyle != null)
                    {
                        returnList.Add(overrideItem.Datastyle);
                    }
                }

                // Styles for the dataset
                if (!string.IsNullOrEmpty(dataset?.Id)
                    && uiModelGroup.Overrides?.ContainsKey(dataset.Id) == true
                    && (uiModelGroup.Overrides[dataset.Id].Datastyle != null || !string.IsNullOrEmpty(uiModelGroup.Overrides[dataset.Id].ClassName)))
                {
                    var overrideX = uiModelGroup.Overrides[dataset.Id];

                    if (!string.IsNullOrEmpty(overrideX.ClassName))
                    {
                        foreach (var className in overrideX.ClassName.Split(' '))
                        {
                            if (classes?.ContainsKey(className) == true
                                && classes?[className].Datastyle != null)
                            {
                                returnList.Add(classes[className].Datastyle);
                            }
                        }
                    }

                    if (overrideX.Datastyle != null)
                    {
                        returnList.Add(overrideX.Datastyle);
                    }
                }
            }

            return returnList;
        }

        /// <summary>
        /// Get the data (rows) for the dataset.
        /// </summary>
        /// <param name="fundings">Fundings data to pass in.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <param name="additionalFundingStreams">Any additional funding streams to lookup.</param>
        /// <returns>The data rows for the dataset.</returns>
        protected List<IFundingApiSearchFunding> GetFundingDataForDataset(
            IEnumerable<IFundingApiSearchFunding> fundings,
            UiModelDataset dataset,
            UIModelAdditionalFundingStream[] additionalFundingStreams)
        {
            if (fundings == null)
            {
                return new List<IFundingApiSearchFunding>();
            }

            var fundingType = typeof(IFundingApiSearchFunding);
            var groupingTypes = !string.IsNullOrWhiteSpace(dataset.GroupingType) ? dataset.GroupingType.Split(',') : null;

            var relevantFundingsQuery = fundings.Where(funding => groupingTypes == null
                || groupingTypes.Contains(funding.GroupingType) || "all".Equals(dataset.GroupingType, StringComparison.InvariantCultureIgnoreCase));

            if (dataset.FundingStreamMappings?.Any() == true)
            {
                var fundingStreamCodes = additionalFundingStreams
                    .Where(afs => dataset.FundingStreamMappings.Contains(afs.Id))
                    .Select(afs => afs.Code);

                relevantFundingsQuery = relevantFundingsQuery
                    .Where(funding => fundingStreamCodes.Contains(funding.FundingStreamCode));
            }

            if (!string.IsNullOrWhiteSpace(dataset.Expression))
            {
                var orClauses = AdaptWhereClause(dataset.Expression).Split(OrCharacter);

                if (orClauses.Length > 1)
                {
                    var newResults = new List<IFundingApiSearchFunding>();

                    foreach (var whereClause in orClauses)
                    {
                        newResults.AddRange(ApplyWhereClauseToFunding(fundingType, whereClause, relevantFundingsQuery));
                    }

                    relevantFundingsQuery = newResults.GroupBy(item => item.Id).Select(items => items.First());
                }
                else
                {
                    var andClauses = AdaptWhereClause(dataset.Expression).Split(AndCharacter);

                    foreach (var whereClause in andClauses)
                    {
                        relevantFundingsQuery = ApplyWhereClauseToFunding(fundingType, whereClause, relevantFundingsQuery);
                    }
                }
            }

            var relevantFunding = relevantFundingsQuery.RefineBasedOnDataset(dataset, fundingType);

            return relevantFunding;
        }

        /// <summary>
        /// Get the data (rows) for the dataset.
        /// </summary>
        /// <param name="providerFunding">Fundings data to pass in.</param>
        /// <param name="dataset">A set of data (e.g. local authority rows).</param>
        /// <param name="additionalFundingStreams">Any additional funding streams to lookup.</param>
        /// <returns>The data rows for the dataset.</returns>
        protected List<IFundingApiSearchProviderFunding> GetProviderFundingDataForDataset(
            IEnumerable<IFundingApiSearchProviderFunding> providerFunding,
            UiModelDataset dataset,
            UIModelAdditionalFundingStream[] additionalFundingStreams)
        {
            var providerFundingType = typeof(IFundingApiSearchProviderFunding);

            var groupingTypes = !string.IsNullOrWhiteSpace(dataset.GroupingType) ? dataset.GroupingType.Split(',') : null;
            var parentGroupingTypes = !string.IsNullOrWhiteSpace(dataset.ParentGroupingType) ? dataset.ParentGroupingType.Split(',') : null;

            var relevantFundingsQuery = providerFunding.Where(funding =>
                (groupingTypes == null
                    || groupingTypes.Contains(funding.ProviderType)
                    || "all".Equals(dataset.GroupingType, StringComparison.InvariantCultureIgnoreCase)) && (parentGroupingTypes == null
                    || parentGroupingTypes.Contains(funding.ParentProviderType)
                    || "all".Equals(dataset.ParentGroupingType, StringComparison.InvariantCultureIgnoreCase)));

            if (dataset.FundingStreamMappings?.Any() == true)
            {
                var fundingStreamCodes = additionalFundingStreams
                    .Where(afs => dataset.FundingStreamMappings.Contains(afs.Id))
                    .Select(afs => afs.Code);

                relevantFundingsQuery = relevantFundingsQuery
                    .Where(funding =>
                        string.IsNullOrEmpty(funding.FundingStreamCode) ||
                        fundingStreamCodes.Contains(funding.FundingStreamCode));
            }

            if (!string.IsNullOrWhiteSpace(dataset.Expression))
            {
                var orClauses = AdaptWhereClause(dataset.Expression).Split(OrCharacter);

                if (orClauses.Length > 1)
                {
                    var newResults = new List<IFundingApiSearchProviderFunding>();

                    foreach (var whereClause in orClauses)
                    {
                        newResults.AddRange(ApplyWhereClauseToProviderFunding(providerFundingType, whereClause, relevantFundingsQuery));
                    }

                    relevantFundingsQuery = newResults.GroupBy(item => item.Id + item.ParentId).Select(items => items.First()).ToList();
                }
                else
                {
                    var andClauses = AdaptWhereClause(dataset.Expression).Split(AndCharacter);

                    foreach (var whereClause in andClauses)
                    {
                        relevantFundingsQuery = ApplyWhereClauseToProviderFunding(providerFundingType, whereClause, relevantFundingsQuery);
                    }
                }
            }

            if (dataset.Expressions != null && dataset.Expressions.Any())
            {
                var orClauses = !string.IsNullOrWhiteSpace(dataset.ExpressionSeparator) && dataset.ExpressionSeparator.Equals("|") ? true : false;
                var expression = ExpressionBuilder.GetExpression<IFundingApiSearchProviderFunding>(dataset.Expressions, orClauses);

                if (expression != null)
                {
                    var deleg = expression.Compile();
                    if (orClauses)
                    {
                        var newResults = new List<IFundingApiSearchProviderFunding>();
                        newResults.AddRange(relevantFundingsQuery.Where(deleg).ToList());
                        relevantFundingsQuery = newResults.GroupBy(item => item.Id + item.ParentId).Select(items => items.First()).ToList();
                    }
                    else
                    {
                        relevantFundingsQuery = relevantFundingsQuery.Where(deleg).ToList();
                    }
                }
            }

            var relevantProviderFunding = relevantFundingsQuery.RefineBasedOnDataset(dataset, providerFundingType);

            return relevantProviderFunding;
        }

        /// <summary>
        /// Get all datasets data.
        /// </summary>
        /// <param name="funding">Funding data to pass in.</param>
        /// <param name="providerFunding">Fundings data to pass in.</param>
        /// <param name="datasets">The dataset definition.</param>
        /// <param name="additionalFundingStreams">Any additional funding streams to lookup.</param>
        /// <param name="primaryFundingStreamCode">aaa.</param>
        /// <returns>The all datasets data.</returns>
        protected List<List<IFundingApiSearch>> GetAllDatasetsData(
            IEnumerable<IFundingApiSearchFunding> funding,
            IEnumerable<IFundingApiSearchProviderFunding> providerFunding,
            List<UiModelDataset> datasets,
            UIModelAdditionalFundingStream[] additionalFundingStreams,
            string primaryFundingStreamCode)
        {
            if (datasets == null)
            {
                return null;
            }

            if (additionalFundingStreams?.Any() == true)
            {
                var newFs = new List<UIModelAdditionalFundingStream>(additionalFundingStreams)
                {
                    new UIModelAdditionalFundingStream { Id = "Primary", Code = primaryFundingStreamCode }
                };

                additionalFundingStreams = newFs.ToArray();
            }

            var returnList = new List<List<IFundingApiSearch>>();

            foreach (var dataset in datasets)
            {
                if (dataset == null)
                {
                    return null;
                }

                if (dataset.DatasetName?.Equals("providerfunding", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    var filteredProviderFunding = GetProviderFundingDataForDataset(providerFunding, dataset, additionalFundingStreams);

                    returnList.Add(filteredProviderFunding.Select(providerFunding => (IFundingApiSearch)providerFunding).ToList());
                    continue;
                }

                var filteredFundings = GetFundingDataForDataset(funding, dataset, additionalFundingStreams);
                returnList.Add(filteredFundings.Select(funding => (IFundingApiSearch)funding).ToList());
            }

            return returnList;
        }

        /// <summary>
        /// DTO Class representing an item and whether or not it is an override.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        protected class OverrideMeta<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="OverrideMeta{T}"/> class.
            /// Construct a new instance of OverrideMeta.
            /// </summary>
            /// <param name="item">The item.</param>
            /// <param name="isOverride">Whether or not the item is an override.</param>
            public OverrideMeta(T item, bool isOverride)
            {
                Item = item;
                IsOverride = isOverride;
            }

            /// <summary>
            /// Gets or sets the item.
            /// </summary>
            public T Item { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether whether or not the item is an override.
            /// </summary>
            public bool IsOverride { get; set; }

            /// <summary>
            /// Determine whether or not the specified object is equal to the current object.
            /// </summary>
            /// <param name="obj">The object to compare.</param>
            /// <returns>True if the specified object is equal to the current object, otherwise false.</returns>
            public override bool Equals(object obj)
            {
                var compareObj = (OverrideMeta<T>)obj;
                return Item.Equals(compareObj.Item) && IsOverride == compareObj.IsOverride;
            }
        }

        private static int? GetClausePosition(string clause)
        {
            var clauseLevels = new Dictionary<int, int>();
            var currentLevel = 0;
            var currentPosition = 0;

            foreach (var character in clause)
            {
                var previousNoneWhitespaceCharacter = GetPrevNoneWhitespaceCharacter(currentPosition, clause);
                var nextCharacter = clause.Length > currentPosition + 1 ? clause[currentPosition + 1] : (char?)null;

                if (character == '(' || character == '[')
                {
                    currentLevel += 1;
                }
                else if (character == ')' || character == ']')
                {
                    currentLevel -= 1;
                }
                else if ((character == EqualsCharacter || character == GreaterThanCharacter || character == LessThanCharacter)
                    && (previousNoneWhitespaceCharacter != EqualsCharacter && nextCharacter != EqualsCharacter))
                {
                    clauseLevels[currentLevel] = currentPosition;
                }

                currentPosition += 1;
            }

            if (!clauseLevels.Keys.Any())
            {
                return null;
            }

            var lowestLevelKey = clauseLevels.Keys.OrderBy(k => k).First();
            return clauseLevels[lowestLevelKey];
        }

        private static void GetAllDatasets(UiModelGroup group, List<UiModelDataset> datasets)
        {
            if (group?.Dataset?.Any() == true)
            {
                datasets.AddRange(group.Dataset);
            }

            if (group?.Groups != null)
            {
                foreach (var subGroup in group.Groups)
                {
                    GetAllDatasets(subGroup, datasets);
                }
            }
        }

        private static string GetSearchTermParameter(string searchTerm, string prefixChar, bool viaChoicePage, string viaChoicePageParameter)
        {
            var returnString = !string.IsNullOrEmpty(searchTerm) ? $"{prefixChar}searchTerm={searchTerm}" : null;

            if (!viaChoicePage)
            {
                return returnString;
            }

            if (string.IsNullOrEmpty(returnString))
            {
                returnString = $"{prefixChar}{viaChoicePageParameter}";
            }
            else if (returnString.Contains("?"))
            {
                returnString += $"&{viaChoicePageParameter}";
            }
            else
            {
                returnString += $"{prefixChar}{viaChoicePageParameter}";
            }

            return returnString;
        }

        private static List<IEnumerable<object>> CheckForAndHandleChunkVariable(
            string inputString,
            Component component,
            bool errorIfVariablesRemaining = true,
            bool neverShowSelectors = false)
        {
            const string CHUNK_ = "CHUNK_";

            if (inputString?.StartsWith(CHUNK_) != true)
            {
                return null;
            }

            var withoutFirstPart = inputString.Replace(CHUNK_, string.Empty);
            var remainingParts = withoutFirstPart.Split('_');
            var firstPageSize = int.Parse(remainingParts[0]);
            var otherPagesSize = int.Parse(remainingParts[1]);
            var variableName = withoutFirstPart.Replace($"{firstPageSize}_{otherPagesSize}_", string.Empty);

            var variableResult = PerformRuntimeReplacements(variableName, component, errorIfVariablesRemaining, neverShowSelectors);

            if (!(variableResult is System.Collections.IEnumerable repeaterItems))
            {
                throw new Exception($"{CHUNK_} can only be used with an IEnumerable");
            }

            var list = repeaterItems.Cast<object>();

            var pages = list.Skip(firstPageSize).Batch(otherPagesSize)
                .Select(batch => (IEnumerable<object>)batch).ToList();
            pages.Insert(0, list.Take(firstPageSize).ToArray());

            return pages;
        }

        private static char? GetPrevNoneWhitespaceCharacter(int currentPosition, string clause)
        {
            const char SPACE = ' ';
            currentPosition -= 1;

            while (currentPosition >= 0)
            {
                var c = clause[currentPosition];

                if (c != SPACE)
                {
                    return c;
                }

                currentPosition -= 1;
            }

            return null;
        }

        private static string ReplaceVariableStrings(string leftHandReplaced, Dictionary<string, object> data)
        {
            if (data?.Keys?.Any() != true)
            {
                return leftHandReplaced;
            }

            var keys = data.Keys.ToList(); // Shallow copy so we dont get errors about updating the collection

            foreach (var key in keys)
            {
                var keyWithPrefix = $"@{key}";
                var altKeyWithPrefix = $"@({key}.";

                while (leftHandReplaced.Contains(keyWithPrefix) || leftHandReplaced.Contains(altKeyWithPrefix))
                {
                    var matchKey = leftHandReplaced.Contains(keyWithPrefix) ? keyWithPrefix : altKeyWithPrefix;
                    leftHandReplaced = ReplaceVariableString(leftHandReplaced, matchKey, key, data);
                }
            }

            return leftHandReplaced;
        }

        private static object GetValueFromNestedProperties(string[] propertiesStrings, object value)
        {
            foreach (var propertiesString in propertiesStrings)
            {
                if (value == null)
                {
                    continue;
                }

                var type = value.GetType();

                if (int.TryParse(propertiesString, out var index) && IsGenericList(value))
                {
                    var itemType = type.GetGenericArguments()[0];

                    if (itemType == typeof(IFundingApiSearch))
                    {
                        var list = (List<IFundingApiSearch>)value;

                        if (list.Count <= index)
                        {
                            return DataValueConstants.NullObjectValue;
                        }

                        value = list[index];
                        continue;
                    }
                    else if (itemType == typeof(IFundingApiSearch))
                    {
                        var list = (List<IFundingApiSearch>)value;

                        if (list.Count <= index)
                        {
                            return DataValueConstants.NullObjectValue;
                        }

                        value = list[index];
                        continue;
                    }

                    throw new NotImplementedException();
                }
                else if (value is ITuple tuple)
                {
                    var itemIndex = int.Parse(propertiesString.Replace("Item", string.Empty)) - 1;
                    value = tuple[itemIndex];

                    continue;
                }

                var property = PropertyInfoHelper.GetProperty(type, propertiesString);

                if (property == null)
                {
                    throw new KeyNotFoundException(propertiesString);
                }

                value = property.GetValue(value, null);
            }

            return value;
        }

        private static bool EvaluateAndOrOrToBool(List<object> clauses, bool isOrQuery, Component component)
        {
            var returnValue = true;
            var isAndQuery = !isOrQuery;

            foreach (var clause in clauses)
            {
                bool result;

                if (clause is EvaluatedExpressionComponent eClause)
                {
                    if (!eClause.Evaluated && eClause.LeftHandComponent is string leftHandString && leftHandString?.Contains("@") == true)
                    {
                        var replacedValue = PerformRuntimeReplacements(eClause.LeftHandComponent, component, neverShowSelectors: true);
                        eClause.LeftHandComponentEvaluatedResult = replacedValue;
                    }

                    eClause.ComparisonResult = CheckEquality(eClause);
                    result = eClause.ComparisonResult.Value;
                }
                else if (clause is bool resultBool)
                {
                    result = resultBool;
                }
                else
                {
                    throw new Exception($"Type {clause.GetType().Name} not understood in and/or statement");
                }

                if (isOrQuery && result)
                {
                    return true;
                }
                else if (isAndQuery && !result)
                {
                    return false;
                }

                returnValue = result;
            }

            return returnValue;
        }

        private static string ReplaceVariableString(
            string leftHandReplaced,
            string keyWithPrefix,
            string key,
            Dictionary<string, object> data)
        {
            var value = data[key];
            var hasPropertyLookup = keyWithPrefix.Contains(".");

            if (hasPropertyLookup)
            {
                var propertiesString = leftHandReplaced.Substring(leftHandReplaced.IndexOf(keyWithPrefix) + keyWithPrefix.Length).Split(')')[0];
                keyWithPrefix += propertiesString + ")";

                var propertiesStrings = propertiesString.Split('.');
                value = GetValueFromNestedProperties(propertiesStrings, value);
            }

            var longValue = leftHandReplaced.Contains(keyWithPrefix) && value is long ? (long)value : -1;

            if (leftHandReplaced.Contains($"{keyWithPrefix}--"))
            {
                leftHandReplaced = leftHandReplaced.Replace($"{keyWithPrefix}--", longValue--.ToString());
                data[key] = longValue;
            }
            else if (leftHandReplaced.Contains($"{keyWithPrefix}++"))
            {
                leftHandReplaced = leftHandReplaced.Replace($"{keyWithPrefix}++", longValue++.ToString());
                data[key] = longValue;
            }
            else if (leftHandReplaced.Contains($"--{keyWithPrefix}"))
            {
                leftHandReplaced = leftHandReplaced.Replace($"--{keyWithPrefix}", (--longValue).ToString());
                data[key] = longValue;
            }
            else if (leftHandReplaced.Contains($"++{keyWithPrefix}"))
            {
                leftHandReplaced = leftHandReplaced.Replace($"++{keyWithPrefix}", (++longValue).ToString());
                data[key] = longValue;
            }
            else
            {
                leftHandReplaced = leftHandReplaced.Replace(keyWithPrefix, value?.ToString());
            }

            return leftHandReplaced;
        }

        private static UiModelGroup GetGroupingTypeOverrideGroup(UiModelGroup uiModelGroup, UiModelDataset dataset)
        {
            if (!string.IsNullOrEmpty(dataset.GroupingType)
                && (uiModelGroup?.Overrides?.ContainsKey(dataset.GroupingType) ?? false))
            {
                return uiModelGroup.Overrides[dataset.GroupingType];
            }

            return null;
        }

        private static UiModelGroup GetDatasetIdOverrideGroup(UiModelGroup uiModelGroup, UiModelDataset dataset)
        {
            if (dataset.Id != null
                && (uiModelGroup?.Overrides?.ContainsKey(dataset.Id) ?? false))
            {
                return uiModelGroup.Overrides[dataset.Id];
            }

            return null;
        }

        private static bool IsANumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal
                    || (value is string valueString && double.TryParse(valueString, out _));
        }

        private static string AdaptWhereClause(string whereClause)
        {
            var bracketsLevel = 0;
            var sb = new StringBuilder();

            foreach (var c in whereClause)
            {
                if (c == '(')
                {
                    bracketsLevel++;
                }
                else if (c == ')')
                {
                    bracketsLevel--;
                }

                if (bracketsLevel > 0 && c == OrCharacter)
                {
                    sb.Append(SubQueryOrCharacter);
                    continue;
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        private static object GetDataSetCount(ComponentConfiguration componentConfiguration, string selectorPath)
        {
            int.TryParse(
                selectorPath
                    .Replace(DatasetCountPrefix, string.Empty)
                    .Replace(DatasetCountSuffix, string.Empty),
                out var dataSetOneBasedIndex);

            if (dataSetOneBasedIndex > 0 &&
                componentConfiguration.AllDatasetsData.Count >= dataSetOneBasedIndex)
            {
                var dataSetData = componentConfiguration.AllDatasetsData[dataSetOneBasedIndex - 1];

                return dataSetData.Count;
            }

            return 0D;
        }

        private static object GetDaysOpenForInYearAcademyConversions(ComponentConfiguration componentConfiguration)
        {
            var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(componentConfiguration.FundingPeriodCode);
            var dateFrom = new DateTime(yearFrom, 4, 2);
            var dateTo = new DateTime(yearTo, 3, 31);
            var isInYearAcademyConversions = componentConfiguration?.ProviderOpenDate >= dateFrom && componentConfiguration?.ProviderOpenDate <= dateTo;
            var daysOpen = ExtractCalculationValue(componentConfiguration, TemplateCalculationId.DaysOpenForInYearAacademyConversions);

            return isInYearAcademyConversions && daysOpen.HasValue ? daysOpen.Value.ToString() : string.Empty;
        }

        private static int? ExtractCalculationValue(ComponentConfiguration componentConfiguration, int templateCalculationId)
        {
            var calcValue = componentConfiguration?.CalculationAndFundingLines?.Calculations?
                .FirstOrDefault(c => c.Key == templateCalculationId).Value?.Value;
            return calcValue != null ? Convert.ToInt32(calcValue) : (int?)null;
        }
    }
}