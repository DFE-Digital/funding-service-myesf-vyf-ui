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
    /// A short term use implementation of funding service to get mock LAREC data.
    /// </summary>
    public class LocalLAREC_FakeApiService : BaseFakeApiService, IFundingApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalLAREC_FakeApiService"/> class.
        /// Create a new instance of a LocalLAREC_FakeApiService.
        /// </summary>
        /// <param name="httpApiService">The HTTP service to use.</param>
        public LocalLAREC_FakeApiService(IHttpApiService httpApiService)
            : base(httpApiService)
        {
        }

        /// <summary>
        /// Gets Funding Data.
        /// </summary>
        /// <param name="requestObj">An FundingApiSearchRequestObject object.</param>
        /// <returns>The static funding search data.</returns>
        public async Task<IFundingApiSearchResponseFunding> SearchFunding(FundingApiSearchRequestObject requestObj)
        {
            var filename = new List<string>
            {
                "LAREC_Static_LA",
                "LAREC_Static_LA_V2"
            };
            var fundings = new List<FundingApiSearchFunding>();

            // for 2223
            foreach (var file in filename)
            {
                fundings.Add((FundingApiSearchFunding)await GetFundingByYear(file, 2022));
            }

            foreach (var file in filename)
            {
                var result = await GetFundingByYear(file, 2021);

                result.FundingPeriodCode = "FY-2122";

                fundings.Add((FundingApiSearchFunding)result);
            }

            foreach (var file in filename)
            {
                var result = await GetFundingByYear(file, 2020);

                result.FundingPeriodCode = "FY-2021";

                fundings.Add((FundingApiSearchFunding)result);
            }

            return new FundingApiSearchResponse
            {
                Funding = fundings
            };
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
            return await SearchProviderFunding(request, null, null);
        }

        /// <summary>
        /// An HTTP api service to fetch funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A funding collection.</returns>
        public async Task<IFundingApiSearchFunding> GetFunding(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var statusChanged = new DateTime(2022, 10, 10);

            if (id == "LAREC_Static_LA_V2")
            {
                statusChanged = new DateTime(2022, 11, 10);
            }

            double total = 1.23;
            var groupingReason = GroupingReason.Information;
            var groupingType = GroupingType.LocalAuthority;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{id}.json");
            var fundingValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents);
            fundingValue.Add("totalValue", total);

            return new FundingApiSearchFunding
            {
                Id = id,
                FundingPeriodCode = "FY-2223",
                FundingStreamCode = "LAREC",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.2",
                TemplateVersion = "1.2",
                FundingValue = JsonConvert.SerializeObject(fundingValue),
                GroupingReason = groupingReason,
                GroupingType = groupingType,
                TotalAmount = total,
                GroupCode = "919",
                RegionName = "London and South-East",
                StatusChangedDate = statusChanged,
                GroupUkprn = "10004801"
            };
        }

        /// <summary>
        /// An HTTP api service to fetch provider funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>The matched provider funding.</returns>
        public async Task<IFundingApiSearchProviderFunding> GetProviderFunding(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var schemaVersion = "1.2";
            var ukprn = "10007063";
            var openReason = string.Empty;

            var dateOpened = new DateTime(2000, 1, 1);
            var filename = "LAREC_static_json";
            var totalAmount = 1.23;
            var orgName = "Truro and Penwith Academy";
            var laName = "Hertfordshire";
            var esfaReferenceUpin = "108441";
            var regionName = "South and South-West";
            var providerType = "Furth";
            var providerSubType = "11ACA";
            var urn = "140294";

            switch (id)
            {
                case "LAREC-FY-2223-10000552-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "LAREC_static_json";
                    totalAmount = 1.23;
                    orgName = "Barton Peveril Academy";
                    laName = "Hampshire";
                    esfaReferenceUpin = "108437";
                    ukprn = "10000552";
                    regionName = "South and South-West";
                    providerType = "Furth";
                    urn = "140294";

                    break;
            }

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");
            var fundingValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents);
            fundingValue.Add("totalValue", totalAmount);

            if (providerSubType == null)
            {
                providerSubType = providerType;
            }

            return new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "FY-2223",
                FundingStreamCode = "LAREC",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = schemaVersion,
                TemplateVersion = "1.2",
                FundingValue = JsonConvert.SerializeObject(fundingValue),
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = ukprn,
                ProviderUpin = esfaReferenceUpin,
                Id = $"0-LAREC-{ukprn}",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                ParentProviderType = GroupingType.LocalAuthority,
                GroupingReason = GroupingReason.Information,
                DateOpened = dateOpened,
                ParentName = laName,
                LocalAuthorityName = laName,
                RegionName = regionName, // Note - 1. This isnt hooked up in the API yet, and i dont think we have access to the ESFA territory.
                StatusChangedDate = new DateTime(2021, 1, 10),
                ProviderUrn = urn,
                OpenReason = openReason
            };
        }

        /// <summary>
        /// Use a mock file to return provider funding data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="parentGroupingType">The groupoing reason (e.g LocalAuthoirty).</param>
        /// <param name="groupingReason">The grouping reason.</param>
        /// <returns>A provider funding collection.</returns>
        protected async Task<IFundingApiSearchResponseProviderFunding> SearchProviderFunding(
            FundingApiSearchRequestObject request,
            string parentGroupingType,
            string groupingReason)
        {
            var ukprn = request.SearchTerm;

            if (string.IsNullOrEmpty(ukprn))
            {
                // LA providers
                return new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = await GetNProviderFunding(
                        15,
                        new List<string>
                        {
                            "10007063",
                            "10060613"
                        },
                        request)
                };
            }

            var providerFundingAllocations = new Dictionary<int, Dictionary<string, string>>();

            var dateOpened = new DateTime(2000, 1, 1);
            var totalAmount = 1.23;
            var orgName = "Truro and Penwith Academy";
            var laName = "Cornwall";
            var esfaReferenceUpin = "108441";
            var regionName = "South and South-West";
            var providerType = "providerType";
            var providerSubType = "providerSubType";
            var urn = "140294";
            var providerStatus = ProviderStatus.Open;

            switch (ukprn)
            {
                case "10060613":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Chamberlayne Academy";
                    laName = "Norfolk";
                    esfaReferenceUpin = "9264111";
                    regionName = "Norwich";
                    urn = "141086";
                    BuildProviderFundingAllocations(providerFundingAllocations, "LAREC_static_json");
                    break;

                default:
                    dateOpened = new DateTime(2000, 1, 1);
                    orgName = "Truro and Penwith Academy";
                    laName = "Cornwall";
                    esfaReferenceUpin = "108441";
                    regionName = "South and South-West";
                    urn = "140294";
                    BuildProviderFundingAllocations(providerFundingAllocations, "LAREC_static_json", "2021-10-20");
                    BuildProviderFundingAllocations(providerFundingAllocations, "LAREC_static_json_V2", "2021-11-20");
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
                var fundingValue = JsonConvert.DeserializeObject<FundingValueNested_1_2>(fileContents);
                var totalValue = fundingValue.TotalValue ?? totalAmount;
                providerSubType ??= providerType;

                providerFunding.Add(
                    new FundingApiSearchProviderFunding
                    {
                        FundingPeriodCode = fundingPeriodCode,
                        FundingStreamCode = "LAREC",
                        FundingVersion = "1_0",
                        ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                        StatementChannelVersion = 1,
                        IsFirstStatementChannelVersion = true,
                        SchemaVersion = "1.2",
                        TemplateVersion = "1.2",
                        FundingValue = JsonConvert.SerializeObject(fundingValue),
                        TotalAmount = totalValue,
                        OrganisationName = orgName,
                        OrganisationUkprn = ukprn,
                        ProviderUpin = esfaReferenceUpin,
                        OrganisationDfeNumber = "1234",
                        Id = $"{loopCount++}-LAREC-{ukprn}",
                        ProviderType = providerType,
                        ProviderSubType = providerSubType,
                        ParentProviderType = parentGroupingType ?? GroupingType.LocalAuthority,
                        GroupingReason = groupingReason ?? GroupingReason.Information,
                        DateOpened = dateOpened,
                        ParentName = laName,
                        RegionName = regionName, // Note - 1. This isnt hooked up in the API yet, and i dont think we have access to the ESFA territory.
                        StatusChangedDate = statusChangeDate,
                        VariationReason = variationReason,
                        ProviderUrn = urn,
                        ProviderStatus = providerStatus
                    });
            }

            return new ProviderFundingApiSearchResponse
            {
                ProviderFunding = providerFunding.OrderByDescending(x => x.StatusChangedDate).ToList()
            };
        }

        private static void BuildProviderFundingAllocations(Dictionary<int, Dictionary<string, string>> providerFundingRequestParams, string fileName, string statusChangedDate = null, string variationReason = null, string fundingPeriodCode = null)
        {
            var index = providerFundingRequestParams.Count + 1;
            providerFundingRequestParams.Add(index, new Dictionary<string, string>
            {
                { "fileName", fileName },
                { "statusChangedDate", statusChangedDate ?? "2021-07-01" },
                { "variationReason", variationReason ?? "Initial allocation." },
                { "fundingPeriodCode", fundingPeriodCode ?? "FY-2223" }
            });
        }

        private async Task<List<IFundingApiSearchProviderFunding>> GetNProviderFunding(int n, List<string> ukprns, FundingApiSearchRequestObject request)
        {
            var returnList = new List<IFundingApiSearchProviderFunding>();
            var lookupCache = new Dictionary<string, IFundingApiSearchProviderFunding>();
            var iInRange = 0;

            for (var idx = 0; idx < n; idx++)
            {
                if (iInRange >= ukprns.Count)
                {
                    iInRange = 0;
                }

                var ukprn = ukprns[iInRange++];
                IFundingApiSearchProviderFunding providerFunding;

                var parentGroupingTypes = new string[]
                {
                    GroupingType.LocalAuthority
                };

                foreach (var parentGroupingType in parentGroupingTypes)
                {
                    var cacheKey = $"{ukprn}{parentGroupingType}";
                    const string groupingReason = GroupingReason.Information;

                    if (lookupCache.ContainsKey(cacheKey))
                    {
                        providerFunding = lookupCache[cacheKey];
                    }
                    else
                    {
                        providerFunding = (await SearchProviderFunding(
                            new FundingApiSearchRequestObject
                            {
                                SearchTerm = ukprn
                            },
                            parentGroupingType,
                            groupingReason)).ProviderFunding.First();

                        lookupCache.Add(cacheKey, providerFunding);
                    }

                    var serialised = JsonConvert.SerializeObject(providerFunding);
                    providerFunding = JsonConvert.DeserializeObject<FundingApiSearchProviderFunding>(serialised);

                    providerFunding.Id += $"-{idx}-{parentGroupingType}";
                    providerFunding.ParentProviderType = parentGroupingType;
                    providerFunding.GroupingReason = groupingReason;
                    providerFunding.DateOpened = providerFunding.DateOpened?.AddYears(idx);
                    providerFunding.OrganisationName = $"Recoupable Academy-{idx}";
                    returnList.Add(providerFunding);
                }
            }

            return returnList;
        }

        private async Task<IFundingApiSearchFunding> GetFundingByYear(string id, int year)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var statusChanged = new DateTime(year, 10, 10);

            if (id == "LAREC_Static_LA_V2")
            {
                statusChanged = new DateTime(year, 11, 10);
            }

            double total = 1.23;
            var groupingReason = GroupingReason.Information;
            var groupingType = GroupingType.LocalAuthority;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{id}.json");
            var fundingValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents);
            fundingValue.Add("totalValue", total);

            return new FundingApiSearchFunding
            {
                Id = id,
                FundingPeriodCode = "FY-2223",
                FundingStreamCode = "LAREC",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.2",
                TemplateVersion = "1.2",
                FundingValue = JsonConvert.SerializeObject(fundingValue),
                GroupingReason = groupingReason,
                GroupingType = groupingType,
                TotalAmount = total,
                GroupCode = "919",
                RegionName = "London and South-East",
                StatusChangedDate = statusChanged,
                GroupUkprn = "10004801"
            };
        }
    }
}