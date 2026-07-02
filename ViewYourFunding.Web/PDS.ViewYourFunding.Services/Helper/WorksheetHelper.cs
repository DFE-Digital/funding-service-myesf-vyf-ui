using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// The worksheet helper class.
    /// </summary>
    public static class WorksheetHelper
    {
        /// <summary>
        /// Gets the not applicable within range value.
        /// </summary>
        /// <param name="uiModelGroup">The UI model group.</param>
        /// <param name="valuesToCompare">Value to compare.</param>
        /// <returns>Evaluated result.</returns>
        public static string GetNotApplicableWithinRangeValueEvaluation(this UiModelGroup uiModelGroup, IEnumerable<double> valuesToCompare)
        {
            if (uiModelGroup.NotApplicableForValueWithinRange?.Maximum.HasValue == true &&
                uiModelGroup.NotApplicableForValueWithinRange?.Minimum.HasValue == true)
            {
                if (valuesToCompare.Any(value =>
                        value < uiModelGroup.NotApplicableForValueWithinRange.Maximum &&
                        value > uiModelGroup.NotApplicableForValueWithinRange.Minimum))
                {
                    return "x";
                }
            }

            return default;
        }
    }
}
