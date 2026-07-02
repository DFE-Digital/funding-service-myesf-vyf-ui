using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Helpers
{
    /// <summary>
    /// Helper methods for display.
    /// </summary>
    public static class DisplayHelper
    {
        /// <summary>
        /// Updates the allocation history lists to show only latest 3 histories.
        /// </summary>
        /// <param name="futureYears">The future years to be shown in allocation history panel.</param>
        /// <param name="currentYearsBeforeThisYear">The current years before the given year to be shown in allocation history panel.</param>
        /// <param name="historicYears">The historic years to be shown in allocation history panel.</</param>
        public static void UpdateAllocationHistoryListToDisplay(IList<(int, int)> futureYears, IList<(int, int)> currentYearsBeforeThisYear, IList<(int, int)> historicYears)
        {
            while (futureYears.Count + currentYearsBeforeThisYear.Count + historicYears.Count > 3)
            {
                var historicYearsCount = historicYears.Count;
                var currentYearsBeforeThisYearCount = currentYearsBeforeThisYear.Count;
                var futureYearsCount = futureYears.Count;

                if (historicYears.Count > 0)
                {
                    historicYears.RemoveAt(historicYearsCount - 1);
                }
                else if (currentYearsBeforeThisYear.Count > 0)
                {
                    currentYearsBeforeThisYear.RemoveAt(currentYearsBeforeThisYearCount - 1);
                }
                else
                {
                    futureYears.RemoveAt(futureYearsCount - 1);
                }
            }
        }
    }
}