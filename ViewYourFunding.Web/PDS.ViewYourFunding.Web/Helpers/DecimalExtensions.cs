namespace PDS.ViewYourFunding.Web.Helpers
{
    /// <summary>
    /// Extension class for decimal related modifications.
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Extension method to convert a decimal into a GB currency formatted string.
        /// </summary>
        /// <param name="decimalValue">The decimal value.</param>
        /// <returns>A decimal as a GB currency formatted string.</returns>
        public static string ToGBCurrency(this decimal decimalValue)
        {
            var formatProvider = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
            return decimalValue.ToString("C", formatProvider);
        }

        /// <summary>
        /// Extension method to convert a decimal into a GB currency formatted string without a decimal place.
        /// </summary>
        /// <param name="decimalValue">The decimal value.</param>
        /// <returns>A decimal as a GB currency formatted string without decimal places.</returns>
        public static string ToGBCurrencyWithoutDecimalPlace(this decimal decimalValue)
        {
            var formatProvider = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
            return decimalValue.ToString("C0", formatProvider);
        }

        /// <summary>
        /// Extension method to convert a decimal into a GB currency formatted string, that doesn't have trailing zeroes.
        /// E.g. 10.00 will return £10 and 123456.99 will return £123,456.99.
        /// </summary>
        /// <param name="decimalValue">The decimal value.</param>
        /// <returns>A decimal as a GB currency formatted string without trailing zeroes.</returns>
        public static string ToGBCurrencyWithoutTrailingZeroes(this decimal decimalValue)
        {
            var convertedToCurrency = decimalValue.ToGBCurrency();
            return convertedToCurrency.EndsWith(".00") ? convertedToCurrency.Split('.')[0] : convertedToCurrency;
        }

        /// <summary>
        /// Extension method to convert a decimal into a 2 DP formatted string, that doesn't have trailing zeroes.
        /// e.g 12.00 will return 12, 123456.01 will return 123,456.01.
        /// </summary>
        /// <param name="decimalValue">The decimal value.</param>
        /// <returns>A decimal as a 2 DP formatted string without trailing zeroes.</returns>
        public static string To2DPWithoutTrailingZeroes(this decimal decimalValue)
        {
            var convertedToCurrency = decimalValue.ToGBCurrencyWithoutTrailingZeroes();
            return convertedToCurrency.Replace("£", string.Empty);
        }

        /// <summary>
        /// Extension method to convert a decimal into a 2 DP formatted percentage string, that doesn't have trailing zeroes.
        /// </summary>
        /// <param name="decimalValue">The decimal value.</param>
        /// <returns>A decimal as a 2 DP formatted percentage string.</returns>
        public static string ToPercentageWith2DecimalPlaces(this decimal decimalValue)
        {
            return $"{decimalValue.To2DPWithoutTrailingZeroes()}%";
        }
    }
}