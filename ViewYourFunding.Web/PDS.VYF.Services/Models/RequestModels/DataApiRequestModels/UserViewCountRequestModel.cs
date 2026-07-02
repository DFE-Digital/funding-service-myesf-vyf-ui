namespace PDS.VYF.Services.Models.RequestModels.DataApiRequestModels
{
    /// <summary>
    /// Represents a request model for user view count.
    /// </summary>
    public class UserViewCountRequestModel
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string UserId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the list of child statements.
        /// </summary>
        public List<ChildStatementModel> ChildStatements { get; set; } = default!;
    }
}
