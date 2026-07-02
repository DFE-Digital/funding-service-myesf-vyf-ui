namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces
{
    /// <summary>
    /// Interface for object that contains tile alert properties.
    /// </summary>
    public interface ITileAlert
    {
        /// <summary>
        /// Gets or sets the alert text.
        /// </summary>
        /// <value>
        /// The alert text.
        /// </value>
        string AlertText { get; set; }
    }
}