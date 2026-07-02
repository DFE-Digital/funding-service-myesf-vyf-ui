namespace PDS.VYF.Services.Models.RequestModels.DataApiRequestModels
{
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    /// <summary>
    /// The Class for Parent.
    /// </summary>
    /// <seealso cref="SearchApiRequestBase" />
    public class ParentSearchApiRequestModel : SearchApiRequestBase<LoggedInParentModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParentSearchApiRequestModel"/> class.
        /// </summary>
        public ParentSearchApiRequestModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentSearchApiRequestModel"/> class.
        /// </summary>
        /// <param name="hasToBeLatestFunding">if set to <c>true</c> [has to be latest funding].</param>
        /// <param name="ukprns">The UKPRNs.</param>
        public ParentSearchApiRequestModel(bool hasToBeLatestFunding, params string[] ukprns)
        {
            this.HasToBeLatestFunding = hasToBeLatestFunding;
            this.ListOfUKPRNs = ukprns.ToList();
        }
    }
}
