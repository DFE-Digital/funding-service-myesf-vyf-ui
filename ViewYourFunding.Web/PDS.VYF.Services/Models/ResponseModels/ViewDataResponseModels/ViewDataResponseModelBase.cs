namespace PDS.VYF.Services.Models.ResponseModels.ViewDataResponseModels
{
    /// <summary>
    /// The view data response model base.
    /// </summary>
    public abstract class ViewDataResponseModelBase
    {
        /// <summary>
        /// Gets or sets the name of the organization.
        /// </summary>
        /// <value>
        /// The name of the organization.
        /// </value>
        public string? OrganizationName { get; set; }

        /// <summary>
        /// Gets or sets the organization urn.
        /// </summary>
        /// <value>
        /// The organization urn.
        /// </value>
        public string? OrganizationUrn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is valid URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid URL; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has user have right access.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has user have right access; otherwise, <c>false</c>.
        /// </value>
        public bool HasUserHaveRightAccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has funding data exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has funding data exists; otherwise, <c>false</c>.
        /// </value>
        public bool HasFundingDataExists { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has parent access child URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has parent access child URL; otherwise, <c>false</c>.
        /// </value>
        public bool HasParentAccessChildUrl { get; set; }
    }
}
