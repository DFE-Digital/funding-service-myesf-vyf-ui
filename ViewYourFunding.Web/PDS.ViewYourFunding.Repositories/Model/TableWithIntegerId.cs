namespace PDS.ViewYourFunding.Repositories.Model
{
    /// <summary>
    /// A table with an integer id.
    /// </summary>
    public abstract class TableWithIntegerId
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public abstract int Id { get; set; }
    }
}