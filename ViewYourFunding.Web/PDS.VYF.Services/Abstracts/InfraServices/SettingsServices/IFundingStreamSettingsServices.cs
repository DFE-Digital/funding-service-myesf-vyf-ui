namespace PDS.VYF.Services.Abstracts.InfraServices.SettingsServices
{
    using PDS.ViewYourFunding.Services.Models;

    /// <summary>
    /// Represents the interface for funding stream settings services.
    /// </summary>
    public interface IFundingStreamSettingsServices
    {
        /// <summary>
        /// Gets the list of funding stream periods for the logged-in user.
        /// </summary>
        /// <param name="isForParent">Indicates whether the funding stream periods are for the parent user.</param>
        /// <returns>The list of funding stream periods.</returns>
        Task<List<string>> GetLoggedInFundingStreamPeriod(bool isForParent);

        /// <summary>
        /// Gets the funding stream by its code.
        /// </summary>
        /// <param name="fundingStreamCode">The code of the funding stream.</param>
        /// <returns>The funding stream.</returns>
        Task<FundingStream?> GetFundingStream(string fundingStreamCode);

        /// <summary>
        /// Gets the dictionary of funding stream codes and names for the logged-in user.
        /// </summary>
        /// <param name="isForParent">Indicates whether the funding stream codes and names are for the parent user.</param>
        /// <returns>The dictionary of funding stream codes and names.</returns>
        Task<Dictionary<string, string>> GetFundingStreamCodeAndName(bool isForParent);

        /// <summary>
        /// Gets the latest publication for the specified funding stream and funding period.
        /// </summary>
        /// <param name="fundingStreamCode">The code of the funding stream.</param>
        /// <param name="fundingPeriodCode">The code of the funding period.</param>
        /// <returns>The latest publication.</returns>
        Task<Publication?> GetLatestPublication(string fundingStreamCode, string fundingPeriodCode);

        /// <summary>
        /// Gets the dictionary of logged-in funding streams.
        /// </summary>
        /// <param name="isForParent">Indicates whether the funding streams are for the parent user.</param>
        /// <returns>The dictionary of logged-in funding streams.</returns>
        Task<Dictionary<string, FundingStream>> GetLoggedInFundingStreams(bool isForParent);

        /// <summary>
        /// Gets the list of funding stream periods from the EmailEnabledFundingPeriod settings.
        /// </summary>
        /// <returns>The list of funding stream periods.</returns>
        Task<List<string>> GetEmailEnabledFundingStreamPeriod();
    }
}
