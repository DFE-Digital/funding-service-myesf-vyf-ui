using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// Class representing an organisation.
    /// For example a school, a Local Authority or a Multi-Academy Trust.
    /// </summary>
    public class Organisation
    {
        /// <summary>
        /// Gets or sets the organisation identifiers.
        /// </summary>
        public IEnumerable<OrganisationIdentifier> Identifiers { get; set; }

        /// <summary>
        /// Gets or sets the organisation name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the organisation type.
        /// </summary>
        public string OrganisationType { get; set; }

        /// <summary>
        /// Gets or sets the parent organisation.
        /// For example, if this organisation is an Academy then the parent could be the MAT.
        /// </summary>
        public Organisation ParentOrganisation { get; set; }

        /// <summary>
        /// Gets or sets the child organisations.
        /// For example, if this organisation is a MAT then the children would be its academies.
        /// </summary>
        public IEnumerable<Organisation> ChildOrganisations { get; set; }
    }
}