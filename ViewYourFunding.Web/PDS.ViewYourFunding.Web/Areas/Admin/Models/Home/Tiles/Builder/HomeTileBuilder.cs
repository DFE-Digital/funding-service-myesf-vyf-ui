using Pds.Core.Common.Identity.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Models;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Builder
{
    /// <summary>
    /// Concrete builder responsible for building tiles for a specify <see cref="HomeTileBuilder"/>.
    /// </summary>
    /// <seealso cref="ITileBuilder" />
    public class HomeTileBuilder : ITileBuilder
    {
        private readonly IEnumerable<ITile> _tiles;
        private readonly IUserContext _userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeTileBuilder"/> class.
        /// </summary>
        /// <param name="tiles">The tiles.</param>
        public HomeTileBuilder(IEnumerable<ITile> tiles)
        {
            _tiles = tiles;
            _userContext = new UserContext();
        }

        /// <summary>
        /// Builds the tiles that have passed the business rules.
        /// </summary>
        /// <returns>
        /// List of <see cref="ITile" />.
        /// </returns>
        public IList<ITile> BuildTiles()
        {
            var availableTiles = new List<ITile>();
            if (_tiles == null)
            {
                return availableTiles;
            }

            foreach (var tile in _tiles.OrderBy(t => t.Order))
            {
                if (tile.IsAvailable(_userContext))
                {
                    (tile as Tile).SetAlert(_userContext);
                    availableTiles.Add(tile);
                }
            }

            return availableTiles;
        }

        /// <summary>
        /// The user context available to all the tiles.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <see cref="ITileBuilder" />.
        /// </returns>
        public ITileBuilder ForUser(User user)
        {
            _userContext.User = user;
            return this;
        }
    }
}