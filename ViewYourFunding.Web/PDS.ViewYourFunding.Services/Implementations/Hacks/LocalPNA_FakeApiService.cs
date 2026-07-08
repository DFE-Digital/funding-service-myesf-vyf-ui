using Newtonsoft.Json;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations.FundingView.ResponseObjects;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Services.RequestObjects;
using PDS.ViewYourFunding.Services.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Implementations.Hacks
{
    /// <summary>
    /// A short term use implementation of funding service to get mock NMSS data.
    /// </summary>
    public class LocalPNA_FakeApiService : BaseFakeApiService, IFundingApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalPNA_FakeApiService"/> class.
        /// Create a new instance of a LocalNMSS_FakeApiService.
        /// </summary>
        /// <param name="httpApiService">The HTTP service to use.</param>
        public LocalPNA_FakeApiService(IHttpApiService httpApiService)
            : base(httpApiService)
        {
        }

        /// <summary>
        /// An HTTP api service to fetch funding.
        /// Not implemented.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A funding collection.</returns>
        public Task<IFundingApiSearchFunding> GetFunding(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An HTTP api service to fetch provider funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A provider funding collection.</returns>
        public async Task<IFundingApiSearchProviderFunding> GetProviderFunding(string id)
        {
            string phaseOfEducation, filename, orgName,
                providerType = string.Empty,
                providerSubType = string.Empty,
                laName = string.Empty,
                laeStab = "--",
                upin = "--",
                urn = "--";

            var dateOpened = new DateTime(2000, 1, 1);
            var ukprn = "10072811";
            var fundingPeriodCode = "AC-2122";

            switch (id)
            {
                case "PNA-AC-2223-10072811-1_0":
                    phaseOfEducation = "Primary";
                    filename = "PNA_Static_FreeSchools";
                    orgName = "Abbey View Primary Academy";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    ukprn = "10072811";
                    urn = "140294";
                    dateOpened = new DateTime(2022, 08, 31);
                    providerType = ProviderTypeExternal.FreeSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "PNA-AC-2122-10072811-1_0":
                    phaseOfEducation = "Primary";
                    filename = "PNA_Static_Standard";
                    orgName = "Abbey View Primary Academy";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    ukprn = "10072811";
                    urn = "140294";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "PNA-AC-2122-10072812-1_0":
                    phaseOfEducation = "Primary";
                    filename = "PNA_Static_DualCensusNoThreshold";
                    orgName = "Abbey View Primary Academy";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    ukprn = "10072812";
                    urn = "141823";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "PNA-AC-2122-10072814-1_0":
                    phaseOfEducation = "Primary";
                    filename = "PNA_Static_DualCensusUsingThreshold";
                    orgName = "Abbey View Primary Academy";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    ukprn = "10072814";
                    urn = "139910";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "PNA-AC-2122-10072815-1_0":
                    phaseOfEducation = "Primary";
                    filename = "PNA_Static_ThresholdUsed";
                    orgName = "Abbey View Primary Academy";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    ukprn = "10072815";
                    urn = "140836";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "PNA-AC-2122-10072816-1_0":
                    phaseOfEducation = "Primary";
                    filename = "PNA_Static_GuaranteedPupilNumbers";
                    orgName = "Abbey View Primary Academy";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    ukprn = "10072816";
                    urn = "230294";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                default:
                    phaseOfEducation = "Primary";
                    filename = "PNA_Static";
                    orgName = "Abbey View Primary Academy";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    ukprn = "10072817";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
            }

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");
            var fundingValue = JsonConvert.DeserializeObject<FundingValueNested_1_0>(fileContents);
            var totalAmount = fundingValue.TotalValue;

            return new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = fundingPeriodCode,
                FundingStreamCode = "PNA",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount ?? 0,
                OrganisationName = orgName,
                OrganisationUkprn = ukprn,
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-PNA-Id1",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                LocalAuthorityName = laName,
                OrganisationDfeNumber = laeStab,
                ProviderUpin = upin,
                ProviderUrn = urn
            };
        }

        /// <summary>
        /// Search the funding collection using supplied filters.
        /// Not implemented.
        /// </summary>
        /// <param name="requestObj">An FundingApiSearchRequestObject object.</param>
        /// <returns>Returns a IFundingApiSearchResponseFunding that contains the funding(s).</returns>
        public Task<IFundingApiSearchResponseFunding> SearchFunding(FundingApiSearchRequestObject requestObj)
        {
            return Task.FromResult<IFundingApiSearchResponseFunding>(new FundingApiSearchResponse());
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="request">An FundingApiSearchLocalAuthoritiesRequest object.</param>
        /// <returns>Nothing - throws an error.</returns>
        public Task<FundingApiSearchLocalAuthoritiesResponse> SearchLocalAuthorities(FundingApiSearchLocalAuthoritiesRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Use a mock file to return provider funding data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="getLatest">Whether to get the latest funding only.</param>
        /// <returns>A provider funding collection.</returns>
        public async Task<IFundingApiSearchResponseProviderFunding> SearchProviderFunding(FundingApiSearchRequestObject request, bool getLatest = false)
        {
            string phaseOfEducation, orgName,
                providerType = string.Empty,
                providerSubType = string.Empty,
                laName = string.Empty,
                laeStab = "--",
                upin = "--",
                urn = "--",
                providerStatus = ProviderStatus.Open,
                groupingReason = GroupingReason.Information;

            var dateOpened = new DateTime(2000, 1, 1);
            var ukPrn = request?.FundingStreams?.FirstOrDefault()?.Filters?
                .FirstOrDefault(x => x.PropertyName == Enums.SearchFilterPropertyName.Ukprn)?.PropertyValue;

            var providerFundingAllocations = new Dictionary<int, Dictionary<string, string>>();

            var searchTerm = string.IsNullOrWhiteSpace(request.SearchTerm) ? ukPrn : request.SearchTerm;
            var parentType = GroupingType.AcademyTrust;

            switch (searchTerm)
            {
                default:
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "PNA_Static");
                    orgName = "Abbey View Primary Academy";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
            }

            var providerFunding = new List<FundingApiSearchProviderFunding>();
            var loopCount = 0;

            foreach (var item in providerFundingAllocations)
            {
                var fileName = item.Value["fileName"];
                var statusChangeDate = Convert.ToDateTime(item.Value["statusChangedDate"]);
                var variationReason = item.Value["variationReason"];
                var fundingPeriodCode = item.Value["fundingPeriodCode"];

                var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{fileName}.json");
                var fundingValue = JsonConvert.DeserializeObject<FundingValueNested_1_0>(fileContents);
                var totalValue = fundingValue.TotalValue ?? 0;

                providerFunding.Add(
                    new FundingApiSearchProviderFunding
                    {
                        FundingPeriodCode = fundingPeriodCode,
                        FundingStreamCode = "PNA",
                        FundingVersion = "1_0",
                        ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                        StatementChannelVersion = 1,
                        IsFirstStatementChannelVersion = true,
                        SchemaVersion = "1.0",
                        TemplateVersion = "1.0",
                        FundingValue = fileContents,
                        TotalAmount = totalValue,
                        OrganisationName = orgName,
                        OrganisationUkprn = request.SearchTerm,
                        ParentProviderType = parentType,
                        GroupingReason = variationReason.Contains("indicative", StringComparison.InvariantCultureIgnoreCase) ? GroupingReason.Indicative : groupingReason,
                        PhaseOfEducation = phaseOfEducation,
                        DateOpened = dateOpened,
                        Id = $"{loopCount++}-PNA-Id30",
                        ProviderType = providerType,
                        ProviderSubType = providerSubType,
                        SearchableOrganisationName = orgName,
                        ParentName = laName,
                        OrganisationDfeNumber = laeStab,
                        ProviderUpin = upin,
                        ProviderUrn = urn,
                        StatusChangedDate = statusChangeDate,
                        VariationReason = variationReason,
                        ProviderStatus = variationReason.Contains("indicative", StringComparison.InvariantCultureIgnoreCase) ? ProviderStatus.IndicativeStatuses.First() : providerStatus
                    });
            }

            return new ProviderFundingApiSearchResponse
            {
                ProviderFunding = providerFunding.OrderByDescending(x => x.StatusChangedDate).ToList()
            };
        }

        private static void BuildProviderFundingAllocations(Dictionary<int, Dictionary<string, string>> providerFundingRequestParams, string fileName, string statusChangedDate = null, string variationReason = null, string fundingPeriodCode = "AC-2122")
        {
            var index = providerFundingRequestParams.Count + 1;
            providerFundingRequestParams.Add(index, new Dictionary<string, string>
            {
                { "fileName", fileName },
                { "statusChangedDate", statusChangedDate ?? "2021-06-01" },
                { "variationReason", variationReason ?? "Initial allocation." },
                { "fundingPeriodCode", fundingPeriodCode }
            });
        }
    }
}
