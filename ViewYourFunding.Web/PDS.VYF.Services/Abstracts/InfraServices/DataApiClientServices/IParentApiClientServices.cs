namespace PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices
{
    using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    public interface IParentApiClientServices
    {
        /// <summary>
        /// Searches the parent.
        /// </summary>
        /// <param name="parentRequest">The parent request.</param>
        /// <param name="setLoggedInFundingStreamPeriods">if set to <c>true</c> [set logged in funding stream period].</param>
        /// <param name="fundingStreamPeriod">The funding stream period.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<LoggedInParentModel>> SearchParent(ParentSearchApiRequestModel parentRequest, bool setLoggedInFundingStreamPeriods, List<string>? fundingStreamPeriod = null);

        /// <summary>
        /// Determines whether the specified ukprn is parent.
        /// </summary>
        /// <param name="ukprn">The ukprn.</param>
        /// <param name="setLoggedInFundingStreamPeriods">if set to <c>true</c> [set relevant funding streams].</param>
        /// <param name="fundingStreamPeriod">The funding stream period.</param>
        /// <returns>
        /// True if the given UKPRN is belongs to parent (MAT / LA).
        /// </returns>
        Task<bool> IsParent(string ukprn, bool setLoggedInFundingStreamPeriods, List<string>? fundingStreamPeriod = null);

        /// <summary>
        /// Determines whether [is my child] [the specified parent ukprn].
        /// </summary>
        /// <param name="parentUKPRN">The parent ukprn.</param>
        /// <param name="childUKPRN">The child ukprn.</param>
        /// <param name="setLoggedInFundingStreamPeriods">if set to <c>true</c> [set relavant funding streams].</param>
        /// <param name="fundingStreamPeriod">The funding stream period.</param>
        /// <returns>
        /// True if given childUkprn is child of given parentUkprn.
        /// </returns>
        Task<bool> IsMyChild(string parentUKPRN, string childUKPRN, bool setLoggedInFundingStreamPeriods, List<string>? fundingStreamPeriod = null);
    }
}
