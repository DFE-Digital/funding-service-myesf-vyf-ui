using Pds.Core.Common.Identity.Models;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces
{
    /// <summary>
    /// Represents the provider/user context used when applying rules.
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        User User { get; set; }
    }
}