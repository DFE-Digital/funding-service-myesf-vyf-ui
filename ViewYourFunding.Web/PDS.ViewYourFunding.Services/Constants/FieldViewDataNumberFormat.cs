namespace PDS.ViewYourFunding.Services.Constants
{
    /// <summary>
    /// Field view data number formats.
    /// </summary>
    public static class FieldViewDataNumberFormat
    {
        /// <summary>
        /// The percentage rate with 2 dp.
        /// </summary>
        public const string PercentageRateWith2DP = nameof(PercentageRateWith2DP);

        /// <summary>
        /// GB Currency format without trailing zeroes (e.g. £1.23 or £1).
        /// </summary>
        public const string GBCurrencyWithoutTrailingZeroes = nameof(GBCurrencyWithoutTrailingZeroes);

        /// <summary>
        /// Weighting format without trailing zeroes (e.g. 1.230004 or 1).
        /// </summary>
        public const string WeightingWithoutTrailingZeroes = nameof(WeightingWithoutTrailingZeroes);


        /// <summary>
        /// GB Currency format (e.g. £1.23 or £1.00).
        /// </summary>
        public const string GBCurrency = nameof(GBCurrency);

        /// <summary>
        /// 2 DP number without trailing zeroes (e.g. 1.23 or 1).
        /// </summary>
        public const string TwoDPWithoutTrailingZeroes = nameof(TwoDPWithoutTrailingZeroes);

        /// <summary>
        /// A percentage with 1 decimal place (e.g. 1.2%).
        /// </summary>
        public const string PercentageWith1DecimalPlace = nameof(PercentageWith1DecimalPlace);

        /// <summary>
        /// A percentage with 2 decimal places (e.g. 1.23).
        /// </summary>
        public const string PercentageWith2DecimalPlaces = nameof(PercentageWith2DecimalPlaces);

        /// <summary>
        /// A percentage with significant decimal places (e.g. 1.23%, 1.2%, 1%).
        /// </summary>
        public const string PercentageWithSignificantDecimalPlaces = nameof(PercentageWithSignificantDecimalPlaces);

        /// <summary>
        /// A GB currency without decimal places (e.g. £7).
        /// </summary>
        public const string GBCurrencyWithoutDecimalPlace = nameof(GBCurrencyWithoutDecimalPlace);

        /// <summary>
        /// A format that has thousands seperators, but no trailing zeroes (e.g. 1345.78 becomes 1,345.78 and 1444 becomes 1,444).
        /// </summary>
        public const string ThousandsSeperatedNoTrailingZeroes = nameof(ThousandsSeperatedNoTrailingZeroes);

        /// <summary>
        /// A format that has thousands seperators, with 2 dp (e.g. 1345.78 becomes 1,345.78 and 1444 becomes 1,444.00).
        /// </summary>
        public const string ThousandsSeperated2DP = nameof(ThousandsSeperated2DP);

        /// <summary>
        /// A format that is used  for rates with 2DP.
        /// </summary>
        public const string RateWith2DP = nameof(RateWith2DP);

        /// <summary>
        /// A format that has thousands seperators, but no trailing zeroes (e.g. 22.78 becomes 22 and 1444 becomes 1,444).
        /// </summary>
        public const string ThousandsSeparatedNoDP = nameof(ThousandsSeparatedNoDP);

        /// <summary>
        /// 1 DP number (e.g. 22.78 becomes 22.8).
        /// </summary>
        public const string OneDPWithoutTrailingZeroes = nameof(OneDPWithoutTrailingZeroes);

        /// <summary>
        /// A format that has thousands seperators, with 5 dp (22.500289 will return 22.50029, 0.512365 will return 0.51236).
        /// </summary>
        public const string ThousandsSeparated5DP = nameof(ThousandsSeparated5DP);

        /// <summary>
        /// A format zero decimal places.
        /// </summary>
        public const string ZeroDecimalPlaces = nameof(ZeroDecimalPlaces);

        /// <summary>
        /// 6 DP number (e.g. 1.2345678 becomes 1.234568).
        /// </summary>
        public const string SixDPWithoutTrailingZeroes = nameof(SixDPWithoutTrailingZeroes);
    }
}