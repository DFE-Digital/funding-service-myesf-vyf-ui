using Microsoft.AspNetCore.Mvc.Rendering;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Helpers
{
    /// <summary>
    /// The layout helper class.
    /// </summary>
    public static class LayoutHelper
    {
        /// <summary>
        /// The organisation funding view scopes.
        /// </summary>
        public static readonly IReadOnlyList<FundingViewScope> OrganisationFundingViewScopes = new List<FundingViewScope>
        {
            FundingViewScope.OrganisationHistorySingleYear,
            FundingViewScope.OrganisationSummary,
            FundingViewScope.OrganisationHistory,
            FundingViewScope.Organisation
        };

        /// <summary>
        /// Maps the layout UI model.
        /// </summary>
        /// <param name="layoutModel">The layout model.</param>
        /// <returns>The layout Model.</returns>
        public static LayoutUiModel MapLayoutUiModel(LayoutModel layoutModel)
        {
            if (string.IsNullOrEmpty(layoutModel.FundingViewType))
            {
                throw new Exception("Funding view type is null - cannot map to UI model");
            }

            if (string.IsNullOrEmpty(layoutModel.FundingViewScope))
            {
                throw new Exception("Funding view scope is null - cannot map to UI model");
            }

            return new LayoutUiModel
            {
                LayoutName = layoutModel.LayoutName,
                LayoutId = layoutModel.Id,
                LastModifiedDateTime = layoutModel.LastModifiedDateTime.ConvertUtcDateTimeToGmtDateTime(),
                FundingViewType = (FundingViewType)Enum.Parse(typeof(FundingViewType), layoutModel.FundingViewType),
                FundingViewScope = (FundingViewScope)Enum.Parse(typeof(FundingViewScope), layoutModel.FundingViewScope)
            };
        }

        /// <summary>
        /// Gets the layouts available.
        /// </summary>
        /// <param name="layoutUiModels">The layout UI models.</param>
        /// <param name="fundingViewType">Type of the layout.</param>
        /// <param name="fundingViewScope">Scope of the layout.</param>
        /// <returns>The select item list.</returns>
        public static IEnumerable<SelectListItem> GetLayoutsAvailable(
            this IReadOnlyList<LayoutUiModel> layoutUiModels,
            FundingViewType fundingViewType,
            FundingViewScope fundingViewScope)
        {
            var layouts = layoutUiModels?.Where(layout =>
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
        /// <param name="layoutUiModels">The layout UI models.</param>
        /// <param name="layoutId">The layout identifier.</param>
        /// <returns>The Layout name.</returns>
        public static string GetLayoutName(this IReadOnlyList<LayoutUiModel> layoutUiModels, string layoutId)
        {
            if (string.IsNullOrEmpty(layoutId))
            {
                return string.Empty;
            }

            return layoutUiModels.FirstOrDefault(layout => layout.LayoutId == layoutId)?.LayoutName;
        }
    }
}