using PDS.ViewYourFunding.Repositories.Enums;

namespace PDS.ViewYourFunding.Services.Helper
{
    /// <summary>
    /// Helpers for FetchData.
    /// </summary>
    public static class FetchDataHelper
    {
        /// <summary>
        /// Gets all fetch data types.
        /// </summary>
        public static FetchData[] AllFetchData
        {
            get
            {
                return new[]
                {
                    FetchData.Publications,
                    FetchData.SettingValues_Setting,
                    FetchData.NextPayments_NextPaymentType,
                    FetchData.NextPaymentTypes_NextPayments,
                    FetchData.Publications_PublicationLayouts
                };
            }
        }
    }
}