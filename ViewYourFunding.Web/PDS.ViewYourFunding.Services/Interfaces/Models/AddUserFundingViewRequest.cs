using System;

namespace PDS.ViewYourFunding.Services.Interfaces.Models
{
    /// <summary>
    /// Request object to add user funding view details.
    /// </summary>
    public class AddUserFundingViewRequest
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the funding identifier.
        /// </summary>
        public string FundingId { get; set; }

        /// <summary>
        /// Gets or sets the datetime when the user visited the allocation statement.
        /// </summary>
        public DateTime ViewedAt { get; set; }
    }
}
