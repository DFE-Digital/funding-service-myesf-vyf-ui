using Pds.Core.Web.Components.Areas.Lists.DTOs;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models
{
    /// <summary>
    /// Represents the view model for a single MAT statement line.
    /// </summary>
    public class MatStatementListItem : BaseListItem
    {
        /// <summary>
        /// Gets or sets the header component.
        /// </summary>
        public Component Header { get; set; }

        /// <summary>
        /// Gets or sets the body component.
        /// </summary>
        public Component Body { get; set; }

        /// <summary>
        /// Gets or sets additional view data.
        /// </summary>
        public Dictionary<string, object> Data { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FundingViewData"/> for this statement line.
        /// </summary>
        public FundingViewData FundingViewData { get; set; }
    }
}