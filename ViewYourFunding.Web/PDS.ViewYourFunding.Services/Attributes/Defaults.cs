using System;

namespace PDS.ViewYourFunding.Services.Attributes
{
    /// <summary>
    /// Specify the defaults for a component.
    /// </summary>
    public class Defaults : Attribute
    {
        /// <summary>
        /// Gets or sets the number format.
        /// </summary>
        public string NumberFormat { get; set; }
    }
}