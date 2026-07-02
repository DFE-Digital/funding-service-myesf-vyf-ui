using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication
{
    /// <summary>
    /// The Publication Class.
    /// </summary>
    /// <seealso cref="PublicationDates" />
    public class PublicationViewModel : PublicationDates
    {
        /// <summary>
        /// Gets or sets the description of this publication, e.g. "New allocations published for the academic year 2019 to 2020".
        /// </summary>
        /// [AllowHtml]
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the publication status.
        /// </summary>
        public PublicationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets an optional override to specify which version of the UI funding view model should be used.
        /// </summary>
        [Display(Name = "UI Model Version")]
        [UiSpreadSheetModelVersion]
        public int? UIModelVersion { get; set; }

        /// <summary>
        /// Gets or sets an optional override to specify which version of the spreadsheet funding view model should be used.
        /// </summary>
        [Display(Name = "Spreadsheet Model Version")]
        [UiSpreadSheetModelVersion]
        public int? SpreadsheetModelVersion { get; set; }

        /// <summary>
        /// Gets or sets the UI model maximum version number.
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public int? UIModelMaxVersion { get; set; }

        /// <summary>
        /// Gets or sets the spreadsheet model maximum version number.
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public int? SpreadsheetModelMaxVersion { get; set; }

        /// <summary>
        /// Gets or sets the username of the last person to update the publication information.
        /// </summary>
        public string LastUpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the newest spreadsheet created date.
        /// </summary>
        /// <value>
        /// The newest spreadsheet created date.
        /// </value>
        public DateTime? NewestSpreadsheetCreatedDate { get; set; }

        #region Adaptive Statement Layout Ids

        /// <summary>
        /// Gets or sets the local authority summary layout identifier.
        /// </summary>
        /// <value>
        /// The local authority summary layout identifier.
        /// </value>
        [Display(Name = "Summary")]
        public string LocalAuthoritySummaryLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the local authority funding breakdown layout identifier.
        /// </summary>
        /// <value>
        /// The local authority funding breakdown layout identifier.
        /// </value>
        [Display(Name = "Funding Breakdown")]
        public string LocalAuthorityFundingBreakdownLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the local authority history layout identifier.
        /// </summary>
        /// <value>
        /// The local authority history layout identifier.
        /// </value>
        [Display(Name = "History")]
        public string LocalAuthorityHistoryLayoutId { get; set; }


        /// <summary>
        /// Gets or sets the local authority history layout identifier.
        /// </summary>
        /// <value>
        /// The local authority history layout identifier.
        /// </value>
        [Display(Name = "History (single year)")]
        public string LocalAuthorityHistorySingleYearLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the provider summary layout identifier.
        /// </summary>
        /// <value>
        /// The provider summary layout identifier.
        /// </value>
        [Display(Name = "Summary")]
        public string ProviderSummaryLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the provider funding breakdown layout identifier.
        /// </summary>
        /// <value>
        /// The provider funding breakdown layout identifier.
        /// </value>
        [Display(Name = "Funding Breakdown")]
        public string ProviderFundingBreakdownLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the provider history layout identifier.
        /// </summary>
        /// <value>
        /// The provider history layout identifier.
        /// </value>
        [Display(Name = "History")]
        public string ProviderHistoryLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the provider history layout identifier.
        /// </summary>
        /// <value>
        /// The provider history layout identifier.
        /// </value>
        [Display(Name = "History (single year)")]
        public string ProviderHistorySingleYearLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the local authority spreadsheet layout identifier.
        /// </summary>
        /// <value>
        /// The local authority spreadsheet layout identifier.
        /// </value>
        [Display(Name = "Local Authority")]
        public string LocalAuthoritySpreadsheetLayoutId { get; set; }

        /// <summary>
        /// Gets or sets the provider spreadsheet layout identifier.
        /// </summary>
        /// <value>
        /// The provider spreadsheet layout identifier.
        /// </value>
        [Display(Name = "Provider")]
        public string ProviderSpreadsheetLayoutId { get; set; }

        #endregion

        /// <summary>
        /// Gets a value indicating whether this instance can be used to generate a spreadsheet.
        /// </summary>
        public bool CanGenerateSpreadSheet
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FundingPeriodCode) && FundingPeriodConstants.FundingPeriodCodeRegex.IsMatch(FundingPeriodCode))
                {
                    var yearTypeCode = FundingPeriodHelper.GetYearTypeCodeFromFundingPeriodCode(FundingPeriodCode);
                    var (yearFrom, yearTo) = FundingPeriodHelper.GetYearsFromCode(FundingPeriodCode);

                    if (yearTypeCode.Equals("AY", StringComparison.InvariantCultureIgnoreCase)
                        && yearFrom == 2018
                        && yearTo == 2019)
                    {
                        return false;
                    }

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the layout UI models.
        /// </summary>
        /// <value>
        /// The layout UI models.
        /// </value>
        public IReadOnlyList<LayoutUiModel> LayoutUiModels { get; set; } = new List<LayoutUiModel>();

        /// <summary>
        /// Gets the available layouts.
        /// </summary>
        /// <param name="fundingViewType">The funding view type (e.g. ViewData).</param>
        /// <param name="fundingViewScope">The funding view scope (e.g. National).</param>
        /// <returns>A list of layouts.</returns>
        public IEnumerable<SelectListItem> GetLayoutsAvailable(FundingViewType fundingViewType, FundingViewScope fundingViewScope)
        {
            var layouts = LayoutUiModels?.Where(layout =>
                layout.FundingViewType == fundingViewType && layout.FundingViewScope == fundingViewScope).ToList();

            if (layouts?.Any() == true)
            {
                var returnList = new List<SelectListItem>
                {
                    new SelectListItem("--", string.Empty)
                };

                returnList.AddRange(layouts.Select(layout => new SelectListItem(layout.LayoutName, layout.LayoutId)));

                return returnList;
            }

            return new List<SelectListItem>
            {
                new SelectListItem("No Layouts set up yet", string.Empty)
            };
        }

        /// <summary>
        /// Gets the name of the layout.
        /// </summary>
        /// <param name="layoutId">The layout identifier.</param>
        /// <returns>The Layout name.</returns>
        public string GetLayoutName(string layoutId)
        {
            if (string.IsNullOrEmpty(layoutId))
            {
                return string.Empty;
            }

            return LayoutUiModels.FirstOrDefault(layout => layout.LayoutId == layoutId)?.LayoutName;
        }
    }
}