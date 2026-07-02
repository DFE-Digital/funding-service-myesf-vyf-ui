using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// An interface exposing get/set operations for the admin screen settings.
    /// </summary>
    public interface IAdminSettingsService
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
        /// Gets all of the settings for a given funding stream.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream id.</param>
        /// <returns>A list containing all of the settings for the given funding stream.</returns>
        Task<IList<SettingValue>> GetSettingsById(int fundingStreamId);

        /// <summary>
        /// Gets the funding stream with the given code.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fetchData">Show we fetch properties or not.</param>
        /// <returns>The funding stream.</returns>
        Task<FundingStream> GetFundingStream(string fundingStreamCode, params FetchData[] fetchData);

        /// <summary>
        /// Gets the funding stream with the given id.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream id.</param>
        /// <param name="fetchData">Show we fetch properties or not.</param>
        /// <returns>The funding stream.</returns>
        Task<FundingStream> GetFundingStreamById(int fundingStreamId, params FetchData[] fetchData);

        /// <summary>
        /// Gets all the funding streams.
        /// </summary>
        /// <param name="fetchData">Show we fetch properties or not.</param>
        /// <returns>The List of Funding streams.</returns>
        Task<IReadOnlyList<FundingStream>> GetAllFundingStreams(params FetchData[] fetchData);

        /// <summary>
        /// Gets all Next Payment Types.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream id.</param>
        /// <returns>The List of Next Payment Types.</returns>
        Task<IList<NextPaymentType>> GetNextPaymentTypes(int fundingStreamId);

        /// <summary>
        /// Gets all Next Payment Types.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream id.</param>
        /// <returns>The List of Next Payments.</returns>
        Task<IList<NextPayment>> GetNextPayments(int fundingStreamId);

        /// <summary>
        /// Gets the Setting value with given funding stream id and setting id.
        /// </summary>
        /// <param name="fundingStreamId">FundingStreamId.</param>
        /// <param name="settingId">settingId.</param>
        /// <returns>The setting value.</returns>
        Task<SettingValue> GetSettingById(int fundingStreamId, int settingId);
    }
}