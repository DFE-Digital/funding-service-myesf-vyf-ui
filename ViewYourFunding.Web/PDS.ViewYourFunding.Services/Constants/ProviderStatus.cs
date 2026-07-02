using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Constants
{
    /// <summary>
    /// Provider Statuses class.
    /// </summary>
    public static class ProviderStatus
    {
        /// <summary>
        /// The indicative status.
        /// </summary>
        public static List<string> IndicativeStatuses = new List<string> { "Proposed to open", "Pending approval" };

        /// <summary>
        /// The open status.
        /// </summary>
        public const string Open = "Open";

        /// <summary>
        /// The closed status.
        /// </summary>
        public const string Closed = "Closed";
    }
}