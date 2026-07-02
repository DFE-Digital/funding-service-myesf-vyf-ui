using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Services
{
    /// <summary>
    /// Implement this interface to get tile alert text based on criteria dependent on external source specific to a tile.
    /// </summary>
    public interface IAlertMessageService
    {
        /// <summary>
        /// Gets the alert object.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Concrete object that implements <see cref="ITileAlert"/>.</returns>
        ITileAlert GetAlert(IUserContext userContext = null);
    }
}