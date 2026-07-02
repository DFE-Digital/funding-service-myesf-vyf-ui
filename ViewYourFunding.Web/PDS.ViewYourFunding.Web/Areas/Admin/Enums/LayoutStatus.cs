using System.ComponentModel.DataAnnotations;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Enums
{
    /// <summary>
    /// The enumeration of the Layout statuses.
    /// </summary>
    public enum LayoutStatus
    {
        /// <summary>
        /// Disabled state - the layout is not visible in the service.
        /// </summary>
        [Display(Name = "Assigned to a disabled publication")]
        Disabled = 0,

        /// <summary>
        /// Preview state - the layout will only be visible to logged-in users with permission, e.g. admin users.
        /// </summary>
        [Display(Name = "Assigned to a publication in preview")]
        Preview = 1,

        /// <summary>
        /// Published state - the layout is publicly visible in the service.
        /// </summary>
        [Display(Name = "Assigned to a publication in live")]
        Published = 2,

        /// <summary>
        /// Not assigned state - the layout is not assigned to a publication.
        /// </summary>
        [Display(Name = "Not Assigned")]
        NotAssigned = 4
    }
}