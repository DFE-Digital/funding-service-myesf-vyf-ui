using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Services;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles
{
    /// <summary>
    /// Represents Digital Allocations tile.
    /// </summary>
    /// <seealso cref="Tile" />
    /// <seealso cref="ITile" />
    public class GlobalSettingsTile : Tile, ITile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalSettingsTile"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="alertService">The alert service.</param>
        /// <param name="linkGenerator">The link generator.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public GlobalSettingsTile(
            ITileDisplayRuleValidator validator,
            IAlertMessageService alertService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
            : base(validator, alertService, linkGenerator, httpContextAccessor)
        {
        }

        /// <inheritdoc/>
        public string Id => "globalSettings";

        /// <inheritdoc/>
        public string Href => GenerateRouteHrefLink(ViewYourFundingConstants.RouteName_AdminGeneralSettingHome, null);

        /// <inheritdoc/>
        public string HeadingText => "General settings";

        /// <inheritdoc/>
        public string BodyText => "DfE staff with the sfs adminstration role can: Read and change the settings in the Settings tables";

        /// <inheritdoc/>
        public int Order => 1;

        /// <inheritdoc/>
        public bool IsAdminTile => true;
    }
}