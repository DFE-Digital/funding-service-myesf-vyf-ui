using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Services;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators;
using System;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles
{
    /// <summary>
    /// Abstract class that defines common properties and methods of a tile.
    /// </summary>
    public abstract class Tile
    {
        /// <summary>
        /// The alert message service.
        /// </summary>
        private readonly IAlertMessageService _alertMessageService;

        /// <summary>
        /// The validator.
        /// </summary>
        private readonly ITileDisplayRuleValidator _validator;

        /// <summary>
        /// The URL helper.
        /// </summary>
        private readonly LinkGenerator _linkGenerator;

        /// <summary>
        /// The HTTP context accessor.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="alertService">The alert service.</param>
        /// <param name="linkGenerator">The link generator.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        protected Tile(
            ITileDisplayRuleValidator validator,
            IAlertMessageService alertService,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
        {
            _validator = validator;
            _alertMessageService = alertService;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the name of the CSS.
        /// </summary>
        /// <value>
        /// The name of the CSS.
        /// </value>
        public virtual string ClassName => "green";

        /// <summary>
        /// Gets or sets the alert.
        /// </summary>
        /// <value>
        /// The alert.
        /// </value>
        public ITileAlert Alert { get; set; }

        /// <summary>
        /// Determines whether this instance has alert.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has alert; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAlert() => Alert != null;

        /// <summary>
        /// Determines whether the specified user context is available.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        ///   <c>true</c> if the specified user context is available; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsAvailable(IUserContext userContext)
        {
            try
            {
                if (_validator == null)
                {
                    throw new NullReferenceException($"{nameof(_validator)} cannot be null!");
                }
                else
                {
                    return _validator.Validate(userContext);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Generates the route href link.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The Route Link.</returns>
        public virtual string GenerateRouteHrefLink(string routeName, object parameters)
        {
            return _linkGenerator.GetPathByRouteValues(_httpContextAccessor.HttpContext, routeName, parameters);
        }

        /// <summary>
        /// Sets the alert.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        public virtual void SetAlert(IUserContext userContext = null)
        {
            try
            {
                Alert = _alertMessageService?.GetAlert(userContext);
            }
            catch (Exception)
            {
                Alert = null;
            }
        }
    }
}