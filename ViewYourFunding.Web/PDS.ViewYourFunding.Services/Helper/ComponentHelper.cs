using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Methods to help with components.
    /// </summary>
    public static class ComponentHelper
    {
        /// <summary>
        /// The logger service.
        /// </summary>
        public static ILoggerAdapter<Component> LoggerService;

        /// <summary>
        /// Application configuration settings.
        /// </summary>
        public static ApplicationConfiguration ApplicationConfiguration;

        private const string FundingValueContextSuffix = ".FundingValue)";
        private const string ContextSchemaVersionSuffix = ".SchemaVersion)";

        private static readonly ConcurrentDictionary<string, string> _componentTypeLookups
            = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// The OAT environment.
        /// </summary>
        private const string OatEnvironment = "Oat";

        private static readonly Regex NumberRegex = new Regex(@"^-?([0-9]|[0-9][0-9,\.]+)([E][+-]{1}[0-9]+)?$", RegexOptions.Compiled);

        private static List<ComponentType> _errorTypes = null;
        private static ConcurrentDictionary<ComponentType, string> _filenames = new ConcurrentDictionary<ComponentType, string>();

        /// <summary>
        /// Get the conditional level.
        /// </summary>
        /// <param name="start">The group we want the parent for.</param>
        /// <param name="topLevel">The parent group to look under.</param>
        /// <param name="type">The type to look for.</param>
        /// <returns>0 if none of the parents are 'Conditional', or a number higher if they are.</returns>
        public static int GetLevel(UiModelGroup start, UiModelGroup topLevel, string type)
        {
            var returnNumber = 0;
            var parent = GetGroupParent(start, topLevel);

            while (parent != null)
            {
                if (parent.Type == type)
                {
                    returnNumber += 1;
                }

                parent = GetGroupParent(parent, topLevel);
            }

            return returnNumber;
        }

        /// <summary>
        /// Get the visibility conditional level.
        /// </summary>
        /// <param name="start">The group we want the parent for.</param>
        /// <param name="topLevel">The parent group to look under.</param>
        /// <returns>0 if none of the parents are 'Conditional', or a number higher if they are.</returns>
        public static int GetVisibilityConditionLevel(UiModelGroup start, UiModelGroup topLevel)
        {
            var returnNumber = 0;
            var parent = GetGroupParent(start, topLevel);

            while (parent != null)
            {
                if (!string.IsNullOrEmpty(parent.VisibilityCondition))
                {
                    returnNumber += 1;
                }

                parent = GetGroupParent(parent, topLevel);
            }

            return returnNumber;
        }

        /// <summary>
        /// Get the parent of a group.
        /// </summary>
        /// <param name="groupToLookFor">The group we want the parent for.</param>
        /// <param name="parent">The parent group to look under.</param>
        /// <returns>A parent group or null.</returns>
        public static UiModelGroup GetGroupParent(UiModelGroup groupToLookFor, UiModelGroup parent)
        {
            if (parent?.Groups == null)
            {
                return null;
            }

            foreach (var child in parent.Groups)
            {
                if (groupToLookFor == child)
                {
                    return parent;
                }

                var recursiveItem = GetGroupParent(groupToLookFor, child);

                if (recursiveItem != null)
                {
                    return recursiveItem;
                }
            }

            return null;
        }

        /// <summary>
        /// Return true if either OAT or Live - false if any other environment.
        /// </summary>
        /// <returns>True if either OAT or Live - false if any other environment.</returns>
        public static bool IsEnvironmentReleaseOrPreProd()
        {
            return ApplicationConfiguration?.IsProductionEnvironment == true
                || OatEnvironment.Equals(ApplicationConfiguration?.Environment, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Return MS Clarity Id.
        /// </summary>
        /// <returns>MS Clarity Id.</returns>
        public static string GetMSClarityId()
        {
            return ApplicationConfiguration?.MSClarityId ?? "MSClarityTestId";
        }

        /// <summary>
        /// Get the instance of the component for this HTTP request (and then increase it).
        /// </summary>
        /// <param name="componentName">The name of the component.</param>
        /// <param name="context">The current HTTPContext.</param>
        /// <returns>An integer (0 or greater).</returns>
        public static int GetAndIncreaseInstanceNumber(string componentName, HttpContext context)
        {
            const int StartNumber = 1;
            var items = context?.Items;

            if (items?.ContainsKey(componentName) == true
                && items.TryGetValue(componentName, out var instanceNumberObj)
                && instanceNumberObj is int instanceNumber)
            {
                context.Items[componentName] = instanceNumber + 1;
                return instanceNumber + 1;
            }
            else if (items?.ContainsKey(componentName) == true)
            {
                // Must be corrupted (null?) so reset it
                items[componentName] = StartNumber;
            }
            else
            {
                items.Add(componentName, StartNumber);
            }

            return StartNumber;
        }

        /// <summary>
        /// Render a component.
        /// </summary>
        /// <typeparam name="T">The type of the MVC htm helper.</typeparam>
        /// <param name="html">A MVC html helper.</param>
        /// <param name="type">The type of component (e.g. General_Literal).</param>
        /// <param name="model">The component to render.</param>
        /// <param name="data">Data to be passed in (late bound stuff)..</param>
        /// <returns>An awaitable task to be used in a view.</returns>
        public static async Task Render<T>(IHtmlHelper<T> html, ComponentType type, Component model, Dictionary<string, object> data)
        {
            if (type == ComponentType.NotSet)
            {
                LoggerService?.LogError($"Model type not set");

                await Render(html, ComponentType.Error_ModelNotSet, new Component(null), data);
                return;
            }

            if (model == null)
            {
                LoggerService?.LogError($"Model not found (likely due to JSON error) for {type}");

                await Render(html, ComponentType.Error_ModelError, new Component(null) { Type = type }, data);
                return;
            }

            model.PageData = data;

            if (!model.RenderItem)
            {
                return;
            }

            if (model.Context is string contextString)
            {
                model.Context =
                    BaseViewYourFundingRenderer.PerformRuntimeReplacements(
                        contextString,
                        model,
                        neverShowSelectors: true);

                model.ContextSchemaVersion =
                    BaseViewYourFundingRenderer.PerformRuntimeReplacements(
                        Convert.ToString(model.OriginalGroup.Context)
                            .Replace(FundingValueContextSuffix, ContextSchemaVersionSuffix),
                        model,
                        neverShowSelectors: true);
                model.MarkValuesUnevaluated();
            }

            try
            {
                HideDataIfEnabled(model, type);
                ShowSelectorsIfEnabledAndApplicable(model, type);
                await ShowBeforeStatementSpecPartsIfEnabledAndApplicable(model, type, html);

                if (await HideIfSpecifiedForStatement(model, html))
                {
                    return;
                }

                var filename = GetFilename(type);
                await html.RenderPartialAsync($"/Views/Components/{filename}.cshtml", model);

                await ShowAfterStatementSpecPartsIfEnabledAndApplicable(model, type, html);
            }
            catch (InvalidOperationException ioeException) when (ioeException.Source == "Microsoft.AspNetCore.Mvc.ViewFeatures")
            {
                LoggerService?.LogError(ioeException, ioeException.Message);

                await Render(html, ComponentType.Error_ComponentNotFound, model, data);
            }
            catch (Exception exception)
            {
                LoggerService?.LogError(exception, exception.Message);

                model.Values = new List<object>
                {
                    exception
                };

                await Render(html, ComponentType.Error_Exception, model, data);
            }
        }

        /// <summary>
        /// Render this component (extension method).
        /// </summary>
        /// <typeparam name="T">The type of the MVC htm helper.</typeparam>
        /// <param name="component">The component to render.</param>
        /// <param name="html">A MVC html helper.</param>
        /// <param name="data">Data to be passed in (late bound stuff)..</param>
        /// <returns>An awaitable task to be used in a view.</returns>
        public static async Task Render<T>(this Component component, IHtmlHelper<T> html, Dictionary<string, object> data)
        {
            var type = component?.Type ?? ComponentType.Error_ModelNotFound;
            await Render(html, type, component, data);
        }

        /// <summary>
        /// Renders component (extension method).
        /// </summary>
        /// <typeparam name="T">The type of the MVC htm helper.</typeparam>
        /// <param name="components">The components to render.</param>
        /// <param name="html">An MVC html helper.</param>
        /// <returns>An awaitable task to be used in a view.</returns>
        public static async Task Render<T>(this List<Component> components, IHtmlHelper<T> html)
        {
            var firstComponent = components?.FirstOrDefault();
            var configuration = firstComponent?.ComponentConfiguration;

            var sectionModel = new Component(configuration)
            {
                Type = ComponentType.General_Section,
                Components = components
            };

            await sectionModel.Render(html, firstComponent?.PageData);

            if (configuration.ShowStatementSpecification)
            {
                await html.RenderPartialAsync($"/Views/Components/Reporting/_StatementFooter.cshtml", firstComponent);
            }
        }

        /// <summary>
        /// Disable using default classes for descendants.
        /// </summary>
        /// <param name="component">The component to apply to.</param>
        public static void DisableDefaultClassesAllDescendants(this Component component)
        {
            if (component == null)
            {
                return;
            }

            component.UseDefaultClasses = false;

            if (component.Components == null)
            {
                return;
            }

            foreach (var subComponent in component.Components)
            {
                subComponent?.DisableDefaultClassesAllDescendants();
            }
        }

        /// <summary>
        /// Generates a bit of text to add to explain selectors.
        /// </summary>
        /// <param name="value">The simplified selector.</param>
        /// <param name="model">The component.</param>
        /// <returns>A bit of text to add to explain selectors.</returns>
        public static string GetSelectorExplanation(string value, Component model)
        {
            var isHtml = model.ComponentConfiguration.IsHtml;
            var simplifiedString = " (";

            if (isHtml)
            {
                simplifiedString += "<i><strong>";
            }

            simplifiedString += value;

            if (model.OriginalGroup?.Context is string contextString
                && !string.IsNullOrEmpty(contextString)
                && contextString.Length < 100)
            {
                simplifiedString += $") ({ReplaceAtIfNeeded(contextString, isHtml)}";
            }

            if (model.OriginalGroup?.Formula?.Contexts?.Any() == true)
            {
                simplifiedString += $") ({string.Join(", ", model.OriginalGroup?.Formula?.Contexts.Select(c => ReplaceAtIfNeeded(c, isHtml)).ToArray())}";
            }

            if (isHtml)
            {
                simplifiedString += "</strong></i>";
            }

            simplifiedString += ")";
            return simplifiedString;
        }

        /// <summary>
        /// Get the current accordion instance number.
        /// </summary>
        /// <param name="component">The component to render.</param>
        /// <returns>The current accordion instance number.</returns>
        public static int GetAccordionInstanceNumber(this Component component)
        {
            var key = "AccordionInstanceNumber";
            var instanceNumberObj = component.PageData?.ContainsKey(key) == true ? component.PageData[key] : null;

            return instanceNumberObj is int instanceNumber ? instanceNumber : 1;
        }

        /// <summary>
        /// Clears the title and values.
        /// </summary>
        /// <param name="component">The component to render.</param>
        public static void ClearTitleAndValues(this Component component)
        {
            component.Values?.Clear();
            component.UnevaluatedValues?.Clear();

            component.Title = null;
            component.UnevaluatedTitle = null;
        }

        /// <summary>
        /// Get either the title or a value.
        /// </summary>
        /// <param name="component">The component to render.</param>
        /// <returns>Either the title or the first value.</returns>
        public static string GetTitleOrValue(this Component component)
        {
            return HtmlSafe(!string.IsNullOrEmpty(component.Title) ? component.Title : component.GetValueString());
        }

        /// <summary>
        /// Get the first value.
        /// </summary>
        /// <param name="component">The component to render.</param>
        /// <returns>The value.</returns>
        public static object GetValue(this Component component)
        {
            return component?.Values?.Any() == true ? component.Values.First() : null;
        }

        /// <summary>
        /// Get the first value as a string.
        /// </summary>
        /// <param name="component">The component to render.</param>
        /// <param name="defaultValue">The default value if no value is present.</param>
        /// <returns>The value as a string, or the default.</returns>
        public static string GetValueString(this Component component, string defaultValue = "")
        {
            return GetValue(component)?.ToString() ?? defaultValue;
        }

        /// <summary>
        /// Clone a component.
        /// </summary>
        /// <param name="component">The component to render.</param>
        /// <param name="properties">Properties to set.</param>
        /// <returns>A cloned component (deep copy).</returns>
        public static Component Clone(this Component component, Action<Component> properties)
        {
            var clonedComponent = component.Clone(false);
            properties(clonedComponent);

            return clonedComponent;
        }

        /// <summary>
        /// Clone a component.
        /// </summary>
        /// <param name="component">The component to render.</param>
        /// <param name="deepClone">Clone the sub levels too.</param>
        /// <returns>A cloned component (deep copy).</returns>
        public static Component Clone(this Component component, bool deepClone = false)
        {
            var subComponents = deepClone ? component.Components.Select(loopComponent => loopComponent.Clone(deepClone)).ToList()
                : component.Components.ToList();

            return new Component(component.ComponentConfiguration)
            {
                Type = component.Type,
                SubType = component.SubType,
                Title = component.UnevaluatedTitle,
                ClassNames = component.UnevaluatedClassNames?.ToList(),
                Components = subComponents,
                Values = component.UnevaluatedValues?.ToList(),
                Attributes = component.Attributes.ToDictionary(entry => entry.Key, entry => entry.Value),
                Variables = component.Variables?.ToDictionary(entry => entry.Key, entry => entry.Value),
                PageData = component.PageData?.ToDictionary(entry => entry.Key, entry => entry.Value),
                VisuallyHidden = component.VisuallyHidden,
                Id = component.Id,
                UseDefaultClasses = component.UseDefaultClasses,
                VisibilityCondition = component.VisibilityCondition,
                EnableVariance = component.EnableVariance,
                VarianceValue = component.VarianceValue,
                VarianceValueFormatted = component.VarianceValueFormatted,
                HideVarianceValue = component.HideVarianceValue,
                OriginalGroup = component.OriginalGroup,
                Context = component.Context
            };
        }

        /// <summary>
        /// Simplify a selector path to a few characters.
        /// </summary>
        /// <param name="inputString">A selector path.</param>
        /// <param name="isHtml">Whether the format is html or not.</param>
        /// <returns>A simpliefied selector string (e.g. C129 or L432).</returns>
        public static string SimplifySelector(string inputString, bool isHtml)
        {
            if (inputString == null)
            {
                return null;
            }

            var returnString = inputString
                .Replace(")].value", string.Empty)
                .Replace(" == ", string.Empty)
                .Replace("==", string.Empty)
                .Replace("$..fundingLines..[?(@.templateLineId", "L")
                .Replace("$..calculations..[?(@.templateCalculationId", "C")
                .Replace("$..fundingLines[?(@.templateLineId", "L")
                .Replace("$..calculations[?(@.templateCalculationId", "C");

            returnString = ReplaceAtIfNeeded(returnString, isHtml);

            returnString = int.TryParse(returnString.Substring(1), out _) ? returnString : inputString;
            returnString = returnString.Replace("@", "&#64;");

            return returnString;
        }

        /// <summary>
        /// Convert a JObect to a collection containing de-duplicated funding lines and calculations.
        /// </summary>
        /// <param name="inputObject">An input dictionary.</param>
        /// <returns>A collection containing de-duplicated funding lines and calculations.</returns>
        public static CalculationAndFundingLinesCollection ConvertJObjectToCalculationsAndFundingLinesCollection(
            IFundingValueNested inputObject)
        {
            var returnItem = new CalculationAndFundingLinesCollection
            {
                Calculations = new Dictionary<int, CalculationNoNesting>(),
                FundingLines = new Dictionary<int, FundingLineNoNesting>()
            };

            double? totalValue = null;

            if (inputObject.TotalValue != null)
            {
                totalValue = inputObject.TotalValue;
            }

            if (inputObject is FundingValueNested_1_0 fundingValue1_0)
            {
                if (fundingValue1_0.FundingValue != null)
                {
                    fundingValue1_0 = fundingValue1_0.FundingValue;

                    if (totalValue == null && fundingValue1_0.TotalValue != null)
                    {
                        totalValue = fundingValue1_0.TotalValue;
                    }
                }

                if (fundingValue1_0.FundingTemplate != null)
                {
                    fundingValue1_0 = fundingValue1_0.FundingTemplate;

                    if (totalValue == null && fundingValue1_0.TotalValue != null)
                    {
                        totalValue = fundingValue1_0.TotalValue;
                    }
                }

                returnItem.TotalValue = totalValue;

                if (fundingValue1_0.FundingLines == null)
                {
                    return returnItem;
                }

                fundingValue1_0.FundingLines.ToList().ForEach(fundingLine =>
                    UpsertFundingLineIfNotAdded(fundingLine, returnItem.FundingLines, returnItem.Calculations));

                if (fundingValue1_0.Calculations != null)
                {
                    fundingValue1_0.Calculations.ToList().ForEach(calculation =>
                        UpsertCalculationIfNotAdded(calculation, returnItem.Calculations));
                }

                return returnItem;
            }

            if (inputObject is FundingValueNested_1_1 fundingValue1_1)
            {
                if (fundingValue1_1.FundingValue != null)
                {
                    fundingValue1_1 = fundingValue1_1.FundingValue;

                    if (totalValue == null && fundingValue1_1.TotalValue != null)
                    {
                        totalValue = fundingValue1_1.TotalValue;
                    }
                }

                if (fundingValue1_1.FundingTemplate != null)
                {
                    fundingValue1_1 = fundingValue1_1.FundingTemplate;

                    if (totalValue == null && fundingValue1_1.TotalValue != null)
                    {
                        totalValue = fundingValue1_1.TotalValue;
                    }
                }

                returnItem.TotalValue = totalValue;

                if (fundingValue1_1.Calculations != null)
                {
                    foreach (var calculation in fundingValue1_1.Calculations)
                    {
                        returnItem.Calculations.Add(calculation.Value.TemplateCalculationId, calculation.Value);
                    }
                }

                if (fundingValue1_1.FundingLines != null)
                {
                    foreach (var fundingLine in fundingValue1_1.FundingLines)
                    {
                        returnItem.FundingLines.Add(fundingLine.Value.TemplateLineId, fundingLine.Value);
                    }
                }

                return returnItem;
            }

            if (inputObject is FundingValueNested_1_2 fundingValue1_2)
            {
                if (fundingValue1_2.FundingValue != null)
                {
                    fundingValue1_2 = fundingValue1_2.FundingValue;

                    if (totalValue == null && fundingValue1_2.TotalValue != null)
                    {
                        totalValue = fundingValue1_2.TotalValue;
                    }
                }

                if (fundingValue1_2.FundingTemplate != null)
                {
                    fundingValue1_2 = fundingValue1_2.FundingTemplate;

                    if (totalValue == null && fundingValue1_2.TotalValue != null)
                    {
                        totalValue = fundingValue1_2.TotalValue;
                    }
                }

                returnItem.TotalValue = totalValue;

                if (fundingValue1_2.Calculations != null)
                {
                    foreach (var calculation in fundingValue1_2.Calculations)
                    {
                        if (!returnItem.Calculations.ContainsKey(calculation.TemplateCalculationId))
                        {
                            returnItem.Calculations.Add(calculation.TemplateCalculationId, calculation);
                        }
                    }
                }

                if (fundingValue1_2.FundingLines != null)
                {
                    foreach (var fundingLine in fundingValue1_2.FundingLines)
                    {
                        if (!returnItem.Calculations.ContainsKey(fundingLine.TemplateLineId))
                        {
                            returnItem.FundingLines.Add(fundingLine.TemplateLineId, fundingLine);
                        }
                    }
                }

                return returnItem;
            }

            throw new Exception($"{inputObject.GetType().Name} not yet supported");
        }

        /// <summary>
        /// Is the passed object a number.
        /// </summary>
        /// <param name="value">An object (usually number or string).</param>
        /// <param name="includeStringImplicit">If true, then strings that look like numbers count as numbers - if false we don't count them.</param>
        /// <returns>True if it is a number, false if not.</returns>
        public static bool IsNumber(object value, bool includeStringImplicit = true)
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
                || (includeStringImplicit && value is string && NumberRegex.IsMatch((string)value));
        }

        /// <summary>
        /// Is the component type an error type.
        /// </summary>
        /// <param name="type">The component type.</param>
        /// <returns>True if an error type, false if not.</returns>
        public static bool IsErrorType(ComponentType? type)
        {
            var errorTypes = GetErrorTypes();
            return type != null && errorTypes.Contains(type.Value);
        }

        private static string ReplaceAtIfNeeded(string c, bool isHtml)
        {
            return isHtml ? c.Replace("@", "&#64;") : c;
        }

        private static void UpsertFundingLineIfNotAdded(
            FundingLine line,
            Dictionary<int, FundingLineNoNesting> lineDictionary,
            Dictionary<int, CalculationNoNesting> calculationDictionary)
        {
            if (line.Calculations != null)
            {
                foreach (var subCalc in line.Calculations)
                {
                    UpsertCalculationIfNotAdded(subCalc, calculationDictionary);
                }
            }

            if (line.FundingLines != null)
            {
                foreach (var subLine in line.FundingLines)
                {
                    UpsertFundingLineIfNotAdded(subLine, lineDictionary, calculationDictionary);
                }
            }

            if (!lineDictionary.ContainsKey(line.TemplateLineId))
            {
                lineDictionary.Add(line.TemplateLineId, line);
                return;
            }
            else if (line.Value == null)
            {
                return;
            }

            var existingLine = lineDictionary[line.TemplateLineId];

            if (existingLine.Value != null)
            {
                return;
            }

            existingLine.Value = line.Value;
        }

        private static void UpsertCalculationIfNotAdded(Calculation calculation, Dictionary<int, CalculationNoNesting> dictionary)
        {
            if (calculation.Calculations != null)
            {
                foreach (var subCalc in calculation.Calculations)
                {
                    UpsertCalculationIfNotAdded(subCalc, dictionary);
                }
            }

            if (!dictionary.ContainsKey(calculation.TemplateCalculationId))
            {
                dictionary.Add(calculation.TemplateCalculationId, calculation);
                return;
            }
            else if (calculation.Value == null)
            {
                return;
            }

            var existingCalculation = dictionary[calculation.TemplateCalculationId];

            if (existingCalculation.Value != null)
            {
                return;
            }

            existingCalculation.Value = calculation.Value;
        }

        private static async Task ShowBeforeStatementSpecPartsIfEnabledAndApplicable<T>(Component model, ComponentType type, IHtmlHelper<T> html)
        {
            if (IgnoreForReportingTools(model, type) || model?.ComponentConfiguration.ShowStatementSpecification != true)
            {
                return;
            }

            var firstValue = model.Values?.FirstOrDefault();
            var firstValueString = firstValue?.ToString();

            var numberFormat = model.OriginalGroup?.Datastyle?.NumberFormat;

            if (!string.IsNullOrEmpty(numberFormat))
            {
                var addition = "<br><br><strong><i>'" + model.OriginalGroup?.Datastyle?.NumberFormat + "'</i></strong>";

                if (firstValue != null && !BaseViewYourFundingRenderer.IsGenericList(firstValue) && !firstValueString.Contains(addition))
                {
                    model.Values[0] = firstValue + addition;
                }
            }

            if (!string.IsNullOrEmpty(model.VisibilityCondition))
            {
                await html.RenderPartialAsync($"/Views/Components/Reporting/_VisibilityConditionAbove.cshtml", model);
            }
        }

        private static async Task ShowAfterStatementSpecPartsIfEnabledAndApplicable<T>(Component model, ComponentType type, IHtmlHelper<T> html)
        {
            if (IgnoreForReportingTools(model, type) || model?.ComponentConfiguration.ShowStatementSpecification != true)
            {
                return;
            }

            if (!string.IsNullOrEmpty(model.VisibilityCondition))
            {
                await html.RenderPartialAsync($"/Views/Components/Reporting/_VisibilityConditionBelow.cshtml", model);
            }
        }

        private static async Task<bool> HideIfSpecifiedForStatement<T>(Component model, IHtmlHelper<T> html)
        {
            if (model?.ComponentConfiguration?.ShowStatementSpecification != true
                || model?.OriginalGroup?.DontShowStatementSpecification != true)
            {
                return false;
            }

            await html.RenderPartialAsync($"/Views/Components/General/RawLiteral.cshtml", new Component(null)
            {
                Title = "<span style='background-color: #444; color: #FFF'>"
                    + System.Net.WebUtility.HtmlEncode(model?.GetTitleOrValue()) + "</span>"
            });

            return true;
        }

        private static void HideDataIfEnabled(Component model, ComponentType type)
        {
            if (IgnoreForReportingTools(model, type) || model?.ComponentConfiguration.ShowData != false)
            {
                return;
            }

            if (model.Values == null || model.Values.Count == 0)
            {
                return;
            }

            model.Values[0] = string.Empty;
        }

        private static void ShowSelectorsIfEnabledAndApplicable(Component model, ComponentType type)
        {
            if (IgnoreForReportingTools(model, type) || model?.ComponentConfiguration.ShowSelectors != true)
            {
                return;
            }

            var originalSelector = model?.OriginalGroup?.Selector;
            var originalExpression = model?.OriginalGroup?.Expression ?? model?.OriginalGroup?.Formula?.Expression;

            if (string.IsNullOrEmpty(originalSelector) && string.IsNullOrEmpty(originalExpression))
            {
                return;
            }

            var isHtml = model?.ComponentConfiguration?.IsHtml == true;

            if (!string.IsNullOrEmpty(originalExpression) && originalExpression.Contains("{"))
            {
                originalExpression = string.Format(
                    originalExpression,
                    model?.OriginalGroup?.Formula?.Selectors.Select(s => SimplifySelector(s, isHtml)).ToArray());
            }

            var firstValue = model.Values?.FirstOrDefault();
            var firstValueString = firstValue?.ToString();

            var value = originalSelector != null ?
                SimplifySelector(originalSelector, isHtml) :
                SimplifySelector(originalExpression, isHtml);

            var simpliedString = GetSelectorExplanation(value, model);

            if (firstValue != null && !BaseViewYourFundingRenderer.IsGenericList(firstValue) && !firstValueString.Contains(simpliedString))
            {
                model.Values[0] = firstValueString + simpliedString;
            }
        }

        private static bool IgnoreForReportingTools(Component model, ComponentType type)
        {
            if (model?.ComponentConfiguration == null)
            {
                return true;
            }

            if (type != model.Type)
            {
                // Clone technique used for certain child types.
                return true;
            }

            if (IsErrorType(type))
            {
                return true;
            }

            if (model?.Type == ComponentType.Conditional_Condition
                || model?.Type == ComponentType.Conditional_Switch
                || model?.Type == ComponentType.General_Repeater)
            {
                return true;
            }

            if (model?.UnevaluatedTitle?.Contains("Exception") == true)
            {
                return true;
            }

            return false;
        }

        private static string HtmlSafe(string inputString)
        {
            return inputString.Replace("£", "&pound;");
        }

        private static string GetFilename(ComponentType type)
        {
            var key = $"GetFilename{type}";

            if (_componentTypeLookups.ContainsKey(key))
            {
                return _componentTypeLookups[key];
            }

            var componentType = GetFilenameUsingReflection(type);

            if (!_componentTypeLookups.ContainsKey(key))
            {
                _componentTypeLookups.TryAdd(key, componentType);
            }

            return componentType;
        }

        private static string GetFilenameUsingReflection(ComponentType type)
        {
            if (_filenames.ContainsKey(type))
            {
                return _filenames[type];
            }

            var typeString = type.ToString();
            var enumType = typeof(ComponentType);
            var allMembers = enumType.GetMembers();

            var valueAttributes = GetDefaultValueAttribute(enumType, allMembers, typeString);

            if (valueAttributes?.Length > 0)
            {
                return valueAttributes?.FirstOrDefault()?.Value?.ToString();
            }

            var filename = typeString.Replace("_", "/");

            if (!_filenames.ContainsKey(type))
            {
                _filenames.TryAdd(type, filename);
            }

            return filename;
        }

        private static List<ComponentType> GetErrorTypes()
        {
            if (_errorTypes != null)
            {
                return _errorTypes;
            }

            var componentTypeType = typeof(ComponentType);
            var allMembers = componentTypeType.GetMembers();

            var returnList = new List<ComponentType>();

            foreach (var componentType in (ComponentType[])Enum.GetValues(typeof(ComponentType)))
            {
                if (!HasErrorTypeAttribute(componentTypeType, allMembers, componentType.ToString()))
                {
                    continue;
                }

                returnList.Add(componentType);
            }

            _errorTypes = returnList;
            return _errorTypes;
        }

        private static bool HasErrorTypeAttribute(Type enumType, MemberInfo[] allMembers, string typeString)
        {
            var enumValueMemberInfo = allMembers.FirstOrDefault(m => m.Name == typeString && m.DeclaringType == enumType);
            var customAttributes = enumValueMemberInfo?.GetCustomAttributes(typeof(ErrorTypeAttribute), false);

            return customAttributes?.Length > 0;
        }

        private static System.ComponentModel.DefaultValueAttribute[] GetDefaultValueAttribute(Type enumType, MemberInfo[] allMembers, string typeString)
        {
            var enumValueMemberInfo = allMembers.FirstOrDefault(m => m.Name == typeString && m.DeclaringType == enumType);
            var customAttributes = enumValueMemberInfo?.GetCustomAttributes(typeof(System.ComponentModel.DefaultValueAttribute), false);

            return customAttributes?.Length > 0
                ? customAttributes.Select(att => (System.ComponentModel.DefaultValueAttribute)att).ToArray()
                : null;
        }
    }
}