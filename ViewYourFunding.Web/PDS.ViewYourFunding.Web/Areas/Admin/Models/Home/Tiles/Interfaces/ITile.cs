namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces
{
    /// <summary>
    /// Defines properties and methods associated with a tile.
    /// </summary>
    public interface ITile
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        string Id { get; }

        /// <summary>
        /// Gets the href.
        /// </summary>
        /// <value>
        /// The href.
        /// </value>
        string Href { get; }

        /// <summary>
        /// Gets the heading text.
        /// </summary>
        /// <value>
        /// The heading text.
        /// </value>
        string HeadingText { get; }

        /// <summary>
        /// Gets the body text.
        /// </summary>
        /// <value>
        /// The body text.
        /// </value>
        string BodyText { get; }

        /// <summary>
        /// Determines whether the specified user context is available.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        ///   <c>true</c> if the specified user context is available; otherwise, <c>false</c>.
        /// </returns>
        bool IsAvailable(IUserContext userContext);

        /// <summary>
        /// Gets or sets the alert.
        /// </summary>
        /// <value>
        /// The alert.
        /// </value>
        ITileAlert Alert { get; set; }

        /// <summary>
        /// Determines whether this instance has alert.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has alert; otherwise, <c>false</c>.
        /// </returns>
        bool HasAlert();

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        int Order { get; }

        /// <summary>
        /// Gets the name of the Css class.
        /// </summary>
        /// <value>
        /// The name of the CSS.
        /// </value>
        string ClassName { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is admin tile.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is admin tile; otherwise, <c>false</c>.
        /// </value>
        bool IsAdminTile { get; }
    }
}