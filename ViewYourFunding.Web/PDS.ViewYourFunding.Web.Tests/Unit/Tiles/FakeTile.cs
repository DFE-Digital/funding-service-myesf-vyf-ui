using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Services;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Tiles
{
    /// <summary>
    /// The Fake Tile class.
    /// </summary>
    /// <seealso cref="Tile" />
    /// <seealso cref="ITile" />
    public class FakeTile : Tile, ITile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeTile"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="alertService">The alert service.</param>
        /// <param name="linkGenerator">The link generator.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public FakeTile(
            ITileDisplayRuleValidator validator,
            IAlertMessageService alertService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
            : base(validator, alertService, linkGenerator, httpContextAccessor)
        {
        }

        /// <inheritdoc/>
        public string Id => "fake";

        /// <inheritdoc/>
        public string Href => GenerateRouteHrefLink("fake", "fake");

        /// <inheritdoc/>
        public string HeadingText => "fake";

        /// <inheritdoc/>
        public string BodyText => "fake";

        /// <inheritdoc/>
        public int Order => 1;

        /// <inheritdoc/>
        public bool IsAdminTile => true;
    }
}