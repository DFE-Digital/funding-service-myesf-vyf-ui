using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Services;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles
{
    /// <summary>
    /// Represents View your funding setting tile.
    /// </summary>
    /// <seealso cref="Tile" />
    public class PdfGenerationActionsTile : Tile, ITile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfGenerationActionsTile"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="alertService">The alert service.</param>
        /// <param name="linkGenerator">The link generator.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public PdfGenerationActionsTile(
            ITileDisplayRuleValidator validator,
            IAlertMessageService alertService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
            : base(validator, alertService, linkGenerator, httpContextAccessor)
        {
        }

        /// <inheritdoc/>
        public string Id => "pdfGenerationActions";

        /// <inheritdoc/>
        public string Href => GenerateRouteHrefLink(ViewYourFundingConstants.RouteName_PdfGenerationActionsHome, null);

        /// <inheritdoc/>
        public string HeadingText => "Document generation actions";

        /// <inheritdoc/>
        public string BodyText => "DfE staff with the sfs administration role can: trigger document generation actions";

        /// <inheritdoc/>
        public int Order => 5;

        /// <inheritdoc/>
        public bool IsAdminTile => true;
    }
}