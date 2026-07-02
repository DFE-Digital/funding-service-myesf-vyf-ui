using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FundingDocumentMeta = PDS.ViewYourFunding.Services.DTOs.FundingDocumentMeta;
using FundingViewData = PDS.ViewYourFunding.Services.DTOs.FundingViewData;
using SearchFilter = PDS.ViewYourFunding.Services.DTOs.SearchFilter;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// A service to create views of funding data.
    /// </summary>
    public interface IFundingViewService
    {
        /// <summary>
        /// Generate a funding document.
        /// </summary>
        /// <param name="fundingStream">The funding stream config(e.g. PE and sport premium).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. FY-1920).</param>
        /// <param name="publishedDate">The published date - as on publication or status changed date.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="fundingDocumentType">The funding document type (e.g. Spreadsheet).</param>
        /// <param name="fundingDocumentScope">An array of funding document scopes (e.g. National).</param>
        /// <param name="fileFormats">File format e.g. csv, ods. by default use a popular vendor specific version (e.g. XLS 2003).</param>
        /// <param name="filters">Optional filters to use when fetching data.</param>
        /// <param name="previewLayoutModel">The preview layout model - if in preview Mode.</param>
        /// <param name="showSelectors">Whether to show selectors or not.</param>
        /// <param name="showStatementSpecification">Whether to show statement specification or not.</param>
        /// <param name="showData">Whether to show data or not.</param>
        /// <param name="iFundingApiSearchFunding">The funding data - if known.</param>
        /// <param name="iFundingApiSearchProviderFunding">The provider funding data - if known.</param>
        /// <param name="explicitFundingPassed">Was funding data passed? (even if it was null).</param>
        /// <param name="explicitProviderFundingPassed">Was provider funding data passed? (even if it was null).</param>
        /// <returns>A byte array representing the spreadsheet.</returns>
        Task<List<FundingDocumentMeta>> GenerateFundingDocument(
            FundingStream fundingStream,
            string fundingPeriodCode,
            DateTime publishedDate,
            Publication publication,
            FundingViewType fundingDocumentType,
            FundingViewScope fundingDocumentScope,
            FileFormat[] fileFormats,
            SearchFilter[] filters = null,
            PreviewLayoutModel previewLayoutModel = null,
            bool showSelectors = false,
            bool showStatementSpecification = false,
            bool showData = true,
            IFundingApiSearchFunding[] iFundingApiSearchFunding = null,
            IFundingApiSearchProviderFunding[] iFundingApiSearchProviderFunding = null,
            bool explicitFundingPassed = false,
            bool explicitProviderFundingPassed = false);

        /// <summary>
        /// Get the maximum UI and spreadsheet schema template version numbers.
        /// </summary>
        /// <param name="fundingStreamCode">The funding stream code (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. FY-2122).</param>
        /// <returns>The maximum UI and spreadsheet schema template version numbers.</returns>
        MaximumUiSpreadsheetVersion GetMaximumUIAndSpreadsheetVersionNumbers(string fundingStreamCode, string fundingPeriodCode);

        /// <summary>
        /// Get a list of request objects.
        /// </summary>
        /// <param name="fundingStreamConfig">The funding stream config for which to generate the dictionary (e.g. DSG).</param>
        /// <param name="fundingPeriodCode">The funding period code (e.g. DSG).</param>
        /// <param name="cutOffDate">The cut off date representing the latest publication date for which data should be retrieved.</param>
        /// <param name="fundingViewScope">An array of scopes for this funding view (e.g. National).</param>
        /// <param name="filters">Optional filters to use when fetching data.</param>
        /// <param name="iFundingApiSearchFunding">The funding data - if known.</param>
        /// <param name="iFundingApiSearchProviderFunding">The provider funding data - if known.</param>
        /// <param name="groupType">The group type.</param>
        /// <returns>A list of return objects.</returns>
        List<FundingApiSearchRequestObject> GetDataRequirements(
            FundingStream fundingStreamConfig,
            string fundingPeriodCode,
            DateTime cutOffDate,
            FundingViewScope fundingViewScope,
            SearchFilter[] filters = null,
            IFundingApiSearchFunding iFundingApiSearchFunding = null,
            IFundingApiSearchProviderFunding iFundingApiSearchProviderFunding = null,
            string groupType = GroupingType.LocalAuthority);

        /// <summary>
        /// Generates funding view data.
        /// </summary>
        /// <param name="componentService">The component service.</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="fundingStreamCode">The funding stream code.</param>
        /// <param name="allFundingStreams">The funding stream config for which to generate the dictionary (e.g. DSG).</param>
        /// <param name="cutOffDate">The cut off date representing the latest publication date for which data should be retrieved.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="modelVersion">Model version number of spreadsheet or UI.</param>
        /// <param name="fundingViewScope">An array of scopes for this funding view (e.g. National).</param>
        /// <param name="componentDefaults">Get defaults for the components.</param>
        /// <param name="fundingDocument">Information about the funding document.</param>
        /// <param name="isLatestOrFinalFundingForYear">Is it the latest or final publication for the year.</param>
        /// <param name="isCurrentYear">Is it the current year.</param>
        /// <param name="previousPublicationDate">The previous publication date (or null).</param>
        /// <param name="filters">Optional filters to use when fetching data.</param>
        /// <param name="searchTerm">Search term that was used to get to this page.</param>
        /// <param name="selectedTab">The selected tab.</param>
        /// <param name="bubbleUpException">Should an exception be bubbled up.</param>
        /// <param name="iFundingApiSearchFunding">The funding data - if known.</param>
        /// <param name="iFundingApiSearchProviderFunding">The provider funding data - if known.</param>
        /// <param name="explicitFundingPassed">Was funding data passed? (even if it was null).</param>
        /// <param name="explicitProviderFundingPassed">Was provider funding data passed? (even if it was null).</param>
        /// <param name="previewLayoutModel">The preview layout model - if in preview Mode.</param>
        /// <param name="selectedVarianceOption">The selected variance option.</param>
        /// <param name="viaChoicePage">Whether we originally entered the site via the allocation type page (pre or post 16).</param>
        /// <param name="showSelectors">Whether to show selectors or not.</param>
        /// <param name="asStatementSpecification">Whether to show the statement specification or not.</param>
        /// <param name="showData">Whether to show data or not.</param>
        /// <returns>A funding view data object for the given parameters.</returns>
        Task<FundingViewData> GenerateFundingViewData(
            IComponentService componentService,
            string fundingPeriodCode,
            string fundingStreamCode,
            FundingStream[] allFundingStreams,
            DateTime cutOffDate,
            Publication publication,
            int? modelVersion,
            FundingViewScope fundingViewScope,
            Dictionary<ComponentType, Defaults> componentDefaults,
            FundingDocument fundingDocument,
            bool isLatestOrFinalFundingForYear,
            bool isCurrentYear,
            DateTime? previousPublicationDate = null,
            SearchFilter[] filters = null,
            string searchTerm = null,
            string selectedTab = null,
            bool bubbleUpException = true,
            IFundingApiSearchFunding[] iFundingApiSearchFunding = null,
            IFundingApiSearchProviderFunding[] iFundingApiSearchProviderFunding = null,
            bool explicitFundingPassed = false,
            bool explicitProviderFundingPassed = false,
            PreviewLayoutModel previewLayoutModel = null,
            VarianceSelectionOption selectedVarianceOption = VarianceSelectionOption.NoComparison,
            bool viaChoicePage = false,
            bool showSelectors = false,
            bool asStatementSpecification = false,
            bool showData = true);
    }
}