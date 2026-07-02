namespace PDS.VYF.Services.Models.RequestModels.DataApiRequestModels
{
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    /// <summary>
    /// The Class for Child Request.
    /// </summary>
    /// <seealso cref="SearchApiRequestBase" />
    public class ChildSearchApiRequestModel : SearchApiRequestBase<LoggedInChildModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChildSearchApiRequestModel"/> class.
        /// </summary>
        public ChildSearchApiRequestModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildSearchApiRequestModel"/> class.
        /// </summary>
        /// <param name="hasToBeLatestFunding">if set to <c>true</c> [has to be latest funding].</param>
        /// <param name="ukprns">The UKPRNs.</param>
        public ChildSearchApiRequestModel(bool hasToBeLatestFunding, params string[] ukprns)
        {
            this.HasToBeLatestFunding = hasToBeLatestFunding;
            this.ListOfUKPRNs = ukprns.ToList();
        }

        /// <summary>
        /// Gets or sets the status changed date only.
        /// </summary>
        /// <value>
        /// The status changed date only.
        /// </value>
        public DateTime? StatusChangedDateOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [has IYO to be removed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has IYO to be removed]; otherwise, <c>false</c>.
        /// </value>
        public bool HasIYOToBeRemoved { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether [do find is latest].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [do find is latest]; otherwise, <c>false</c>.
        /// </value>
        public bool DoFindIsLatest { get; set; } = false;
    }
}
