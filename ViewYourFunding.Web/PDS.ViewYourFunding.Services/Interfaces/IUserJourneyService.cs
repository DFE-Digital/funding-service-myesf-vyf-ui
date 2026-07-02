using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// An interface exposing get operations for settings in the View Your Funding area.
    /// </summary>
    public interface IUserJourneyService
    {
        /// <summary>
        /// Get publications for a specific funding stream.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <returns>A list of publications.</returns>
        Task<IList<Publication>> GetPublications(string fundingStreamCode);

        /// <summary>
        /// Gets all of the settings for a given funding stream.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <returns>A list containing all of the settings for the given funding stream.</returns>
        Task<IList<SettingValue>> GetSettings(string fundingStreamCode);

        /// <summary>
        /// Gets the funding stream with the given code.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <returns>The funding stream.</returns>
        Task<FundingStream> GetFundingStream(string fundingStreamCode);

        /// <summary>
        /// Gets all funding streams.
        /// </summary>
        /// <returns>A list of funding streams.</returns>
        Task<IList<FundingStream>> GetFundingStreams();
    }
}