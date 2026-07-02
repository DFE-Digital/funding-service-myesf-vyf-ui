namespace PDS.ViewYourFunding.Repositories.Enums
{
    /// <summary>
    /// The types of setting that the system supports.
    /// </summary>
    public enum ToggleSettingType
    {
        /// <summary>
        /// A setting to determine if the Digital allocations feature is available or not.
        /// </summary>
        DisplayDigitalAllocations = 0,

        /// <summary>
        /// A setting used by VYF to toggle between the current Funding (Allocations) API and the new OAuth secured Funding API.
        /// </summary>
        UseSecureViewYourFundingAPi = 1,
    }
}