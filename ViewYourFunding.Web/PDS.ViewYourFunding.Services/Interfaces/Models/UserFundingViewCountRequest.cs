using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// Request for number of unread new and updated provider fundings for an user.
    /// </summary>
    public class UserFundingViewCountRequest
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the funding ids and statement versions available for the user.
        /// </summary>
        public List<FundingVersionDetail> FundingVersionDetails { get; set; }
    }
}
