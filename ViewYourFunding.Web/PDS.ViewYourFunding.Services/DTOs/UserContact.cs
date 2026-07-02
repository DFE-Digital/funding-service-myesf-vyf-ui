using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.DTOs
{
    /// <summary>
    /// Class representing user contact information.
    /// </summary>
    public class UserContact
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the list of role codes held by the user, e.g. ContractManager.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}