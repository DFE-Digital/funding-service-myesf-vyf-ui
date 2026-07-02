using PDS.ViewYourFunding.Services.Enums;
using System;

namespace PDS.ViewYourFunding.Services.Attributes
{
    /// <summary>
    /// An attribute to determine which types are applicable for this scope.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ApplicableForAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicableForAttribute"/> class.
        /// </summary>
        /// <param name="type">The funding view type (e.g. ViewData).</param>
        /// <param name="overrideName">An optional 'override' name.</param>
        public ApplicableForAttribute(FundingViewType type, string overrideName = null)
        {
            Type = type;
            OverrideName = overrideName;
        }

        /// <summary>
        /// Gets or sets the funding view type (e.g. ViewData).
        /// </summary>
        public FundingViewType Type { get; set; }

        /// <summary>
        ///  Gets or sets a name to use for this funding view type.
        /// </summary>
        public string OverrideName { get; set; }
    }
}