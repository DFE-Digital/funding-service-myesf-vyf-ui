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
    /// A short term use implementation of funding service to get mock 1416 data.
    /// </summary>
    public class Local1416_FakeApiService : BaseFakeApiService, IFundingApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Local1416_FakeApiService"/> class.
        /// </summary>
        /// <param name="httpApiService">The http api service.</param>
        public Local1416_FakeApiService(IHttpApiService httpApiService) : base(httpApiService)
        {
        }

        /// <inheritdoc/>
        public Task<IFundingApiSearchFunding> GetFunding(string id)
        {
            throw new NotImplementedException(nameof(GetFunding));
        }

        /// <inheritdoc/>
        public async Task<IFundingApiSearchProviderFunding> GetProviderFunding(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var schemaVersion = "1.2";
            var fundingVersion = "1_0";
            var channelVersions = FundingVersionHelper.BuildChannelVersions(1);
            var statementChannelVersion = 1;
            var isFirstStatementChannelVersion = true;
            var ukprn = "10072811";
            var openReason = string.Empty;
            var dateOpened = new DateTime(2000, 1, 1);
            var filename = "1416_Static_FE";
            var totalAmount = 1.23;
            var orgName = "Truro and Penwith College";
            var laName = "Cornwall";
            var esfaReferenceUpin = string.Empty;
            var regionName = "South and South-West";
            var providerType = "Furth";
            var providerSubType = "11ACA";
            var urn = "140294";

            switch (id)
            {
                case "1416-AS-2122-10072811-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    fundingVersion = "2_0";
                    totalAmount = 1.23;
                    orgName = "Barton Peveril College";
                    laName = "Hampshire";
                    esfaReferenceUpin = "108437";
                    ukprn = "10007063";
                    regionName = "South and South-West";
                    urn = "140294";
                    break;

                case "1416-AS-2122-10007063-1_0":
                    filename = "1416_Static_FE_Without_PupilPremium";
                    orgName = "Simon Balle All-Through School";
                    laName = "Hertfordshire";
                    esfaReferenceUpin = "9194067";
                    regionName = "South and South-West";
                    dateOpened = new DateTime(2013, 11, 01);
                    ukprn = "10034949";
                    urn = "140294";
                    break;

                case "1416-AY-2425-10072811-1_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    fundingVersion = "1_0";
                    totalAmount = 1.23;
                    orgName = "Barton Peveril College";
                    laName = "Hampshire";
                    esfaReferenceUpin = "108437";
                    ukprn = "10007063";
                    regionName = "South and South-West";
                    urn = "140294";
                    filename = "1416_Static_Summary_English_and_Maths_funding";
                    break;
            }

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");
            var fundingValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents);
            fundingValue.Add("totalValue", totalAmount);

            return new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AS-2122",
                FundingStreamCode = "1416",
                FundingVersion = fundingVersion,
                ChannelVersions = channelVersions,
                StatementChannelVersion = statementChannelVersion,
                IsFirstStatementChannelVersion = isFirstStatementChannelVersion,
                SchemaVersion = schemaVersion,
                TemplateVersion = "1.0",
                FundingValue = JsonConvert.SerializeObject(fundingValue),
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = ukprn,
                ProviderUpin = esfaReferenceUpin,
                Id = id,
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                ParentProviderType = GroupingType.Provider,
                GroupingReason = GroupingReason.Contracting,
                DateOpened = dateOpened,
                ParentName = laName,
                LocalAuthorityName = laName,
                RegionName = regionName, // Note - 1. This isnt hooked up in the API yet, and i dont think we have access to the ESFA territory.
                StatusChangedDate = new DateTime(2021, 1, 10),
                ProviderUrn = urn,
                OpenReason = openReason
            };
        }

        /// <inheritdoc/>
        public Task<IFundingApiSearchResponseFunding> SearchFunding(FundingApiSearchRequestObject requestObj)
        {
            return Task.FromResult<IFundingApiSearchResponseFunding>(new FundingApiSearchResponse());
        }

        /// <inheritdoc/>
        public Task<FundingApiSearchLocalAuthoritiesResponse> SearchLocalAuthorities(FundingApiSearchLocalAuthoritiesRequest request)
        {
            throw new NotImplementedException(nameof(SearchLocalAuthorities));
        }

        /// <inheritdoc/>
        public async Task<IFundingApiSearchResponseProviderFunding> SearchProviderFunding(FundingApiSearchRequestObject request, bool getLatest = false)
        {
            string orgName = "Truro and Penwith College",
                esfaReferenceUpin = "--",
                urn = "--",
                laName = "Cornwall",
                regionName = "South and South-West", laeStab = "1234";

            var dateOpened = new DateTime(2000, 1, 1);
            var ukPrn = request?.FundingStreams?.FirstOrDefault()?.Filters?
                .FirstOrDefault(x => x.PropertyName == Enums.SearchFilterPropertyName.Ukprn)?.PropertyValue;

            var providerFundingAllocations = new Dictionary<int, Dictionary<string, string>>();

            var searchTerm = string.IsNullOrWhiteSpace(request.SearchTerm) ? ukPrn : request.SearchTerm;

            switch (searchTerm)
            {
                case "10072811":
                case "10034949":
                    BuildProviderFundingAllocations(providerFundingAllocations, "1416_Static_FE_WithDisadvantage", "2021-10-01", "Revised allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "1416_Static_FE_WithDisadvantage_Allocation_1", "2021-09-06", "Initial allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "1416_Static_FE_WithDisadvantage_Allocation_2", "2020-06-06", "Initial allocation.", "AS-2021");
                    BuildProviderFundingAllocations(providerFundingAllocations, "1416_Static_FE_WithDisadvantage_Allocation_3", "2020-07-06", "Revised allocation.", "AS-2021");
                    urn = "140294";
                    esfaReferenceUpin = "108437";
                    laeStab = "1234";
                    urn = "140294";
                    break;

                default:
                    orgName = "Simon Balle All-Through School";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1416_Static_FE_Without_PupilPremium", "2021-09-01", "Revised allocation.");
                    laName = "Hertfordshire";
                    esfaReferenceUpin = "9194067";
                    regionName = "South and South-West";
                    dateOpened = new DateTime(2013, 11, 01);
                    laeStab = "5678";
                    urn = "140294";

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
                         FundingStreamCode = "1416",
                         FundingVersion = "1_0",
                         ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                         StatementChannelVersion = 1,
                         IsFirstStatementChannelVersion = true,
                         SchemaVersion = "1.2",
                         TemplateVersion = "1.0",
                         FundingValue = fileContents,
                         TotalAmount = totalValue,
                         OrganisationName = orgName,
                         OrganisationUkprn = request.SearchTerm,
                         OrganisationDfeNumber = laeStab,
                         Id = $"{loopCount++}-1416-{request.SearchTerm}",
                         ProviderType = "Furth",
                         ProviderSubType = "11ACA",
                         ParentProviderType = GroupingType.Provider,
                         GroupingReason = GroupingReason.Contracting,
                         DateOpened = new DateTime(2000, 1, 1),
                         ParentName = laName,
                         RegionName = regionName, // Note - 1. This isnt hooked up in the API yet, and i dont think we have access to the ESFA territory.,
                         StatusChangedDate = statusChangeDate,
                         VariationReason = "Initial allocation.",
                         ProviderUpin = esfaReferenceUpin,
                         ProviderUrn = urn
                     });
            }

            return new ProviderFundingApiSearchResponse
            {
                ProviderFunding = providerFunding.OrderByDescending(x => x.StatusChangedDate).ToList()
            };
        }

        private static void BuildProviderFundingAllocations(Dictionary<int, Dictionary<string, string>> providerFundingRequestParams, string fileName, string statusChangedDate = null, string variationReason = null, string fundingPeriodCode = "AY-2122")
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
