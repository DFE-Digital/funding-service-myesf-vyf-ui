namespace PDS.ViewYourFunding.Services.Enums
{
    /// <summary>
    /// The options for variance comparison.
    /// </summary>
    public enum VarianceSelectionOption
    {
        /// <summary>
        /// Calculate variance with previous year final statement.
        /// </summary>
        FinalStatementPreviousYear,

        /// <summary>
        /// Calculate variance with current year previous statement.
        /// </summary>
        PreviousStatementCurrentYear,

        /// <summary>
        /// Don't compare and show variance.
        /// </summary>
        NoComparison
    }
}
