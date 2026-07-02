using Pds.Core.Common.Identity.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Models
{
    /// <summary>
    /// Class to represent the user context who is expecting to see the tiles if any available.
    /// </summary>
    /// <seealso cref="UserContext" />
    public class UserContext : IUserContext
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public User User { get; set; }
    }
}