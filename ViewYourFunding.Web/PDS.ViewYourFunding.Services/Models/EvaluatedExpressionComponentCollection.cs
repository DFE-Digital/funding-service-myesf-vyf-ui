using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A collection of evaluation expression components.
    /// </summary>
    public class EvaluatedExpressionComponentCollection
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public List<EvaluatedExpressionComponent> Items { get; set; } = new List<EvaluatedExpressionComponent>();

        /// <summary>
        /// Gets or sets a value indicating whether it is an or query (implicitly an AND if not).
        /// </summary>
        public bool IsOrQuery { get; set; }
    }
}