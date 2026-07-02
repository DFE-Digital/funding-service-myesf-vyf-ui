namespace PDS.ViewYourFunding.Repositories.Enums
{
    /// <summary>
    /// The extra tables to fetch.
    /// </summary>
    public enum FetchData
    {
        /// <summary>
        /// The Publications table.
        /// </summary>
        Publications = 1,

        /// <summary>
        /// The SettingValues and Setting tables.
        /// </summary>
        SettingValues_Setting = 2,

        /// <summary>
        /// The NextPayments and NextPaymentType tables.
        /// </summary>
        NextPayments_NextPaymentType = 3,

        /// <summary>
        /// The NextPaymentTypes and NextPayments tables.
        /// </summary>
        NextPaymentTypes_NextPayments = 4,

        /// <summary>
        /// The Publications and PublicationLayouts tables.
        /// </summary>
        Publications_PublicationLayouts = 5
    }
}