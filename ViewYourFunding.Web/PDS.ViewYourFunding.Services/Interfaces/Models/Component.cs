using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// Represents a UI rendering component.
    /// </summary>
    public class Component
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        /// <param name="componentConfiguration">The component configuration (funding stream code details etc..).</param>
        public Component(ComponentConfiguration componentConfiguration)
        {
            ComponentConfiguration = componentConfiguration;
            PageData = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the the configuration (stream information etc...).
        /// </summary>
        public ComponentConfiguration ComponentConfiguration { get; set; }

        /// <summary>
        /// Gets or sets any extra information.
        /// </summary>
        public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public ComponentType Type { get; set; }

        /// <summary>
        /// Gets or sets the sub type.
        /// </summary>
        public string SubType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether default classes should be applied.
        /// </summary>
        public bool UseDefaultClasses { get; set; } = true;

        /// <summary>
        /// Gets or sets the original group.
        /// </summary>
        public UiModelGroup OriginalGroup { get; set; }

        /// <summary>
        /// Gets or sets the context. This property is used to override the existing data lookup and accepts a variable (examples - @dataset2 and @(RepeaterItem.FundingValue)).
        /// </summary>
        public object Context { get; set; }

        /// <summary>
        /// Gets or sets the context schema version.
        /// </summary>
        public object ContextSchemaVersion { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get
            {
                if (!_titleEvaluated)
                {
                    _title = BaseViewYourFundingRenderer.PerformRuntimeReplacements(_title, this)?.ToString();
                    _titleEvaluated = true;
                }

                return _title;
            }

            set
            {
                _title = value;
                UnevaluatedTitle = value;
                _titleEvaluated = false;
            }
        }

        /// <summary>
        /// Gets or sets the child components.
        /// </summary>
        public List<Component> Components { get; set; } = new List<Component>();


        /// <summary>
        /// Gets the visible child components.
        /// </summary>
        /// <returns>The List of Components.</returns>
        public List<Component> VisibleComponents()
        {
            if (Components != null)
            {
                return Components.Where(component => component.RenderItem).ToList();
            }

            return new List<Component>();
        }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public List<object> Values
        {
            get
            {
                if (!_valuesEvaluated)
                {
                    _values = BaseViewYourFundingRenderer.EvaluateValues(_values, this);
                    MarkValuesEvaluated();
                }

                return _values;
            }

            set
            {
                _values = value;
                UnevaluatedValues = value?.ToList();
                _valuesEvaluated = false;
            }
        }

        /// <summary>
        /// Sets the values as evaluated.
        /// </summary>
        public void MarkValuesEvaluated()
        {
            _valuesEvaluated = true;
        }

        /// <summary>
        /// Sets the values as un-evaluated.
        /// </summary>
        public void MarkValuesUnevaluated()
        {
            _valuesEvaluated = false;
        }

        /// <summary>
        /// Gets or sets the class names.
        /// </summary>
        public List<string> ClassNames
        {
            get
            {
                if (!_classNamesEvaluated)
                {
                    if (_classNames != null)
                    {
                        var newList = new List<string>();

                        foreach (var className in _classNames)
                        {
                            var result = BaseViewYourFundingRenderer.PerformRuntimeReplacements(className, this)
                                ?.ToString();
                            newList.Add(result);
                        }

                        _classNames = newList;
                    }

                    _classNamesEvaluated = true;
                }

                return _classNames;
            }

            set
            {
                _classNames = value;
                UnevaluatedClassNames = value;
                _classNamesEvaluated = false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component is visually hidden or not.
        /// </summary>
        public bool VisuallyHidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the visibility condition to be satisfied for the component to be rendered or not.
        /// </summary>
        public string VisibilityCondition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component will display variance data or not.
        /// </summary>
        public bool EnableVariance { get; set; }

        /// <summary>
        /// Gets or sets the variance value.
        /// </summary>
        /// <value>
        /// The variance value.
        /// </value>
        public double VarianceValue { get; set; }

        /// <summary>
        /// Gets or sets the variance value formatted.
        /// </summary>
        /// <value>
        /// The variance value formatted.
        /// </value>
        public string VarianceValueFormatted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide the variance value.
        /// </summary>
        public bool HideVarianceValue { get; set; }

        /// <summary>
        /// Gets a value indicating whether the component is rendered or not.
        /// </summary>
        public bool RenderItem
        {
            get
            {
                return BaseViewYourFundingRenderer.EvaluateVisibilityCondition(this);
            }
        }

        /// <summary>
        /// Gets or sets the data to be passed in at a page level.
        /// </summary>
        public Dictionary<string, object> PageData { get; set; }

        /// <summary>
        /// Gets or sets the unevaluated values.
        /// </summary>
        public List<object> UnevaluatedValues { get; set; } = new List<object>();

        /// <summary>
        /// Gets or sets the unevaluated title.
        /// </summary>
        public string UnevaluatedTitle { get; set; }

        /// <summary>
        /// Gets or sets the unevaluated class names.
        /// </summary>
        public List<string> UnevaluatedClassNames { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating the alternative title for the component to be rendered.
        /// </summary>
        public string AlternativeTitle { get; set; }

        private bool _valuesEvaluated = false;
        private bool _titleEvaluated = false;
        private bool _classNamesEvaluated = false;

        private string _title;
        private List<object> _values = new List<object>();
        private List<string> _classNames = new List<string>();
    }
}