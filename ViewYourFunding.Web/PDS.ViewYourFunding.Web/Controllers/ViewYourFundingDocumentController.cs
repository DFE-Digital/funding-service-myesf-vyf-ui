using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Controllers
{
    /// <summary>
    /// Funding controller.
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ViewYourFundingDocumentController : ControllerBase
    {
        private readonly IFundingViewService _generatorService;
        private readonly IFundingDocumentStorageService _fundingDocumentStoreService;
        private readonly IUserJourneyService _userJourneyService;
        private readonly ILoggerAdapter<ViewYourFundingDocumentController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewYourFundingDocumentController"/> class.
        /// </summary>
        /// <param name="generatorService">Service used to generate funding documents given a dataset.</param>
        /// <param name="fundingDocumentStoreService">Service used to store funding documents.</param>
        /// <param name="settingsService">The settings service to use to lookup values.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="mapper">The mapper.</param>
        public ViewYourFundingDocumentController(
            IFundingViewService generatorService,
            IFundingDocumentStorageService fundingDocumentStoreService,
            IUserJourneyService settingsService,
            ILoggerAdapter<ViewYourFundingDocumentController> logger,
            IMapper mapper)
        {
            _generatorService = generatorService;
            _fundingDocumentStoreService = fundingDocumentStoreService;
            _userJourneyService = settingsService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Download a spreadsheet.
        /// </summary>
        /// <param name="fileName">The internal filename as it is in storage.</param>
        /// <param name="publishedDate">The published date of the file.</param>
        /// <returns>Downloads a file to the calling browser.</returns>
        [HttpGet]
        [Route(FundingConstants.Route_DownloadSpreadsheet, Name = FundingConstants.RouteName_DownloadSpreadsheet)]
        public async Task<IActionResult> DownloadSpreadsheet(string fileName, DateTime publishedDate)
        {
            var streamData = new StreamContent(await _fundingDocumentStoreService.GetFile(fileName));
            var fundingStreams = await BaseFundingController.GetActiveFundingStreams(_userJourneyService);

            var downloadFileName =
                FilenameHelper.BuildOutputSpreadsheetFilename(fileName, publishedDate, fundingStreams);

            var contentType = fileName.EndsWith(FundingDocumentFileType.Spreadsheet_OpenFormat, StringComparison.InvariantCultureIgnoreCase)
                ? FundingDocumentFileType.Spreadsheet_OpenFormatContentType
                : FundingDocumentFileType.Spreadsheet_ExcelFormatContentType;

            return File(await streamData.ReadAsStreamAsync(), contentType, downloadFileName);
        }

        /// <summary>
        /// Gets the funding document metadata.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="cutoffDate">The cutoff date.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="modelVersion">Model version number of spreadsheet or UI.</param>
        /// <param name="forceRegenerate">if set to <c>true</c> [force regenerate].</param>
        /// <param name="fundingDocumentType">Type of the funding document.</param>
        /// <param name="fundingDocumentScope">The funding document scope.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>A list of URI and other meta data for the funding documents.</returns>
        [HttpGet]
        [Route(FundingConstants.Route_GetFundingDocumentMetadata, Name = FundingConstants.RouteName_GetFundingDocumentMetadata)]
        public async Task<List<FundingDocumentMetaResponse>> GetFundingDocumentMetadata(
            string fundingStreamCode,
            string fundingPeriodCode,
            DateTime cutoffDate,
            DateTime publicationDate,
            int? modelVersion,
            bool forceRegenerate,
            FundingViewType fundingDocumentType = FundingViewType.Spreadsheet,
            FundingViewScope fundingDocumentScope = FundingViewScope.National,
            string filters = null)
        {
            var filtersCollection = filters != null ? JsonConvert.DeserializeObject<SearchFilter[]>(filters) : null;

            var tasks = new List<Task<FundingDocumentMetaResponse>>();
            var openDocumentFormatStates = new FileFormat[3] { FileFormat.ODS, FileFormat.CSV, FileFormat.XLS };

            foreach (var openDocumentFormat in openDocumentFormatStates)
            {
                tasks.Add(GetFundingDocumentMetadata(
                    openDocumentFormat,
                    fundingStreamCode,
                    fundingPeriodCode,
                    cutoffDate,
                    publicationDate,
                    modelVersion,
                    forceRegenerate,
                    fundingDocumentType,
                    fundingDocumentScope,
                    filtersCollection));
            }

            var returnList = new List<FundingDocumentMetaResponse>();

            foreach (var task in tasks)
            {
                returnList.Add(await task);
            }

            return returnList;
        }

        /// <summary>
        /// Generate funding document path for the newest publication of data for this stream + period (open format and not).
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. AY-1920).</param>
        /// <param name="cutoffDate">Only get funding data published before this date and time.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="modelVersion">The model version.</param>
        /// <param name="fundingDocumentType">The funding document type (e.g. Spreadsheet).</param>
        /// <param name="fundingDocumentScope">The funding document scope (e.g. National).</param>
        /// <param name="filters">Optional filters to use when fetching data.</param>
        /// <returns>A URI for the funding document along with meta information.</returns>
        [HttpGet]
        [Route(FundingConstants.Route_GenerateFundingDocument)]
        public async Task<List<FundingDocumentMetaResponse>> GenerateFundingDocument(
            string fundingStreamCode,
            string fundingPeriodCode,
            DateTime cutoffDate,
            DateTime publicationDate,
            int? modelVersion = 1,
            FundingViewType fundingDocumentType = FundingViewType.Spreadsheet,
            FundingViewScope fundingDocumentScope = FundingViewScope.National,
            string filters = null)
        {
            _logger?.LogInformation("DEBUG1 Action:GenerateFundingDocument() Start");

            var filtersCollection = filters != null ? JsonConvert.DeserializeObject<SearchFilter[]>(filters) : null;

            var openDocumentFormatStates = new FileFormat[2] { FileFormat.ODS, FileFormat.CSV };

            var returnList = new List<FundingDocumentMetaResponse>();
            returnList.AddRange(await GenerateFundingDocument(
                openDocumentFormatStates,
                fundingStreamCode,
                fundingPeriodCode,
                cutoffDate,
                publicationDate,
                modelVersion,
                fundingDocumentType,
                fundingDocumentScope,
                filtersCollection));

            _logger?.LogInformation($"DEBUG1 Action: GenerateFundingDocument() End Count: {returnList?.Count}");
            return returnList;
        }

        /// <summary>
        /// Get the maximum UI and spreadsheet schema template version numbers.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. FY-2122).</param>
        /// <returns>The maximum UI and spreadsheet schema template version numbers.</returns>
        [HttpGet]
        [Route(FundingConstants.Route_GetMaxUIAndSpreadsheetVersionNumbers)]
        public MaximumUiSpreadsheetVersion GetMaxUiAndSpreadsheetVersionNumbers(string fundingStreamCode, string fundingPeriodCode)
        {
            return _generatorService.GetMaximumUIAndSpreadsheetVersionNumbers(fundingStreamCode, fundingPeriodCode);
        }

        /// <summary>
        /// Generate the funding document to a byte array.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. AY-1920).</param>
        /// <param name="cutoffDate">Only get funding data published before this date and time.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="modelVersion">Model version number of spreadsheet or UI.</param>
        /// <param name="fileFormat">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <param name="fundingDocumentType">The funding document type (e.g. Spreadsheet).</param>
        /// <param name="fundingDocumentScope">The funding document scope (e.g. National).</param>
        /// <param name="filters">Optional filters to use when fetching data.</param>
        /// <returns>A funding document as a byte array.</returns>
        [HttpGet]
        [Route(FundingConstants.Route_GenerateFundingDocumentToByteArray)]
        public async Task<byte[]> GenerateFundingDocumentToByteArray(
            string fundingStreamCode,
            string fundingPeriodCode,
            DateTime cutoffDate,
            DateTime publicationDate,
            int? modelVersion,
            FileFormat fileFormat,
            FundingViewType fundingDocumentType = FundingViewType.Spreadsheet,
            FundingViewScope fundingDocumentScope = FundingViewScope.National,
            string filters = null)
        {
            var filtersCollection = filters != null ? JsonConvert.DeserializeObject<SearchFilter[]>(filters) : null;

            var fundingStreams = await BaseFundingController.GetActiveFundingStreams(_userJourneyService);
            var fundingStream = fundingStreams.First(fs =>
                fs.FundingStreamCode.Equals(fundingStreamCode, StringComparison.InvariantCultureIgnoreCase));

            var publication = new Publication
            {
                CutOffDate = cutoffDate,
                PublishedDate = publicationDate,
                SpreadsheetModelVersion = modelVersion,
                PublicationLayouts = new List<PublicationLayout>
                {
                    new PublicationLayout
                    {
                        FundingViewType = fundingDocumentType,
                        FundingViewScope = fundingDocumentScope,
                        LayoutId = Helpers.FundingStreamHelper.AsWebModel(fundingStream, _mapper)?.NationalSpreadsheetLayoutId
                    }
                }
            };

            var result = (await _generatorService.GenerateFundingDocument(
                fundingStream,
                fundingPeriodCode,
                publicationDate,
                publication,
                fundingDocumentType,
                fundingDocumentScope,
                new[] { fileFormat },
                filtersCollection)).First();

            return result.Data;
        }

        /// <summary>
        /// Get the funding document path for the newest publication of data for this stream + period.
        /// </summary>
        /// <param name="fileFormat">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. AY-1920).</param>
        /// <param name="cutoffDate">Only get funding data published before this date and time.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="modelVersion">Model version number of spreadsheet or UI.</param>
        /// <param name="forceRegenerate">If true, the funding document will be regenerated on the fly, otherwise it will be returned from storage
        /// <param name="fundingDocumentType">The funding document type (e.g. Spreadsheet).</param>
        /// <param name="fundingDocumentScope">The funding document scope (e.g. National).</param>
        /// <param name="filters">Optional filters to use when fetching data.</param>
        /// if it already exists.</param>
        /// <returns>A list of URI and other meta data for the funding documents.</returns>
        private async Task<FundingDocumentMetaResponse> GetFundingDocumentMetadata(
            FileFormat fileFormat,
            string fundingStreamCode,
            string fundingPeriodCode,
            DateTime cutoffDate,
            DateTime publicationDate,
            int? modelVersion,
            bool forceRegenerate,
            FundingViewType fundingDocumentType = FundingViewType.Spreadsheet,
            FundingViewScope fundingDocumentScope = FundingViewScope.National,
            SearchFilter[] filters = null)
        {
            FundingDocumentMetaResponse responseItem = null;

            if (!forceRegenerate)
            {
                var filename = FilenameHelper.BuildFundingDocumentFilename(
                    fundingStreamCode,
                    fundingPeriodCode,
                    cutoffDate,
                    fileFormat,
                    fundingDocumentType,
                    fundingDocumentScope,
                    filters);

                responseItem = await _fundingDocumentStoreService.GetMetadata(filename);
            }

            if (forceRegenerate || responseItem == null)
            {
                return (await GenerateFundingDocument(new[] { fileFormat }, fundingStreamCode, fundingPeriodCode, cutoffDate, publicationDate, modelVersion)).First();
            }

            return responseItem;
        }

        /// <summary>
        /// Generate the funding document path for the newest publication of data for this stream + period.
        /// </summary>
        /// <param name="fileFormats">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. AY-1920).</param>
        /// <param name="cutoffDate">Only get funding data published before this date and time.</param>
        /// <param name="publicationDate">The publication date.</param>
        /// <param name="modelVersion">Model version number of spreadsheet or UI.</param>
        /// <param name="fundingDocumentType">The funding document type (e.g. Spreadsheet).</param>
        /// <param name="fundingDocumentScope">The funding document scope (e.g. National).</param>
        /// <param name="filters">Optional filters to use when fetching data.</param>
        /// <returns>A URI for the funding document along with meta information.</returns>
        private async Task<List<FundingDocumentMetaResponse>> GenerateFundingDocument(
            FileFormat[] fileFormats,
            string fundingStreamCode,
            string fundingPeriodCode,
            DateTime cutoffDate,
            DateTime publicationDate,
            int? modelVersion,
            FundingViewType fundingDocumentType = FundingViewType.Spreadsheet,
            FundingViewScope fundingDocumentScope = FundingViewScope.National,
            SearchFilter[] filters = null)
        {
            _logger?.LogInformation("DEBUG1 Method:GenerateFundingDocument() Start");

            var fundingStreams = await BaseFundingController.GetActiveFundingStreams(_userJourneyService);
            var fundingStream = fundingStreams.First(fs =>
                fs.FundingStreamCode.Equals(fundingStreamCode, StringComparison.InvariantCultureIgnoreCase));

            var publication = new Publication
            {
                CutOffDate = cutoffDate,
                PublishedDate = publicationDate,
                SpreadsheetModelVersion = modelVersion,
                PublicationLayouts = new List<PublicationLayout>
                {
                    new PublicationLayout
                    {
                        FundingViewType = fundingDocumentType,
                        FundingViewScope = fundingDocumentScope,
                        LayoutId = Helpers.FundingStreamHelper.AsWebModel(fundingStream, _mapper)?.NationalSpreadsheetLayoutId
                    }
                }
            };

            var outputData = await _generatorService.GenerateFundingDocument(
                fundingStream,
                fundingPeriodCode,
                publicationDate,
                publication,
                fundingDocumentType,
                fundingDocumentScope,
                fileFormats,
                filters);

            var results = new List<FundingDocumentMetaResponse>();

            foreach (var output in outputData)
            {
                var filename = FilenameHelper.BuildFundingDocumentFilename(
                    fundingStreamCode,
                    fundingPeriodCode,
                    publicationDate,
                    output.FileFormat,
                    fundingDocumentType,
                    fundingDocumentScope,
                    filters);

                var result = await _fundingDocumentStoreService.Upload(
                    filename,
                    output.Data,
                    output.PublicationDate,
                    cutoffDate,
                    filename.Split('.')[1],
                    fundingPeriodCode,
                    fundingStreamCode);

                results.Add(result);
            }

            _logger?.LogInformation(results == null || results.Count == 0
                ? $"DEBUG1 Method:GenerateFundingDocument() End. result is null"
                : $"DEBUG1 Method:GenerateFundingDocument() End. result.FundingStreamCode is {results.First().FundingStreamCode}");

            return results;
        }
    }
}