namespace PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices
{
    using PDS.VYF.Services.Enums;
    using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
    using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

    /// <summary>
    /// Represents the interface for child API client services.
    /// </summary>
    public interface IChildApiClientServices
    {
        /// <summary>
        /// Checks if the statement is the latest.
        /// </summary>
        /// <param name="id">The ID of the statement.</param>
        /// <param name="childRequest">The child search API request model.</param>
        /// <param name="setLoggedInFundingStreamPeriods">A flag indicating whether to set logged in funding stream periods.</param>
        /// <param name="fundingStreamPeriod">The list of funding stream periods.</param>
        /// <returns>A task representing the asynchronous operation that returns true if the statement is the latest, otherwise false.</returns>
        Task<bool> IsLatestStatement(string id, ChildSearchApiRequestModel childRequest, bool setLoggedInFundingStreamPeriods, List<string>? fundingStreamPeriod = null);

        /// <summary>
        /// Checks if the statement is the latest.
        /// </summary>
        /// <param name="parentUKPRN">The parent UKPRN.</param>
        /// <param name="childUKPRN">All children UKPRNs for parent.</param>
        /// <returns>A task representing the asynchronous operation that returns true if the statement is the latest, otherwise false.</returns>
        Task<List<string>> GetCurrentChildUkprnsForParent(string parentUKPRN, List<string>? childUKPRN = null);

        /// <summary>
        /// Searches for children based on the child search API request model.
        /// </summary>
        /// <param name="childRequest">The child search API request model.</param>
        /// <param name="setLoggedInFundingStreamPeriods">A flag indicating whether to set logged in funding stream periods.</param>
        /// <param name="fundingStreamPeriod">The list of funding stream periods.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of logged in child models.</returns>
        Task<List<LoggedInChildModel>> SearchChild(ChildSearchApiRequestModel childRequest, bool setLoggedInFundingStreamPeriods, List<string>? fundingStreamPeriod = null);

        /// <summary>
        /// Searches for children of a parent based on the parent UKPRN and child search API request model.
        /// </summary>
        /// <param name="parentUKPRN">The UKPRN of the parent.</param>
        /// <param name="childRequest">The child search API request model.</param>
        /// <param name="setLoggedInFundingStreamPeriods">A flag indicating whether to set logged in funding stream periods.</param>
        /// <param name="fundingStreamPeriod">The list of funding stream periods.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of logged in child models.</returns>
        Task<List<LoggedInChildModel>> SearchChildrenOfAParent(string parentUKPRN, ChildSearchApiRequestModel childRequest, bool setLoggedInFundingStreamPeriods, List<string>? fundingStreamPeriod = null);

        /// <summary>
        /// Gets the child comparison.
        /// </summary>
        /// <param name="childComparisonRequest">The child comparison request.</param>
        /// <returns>List of ChildComparisonResponse.</returns>
        Task<Dictionary<ComparisonTypeEnum, ChildComparisonResponse>> GetChildComparison(ChildComparisonRequest childComparisonRequest);

        /// <summary>
        /// Return latest funding period is the latest.
        /// </summary>
        /// <param name="childRequest">The child search API request model.</param>
        /// <returns>A task representing the asynchronous operation that returns latest funding period for available data.</returns>
        Task<List<string>> LatestFundingPeriod(ChildSearchApiRequestModel childRequest);
    }
}
