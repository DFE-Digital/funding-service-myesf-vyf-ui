using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pds.Core.Common.Identity.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;

namespace PDS.ViewYourFunding.Web.Controllers
{
    /// <summary>
    /// The Cache API Controller.
    /// </summary>
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CacheController : Controller
    {
        /// <summary>
        /// The caching service.
        /// </summary>
        private readonly ICacheService _cachingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheController"/> class.
        /// </summary>
        /// <param name="cachingService">The caching service.</param>
        public CacheController(ICacheService cachingService)
        {
            _cachingService = cachingService;
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        /// <returns>The negotiated content result.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route(ViewYourFundingConstants.Route_ClearCache, Name = ViewYourFundingConstants.RouteName_ClearCache)]
        public IActionResult ClearCache()
        {
            _cachingService.ClearCache();
            return Ok("Cache Cleared Successfully.");
        }
    }
}