using Microsoft.AspNetCore.Mvc;
using PDS.ViewYourFunding.Web.Areas.Admin.Binders;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// Represents the filter options for layout management page.
    /// </summary>
    [ModelBinder(BinderType = typeof(FilterTypeBinder))]
    public class LayoutFilter
    {
        /// <summary>
        /// Gets or sets the Page number.
        /// </summary>
        /// <value>
        /// The Page number.
        /// </value>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the list of funding streams.
        /// </summary>
        /// <value>
        /// The funding streams.
        /// </value>
        public List<Filter> FundingStreams { get; set; }

        /// <summary>
        /// Gets or sets the list of funding view types.
        /// </summary>
        /// <value>
        /// The funding view types.
        /// </value>
        public List<Filter> FundingViewTypes { get; set; }

        /// <summary>
        /// Gets or sets the list of funding view scopes.
        /// </summary>
        /// <value>
        /// The funding view scopes.
        /// </value>
        public List<Filter> FundingViewScopes { get; set; }
    }
}