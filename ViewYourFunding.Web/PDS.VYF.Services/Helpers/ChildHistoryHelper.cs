namespace PDS.VYF.Services.Helpers
{
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;
    using PDS.VYF.Services.Models.ViewDataModels;

    public static class ChildHistoryHelper
    {
        /// <summary>
        /// Get a Child History Model In a Year.
        /// </summary>
        /// <param name="childFundingData">Child Funding Data.</param>
        /// <param name="model">Child History Page Row model.</param>
        /// <param name="isLatestFundingPeriodCode">Latest Funding Period Code.</param>
        /// <returns>A model Child History Page Row.</returns>
        public static ChildHistoryPageRow GetChildHistoryModelInaYear(LoggedInChildModel childFundingData, ChildHistoryPageRow model, bool isLatestFundingPeriodCode)
        {
            if (childFundingData.InYearOpener == false)
            {
                model = new ChildHistoryPageRow
                {
                    FundingPeriodCode = childFundingData.FundingPeriodCode!,
                    StatusChangedDate = childFundingData.StatusChangedDate ?? DateTime.Today,
                    GroupingReason = childFundingData.ParentInfo?.FirstOrDefault()?.GroupingReason ?? string.Empty,
                    VariationReason = childFundingData.StatementType == "New" ? "Initial allocation statement" : "Revised allocation statement",
                    IsLatest = isLatestFundingPeriodCode && childFundingData.IsLatest == true,
                    IsFinal = !isLatestFundingPeriodCode && childFundingData.IsLatest == true,
                };
            }
            else
            {
                model = new ChildHistoryPageRow
                {
                    FundingPeriodCode = childFundingData.FundingPeriodCode!,
                    StatusChangedDate = childFundingData.StatusChangedDate ?? DateTime.Today,
                    GroupingReason = childFundingData.ParentInfo?.FirstOrDefault()?.GroupingReason ?? string.Empty,
                    VariationReason = childFundingData.IsIndicative == true
                                        ? childFundingData.StatementType == "New" ? "Indicative part year allocation statement" : "Revised indicative part year allocation statement"
                                        : IsSecondYearInYearOpenerCalculation(childFundingData.YearFrom, childFundingData.DateOpened) == true
                                            ? childFundingData.StatementType == "New" ? "Initial allocation statement" : "Revised allocation statement"
                                            : childFundingData.StatementType == "New" ? "Final part year allocation statement" : "Revised final part year allocation statement",
                    IsLatest = isLatestFundingPeriodCode && childFundingData.IsLatest == true,
                    IsFinal = !isLatestFundingPeriodCode && childFundingData.IsLatest == true,
                };
            }

            return model;
        }

        /// <summary>
        /// Update the statement type by grouping scenarios.
        /// </summary>
        /// <param name="childFundingData">Child Funding Data.</param>
        public static void UpdateStatementTypeByGroupingScenarios(IEnumerable<LoggedInChildModel> childFundingData)
        {
            var intermediateGroupsWithScenario = childFundingData.GroupBy(a => new
            {
                ScenarioKey = GetScenarioGroup(a),
                YearFromKey = a.YearFrom ?? 0,
                YearToKey = a.YearTo ?? 0,
            });

            foreach (var group in intermediateGroupsWithScenario)
            {
                var orderedGroup = group.OrderBy(d => d.StatusChangedDate).ThenBy(d => d.FundingVersionInt).ToList();

                if (orderedGroup.Count != 0)
                {
                    orderedGroup.First().StatementType = "New";

                    foreach (var subsequentItem in orderedGroup.Skip(1))
                    {
                        subsequentItem.StatementType = "Updated";
                    }
                }
            }
        }

        private static string GetScenarioGroup(LoggedInChildModel data)
        {
            if (data.IsIndicative == true)
            {
                return "Indicative";
            }
            else if (data.InYearOpener == true)
            {
                return "IYO Part Year";
            }
            else
            {
                return "Main";
            }
        }

        private static bool IsSecondYearInYearOpenerCalculation(int? yearFrom, DateTime? dateOpened)
        {
            var previousYearPostAprilStartDate = new DateTime(yearFrom!.Value, 4, 1);
            var previousYearPostAprilEndDate = new DateTime(yearFrom!.Value, 8, 31);

            var isSecondYearInYearOpener = dateOpened >= previousYearPostAprilStartDate && dateOpened <= previousYearPostAprilEndDate;

            return isSecondYearInYearOpener;
        }
    }
}