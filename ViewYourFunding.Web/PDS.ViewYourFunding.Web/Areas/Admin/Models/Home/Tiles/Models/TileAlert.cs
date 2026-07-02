using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Models
{
    /// <summary>
    /// Defines attributes exposed to display a tile alert in the tile.
    /// </summary>
    /// <seealso cref="ITileAlert" />
    public class TileAlert : ITileAlert
    {
        /// <summary>
        /// Gets or sets the alert text.
        /// </summary>
        /// <value>
        /// The alert text.
        /// </value>
        public string AlertText { get; set; }
    }
}