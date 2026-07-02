#define vyfv2

using Microsoft.AspNetCore.Mvc;
using Pds.Core.Common.Identity.Models;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Web.Enums;
using PDS.ViewYourFunding.Web.Models.GlobalSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PDS.ViewYourFunding.Web.Controllers
{
    /// <summary>
    /// Get global setting for View Your Funding.
    /// </summary>
    public partial class GlobalSettingsController : BaseFundingController
    {
        /// <summary>
        /// Returns string of TRUE if logged in VYF area is available to admin.
        /// </summary>
        /// <returns>TRUE if logged in VYF area is available.</returns>
        [HttpGet, Produces("text/plain")]
        [Route("IsLoggedInAdminViewAvailable")]
        public async Task<string> GetLoggedInAdminViewYourFundingSetting()
        {
            var result = await GetGlobalSetting(GlobalSettingTypeConstants.DisplayViewYourFundingLoggedInAdminViewTypeId);
            return result ?? $"Setting type {GlobalSettingTypeConstants.DisplayViewYourFundingLoggedInAdminViewTypeId} not found.";
        }

        /// <summary>
        /// Returns string of TRUE if logged in VYF area is available to providers.
        /// </summary>
        /// <returns>TRUE if logged in VYF area is available.</returns>
        [HttpGet, Produces("text/plain")]
        [Route("IsLoggedInProviderViewAvailable")]
        public async Task<string> GetLoggedInProviderViewYourFundingSetting()
        {
            var result = await GetGlobalSetting(GlobalSettingTypeConstants.DisplayViewYourFundingLoggedInProviderViewTypeId);
            return result ?? $"Setting type {GlobalSettingTypeConstants.DisplayViewYourFundingLoggedInProviderViewTypeId} not found.";
        }

        /// <summary>
        /// Returns logged in view URI.
        /// </summary>
        /// <returns>Logged in view URI.</returns>
        [HttpGet, Produces("text/plain")]
        [Route("GetLoggedInAdminViewUrl")]
        public async Task<string> GetLoggedInAdminViewUrlSetting()
        {
            return await BuildPath(await GetGlobalSetting(GlobalSettingTypeConstants.UrlForLoggedInAdminViewTypeId));
        }

        /// <summary>
        /// Returns logged in view URI.
        /// </summary>
        /// <returns>Logged in view URI.</returns>
        [HttpGet, Produces("text/plain")]
        [Route("GetLoggedInProviderViewUrl")]
        public async Task<string> GetLoggedInProviderViewUrlSetting()
        {
            return await BuildPath(await GetGlobalSetting(GlobalSettingTypeConstants.UrlForLoggedInProviderViewTypeId));
        }

        /// <summary>
        /// Returns string of TRUE if logged in VYF area is available to Multiple Academy Trusts.
        /// </summary>
        /// <returns>TRUE if logged in VYF area is available.</returns>
        [HttpGet, Produces("text/plain")]
        [Route("IsLoggedInMultipleAcademyTrustViewAvailable")]
        public async Task<string> GetLoggedInMultipleAcademyTrustViewYourFundingSetting()
        {
            var result = await GetGlobalSetting(GlobalSettingTypeConstants.DisplayLoggedInMultipleAcademyTrustViewTypeId);
            return result ?? $"Setting type {GlobalSettingTypeConstants.DisplayLoggedInMultipleAcademyTrustViewTypeId} not found.";
        }

        /// <summary>
        /// Returns logged in view URI.
        /// </summary>
        /// <returns>Logged in view URI.</returns>
        [HttpGet, Produces("text/plain")]
        [Route("GetLoggedInMultipleAcademyTrustViewUrl")]
        public async Task<string> GetLoggedInMultipleAcademyTrustViewUrlSetting()
        {
            return await BuildPath(await GetGlobalSetting(GlobalSettingTypeConstants.UrlForLoggedInMultipleAcademyTrustViewTypeId));
        }

        /// <summary>
        /// Returns logged in view URI.
        /// </summary>
        /// <param name="ukprn">The UKPRN to look up.</param>
        /// <param name="principal">The principal to check for unseen fundings for.</param>
        /// <returns>Logged in view URI.</returns>
#if !vyfv2
        [HttpGet]
        [Route("GetInfoForLoggedInProvider")]
#endif
        public async Task<ActionResult<InfoForLoggedInProvider>> GetInfoForLoggedInProvider(string ukprn, string principal)
            => await GetInfoForLoggedIn(ukprn, principal);

        /// <summary>
        /// Returns logged in view URI.
        /// </summary>
        /// <param name="ukprn">The UKPRN to look up.</param>
        /// <param name="principal">The principal to check for unseen fundings for.</param>
        /// <returns>Logged in view URI.</returns>
#if !vyfv2
        [HttpGet]
        [Route("GetInfoForLoggedInMultipleAcademyTrust")]
#endif
        public async Task<ActionResult<InfoForLoggedInProvider>> GetInfoForLoggedInMultipleAcademyTrust(string ukprn, string principal)
            => await GetInfoForLoggedIn(ukprn, principal);

        /// <summary>
        /// Determines whether the given ukprn belongs to a multiple academy trust (MAT).
        /// </summary>
        /// <param name="ukprn">The ukprn.</param>
        /// <returns>True if it is a MAT.</returns>
#if !vyfv2
        [HttpGet]
        [Route("IsMultipleAcademyTrust")]
#endif
        public async Task<bool> IsMultipleAcademyTrust(int ukprn)
        {
            if (ukprn == 0)
            {
                return false;
            }

            var toggledSetting = await GetGlobalSetting(GlobalSettingTypeConstants.DisplayLoggedInMultipleAcademyTrustViewTypeId);
            bool.TryParse(toggledSetting, out var toggledOn);
            if (!toggledOn)
            {
                return false;
            }

            try
            {
                return await CheckMatStatus(new User { Ukprn = ukprn }, FundingUIViewType.Providers_LoggedIn);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return false;
            }
        }

        private async Task<InfoForLoggedInProvider> GetInfoForLoggedIn(string ukprn, string principal)
        {
            var isValidUKPRN = int.TryParse(ukprn, out int intUkprn) && ukprn.Length == 8;

            if (isValidUKPRN)
            {
                var isMAT = await IsMultipleAcademyTrust(intUkprn);

                var gsToggleOnTypeId = isMAT ? GlobalSettingTypeConstants.DisplayLoggedInMultipleAcademyTrustViewTypeId : GlobalSettingTypeConstants.DisplayViewYourFundingLoggedInProviderViewTypeId;
                var toggledSetting = await GetGlobalSetting(gsToggleOnTypeId);

                if (bool.TryParse(toggledSetting, out bool toggledOn) && toggledOn)
                {
                    var gsUrlTypeId = isMAT ? GlobalSettingTypeConstants.UrlForLoggedInMultipleAcademyTrustViewTypeId : GlobalSettingTypeConstants.UrlForLoggedInProviderViewTypeId;
                    var path = await BuildPath(await GetGlobalSetting(gsUrlTypeId));

                    List<IFundingApiSearchProviderFunding> providerFundings = await GetProviderFundings(ukprn, isMAT);

                    IUserFundingViewCountResponse userFundingViewCount = new UserFundingViewCountResponse();
                    providerFundings = GetLatestProviderFundingsByFundingStreams(providerFundings);

                    try
                    {
                        userFundingViewCount = await GetUserFundingCount(principal, providerFundings);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }

                    return new InfoForLoggedInProvider
                    {
                        ToggledOn = toggledOn,
                        FundingsExists = providerFundings?.Count > 0,
                        NewFundingsNotRead = userFundingViewCount.UnreadNewFundings,
                        UpdatedFundingsNotRead = userFundingViewCount.UnreadUpdatedFundings,
                        Path = path
                    };
                }
            }

            return new InfoForLoggedInProvider
            {
                ToggledOn = false
            };
        }

        private async Task<List<IFundingApiSearchProviderFunding>> GetProviderFundings(string ukprn, bool isMAT)
        {
            try
            {
                FundingUIViewType fundingUIViewType = isMAT ? FundingUIViewType.Organisations_LoggedIn : FundingUIViewType.Providers_LoggedIn;
                var fundingStreams = await GetRelevantFundingStreams(fundingUIViewType);

                if (isMAT)
                {
                    var dataRequirements = await GetDataRequirements(fundingStreams, null, false, ukprn);
                    var fundingRequestObject = SquashDataRequirements(dataRequirements, "Funding");
                    var fundingData = await _fundingApiService.SearchFunding(fundingRequestObject);
                    var ukprnList = GetProviderFundingIds(fundingData.Funding.Select(funding => funding.ProviderFundings))?.ToList();

                    if (ukprnList?.Count > 0)
                    {
                        return await DoProviderFundingSearch(null, fundingStreams, null, null, multipleUkPrnSearchList: string.Join("|", ukprnList));
                    }
                }
                else
                {
                    return await DoProviderFundingSearch(ukprn, fundingStreams, null, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return new List<IFundingApiSearchProviderFunding>();
        }
    }
}