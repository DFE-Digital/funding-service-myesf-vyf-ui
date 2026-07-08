using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Helper Class to identify and filter based on is NewOpenerProvisionalAllocation or not. Introduced as part of UIFSM enhancements.
    /// </summary>
    public static class UIFSMNewOpenerAndFinalAllocationFiltersHelper
    {
        /// <summary>
        /// Check if Calc ID 39 and if value is not equal to 0, then it is New Opener.
        /// </summary>
        /// <param name="fundingApiSearch">Provide Funding Api Search Data.</param>
        /// <returns>Returns true if the Provider has New Opener Provisional Allocation.</returns>
        public static bool CheckProviderHasNewOpenerProvisionalAllocation(this IFundingApiSearch fundingApiSearch)
        {
            var allCalcIDs = GetAllCalculations(fundingApiSearch);

            const int CalcID_NewOpenerProvisionalAllocation = 39;

            return allCalcIDs?.Any(a => a.TemplateCalculationId == CalcID_NewOpenerProvisionalAllocation
                                            && double.TryParse(Convert.ToString(a.Value), out double amount)
                                            && amount > 0) ?? false;
        }

        /// <summary>
        /// Check if Calc ID 18 (FinalAllocation) or Calc ID 45 (Aggregated Predecessor Provisional Allocation) has non zero value and if value is not equal to 0, then the provider has final allocation.
        /// </summary>
        /// <param name="fundingApiSearch">Provide Funding Api Search Data.</param>
        /// <returns>Returns true if the Provider has New Opener Provisional Allocation.</returns>
        public static bool CheckProviderHasFinalAllocation(this IFundingApiSearch fundingApiSearch)
        {
            var allCalcIDs = GetAllCalculations(fundingApiSearch);

            const int CalcID_FinalAllocation = 18;
            const int CalcID_AggregatedPredecessorProvisionalAllocation = 45;

            var objFinalAllocationAmount = allCalcIDs?.FirstOrDefault(a => a.TemplateCalculationId == CalcID_FinalAllocation)?.Value ?? "0";
            decimal.TryParse(Convert.ToString(objFinalAllocationAmount), out decimal decFinalAllocationAmount);

            var objAggregatedPredecessorProvisionalAllocationAmount = allCalcIDs?.FirstOrDefault(a => a.TemplateCalculationId == CalcID_AggregatedPredecessorProvisionalAllocation)?.Value ?? "0";
            decimal.TryParse(Convert.ToString(objAggregatedPredecessorProvisionalAllocationAmount), out decimal decAggregatedPredecessorProvisionalAllocationAmount);

            return (decFinalAllocationAmount + decAggregatedPredecessorProvisionalAllocationAmount) != 0;
        }

        /// <summary>
        /// Get Final Allocation School Type for UIFSM Report.
        /// </summary>
        /// <param name="fundingApiSearch">Provide Funding Api Search Data.</param>
        /// <returns>Returns true if the Provider has New Opener Provisional Allocation.</returns>
        public static string GetFinalAllocationSchoolType(this IFundingApiSearch fundingApiSearch)
        {
            var allCalcIDs = GetAllCalculations(fundingApiSearch);

            const int CalcID_FinalAllocationSchoolType = 74;

            return Convert.ToString(allCalcIDs
                                        ?.FirstOrDefault(a => a.TemplateCalculationId == CalcID_FinalAllocationSchoolType)
                                        ?.Value);
        }

        private static IEnumerable<CalculationNoNesting> GetAllCalculations(IFundingApiSearch fundingApiSearch)
        {
            try
            {
                double schemaVersion = SchemaVersionHelper.GetFundingValueDataValidatedSchemaVersion(fundingApiSearch);

                if (schemaVersion == 1.0)
                {
                    var fundingValueNested_1_0 = JsonConvert.DeserializeObject<FundingValueNested_1_0>(fundingApiSearch.FundingValue);

                    return GetAllCalculations(fundingValueNested_1_0);
                }
                else if (schemaVersion == 1.1)
                {
                    var fundingValueNested_1_1 = JsonConvert.DeserializeObject<FundingValueNested_1_1>(fundingApiSearch.FundingValue);

                    return fundingValueNested_1_1?.Calculations?.Values.ToList();
                }
                else if (schemaVersion == 1.2)
                {
                    var fundingValueNested_1_2 = JsonConvert.DeserializeObject<FundingValueNested_1_2>(fundingApiSearch.FundingValue);

                    return fundingValueNested_1_2?.Calculations;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static IEnumerable<Calculation> GetAllCalculations(FundingValueNested_1_0 fundingValueNested_1_0)
        {
            var calculations = new List<Calculation>();

            if (fundingValueNested_1_0.Calculations?.Any() == true)
            {
                calculations.AddRange(fundingValueNested_1_0.Calculations);
            }

            if (fundingValueNested_1_0.FundingLines?.Any() == true)
            {
                calculations.AddRange(fundingValueNested_1_0.FundingLines.SelectRecursive(a => a.FundingLines).SelectMany(a => a.Calculations).ToList());
            }

            if (fundingValueNested_1_0.FundingValue is not null)
            {
                calculations.AddRange(GetAllCalculations(fundingValueNested_1_0.FundingValue));
            }

            return calculations;
        }
    }
}
