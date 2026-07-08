namespace PDS.VYF.Services.Implementations.AppServices
{
    using Ardalis.GuardClauses;
    using Pds.Core.Logging;
    using PDS.ViewYourFunding.Services.Constants;
    using PDS.VYF.Services.Abstracts.AppServices;
    using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
    using PDS.VYF.Services.Abstracts.InfraServices.SettingsServices;
    using PDS.VYF.Services.Extensions.Core;
    using PDS.VYF.Services.Models.ApiModels;
    using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
    using System.Globalization;

    /// <summary>
    /// The Service class for getting LoggedIn Info.
    /// </summary>
    /// <seealso cref="PDS.VYF.Services.Abstracts.AppServices.ILoggedInApiServices" />
    public class LoggedInApiServices : ILoggedInApiServices
    {
        private readonly ILoggerAdapter<LoggedInApiServices> loggerService;
        private readonly IParentApiClientServices parentApiClientServices;
        private readonly IChildApiClientServices childApiClientServices;
        private readonly IGlobalSettingsService globalSettingsService;
        private readonly IFundingStreamSettingsServices fundingStreamSettingsServices;
        private readonly IUserCountApiClientServices userCountApiClientServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggedInApiServices"/> class.
        /// </summary>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="parentApiClientServices">The parent API client services.</param>
        /// <param name="childApiClientServices">The child API client services.</param>
        /// <param name="globalSettingsService">The global settings service.</param>
        /// <param name="fundingStreamSettingsServices">The funding stream settings services.</param>
        /// <param name="userCountApiClientServices">The user count API client services.</param>
        public LoggedInApiServices(
            ILoggerAdapter<LoggedInApiServices> loggerService,
            IParentApiClientServices parentApiClientServices,
            IChildApiClientServices childApiClientServices,
            IGlobalSettingsService globalSettingsService,
            IFundingStreamSettingsServices fundingStreamSettingsServices,
            IUserCountApiClientServices userCountApiClientServices)
        {
            this.loggerService = loggerService;
            this.parentApiClientServices = parentApiClientServices;
            this.childApiClientServices = childApiClientServices;
            this.globalSettingsService = globalSettingsService;
            this.fundingStreamSettingsServices = fundingStreamSettingsServices;
            this.userCountApiClientServices = userCountApiClientServices;
        }

        /// <summary>
        /// Gets the information for logged in.
        /// </summary>
        /// <param name="ukprn">The ukprn.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The LoggedInInfo.</returns>
        /// <param name="scheme">The Schema.</param>
        /// <param name="host">The Host.</param>
        public async Task<LoggedInInfo> GetInfoForLoggedIn(string ukprn, string principal, string scheme, string host)
        {
            if (this.Validate(ukprn, principal))
            {
                var fundingperiodcodes = await this.fundingStreamSettingsServices.GetEmailEnabledFundingStreamPeriod();

                var isParent = await this.parentApiClientServices.IsParent(ukprn, false, fundingperiodcodes);

                var gsToggleOnTypeId = isParent ? GlobalSettingTypeConstants.DisplayLoggedInMultipleAcademyTrustViewTypeId : GlobalSettingTypeConstants.DisplayViewYourFundingLoggedInProviderViewTypeId;
                var toggledSetting = (await this.globalSettingsService.GetFirstOrDefault(gsToggleOnTypeId))?.Value;

                if (bool.TryParse(toggledSetting, out bool toggledOn) && toggledOn)
                {
                    ParentSearchApiRequestModel parentRequest = new()
                    {
                        HasToBeLatestFunding = true,
                        ListOfUKPRNs = new List<string>() { ukprn },
                    };

                    ChildSearchApiRequestModel childRequest = new()
                    {
                        ListOfUKPRNs = new List<string>() { ukprn },
                        HasToBeLatestFunding = true,
                    };

                    childRequest.SetSelectFields(a => new { a.Id, a.StatementType, a.FundingStreamCode, a.StatusChangedDateOnly, });
                    parentRequest.SetSelectFields(a => new { a.GroupUkprn, a.ChildUKPRNs, });

                    if (isParent)
                    {
                        parentRequest.FundingStreamPeriods = fundingperiodcodes;
                        childRequest.FundingStreamPeriods = fundingperiodcodes;
                    }
                    else
                    {
                        var getLatestChildFundingPeriod = await this.childApiClientServices.LatestFundingPeriod(childRequest);
                        childRequest.FundingStreamPeriods = getLatestChildFundingPeriod;
                    }

                    var latestStatements = isParent
                        ? await this.childApiClientServices.SearchChildrenOfAParent(ukprn, childRequest, false)
                        : await this.childApiClientServices.SearchChild(childRequest, false);

                    if (latestStatements == null || latestStatements.Count == 0)
                    {
                        return new LoggedInInfo();
                    }

                    latestStatements = isParent ? latestStatements.GroupBy(x => x.OrganisationUkprn)
                    .Select(g =>
                    g.OrderByDescending(x => x.FundingStreamPeriod)
                     .First()).OrderByDescending(y => y.StatusChangedDate).ToList() : latestStatements;

                    var gsUrlTypeId = isParent ? GlobalSettingTypeConstants.UrlForLoggedInMultipleAcademyTrustViewTypeId : GlobalSettingTypeConstants.UrlForLoggedInProviderViewTypeId;
                    var pathSettings = await this.globalSettingsService.GetFirstOrDefault(gsUrlTypeId);

                    var path = await this.BuildPath(pathSettings?.Value ?? string.Empty, scheme, host);

                    var loggedInFundingStreams = await this.fundingStreamSettingsServices.GetLoggedInFundingStreams(isParent);

                    var filteredStatements = latestStatements.Where(statement =>
                    {
                        if (loggedInFundingStreams.TryGetValue(statement.FundingStreamCode!, out var fundingStream))
                        {
                            var digitalStatementsGoLiveDateStr = fundingStream?.SettingValues.FirstOrDefault(setting => setting.Setting.SettingName == SettingName.DigitalStatementsGoLiveDate)?.Value;

                            DateTime? digitalStatementsGoLiveDate = null;

                            if (digitalStatementsGoLiveDateStr != null && DateTime.TryParseExact(
                                            digitalStatementsGoLiveDateStr,
                                            DateConstants.SettingDateFormat,
                                            DateConstants.EnGbCultureInfo,
                                            DateTimeStyles.AdjustToUniversal,
                                            out var date))
                            {
                                digitalStatementsGoLiveDate = date;
                            }

                            return digitalStatementsGoLiveDate != null && statement.StatusChangedDateOnly >= digitalStatementsGoLiveDate;
                        }

                        return false;
                    }).ToList();

                    var userViewCountResponse = filteredStatements?.Count > 0 ? await this.userCountApiClientServices.GetUserViewCount(new UserViewCountRequestModel
                    {
                        UserId = principal,
                        ChildStatements = filteredStatements.Select(a => new ChildStatementModel() { ChildId = a.Id!, StatementType = a.StatementType! }).ToList(),
                    }) : new Models.ResponseModels.DataApiResponseModels.UserViewCountResponse();

                    return new LoggedInInfo
                    {
                        FundingsExists = latestStatements.Count > 0,
                        NewFundingsNotRead = userViewCountResponse?.NewCount ?? 0,
                        UpdatedFundingsNotRead = userViewCountResponse?.UpdatedCount ?? 0,
                        Path = path,
                        ToggledOn = true,
                    };
                }
            }

            return new LoggedInInfo();
        }

        private bool Validate(string ukprn, string principal)
        {
            try
            {
                Guard.Against.ValidUkrpn(ukprn);
                Guard.Against.ValidEmail(principal);
                return true;
            }
            catch (Exception ex)
            {
                this.loggerService.LogError(ex.Message);
                return false;
            }
        }

        private async Task<string> BuildPath(string result, string schema, string host)
        {
            const string SchemeHostSeperator = "://";

            if (result?.Contains(SchemeHostSeperator) == true)
            {
                return result;
            }

            var publicFacingUrlLeftPart = await this.globalSettingsService.GetFirstOrDefault(GlobalSettingTypeConstants.PublicFacingUrlLeftPart);

            if (string.IsNullOrEmpty(publicFacingUrlLeftPart?.Value) && string.IsNullOrWhiteSpace(schema) && string.IsNullOrWhiteSpace(host))
            {
                return result ?? string.Empty;
            }

            var root = !string.IsNullOrEmpty(publicFacingUrlLeftPart?.Value) ?
                publicFacingUrlLeftPart?.Value : $"{schema}{SchemeHostSeperator}{host}";

            if (string.IsNullOrEmpty(root))
            {
                return result ?? string.Empty;
            }

            var uriBuilder = new UriBuilder(root)
            {
                Path = result,
            };

            return uriBuilder.ToString();
        }
    }
}
