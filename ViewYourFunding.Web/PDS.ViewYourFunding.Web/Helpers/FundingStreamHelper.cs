using AutoMapper;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAdminModels = PDS.ViewYourFunding.Web.Areas.Admin.Models;

namespace PDS.ViewYourFunding.Web.Helpers
{
    /// <summary>
    /// Helper methods for working with funding streams.
    /// </summary>
    public static class FundingStreamHelper
    {
        /// <summary>
        /// Get the latest publication.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <param name="previewModeEnabled">Is preview mode enabled for the current user.</param>
        /// <param name="publishedDate">An optional published date to limit to.</param>
        /// <param name="fundingPeriodCode">An optional funding period code to limit to.</param>
        /// <param name="finalPublicationDate">An optional final publication date to limit to.</param>
        /// <returns>The latest publication.</returns>
        public static Publication GetLatestPublication(
            this FundingStream fundingStream,
            bool previewModeEnabled,
            DateTime? publishedDate = null,
            string fundingPeriodCode = null,
            DateTime? finalPublicationDate = null)
        {
            if (fundingStream.Publications == null)
            {
                return default;
            }

            return fundingStream.Publications
                .Where(IsValidPublication)
                .OrderByDescending(p => p.PublishedDate)
                .FirstOrDefault();

            bool IsValidPublication(Publication publication)
            {
                var publishedDateFilter = publishedDate == null || publication.PublishedDate == publishedDate;
                var publicationStatusFilter = publication.Status == PublicationStatus.Published || (previewModeEnabled && publication.Status == PublicationStatus.Preview);
                var fundingPeriodFilter = fundingPeriodCode == null || publication.FundingPeriodCode == fundingPeriodCode;
                var finalPublicationDateFilter = finalPublicationDate == null || publication.PublishedDate <= finalPublicationDate;

                return publishedDateFilter && publicationStatusFilter && fundingPeriodFilter && finalPublicationDateFilter;
            }
        }

        /// <summary>
        /// Get the earliest publication.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <param name="previewModeEnabled">Is preview mode enabled for the current user.</param>
        /// <param name="publishedDate">An optional published date to limit to.</param>
        /// <param name="fundingPeriodCode">An optional funding period code to limit to.</param>
        /// <param name="finalPublicationDate">An optional final publication date to limit to.</param>
        /// <returns>The earliest publication.</returns>
        public static Publication GetEarliestPublication(
            this FundingStream fundingStream,
            bool previewModeEnabled,
            DateTime? publishedDate = null,
            string fundingPeriodCode = null,
            DateTime? finalPublicationDate = null)
        {
            if (fundingStream.Publications == null)
            {
                return default;
            }

            return fundingStream.Publications
                .Where(IsValidPublication)
                .OrderBy(p => p.PublishedDate)
                .FirstOrDefault();

            bool IsValidPublication(Publication publication)
            {
                var publishedDateFilter = publishedDate == null || publication.PublishedDate == publishedDate;
                var publicationStatusFilter = publication.Status == PublicationStatus.Published || (previewModeEnabled && publication.Status == PublicationStatus.Preview);
                var fundingPeriodFilter = fundingPeriodCode == null || publication.FundingPeriodCode == fundingPeriodCode;
                var finalPublicationDateFilter = finalPublicationDate == null || publication.PublishedDate <= finalPublicationDate;

                return publishedDateFilter && publicationStatusFilter && fundingPeriodFilter && finalPublicationDateFilter;
            }
        }

        /// <summary>
        /// Get the latest publication.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <param name="previewModeEnabled">Is preview mode enabled for the current user.</param>
        /// <param name="publishedDate">An optional published date to limit to.</param>
        /// <param name="fundingPeriodCode">An optional funding period code to limit to.</param>
        /// <param name="finalPublicationDate">An optional final publication date to limit to.</param>
        /// <returns>The latest publication.</returns>
        public static List<Publication> GetPublicationsForPeriod(
            this FundingStream fundingStream,
            bool previewModeEnabled,
            DateTime? publishedDate = null,
            string fundingPeriodCode = null,
            DateTime? finalPublicationDate = null)
        {
            if (fundingStream.Publications == null)
            {
                return default;
            }

            return fundingStream.Publications
                .Where(IsValidPublication)
                .OrderByDescending(p => p.PublishedDate)
                .ToList();

            bool IsValidPublication(Publication publication)
            {
                var publishedDateFilter = publishedDate == null || publication.PublishedDate == publishedDate;
                var publicationStatusFilter = publication.Status == PublicationStatus.Published || (previewModeEnabled && publication.Status == PublicationStatus.Preview);
                var fundingPeriodFilter = fundingPeriodCode == null || publication.FundingPeriodCode == fundingPeriodCode;
                var finalPublicationDateFilter = finalPublicationDate == null || publication.PublishedDate <= finalPublicationDate;

                return publishedDateFilter && publicationStatusFilter && fundingPeriodFilter && finalPublicationDateFilter;
            }
        }

