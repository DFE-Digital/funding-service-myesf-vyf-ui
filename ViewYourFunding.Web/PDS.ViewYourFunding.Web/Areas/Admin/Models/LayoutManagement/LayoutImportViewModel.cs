using Microsoft.AspNetCore.Mvc.Rendering;
using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.Attributes;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// The view model for the import layout page.
    /// </summary>
    public class LayoutImportViewModel : LayoutManagementPageViewModel
    {
        #region PageViewModel Overrides

        /// <summary>
        /// Gets the breadcrumbs to show on this page.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems
            => new List<BreadCrumbViewModel>
            {
                AdminHomeBreadCrumb,
                LayoutManagementHomeBreadCrumb(false),
                ImportLayoutBreadCrumb(true)
            };

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds page layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        #endregion


        #region Properties

        /// <summary>
        /// Gets or sets the funding stream identifier.
        /// </summary>
        /// <value>
        /// The funding stream identifier.
        /// </value>
        [Display(Name = "Funding Stream")]
        public int FundingStreamId { get; set; }

        /// <summary>
        /// Gets or sets the funding view type.
        /// </summary>
        /// <value>
        /// The funding view type.
        /// </value>
        [Display(Name = "Funding view type")]
        public FundingViewType FundingViewType { get; set; }

        /// <summary>
        /// Gets or sets the funding view scope.
        /// </summary>
        /// <value>
        /// The funding view scope.
        /// </value>
        [Display(Name = "Layout type")]
        public FundingViewScope FundingViewScope { get; set; }

        /// <summary>
        /// Get the funding view scope select items (id, title and richer information).
        /// </summary>
        /// <returns>A list of titles, values and some richer information.</returns>
        public List<FundingViewScopeSelectItem> FundingViewScopeSelectItems()
        {
            var returnList = new List<FundingViewScopeSelectItem>();
            var fundingViewScopes = Enum.GetValues(typeof(FundingViewScope))
                .Cast<FundingViewScope>()
                .OrderBy(en => en.ToString())
                .ToList();

            foreach (var fundingViewScope in fundingViewScopes)
            {
                var titleAttribute = GetAttributes<DisplayAttribute>(fundingViewScope).FirstOrDefault();
                var title = titleAttribute?.Name ?? fundingViewScope.ToString();

                var applicableForAttributes = GetAttributes<ApplicableForAttribute>(fundingViewScope);

                var applicableForTypesString = string.Empty;
                var namesForTypesString = string.Empty;

                var first = true;
                applicableForAttributes = applicableForAttributes.Where(type => type.Type == FundingViewType.Spreadsheet || type.Type == FundingViewType.ViewData || type.Type == FundingViewType.Pdf).ToList();
                foreach (var attribute in applicableForAttributes)
                {
                    if (!first)
                    {
                        applicableForTypesString += ",";
                        namesForTypesString += ",";
                    }

                    applicableForTypesString += $"{(int)attribute.Type}";
                    namesForTypesString += attribute.OverrideName ?? title;

                    first = false;
                }

                if (!string.IsNullOrWhiteSpace(applicableForTypesString))
                {
                    returnList.Add(new FundingViewScopeSelectItem
                    {
                        ID = ((int)fundingViewScope).ToString(),
                        Title = title,
                        ApplicableForTypesString = applicableForTypesString,
                        NamesForTypes = namesForTypesString
                    });
                }
            }

            return returnList;
        }

        /// <summary>
        /// Get the funding view Type select list items.
        /// </summary>
        /// <returns>Returns the funding view Type select list.</returns>
        public IEnumerable<SelectListItem> FundingViewTypeSelectItems()
        {
            return Enum.GetValues(typeof(FundingViewType))
              .Cast<FundingViewType>()
              .Where(value => value != FundingViewType.Other)
              .OrderBy(en => en.ToString()).Select(fundingViewType => new SelectListItem
              {
                  Text = GetAttributes<DisplayAttribute>(fundingViewType).FirstOrDefault()?.Name?.ToString()
                         ?? fundingViewType.ToString(),
                  Value = ((int)fundingViewType).ToString()
              }).ToList();
        }

        private static IEnumerable<TAttribute> GetAttributes<TAttribute>(Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()?
                .GetCustomAttributes<TAttribute>();
        }

        /// <summary>
        /// Gets or sets the funding streams.
        /// </summary>
        /// <value>
        /// The next payment types.
        /// </value>
        public IEnumerable<SelectListItem> FundingStreams { get; set; }

        /// <summary>
        /// Gets or sets the name of the layout.
        /// </summary>
        /// <value>
        /// The name of the layout.
        /// </value>
        [Required]
        [Display(Name = "Layout Name")]
        [LayoutNameInUse(nameof(FundingStreamId), nameof(FundingViewType), nameof(FundingViewScope), ErrorMessage = "The layout name is already attached to a published publication.")]
        public string LayoutName { get; set; }

        #endregion
    }
}