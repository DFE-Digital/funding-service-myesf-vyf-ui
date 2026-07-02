namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A variable for use in a rendered component.
    /// </summary>
    public class UIModelVariable
    {
        /// <summary>
        /// Gets or sets the name of the variable (e.g. schoolsAllocation).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets an expression (e.g. PublicationUiModelVersion>1).
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets a JSON path selector (e.g. $..fundingLines[?(@.templateLineId == 145)].value).
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the variable has been evaluated.
        /// </summary>
        public bool Evaluated { get; set; }

        /// <summary>
        /// Gets or sets the value. This is either set directly or from the evaluation of the expression or selector.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public double? Scale { get; set; }

        /// <summary>
        /// Gets or sets something to be used in conjuction with an expression. If the expression evaluates to true, set the value to this.
        /// </summary>
        public string TrueResponse { get; set; }

        /// <summary>
        /// Gets or sets something to be used in conjuction with an expression. If the expression evaluates to false, set the value to this.
        /// </summary>
        public string FalseResponse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the selectors should never be rendered (for example - for images).
        /// </summary>
        public bool NeverShowSelector { get; set; }
    }
}