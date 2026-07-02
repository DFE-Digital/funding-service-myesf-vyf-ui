namespace PDS.VYF.Services.Models.RequestModels.DataApiRequestModels
{
    /// <summary>
    /// Represents a model for child statement.
    /// </summary>
    public class ChildStatementModel
    {
        /// <summary>
        /// Gets or sets the child ID.
        /// </summary>
        public string ChildId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the statement type.
        /// </summary>
        public string StatementType { get; set; } = default!;
    }
}
