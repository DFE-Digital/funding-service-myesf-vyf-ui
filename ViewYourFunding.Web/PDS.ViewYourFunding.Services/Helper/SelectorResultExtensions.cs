namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// The selector result extension class.
    /// </summary>
    public static class SelectorResultExtensions
    {
        /// <summary>
        /// Converts to boolean yes or no.
        /// </summary>
        /// <param name="selectorResult">The selector result.</param>
        /// <returns>Yes Or No result.</returns>
        public static string ToBooleanYesOrNo(this object selectorResult)
        {
            bool.TryParse(selectorResult?.ToString(), out var result);

            if (result)
            {
                return "Yes";
            }

            return "No";
        }

        /// <summary>
        /// Converts to the sparsity methodology display value.
        /// </summary>
        /// <param name="selectorResult">The selector result.</param>
        /// <returns>The sparsity methodology display value.</returns>
        public static string ToSparsityMethodologyDisplayValue(this int selectorResult)
        {
            switch (selectorResult)
            {
                case 0:
                    return "Lump sum";
                case 1:
                    return "Taper";
                default:
                    return "National funding formula";
            }
        }

        /// <summary>
        /// Converts to the sparsity methodology display value for pdfs.
        /// </summary>
        /// <param name="selectorResult">The selector result.</param>
        /// <returns>The sparsity methodology display value.</returns>
        public static string ToSparsityMethodologyDisplayValuePdfs(this string selectorResult)
        {
            switch (selectorResult)
            {
                case "Lump sum":
                    return "Fixed";
                case "Taper":
                    return "Tapered";
                default:
                    return "NFF";
            }
        }
    }
}