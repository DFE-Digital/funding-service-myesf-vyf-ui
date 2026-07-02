using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.DTOs
{
    /// TODO when the application uses the DFE sign in package we will need to discuss about a new version with the name properties.
    /// <summary>
    /// A class representing a user of the service.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether indicates whether or not the user has been authenticated.
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the UKPRN.
        /// </summary>
        public int? Ukprn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether indicates whether or not the user is an external user.
        /// </summary>
        public bool IsExternalUser { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider to which the user is associated.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's roles.
        /// </summary>
        public IReadOnlyCollection<string> Roles { get; set; }
    }
}
