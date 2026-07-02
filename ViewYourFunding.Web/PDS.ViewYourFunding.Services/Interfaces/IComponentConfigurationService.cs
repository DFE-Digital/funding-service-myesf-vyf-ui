using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Services.Interfaces
{
    /// <summary>
    /// Gets the component configuration based on passed properties for UI and spreadsheet.
    /// </summary>
    public interface IComponentConfigurationService
    {
        /// <summary>
        /// Gets the component configuration for the json data passed and the dataset definition.
        /// </summary>
        /// <param name="fundingProperties">The funding properties.</param>
        /// <param name="data">The data to work against.</param>
        /// <param name="datasetDefinition">The dataset definition.</param>
        /// <param name="allDatasetsData">The first datasets data.</param>
        /// <param name="uiModel">The UI model at the top level.</param>
        /// <param name="fundingStreamConfig">The funding stream details.</param>
        /// <param name="fundingDocument">The funding document.</param>
        /// <param name="publicationDate">The publication date (or null).</param>
        /// <param name="previousPublicationDate">The previous publication date (or null).</param>
        /// <param name="fundingPeriodCode">The funding period code.</param>
        /// <param name="asOfMonth">'As of' month (e.g. Apr).</param>
        /// <param name="asOfYear">'As of' year (e.g. 2010).</param>
        /// <param name="searchTerm">The search term that was used to get here.</param>
        /// <param name="selectedTab">The selected tab.</param>
        /// <param name="publicationUiModelVersion">The publication ui model version.</param>
        /// <param name="selectedVarianceOption">The selected variance option.</param>
        /// <param name="isLatestOrFinalFundingForYear">Is the latest or final funding publication for the year.</param>
        /// <param name="isCurrentYear">Is the current year (true or false).</param>
        /// <param name="isALoggedInView">Is the view intended for logged in users.</param>
        /// <param name="showSelectors">Should selectors be shown (used for debugging).</param>
        /// <param name="viaChoicePage">Whether we originally entered the site via the allocation type page (pre or post 16).</param>
        /// <param name="asStatementSpecification">Whether to show the statement specification or not.</param>
        /// <param name="showData">Whether to show data or not.</param>
        /// <param name="isHtml">Whether the output is HTML type or not.</param>
        /// <returns>Returns populated Component Configuration.</returns>
        ComponentConfiguration GetComponentConfiguration(
            FundingProperties fundingProperties,
            IFundingApiSearch data,
            UiModelDataset datasetDefinition,
            List<List<IFundingApiSearch>> allDatasetsData,
            UiModel uiModel,
            FundingStream fundingStreamConfig,
            FundingDocument fundingDocument,
            DateTime? publicationDate,
            DateTime? previousPublicationDate,
            string fundingPeriodCode,
            string asOfMonth,
            string asOfYear,
            string searchTerm,
            string selectedTab,
            int? publicationUiModelVersion,
            VarianceSelectionOption selectedVarianceOption,
            bool isLatestOrFinalFundingForYear,
            bool isCurrentYear,
            bool isALoggedInView,
            bool showSelectors,
            bool viaChoicePage,
            bool asStatementSpecification,
            bool showData,
            bool isHtml);

        /// <summary>
        /// Gets the funding properties.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="dataset">The dataset.</param>
        /// <returns>The Funding properties.</returns>
        FundingProperties GetFundingProperties(IFundingApiSearch data, UiModelDataset dataset);
    }
}
