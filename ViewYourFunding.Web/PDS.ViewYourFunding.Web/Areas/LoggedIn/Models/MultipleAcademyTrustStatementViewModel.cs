using Microsoft.AspNetCore.Mvc;
using Pds.Core.Web.Components.Areas.Lists.DTOs;
using Pds.Core.Web.Components.Areas.Lists.Models;
using Pds.Core.Web.Models.Hyperlinks;
using PDS.ViewYourFunding.Services.DTOs;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Constants;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Models
{
    /// <summary>
    /// Logged in provider statement page view model.
    /// </summary>
    public class MultipleAcademyTrustStatementViewModel : LoggedInProviderBasePage, IListViewModel
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

        /// <inheritdoc/>
        public string ListContainerPartialName => "~/Areas/LoggedIn/Views/MultipleAcademyTrust/List/_MATStatementsListContainer.cshtml";

        /// <inheritdoc/>
        public string NoListItemsPartialName => "~/Areas/LoggedIn/Views/MultipleAcademyTrust/List/_MATStatementsListNoItems.cshtml";

        /// <inheritdoc/>
        public string ListItemPartialName => "~/Areas/LoggedIn/Views/MultipleAcademyTrust/List/_MATStatementsListItem.cshtml";

        /// <inheritdoc/>
        public string ListItemTemplatePartialName => "~/Areas/LoggedIn/Views/MultipleAcademyTrust/List/_MATStatementsListItemTemplate.cshtml";

        /// <inheritdoc/>
        public bool ListItemsAreSelectable => false;

        /// <inheritdoc/>
        public string ListItemSelectionInputName { get; }

        /// <inheritdoc/>
        public IEnumerable<BaseListItem> ListItems { get; set; }

        /// <inheritdoc/>
        public IEnumerable<IFilterCategoryViewModel> FilterCategories { get; set; } = Enumerable.Empty<IFilterCategoryViewModel>();

        /// <inheritdoc/>
        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel();

        /// <inheritdoc/>
        public string ItemTypeSingular => "Allocation statement";

        /// <inheritdoc/>
        public string ItemTypePlural => "Allocation statements";

        /// <summary>
        /// Gets or sets the number of items in the list before the 'Back to top' link is displayed.
        /// </summary>
        public int BackToTopLinkMinimumCount { get; set; }

        /// <inheritdoc/>
        public string ListItemAsyncDataEndPoint(IUrlHelper url)
        {
            // this is used for progressive js enhancements which have not been enabled at this time.
            return string.Empty;
        }
    }
}