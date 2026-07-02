using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Models;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Services
{
    /// <summary>
    /// Abstract class for retrieving alerts.
    /// </summary>
    public abstract class AlertMessageService
    {
        /// <summary>
        /// Gets the alert.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>null as default unless derived class overrides this method.</returns>
        public virtual ITileAlert GetAlert(IUserContext userContext = null)
        {
            return null;
        }

        /// <summary>
        /// Generates the alert based on entity count.
        /// </summary>
        /// <param name="entityCount">The entity count.</param>
        /// <returns>A tile alert.</returns>
        protected ITileAlert GenerateAlert(int entityCount)
        {
            return entityCount > 1 ? CreateTileAlert($"{entityCount} new alerts") : entityCount == 1 ? CreateTileAlert($"{entityCount} new alert") : null;
        }

        private static ITileAlert CreateTileAlert(string alertMessage)
        {
            return new TileAlert { AlertText = alertMessage };
        }
    }
}