using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A group concept (e.g. a spreadsheet, or a header in a spreadsheet).
    /// </summary>
    public class UiModelGroup
    {
        private Dictionary<string, UiModelGroup> _overrides = null, _rawOverrides = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiModelGroup"/> class.
        /// Default constructor (used for deserialisation) for UI model groups.
        /// </summary>
        public UiModelGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiModelGroup"/> class.
        /// UI Model group accepting the minimum generally required fields.
        /// </summary>
        /// <param name="title">Title of the group.</param>
        /// <param name="selector">Selector to get the data for the group.</param>
        public UiModelGroup(object title, string selector)
        {
            Title = title;
            Selector = selector;
        }

        /// <summary>
        /// Gets or sets the id of a group.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the type of template (e.g. simpletable).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the type of template (e.g. simpletable).
        /// </summary>
        [JsonProperty("subtype")]
        public string SubType { get; set; }

        /// <summary>
        /// Gets or sets the title to show for a group header.
        /// </summary>
        public object Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether groups should be renderered as rows.
        /// </summary>
        public bool RenderGroupsAsRows { get; set; }

        /// <summary>
        /// Gets or sets the JsonPath to use to get the data.
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// Gets or sets the JsonPath to use to get the value which this group is dependent on.
        /// </summary>
        public string DependentSelectors { get; set; }

        /// <summary>
        /// Gets or sets the explicit value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the formula to use to calculate the data.
        /// </summary>
        public UiModelFormula Formula { get; set; }

        /// <summary>
        /// Gets or sets the scale to show the value as (e.g. 100000000).
        /// </summary>
        public double? Scale { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only show if there is data (i.e. 1 or more result from any of the datasets).
        /// </summary>
        [JsonProperty("onlyshowifdata")]
        public bool OnlyShowIfData { get; set; }

        /// <summary>
        /// Gets or sets detail to show with the header (for spreadhseets, these show as seperate rows below the header).
        /// </summary>
        public List<UiModelGroup> Detail { get; set; }

        /// <summary>
        /// Gets or sets (Only applicable for tables/spreadsheets) How many columns this should span.
        /// </summary>
        public int? Colspan { get; set; }

        /// <summary>
        /// Gets or sets (Only applicable for tables/spreadsheets) How many rows this should span.
        /// </summary>
        public int? Rowspan { get; set; }

        /// <summary>
        /// Gets or sets should the new rows be added after applying row span (spreadsheet only).
        /// </summary>
        [JsonProperty("rowspanwithnewrows")]
        public bool? RowSpanWithNewRows { get; set; }

        /// <summary>
        /// Gets or sets a path to an image.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the context. This property is used to override the existing data lookup and accepts a variable (examples - @dataset2 and @(RepeaterItem.FundingValue)).
        /// </summary>
        public object Context { get; set; }

        /// <summary>
        /// Gets or sets sub groups of this group (e.g. if this group is a worksheet, the groups underneath this may be columns on that worksheet).
        /// </summary>
        public List<UiModelGroup> Groups { get; set; }

        /// <summary>
        /// Gets or sets styles to apply to the group header.
        /// </summary>
        public UiModelStyle Style { get; set; }

        /// <summary>
        /// Gets or sets styles to apply to the group data.
        /// </summary>
        public UiModelStyle Datastyle { get; set; }

        /// <summary>
        /// Gets or sets dataset for the group data (only used for top level groups (e.g. for sheets in a spreadsheet).
        /// </summary>
        public List<UiModelDataset> Dataset { get; set; }

        /// <summary>
        /// Gets or sets footer text to add.
        /// </summary>
        public string[] Footer { get; set; }

        /// <summary>
        /// Gets or sets overrides in their raw form, as read from the JSON model. The dictionary keys may be delimited by comma or semicolon.
        /// </summary>
        [JsonProperty("overrides")]
        public Dictionary<string, UiModelGroup> RawOverrides
        {
            get
            {
                return _rawOverrides;
            }

            set
            {
                _rawOverrides = value;
                _overrides = null;
            }
        }

        /// <summary>
        /// Gets or sets overrides, e.g. to override the styles, or to specify different values for certain grouping types.
        /// Equivalent to <see cref="RawOverrides"/>, but with any delimited keys expanded into multiple elements.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, UiModelGroup> Overrides
        {
            get
            {
                if (RawOverrides == null)
                {
                    return null;
                }

                if (_overrides != null)
                {
                    return _overrides;
                }

                var overrides = new Dictionary<string, UiModelGroup>();

                foreach (var rawOverridesKey in RawOverrides.Keys)
                {
                    var expandedKeys = rawOverridesKey
                        .Split(new[] { ';', ',' }, System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(k => k.Trim());

                    var currentGroup = RawOverrides[rawOverridesKey];

                    foreach (var key in expandedKeys)
                    {
                        overrides.Add(key, currentGroup);
                    }
                }

                _overrides = overrides;
                return overrides;
            }

            set
            {
                _overrides = null;
                RawOverrides = value;
            }
        }

        /// <summary>
        /// Gets or sets classes applied to this funding.
        /// </summary>
        [JsonProperty("classname")]
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether default classes should be applied.
        /// </summary>
        [JsonProperty("useDefaultClasses")]
        public bool UseDefaultClasses { get; set; } = true;

        /// <summary>
        /// Gets or sets should the cell/group have a dropdown added to it.
        /// </summary>
        [JsonProperty("dropdown")]
        public UiModelDropDown DropDown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether should the cell have auto filters applied to it (spreadsheet only).
        /// </summary>
        [JsonProperty("autofilter")]
        public bool AutoFilter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the row to start auto filter.
        /// </summary>
        [JsonProperty("autoFilterStartRow")]
        public int AutoFilterStartRow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the header row.
        /// </summary>
        [JsonProperty("headerRow")]
        public string HeaderRow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether should the cell value be absoluted (made positive if negative) if it is a number type.
        /// </summary>
        public bool Absolute { get; set; }

        /// <summary>
        /// Gets or sets any specific variables.
        /// </summary>
        public List<UIModelVariable> Variables { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component is visually hidden or not.
        /// </summary>
        [JsonProperty("visuallyhidden")]
        public bool VisuallyHidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component is visually hidden or not.
        /// </summary>
        [JsonProperty("visibilityCondition")]
        public string VisibilityCondition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component will display variance data or not.
        /// </summary>
        public bool EnableVariance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide the variance value.
        /// </summary>
        public bool HideVarianceValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component is rendered or not.
        /// </summary>
        public bool RenderItem { get; set; } = true;

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        [JsonProperty("attributes")]
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets a value indicating whether this shouldnt show on a statement specification.
        /// </summary>
        public bool DontShowStatementSpecification { get; set; }

        /// <summary>
        /// Gets or sets the alternative title to show for a group header.
        /// </summary>
        public object AlternativeTitle { get; set; }

        /// <summary>
        /// Gets or sets the not applicable value limit in a group header.
        /// </summary>
        public int? NotApplicableForValueBelow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show blank for zero or null.
        /// </summary>
        public bool ShowBlanksForNullOrZero { get; set; }

        /// <summary>
        /// Gets or sets a value for the Superscript.
        /// </summary>
        public string SuperScript { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether get or sets a value for HideForNotApplicableValues.
        /// </summary>
        public bool HideForNotApplicableValues { get; set; }

        /// <summary>
        /// Gets or sets the not applicable value limit within range in a group header.
        /// </summary>
        [JsonProperty("notApplicableForValueWithinRange")]
        public UiModelNotApplicableForValueWithinRange NotApplicableForValueWithinRange { get; set; }
    }
}