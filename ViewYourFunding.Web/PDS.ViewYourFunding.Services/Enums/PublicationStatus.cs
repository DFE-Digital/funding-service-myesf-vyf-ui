namespace PDS.ViewYourFunding.Services.Enums
{
    /// <summary>
    /// Enumeration of statuses that a publication can have.
    /// </summary>
    public enum PublicationStatus
    {
        /// <summary>
        /// Disabled state - the publication is not visible in the service.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// Preview state - the publication will only be visible to logged-in users with permission, e.g. admin users.
        /// </summary>
        Preview = 1,

        /// <summary>
        /// Published state - the publication is publically visible in the service.
        /// </summary>
        Published = 2
    }
}