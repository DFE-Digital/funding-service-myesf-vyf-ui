using Microsoft.AspNetCore.Mvc;
using Pds.Core.Web.Components.Areas.Lists.DTOs;
using Pds.Core.Web.Components.Areas.Lists.Models;
using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.ViewModels.Parent
{
    public class ParentSummaryPageViewModel : LoggedInBasePageViewModel, IListViewModel
    {
        /// <summary>
        /// Gets the list of breadcrumbs to show.
        /// </summary>
        public override IList<BreadCrumbViewModel> BreadCrumbItems => SecondLevelBaseBreadCrumbItems(true);

        /// <summary>
        /// Gets a value indicating whether whether or not to use the two-thirds column layout.
        /// </summary>
        public override bool IsTwoThirdsLayout => false;

        /// <summary>
        /// Gets the title to use in the html `title` tag.
        /// </summary>
        public override string BrowserTitle => LoggedInConstants.BrowserTitle_AllocationStatements;

        /// <summary>
        /// Gets the title in the page's main content section.
        /// </summary>
        public override string HeaderTitle => LoggedInConstants.HeaderTitle_StandardMYESFHeader;

        /// <inheritdoc/>
        public override string HeaderLink => "/";

        /// <summary>
        /// Gets or sets the funding view data for the provider.
        /// </summary>
        public Dictionary<string, FundingViewData> FundingViewData { get; set; } = new Dictionary<string, FundingViewData>();

        public string ListContainerPartialName => "~/Areas/LoggedIn/Views/Parent/List/_MATStatementsListContainer.cshtml";

        public string NoListItemsPartialName => "~/Areas/LoggedIn/Views/Parent/List/_MATStatementsListNoItems.cshtml";

        public string ListItemPartialName => "~/Areas/LoggedIn/Views/Parent/List/_MATStatementsListItem.cshtml";

        public string ListItemTemplatePartialName => "~/Areas/LoggedIn/Views/Parent/List/_MATStatementsListItemTemplate.cshtml";

        public bool ListItemsAreSelectable => false;

        public string ListItemSelectionInputName { get; }

        public IEnumerable<BaseListItem> ListItems { get; set; }

        public IEnumerable<IFilterCategoryViewModel> FilterCategories { get; set; } = Enumerable.Empty<IFilterCategoryViewModel>();

        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel();

        public string ItemTypeSingular => "Allocation statement";

        public string ItemTypePlural => "Allocation statements";

        /// <summary>
        /// Gets or sets the number of items in the list before the 'Back to top' link is displayed.
        /// </summary>
        public int BackToTopLinkMinimumCount { get; set; }

        public string ListItemAsyncDataEndPoint(IUrlHelper url)
        {
            // this is used for progressive js enhancements which have not been enabled at this time.
            return string.Empty;
        }
    }
}
