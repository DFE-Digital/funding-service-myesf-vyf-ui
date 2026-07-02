namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSettingAdmin
{
    /// <summary>
    /// The view model for the global setting edit page.
    /// </summary>
    public class GlobalSettingEditViewModel : GlobalSettingPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the page title.
        /// </summary>
        public override string ContentTitle => string.Empty;

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion
    }
}