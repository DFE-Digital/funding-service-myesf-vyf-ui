using PDS.ViewYourFunding.Services.Enums;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>The organisation identifier.</summary>
    public class OrganisationIdentifier
    {
        /// <summary>
        /// Gets or sets the identifier type.
        /// </summary>
        public OrganisationIdentifierType Type { get; set; }

        /// <summary>
        /// Gets or sets the identifier value.
        /// </summary>
        public string Value { get; set; }
    }
}