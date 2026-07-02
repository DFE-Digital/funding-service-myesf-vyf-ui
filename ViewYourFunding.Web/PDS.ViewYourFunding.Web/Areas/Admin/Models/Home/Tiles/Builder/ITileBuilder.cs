using Pds.Core.Common.Identity.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Builder
{
    /// <summary>
    /// Represents interface for ITileBuilder.
    /// </summary>
    public interface ITileBuilder
    {
        /// <summary>
        /// For the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><see cref="ITileBuilder"/>The tile builder.</returns>
        ITileBuilder ForUser(User user);

        /// <summary>
        /// Builds the tiles.
        /// </summary>
        /// <returns>List of <see cref="ITile"/>.</returns>
        IList<ITile> BuildTiles();
    }
}