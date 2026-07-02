using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// Class representing the response from the user contact lookup API.
    /// </summary>
    public class UserContactLookupResponse
    {
        /// <summary>
        /// Gets or sets the UKPRN.
        /// </summary>
        public string Ukprn { get; set; }

        /// <summary>
        /// Gets or sets the list of users found.
        /// </summary>
        public IEnumerable<UserContact> Users { get; set; }
    }
}