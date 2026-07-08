using Microsoft.Extensions.Options;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.Models.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace PDS.ViewYourFunding.Services.Implementations
{
    /// <summary>
    /// Tasks to do with creating components.
    /// </summary>
    public class ComponentService : BaseViewYourFundingRenderer, IComponentFactory, IComponentService
    {
        private static readonly ConcurrentDictionary<string, ComponentType> _componentTypeLookups
            = new ConcurrentDictionary<string, ComponentType>();

        private static Regex numberRegex = new Regex(@"^-?[0-9][0-9,\.]+$", RegexOptions.Compiled);

        private const string FundingValueContextSuffix = ".FundingValue)";
        private const string ContextSchemaVersionSuffix = ".SchemaVersion)";

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentService"/> class.
        /// </summary>
        /// <param name="componentLoggerService">The service to use for logging for components.</param>
        /// <param name="applicationConfigOptions">The config service to use to lookup config info.</param>
        public ComponentService(
            ILoggerAdapter<Component> componentLoggerService,
            IOptions<ApplicationConfiguration> applicationConfigOptions)
        {
            ComponentHelper.LoggerService = componentLoggerService;
            ComponentHelper.ApplicationConfiguration = applicationConfigOptions?.Value;
        }

        /// <summary>
        /// Checks whether the object contains some number type.
        /// </summary>
        /// <param name="value">The object to test.</param>
        /// <returns>True if a number type - false if not.</returns>
        public static bool IsNumber(object value)
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
                || (value is string && numberRegex.IsMatch((string)value));
        }

        /// <summary>
        /// Style a number using a number format.
        /// </summary>
        /// <param name="number">A number to style.</param>
        /// <param name="absolute">Whether to absolute it (remove minus sign).</param>
        /// <param name="numberFormat">The number format to style in.</param>
        /// <param name="scale">The scale to be applied.</param>
        /// <returns>A number generally returned as a string.</returns>
        public static object StyleNumber(object number, bool absolute, string numberFormat, double? scale = null)
        {
            var selectorResultNumeric = number is double ? (double)number : Convert.ToDouble(number);

            if (scale != null && scale != 0)
            {
                selectorResultNumeric /= scale.Value;
            }

            if (absolute)
            {
                selectorResultNumeric = Math.Abs(selectorResultNumeric);
            }

            if (string.IsNullOrEmpty(numberFormat))
            {
                return selectorResultNumeric;
            }

            switch (numberFormat)
            {
                case FieldViewDataNumberFormat.GBCurrencyWithoutTrailingZeroes:
                    return selectorResultNumeric.ToGBCurrencyWithoutTrailingZeroes();
                case FieldViewDataNumberFormat.WeightingWithoutTrailingZeroes:
                    return selectorResultNumeric.ToWeightingWithoutTrailingZeroes();
                case FieldViewDataNumberFormat.ThousandsSeperatedNoTrailingZeroes:
                    return selectorResultNumeric.ToThousandsSeperatedNoTrailingZeroes();
                case FieldViewDataNumberFormat.ThousandsSeperated2DP:
                    return selectorResultNumeric.ToThousandsSeperated2DP();
                case FieldViewDataNumberFormat.GBCurrency:
                    return selectorResultNumeric.ToGBCurrency();
                case FieldViewDataNumberFormat.TwoDPWithoutTrailingZeroes:
                    return selectorResultNumeric.To2DPWithoutTrailingZeroes();
                case FieldViewDataNumberFormat.PercentageWith1DecimalPlace:
                    return selectorResultNumeric.ToPercentageWith1DecimalPlace();
                case FieldViewDataNumberFormat.PercentageWith2DecimalPlaces:
                    return selectorResultNumeric.ToPercentageWith2DecimalPlaces();
                case FieldViewDataNumberFormat.GBCurrencyWithoutDecimalPlace:
                    return selectorResultNumeric.ToGBCurrencyWithoutDecimalPlace();
                case FieldViewDataNumberFormat.PercentageWithSignificantDecimalPlaces:
                    return selectorResultNumeric.ToPercentageWithSignificantDigits();
                case FieldViewDataNumberFormat.SixDPWithoutTrailingZeroes:
                    return selectorResultNumeric.To6DPWithoutTrailingZeroes();
                case FieldViewDataNumberFormat.OneDPWithoutTrailingZeroes:
                    return selectorResultNumeric.To1DPWithoutTrailingZeroes();
            }

            return selectorResultNumeric.ToString(numberFormat);
        }

        /// <inheritdoc/>
        public Component GetComponent(
            UiModelGroup group,
            ComponentConfiguration componentConfiguration,
            ComponentConfiguration secondComponentConfiguration,
            Dictionary<ComponentType, Defaults> componentDefaults,
            DateTime publicationDate,
            string searchTerm,
            Dictionary<string, object> variables)
        {
            var values = GetFundingValues(group, componentConfiguration, secondComponentConfiguration, componentDefaults, publicationDate, searchTerm, variables);

            double varianceValue = 0;
            var varianceValueFormatted = string.Empty;

            if (group.EnableVariance && secondComponentConfiguration != null && componentConfiguration.VarianceSelectionOption != VarianceSelectionOption.NoComparison)
            {
                (varianceValue, varianceValueFormatted) = CalculateVarianceValue(group, componentConfiguration, secondComponentConfiguration);
            }

            var componentLevelVariables = new Dictionary<string, object>();

            if (group.Variables != null)
            {
                foreach (var variable in group.Variables)
                {
                    var value = EvaluateVariable(variable, componentConfiguration, publicationDate, searchTerm, variables);
                    componentLevelVariables.Add(variable.Name, value);
                }
            }

            var title = PerformReplacements(group.Title, componentConfiguration);
            var alternativeTitle = PerformReplacements(group.AlternativeTitle, componentConfiguration);

            var components = group.Groups?
                .Where(subGroup => subGroup.Type != "Data/Value")
                .Select(subGroup => GetComponent(
                    subGroup,
                    componentConfiguration,
                    secondComponentConfiguration,
                    componentDefaults,
                    publicationDate,
                    searchTerm,
                    variables)).ToList()
                ?? new List<Component>();

            var type = GetComponentTypeFromString(group.Type);
            var subtype = group.SubType;

            if (type == ComponentType.Error_ComponentNotFound)
            {
                subtype = group.Type;
            }

            return new Component(componentConfiguration)
            {
                Id = group.Id,
                OriginalGroup = group,
                Title = title,
                AlternativeTitle = alternativeTitle,
                Values = values,
                Type = type,
                SubType = subtype,
                Components = components,
                ClassNames = !string.IsNullOrEmpty(group.ClassName) ? group.ClassName.Split(' ').ToList() : new List<string>(),
                Variables = componentLevelVariables,
                VisuallyHidden = group.VisuallyHidden,
                VisibilityCondition = group.VisibilityCondition,
                EnableVariance = group.EnableVariance,
                HideVarianceValue = group.HideVarianceValue,
                VarianceValue = varianceValue,
                VarianceValueFormatted = varianceValueFormatted,
                Attributes = group.Attributes,
                PageData = new Dictionary<string, object>(),
                UseDefaultClasses = group.UseDefaultClasses,
                Context = group.Context
            };
        }

        /// <inheritdoc/>
        public Component CreateSimple(ComponentType type, object value)
        {
            return new Component(null)
            {
                Type = type,
                Values = new List<object>
                {
                    value
                }
            };
        }

        /// <inheritdoc/>
        public object EvaluateVariable(
            UIModelVariable variable,
            ComponentConfiguration componentConfiguration,
            DateTime publicationDate,
            string searchTerm,
            Dictionary<string, object> variables)
        {
            var value = variable.Value;

            if (!string.IsNullOrEmpty(variable.Expression))
            {
                value = EvaluateWhereClause(variable.Expression, componentConfiguration, variables);

                if (value is bool valueBool)
                {
                    if (!valueBool && variable.FalseResponse != null)
                    {
                        value = PerformReplacements(variable.FalseResponse, componentConfiguration);
                    }
                    else if (valueBool && variable.TrueResponse != null)
                    {
                        value = PerformReplacements(variable.TrueResponse, componentConfiguration);
                    }
                }

                variable.Evaluated = true;
            }
            else if (!string.IsNullOrEmpty(variable.Selector))
            {
                value = GetFundingValue(
                    new UiModelGroup { Selector = variable.Selector, Scale = variable.Scale },
                    componentConfiguration,
                    publicationDate,
                    searchTerm,
                    variables);

                if (variable.Scale != null && IsNumber(value))
                {
                    var valueDouble = value is double ? (double)value : Convert.ToDouble(value);
                    value = valueDouble / variable.Scale;
                }

                variable.Evaluated = true;
            }
            else if (value != null && value is string valueString && valueString != string.Empty)
            {
                variable.Evaluated = true;
            }

            return value;
        }

        /// <summary>
        /// Get the funding value.
        /// </summary>
        /// <param name="modelGroup">The model group.</param>
        /// <param name="componentConfiguration">The cmponent configuration.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="variables">Dictionary of variables.</param>
        /// <returns>An object containing funding value.</returns>
        /// <exception cref="Exception">Exception, More then one expression component supplied.</exception>
        public object GetFundingValue(
            UiModelGroup modelGroup,
            ComponentConfiguration componentConfiguration,
            DateTime publicationDate,
            string searchTerm,
            Dictionary<string, object> variables)
        {
            if (modelGroup.Context != null && !string.IsNullOrEmpty(modelGroup.Selector))
            {
                return new EvaluatedExpressionComponent
                {
                    OriginalExpression = modelGroup.Selector,
                    LeftHandComponent = modelGroup.Selector,
                    IsSelector = true,
                    Context = modelGroup.Context
                };
            }

            if (!string.IsNullOrEmpty(modelGroup.Expression))
            {
                var evaluatedExpressions =
                    GetEvaluatedExpressionComponents(modelGroup.Expression, componentConfiguration, variables);

                if (evaluatedExpressions.Count != 1)
                {
                    throw new Exception("More then one expression component supplied");
                }

                var evaluatedExpression = evaluatedExpressions.First();

                if (!string.IsNullOrEmpty(evaluatedExpression.RightHandComponent))
                {
                    return CheckEquality(evaluatedExpression);
                }

                return evaluatedExpression.Evaluated
                    ? evaluatedExpression.LeftHandComponentEvaluatedResult
                    : evaluatedExpression;
            }

            if (!string.IsNullOrEmpty(modelGroup.Value))
            {
                return PerformReplacements(modelGroup.Value, componentConfiguration);
            }

            var formula = modelGroup.Formula;

            if (!string.IsNullOrEmpty(formula?.Expression))
            {
                var contextSchemaDataList = new List<ContextSchemaData>();

                if (formula.Contexts?.Where(c => c != null).Any() == true)
                {
                    foreach (var contextString in formula.Contexts)
                    {
                        if (string.IsNullOrEmpty(contextString))
                        {
                            contextSchemaDataList.Add(null);
                            continue;
                        }

                        var context = PerformRuntimeReplacements(contextString, new Component(componentConfiguration), neverShowSelectors: true);
                        var schemaVersion = PerformRuntimeReplacements(
                            contextString.Replace(FundingValueContextSuffix, ContextSchemaVersionSuffix),
                            new Component(componentConfiguration),
                            neverShowSelectors: true);

                        contextSchemaDataList.Add(new ContextSchemaData { Context = context, SchemaVersion = schemaVersion });
                    }
                }

                var i = 0;

                // Process each selector to get the value
                var selectorResults = formula.Selectors?.Select(currentSelector =>
                {
                    var currentContextSchemaData = contextSchemaDataList[i++];

                    if (currentContextSchemaData.Context is string contextString)
                    {
                        if (contextString.Equals(DataValueConstants.NullObjectValue, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return 0;
                        }

                        double.TryParse(Convert.ToString(currentContextSchemaData.SchemaVersion), out var schemaVersion);

                        schemaVersion = schemaVersion > 0 ? schemaVersion : componentConfiguration.SchemaVersion;

                        var conf = new ComponentConfiguration
                        {
                            Data = new FundingApiSearchFunding
                            {
                                FundingValue = contextString
                            },
                            ConstantDefinedVariables = componentConfiguration.ConstantDefinedVariables,
                            SchemaVersion = schemaVersion
                        };

                        var replacedSelector = (string)PerformRuntimeReplacements(
                            currentSelector,
                            new Component(conf),
                            false,
                            true);

                        return Select(conf, replacedSelector);
                    }

                    return Select(componentConfiguration, currentSelector);
                });

                // Inject the selector results into the expression
                var expression = string.Format(formula.Expression, selectorResults?.ToArray() ?? new object[0]);

                // Evaluate the expression and return the result
                using (var dataTable = new DataTable())
                {
                    return dataTable.Compute(expression, string.Empty);
                }
            }

            var selector = modelGroup.Selector;

            if (string.IsNullOrEmpty(selector))
            {
                return null;
            }

            selector = PerformReplacements(selector, componentConfiguration, canShowSelector: false);
            return Select(componentConfiguration, selector);
        }

        private static (double, string) CalculateVarianceValue(
            UiModelGroup modelGroup,
            ComponentConfiguration componentConfiguration,
            ComponentConfiguration secondComponentConfiguration)
        {
            if (componentConfiguration.Data == null || secondComponentConfiguration == null || string.IsNullOrWhiteSpace(modelGroup.Selector))
            {
                return default;
            }

            var currentValueItem = Select(componentConfiguration, modelGroup.Selector, false);
            var previousValueItem = Select(secondComponentConfiguration, modelGroup.Selector, false);

            if (!double.TryParse(currentValueItem?.ToString(), out var currentValue) || !double.TryParse(previousValueItem?.ToString(), out var previousValue))
            {
                return default;
            }

            var varianceValue = currentValue - previousValue;
            var varianceValueFormatted = GetVarianceValueFormatted(modelGroup, varianceValue);
            return (varianceValue, varianceValueFormatted);
        }

        private static string GetVarianceValueFormatted(
            UiModelGroup modelGroup,
            double varianceValue)
        {
            var numberFormat = modelGroup.Datastyle?.NumberFormat;
            var scale = modelGroup?.Scale;

            var displayVarianceValue = Math.Abs(varianceValue);
            if (scale != null && scale != 0)
            {
                displayVarianceValue /= scale.Value;
            }

            if (string.IsNullOrEmpty(numberFormat))
            {
                return displayVarianceValue.ToString();
            }

            switch (numberFormat)
            {
                case FieldViewDataNumberFormat.GBCurrencyWithoutTrailingZeroes:
                    return displayVarianceValue.ToGBCurrencyWithoutTrailingZeroes();
                case FieldViewDataNumberFormat.WeightingWithoutTrailingZeroes:
                    return displayVarianceValue.ToWeightingWithoutTrailingZeroes();
                case FieldViewDataNumberFormat.ThousandsSeperatedNoTrailingZeroes:
                    return displayVarianceValue.ToThousandsSeperatedNoTrailingZeroes();
                case FieldViewDataNumberFormat.ThousandsSeperated2DP:
                    return displayVarianceValue.ToThousandsSeperated2DP();
                case FieldViewDataNumberFormat.GBCurrency:
                    return displayVarianceValue.ToGBCurrency();
                case FieldViewDataNumberFormat.TwoDPWithoutTrailingZeroes:
                    return displayVarianceValue.To2DPWithoutTrailingZeroes();
                case FieldViewDataNumberFormat.PercentageWith1DecimalPlace:
                    return displayVarianceValue.ToPercentageWith1DecimalPlace();
                case FieldViewDataNumberFormat.PercentageWith2DecimalPlaces:
                    return displayVarianceValue.ToPercentageWith2DecimalPlaces();
                case FieldViewDataNumberFormat.GBCurrencyWithoutDecimalPlace:
                    return displayVarianceValue.ToGBCurrencyWithoutDecimalPlace();
                case FieldViewDataNumberFormat.SixDPWithoutTrailingZeroes:
                    return displayVarianceValue.To6DPWithoutTrailingZeroes();
                case FieldViewDataNumberFormat.PercentageWithSignificantDecimalPlaces:
                    return displayVarianceValue.ToPercentageWithSignificantDigits();
                case FieldViewDataNumberFormat.OneDPWithoutTrailingZeroes:
                    return displayVarianceValue.To1DPWithoutTrailingZeroes();
            }

            return displayVarianceValue.ToString(numberFormat);
        }

        private ComponentType GetComponentTypeFromString(string groupTypeName)
        {
            var key = $"GetComponentTypeFromString_{groupTypeName}";

            if (_componentTypeLookups.ContainsKey(key))
            {
                return _componentTypeLookups[key];
            }

            var componentType = GetComponentTypeFromStringUsingReflection(groupTypeName);
            _componentTypeLookups.TryAdd(key, componentType);

            return componentType;
        }

        private ComponentType GetComponentTypeFromStringUsingReflection(string groupTypeName)
        {
            if (string.IsNullOrEmpty(groupTypeName))
            {
                return ComponentType.NotSet;
            }

            var enumType = typeof(ComponentType);
            var groupTypeNameWithUnderscores = groupTypeName.Replace("/", "_");

            foreach (var componentType in (ComponentType[])Enum.GetValues(enumType))
            {
                var componentTypeString = componentType.ToString();

                if (componentTypeString == groupTypeNameWithUnderscores)
                {
                    return componentType;
                }

                var memberInfos = enumType.GetMember(componentTypeString);
                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
                var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(System.ComponentModel.DefaultValueAttribute), false);

                if (valueAttributes.Length > 0 && ((System.ComponentModel.DefaultValueAttribute)valueAttributes[0]).Value?.ToString() == groupTypeName)
                {
                    return componentType;
                }
            }

            return ComponentType.Error_ComponentNotFound;
        }

        private List<object> GetFundingValues(
            UiModelGroup modelGroup,
            ComponentConfiguration componentConfiguration,
            ComponentConfiguration secondComponentConfiguration,
            Dictionary<ComponentType, Defaults> componentDefaults,
            DateTime publicationDate,
            string searchTerm,
            Dictionary<string, object> variables)
        {
            var returnList = new List<object>();
            var groupsToProcess = new List<UiModelGroup>
            {
                modelGroup
            };

            // We have some direct child selectors
            if (modelGroup.Groups?.Any(subGroup => subGroup.Type == "Data/Value") == true)
            {
                var childSelectorGroups = modelGroup.Groups.Where(subGroup => subGroup.Type == "Data/Value").ToList();
                groupsToProcess.AddRange(childSelectorGroups);
            }

            foreach (var groupToProcess in groupsToProcess)
            {
                if (!string.IsNullOrEmpty(groupToProcess.Type))
                {
                    var componentType = GetComponentTypeFromString(groupToProcess.Type);

                    if (componentDefaults?.ContainsKey(componentType) == true)
                    {
                        // Copy over the defaults
                        var relevantDefaults = componentDefaults[componentType];

                        if (!string.IsNullOrEmpty(relevantDefaults.NumberFormat) && string.IsNullOrEmpty(groupToProcess.Datastyle?.NumberFormat))
                        {
                            if (groupToProcess.Datastyle == null)
                            {
                                groupToProcess.Datastyle = new UiModelStyle();
                            }

                            groupToProcess.Datastyle.NumberFormat = relevantDefaults.NumberFormat;
                        }
                    }
                }

                if (groupToProcess.Type == "Conditional/Condition" || groupToProcess.Type == "Conditional/Switch")
                {
                    returnList.Add(EvaluateWhereClause(groupToProcess.Expression, componentConfiguration, variables));
                    continue;
                }

                var selectorResult = GetFundingValue(
                    groupToProcess,
                    componentConfiguration,
                    publicationDate,
                    searchTerm,
                    variables);

                if (selectorResult == null)
                {
                    if (componentConfiguration?.ShowSelectors == true && !string.IsNullOrEmpty(groupToProcess.Selector))
                    {
                        returnList.Add("<span style='color: #F00'>NOT MATCHED</span>");
                    }

                    continue;
                }

                var booleanFormat = groupToProcess.Datastyle?.BooleanFormat;

                if (booleanFormat == FieldViewDataBooleanFormat.BooleanYesNo)
                {
                    returnList.Add(selectorResult.ToBooleanYesOrNo());
                    continue;
                }

                var enumFormat = groupToProcess.Datastyle?.EnumFormat;

                if (enumFormat == FieldViewDataEnumFormat.SparsityMethodologyPdf)
                {
                    returnList.Add(selectorResult?.ToString().ToSparsityMethodologyDisplayValuePdfs());
                    continue;
                }

                if (int.TryParse(selectorResult.ToString(), out var enumValue))
                {
                    if (enumFormat == FieldViewDataEnumFormat.SparsityMethodology)
                    {
                        returnList.Add(enumValue.ToSparsityMethodologyDisplayValue());
                        continue;
                    }
                }

                if (!ComponentHelper.IsNumber(selectorResult))
                {
                    returnList.Add(selectorResult);
                    continue;
                }

                returnList.Add(StyleNumber(selectorResult, groupToProcess.Absolute, groupToProcess.Datastyle?.NumberFormat, modelGroup?.Scale));
            }

            return returnList;
        }
    }
}