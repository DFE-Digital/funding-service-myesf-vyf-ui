using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Models;
using System;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Services
{
    /// <summary>
    /// Gets any alerts that need to be displayed in the <see cref="DigitalAllocationAlertService"/> tile.
    /// </summary>
    /// <seealso cref="AlertMessageService" />
    /// <seealso cref="IAlertMessageService" />
    public class DigitalAllocationAlertService : AlertMessageService, IAlertMessageService
    {
        /// <summary>
        /// Gets the alert to be displayed in the digital allocation tile.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// The Tile alert.
        /// </returns>
        public override ITileAlert GetAlert(IUserContext userContext = null)
        {
            var unreadAllocationsCount = GetUnreadAllocationsCount(userContext);

            if (unreadAllocationsCount == 0)
            {
                return null;
            }

            if (unreadAllocationsCount == 1)
            {
                return new TileAlert { AlertText = $"{unreadAllocationsCount} new statement" };
            }

            return new TileAlert { AlertText = $"{unreadAllocationsCount} new statements" };
        }

        /// TODO when we do allocation tiles.
        /// <summary>
        /// Gets the unread adults allocations count associated with a given provider and user.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>The count of unread allocations.</returns>
        /// <exception cref="Exception">User context is missing and required.</exception>
        private int GetUnreadAllocationsCount(IUserContext userContext)
        {
            if (userContext == null)
            {
                throw new Exception("User context is missing and required!");
            }

            return 0;
        }
    }
}