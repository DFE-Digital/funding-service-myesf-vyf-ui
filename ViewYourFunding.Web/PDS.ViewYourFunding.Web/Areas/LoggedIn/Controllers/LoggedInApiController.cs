#define vyfv2

using Microsoft.AspNetCore.Mvc;
using PDS.ViewYourFunding.Web.Attributes;
using PDS.VYF.Services.Abstracts.AppServices;
using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Models.ApiModels;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.LoggedIn.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [TokenAuthorize]
    public class LoggedInApiController : ControllerBase
    {
        private readonly ILoggedInApiServices loggedInApiServices;
        private readonly IParentApiClientServices parentApiClientServices;

        public LoggedInApiController(ILoggedInApiServices loggedInApiServices, IParentApiClientServices parentApiClientServices)
        {
            this.loggedInApiServices = loggedInApiServices;
            this.parentApiClientServices = parentApiClientServices;
        }

        /// <summary>
        /// Gets the information for logged in.
        /// </summary>
        /// <param name="ukprn">The ukprn.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The LoggedInInfo.</returns>
        [HttpGet]
        [Route("api/LoggedInApi/GetInfoForLoggedIn")]
        public async Task<LoggedInInfo> GetInfoForLoggedIn(string ukprn, string principal)
            => await this.loggedInApiServices.GetInfoForLoggedIn(ukprn, principal, Request.Scheme, Request.Host.ToString());

        // This is for backward compatibility. Once Monolith side code changes done, we can remove below methods.
#if vyfv2
        [HttpGet]
        [Route("api/globalsettings/IsMultipleAcademyTrust")]
        public async Task<bool> IsParent(string ukprn)
            => await this.parentApiClientServices.IsParent(ukprn, true);

        [HttpGet]
        [Route("api/globalsettings/GetInfoForLoggedInProvider")]
        public async Task<LoggedInInfo> GetInfoForLoggedInProvider(string ukprn, string principal)
            => await this.loggedInApiServices.GetInfoForLoggedIn(ukprn, principal, Request.Scheme, Request.Host.ToString());

        [HttpGet]
        [Route("api/globalsettings/GetInfoForLoggedInMultipleAcademyTrust")]
        public async Task<LoggedInInfo> GetInfoForLoggedInMultipleAcademyTrust(string ukprn, string principal)
            => await this.loggedInApiServices.GetInfoForLoggedIn(ukprn, principal, Request.Scheme, Request.Host.ToString());
#endif
    }
}
