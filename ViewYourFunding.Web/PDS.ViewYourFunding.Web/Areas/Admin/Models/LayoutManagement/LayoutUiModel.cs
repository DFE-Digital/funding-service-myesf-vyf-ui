using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement
{
    /// <summary>
    /// The layout UI Model.
    /// </summary>
    public class LayoutUiModel
    {
        /// <summary>
        /// Gets or sets the name of the layout.
        /// </summary>
        /// <value>
        /// The name of the layout.
        /// </value>
        public string LayoutName { get; set; }

        /// <summary>
        /// Gets or sets the name of the funding stream.
        /// </summary>
        /// <value>
        /// The name of the funding stream.
        /// </value>
        public string FundingStreamName { get; set; }

        /// <summary>
        /// Gets or sets the funding view type.
        /// </summary>
        /// <value>
        /// The funding view type.
        /// </value>
        public FundingViewType FundingViewType { get; set; }

        /// <summary>
        /// Gets the name of the funding view type.
        /// </summary>
        /// <value>
        /// The name of the funding view type.
        /// </value>
        public string FundingViewTypeName => GetDisplayName(FundingViewType);

        /// <summary>
        /// Gets or sets the funding view scope.
        /// </summary>
        /// <value>
        /// The funding view scope.
        /// </value>
        public FundingViewScope FundingViewScope { get; set; }

        /// <summary>
        /// Gets the name of the funding view scope.
        /// </summary>
        /// <value>
        /// The name of the funding view scope.
        /// </value>
        public string FundingViewScopeName => GetDisplayName(FundingViewScope);

        /// <summary>
        /// Gets or sets the last modified date time.
        /// </summary>
        /// <value>
        /// The last modified date time.
        /// </value>
        public DateTime LastModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public LayoutStatus Status { get; set; }

        /// <summary>
        /// Gets the name of layout status description.
        /// </summary>
        /// <value>
        /// The name of the layout status description.
        /// </value>
        public string StatusName => GetDisplayName(Status);

        /// <summary>
        /// Gets or sets the layout identifier.
        /// </summary>
        /// <value>
        /// The layout identifier.
        /// </value>
        public string LayoutId { get; set; }

        /// <summary>
        /// Gets a value indicating whether to show preview link.
        /// </summary>
        /// <value>
        ///  True if the preview link is to be shown.
        /// </value>
        public bool ShowPreviewLink => FundingViewType == FundingViewType.ViewData;

        /// <summary>
        /// Gets or sets the funding stream Id.
        /// </summary>
        /// <value>
        /// The funding stream Id.
        /// </value>
        public int FundingStreamId { get; set; }

        private string GetDisplayName(Enum value)
        {
            return value
                .GetType()
                .GetField(value.ToString())?
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .SingleOrDefault() is DisplayAttribute attribute ? attribute.Name : value.ToString();
        }
    }
}