        /// <summary>
        /// Get all funding period codes for publications.
        /// </summary>
        /// <param name="fundingStream">The funding stream.</param>
        /// <param name="previewModeEnabled">Is preview mode enabled for the current user.</param>
        /// <returns>The list of funding period codes.</returns>
        public static IEnumerable<string> GetAllFundingPeriodCodesForPublications(this FundingStream fundingStream, bool previewModeEnabled)
        {
            return fundingStream.Publications
                .Where(publication => (publication.Status == PublicationStatus.Published
                    || (previewModeEnabled && publication.Status == PublicationStatus.Preview)))
                .Select(publication => publication.FundingPeriodCode).Distinct();
        }

        /// <summary>
        /// Convert a services funding stream to a web model funding stream.
        /// </summary>
        /// <param name="fundingStream">The services funding stream to convert.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A web model funding stream.</returns>
        public static Models.FundingStream.FundingStream AsWebModel(FundingStream fundingStream, IMapper mapper)
        {
            var webFundingStream = mapper.Map<Models.FundingStream.FundingStream>(fundingStream);

            if (webFundingStream == null)
            {
                return null;
            }

            webFundingStream.NationalUiLayoutId = fundingStream.SettingValues?
                .FirstOrDefault(sv => sv?.Setting?.ValueDataType == SettingValueDataType.NationalLayout)?.Value;
            webFundingStream.NationalSpreadsheetLayoutId = fundingStream.SettingValues?
                .FirstOrDefault(sv => sv?.Setting?.ValueDataType == SettingValueDataType.NationalSpreadsheetLayout)?.Value;

            return webFundingStream;
        }

        /// <summary>
        /// Convert a services funding stream to a web admin model funding stream.
        /// </summary>
        /// <param name="fundingStream">The services funding stream to convert.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A web admin model funding stream.</returns>
        public static WebAdminModels.FundingStream.FundingStream AsWebAdminModel(FundingStream fundingStream, IMapper mapper)
        {
            var webFundingStream = mapper.Map<WebAdminModels.FundingStream.FundingStream>(fundingStream);

            if (webFundingStream == null)
            {
                return null;
            }

            webFundingStream.NationalUiLayoutId = fundingStream.SettingValues?
                .FirstOrDefault(sv => sv?.Setting?.ValueDataType == SettingValueDataType.NationalLayout)?.Value;
            webFundingStream.NationalSpreadsheetLayoutId = fundingStream.SettingValues?
                .FirstOrDefault(sv => sv?.Setting?.ValueDataType == SettingValueDataType.NationalSpreadsheetLayout)?.Value;

            return webFundingStream;
        }


        /// <summary>
        /// Convert a dictionary of services funding streams to web model funding streams.
        /// </summary>
        /// <param name="fundingStreams">The funding streams to convert.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>A dictionary of funding streams.</returns>
        public static Dictionary<string, Models.FundingStream.FundingStream> AsWebModel(
            Dictionary<string, FundingStream> fundingStreams,
            IMapper mapper)
        {
            var returnDictionary = new Dictionary<string, Models.FundingStream.FundingStream>();

            foreach (var fundingStream in fundingStreams)
            {
                returnDictionary.Add(fundingStream.Key, AsWebModel(fundingStream.Value, mapper));
            }

            return returnDictionary;
        }

        /// <summary>
        /// Convert an ienumerable of services funding stream to web model funding streams.
        /// </summary>
        /// <param name="fundingStreams">The funding streams to convert.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>An ienumerable list of funding streams.</returns>
        public static IEnumerable<Models.FundingStream.FundingStream> AsWebModel(
            IEnumerable<FundingStream> fundingStreams,
            IMapper mapper)
        {
            foreach (var fundingStream in fundingStreams)
            {
                yield return AsWebModel(fundingStream, mapper);
            }
        }
    }
}