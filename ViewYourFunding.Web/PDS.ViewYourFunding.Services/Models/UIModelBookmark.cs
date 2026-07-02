namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// A bookmark.
    /// </summary>
    public class UIModelBookmark
    {
        /// <summary>
        /// Gets or sets the title of the bookmark.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text to find.
        /// </summary>
        public string TextToFind { get; set; }

        /// <summary>
        /// Gets or sets the instance to match (1 indexed).
        /// </summary>
        public int InstanceToMatch { get; set; } = 1;
    }
}