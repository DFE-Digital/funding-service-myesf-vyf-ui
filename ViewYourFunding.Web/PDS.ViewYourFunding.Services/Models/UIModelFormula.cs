namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A formula to use to calculate data.
    /// </summary>
    public class UiModelFormula
    {
        /// <summary>
        /// Gets or sets the expression that will be evaluated, which can reference selectors by their position, e.g. "{0}/{1}".
        /// It must use the expression syntax described here: https://docs.microsoft.com/en-us/dotnet/api/system.data.datacolumn.expression?view=netframework-4.8#expression-syntax.
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets an array containing the JsonPath selectors to use to get the inputs for the expression.
        /// </summary>
        public string[] Selectors { get; set; }

        /// <summary>
        /// Gets or sets an optional parameter allowing us to cut off to a number of decimal places.
        /// </summary>
        public int? DecimalPlaces { get; set; }

        /// <summary>
        /// Gets or sets the contexts array (optional).
        /// </summary>
        public string[] Contexts { get; set; }
    }
}