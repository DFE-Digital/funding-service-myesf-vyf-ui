using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using PublicationStatus = PDS.ViewYourFunding.Services.Enums.PublicationStatus;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Strategies.LayoutManagement
{
    /// <summary>
    /// The preview layout action base class.
    /// </summary>
    public class PreviewLayoutActionBase
    {
        /// <summary>
        /// The admin settings service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewLayoutActionBase" /> class.
        /// </summary>
        /// <param name="adminSettingsService">The admin settings service.</param>
        public PreviewLayoutActionBase(
            IAdminSettingsService adminSettingsService)
        {
            _adminSettingsService = adminSettingsService;
        }

        /// <summary>
        /// Gets the funding stream by identifier.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="includedProperties">the properties to include in data call.</param>
        /// <returns>The funding stream.</returns>
        public async Task<Services.Models.FundingStream> GetFundingStreamById(int fundingStreamId, FetchData[] includedProperties = null)
        {
            return await _adminSettingsService.GetFundingStreamById(fundingStreamId, includedProperties);
        }

        /// <summary>
        /// Gets the published date path formatted.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>The Published Date Formatted.</returns>
        public string GetPublishedDatePathFormatted(Services.Models.FundingStream fundingStream)
        {
            return fundingStream.Publications?.OrderByDescending(publication => publication.PublishedDate)
                .FirstOrDefault(publication =>
                    publication.Status == PublicationStatus.Published ||
                    publication.Status == PublicationStatus.Preview)?.PublishedDatePathFormatted;
        }

        /// <summary>
        /// Gets the funding stream name path formatted.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <returns>The Funding stream name formatted.</returns>
        public string GetFundingStreamNamePathFormatted(Services.Models.FundingStream fundingStream)
        {
            return fundingStream.FundingStreamName.ToLower().Replace(" ", "-");
        }
    }
}