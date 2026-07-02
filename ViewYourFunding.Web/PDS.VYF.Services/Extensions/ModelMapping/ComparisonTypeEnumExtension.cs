using PDS.ViewYourFunding.Services.Enums;
using PDS.VYF.Services.Enums;

namespace PDS.VYF.Services.Extensions.ModelMapping
{
    /// <summary>
    /// The Comparison Type Enum Extensions.
    /// </summary>
    public static class ComparisonTypeEnumExtension
    {
        /// <summary>
        /// Converts to.
        /// </summary>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>The VarianceSelectionOption.</returns>
        public static VarianceSelectionOption ConvertToVarianceSelectionOption(this ComparisonTypeEnum comparisonType)
        {
            return comparisonType switch
            {
                ComparisonTypeEnum.PreviousStatementCurrentYear => VarianceSelectionOption.PreviousStatementCurrentYear,
                ComparisonTypeEnum.FinalStatementPreviousYear => VarianceSelectionOption.FinalStatementPreviousYear,
                _ => VarianceSelectionOption.NoComparison
            };
        }
    }
}
