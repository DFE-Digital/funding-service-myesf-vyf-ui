using Pds.Core.Web.Models.Hyperlinks;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Models.Shared
{
    /// <summary>
    /// The view model for a 'related' section of a page.
    /// </summary>
    public class RelatedSectionViewModel
    {
        /// <summary>
        /// Gets or sets the title to use for this 'related' section.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the list of links to show in this 'related' section.
        /// </summary>
        public List<BaseLinkViewModel> Links { get; set; }
    }
}