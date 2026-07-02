using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Services;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles
{
    /// <summary>
    /// Represents the Layout Management tile.
    /// </summary>
    /// <seealso cref="Tile" />
    public class LayoutManagementTile : Tile, ITile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutManagementTile"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="alertService">The alert service.</param>
        /// <param name="linkGenerator">The link generator.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public LayoutManagementTile(
            ITileDisplayRuleValidator validator,
            IAlertMessageService alertService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
            : base(validator, alertService, linkGenerator, httpContextAccessor)
        {
        }

        /// <inheritdoc/>
        public string Id => "LayoutManagementTile";

        /// <inheritdoc/>
        public string Href => GenerateRouteHrefLink(ViewYourFundingConstants.RouteName_AdminLayoutManagementHome, null);

        /// <inheritdoc/>
        public string HeadingText => "Layout management settings";

        /// <inheritdoc/>
        public string BodyText => "DfE staff with the sfs administration role can: Read and change the layout management settings";

        /// <inheritdoc/>
        public int Order => 3;

        /// <inheritdoc/>
        public bool IsAdminTile => true;
    }
}