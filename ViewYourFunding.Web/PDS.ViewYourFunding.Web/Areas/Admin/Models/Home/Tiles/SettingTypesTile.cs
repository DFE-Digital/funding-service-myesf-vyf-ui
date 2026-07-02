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
    public class SettingTypesTile : Tile, ITile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingTypesTile"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="alertService">The alert service.</param>
        /// <param name="linkGenerator">The link generator.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public SettingTypesTile(
            ITileDisplayRuleValidator validator,
            IAlertMessageService alertService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
            : base(validator, alertService, linkGenerator, httpContextAccessor)
        {
        }

        /// <inheritdoc/>
        public string Id => "settingTypes";

        /// <inheritdoc/>
        public string Href => GenerateRouteHrefLink(ViewYourFundingConstants.RouteName_AdminSettingTypesHome, null);

        /// <inheritdoc/>
        public string HeadingText => "Setting types";

        /// <inheritdoc/>
        public string BodyText => "DfE staff with the sfs administration role can: Read, add and change the setting types.";

        /// <inheritdoc/>
        public int Order => 4;

        /// <inheritdoc/>
        public bool IsAdminTile => true;
    }
}