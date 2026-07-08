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
    public class LocalNMSS_FakeApiService : BaseFakeApiService, IFundingApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalNMSS_FakeApiService"/> class.
        /// Create a new instance of a LocalNMSS_FakeApiService.
        /// </summary>
        /// <param name="httpApiService">The HTTP service to use.</param>
        public LocalNMSS_FakeApiService(IHttpApiService httpApiService)
            : base(httpApiService)
        {
        }

        /// <summary>
        /// Search the funding collection using supplied filters.
        /// </summary>
        /// <param name="requestObj">An FundingApiSearchRequestObject object.</param>
        /// <returns>Returns a IFundingApiSearchResponseFunding that contains the funding(s).</returns>
        public async Task<IFundingApiSearchResponseFunding> SearchFunding(FundingApiSearchRequestObject requestObj)
        {
            var filename = "NMSS_Static_LA";
            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");
            var fundingValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents);

            return new FundingApiSearchResponse
            {
                Funding = new List<FundingApiSearchFunding>
                {
                   new FundingApiSearchFunding
                   {
                       FundingPeriodCode = "AY-2122",
                       FundingStreamCode = "NMSS",
                       FundingVersion = "1_0",
                       ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                       StatementChannelVersion = 1,
                       IsFirstStatementChannelVersion = true,
                       SchemaVersion = "1.0",
                       TemplateVersion = "1.0",
                       GroupingReason = GroupingReason.Information,
                       FundingValue = JsonConvert.SerializeObject(fundingValue),
                       GroupingType = GroupingType.LocalAuthority
                   }
                }
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
            return await SearchProviderFunding(request, null);
        }

        /// <summary>
        /// Use a mock file to return provider funding data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="parentGroupingType">The grouping reason (e.g LocalAuthoirty).</param>
        /// <returns>A matched provider funding.</returns>
        public async Task<IFundingApiSearchResponseProviderFunding> SearchProviderFunding(FundingApiSearchRequestObject request, string parentGroupingType)
        {
            var ukprn = request.SearchTerm;

            if (string.IsNullOrEmpty(ukprn))
            {
                // LA providers
                return new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = await GetNProviderFunding(5, new List<string>
                    {
                        "10007063",
                        "10000552",
                        "10034690",
                        "10030654",
                        "10001929",
                        "10000866",
                        "10006247"
                    })
                };
            }

            var dateOpened = new DateTime(2000, 1, 1);
            var filename = "NMSS_Static";
            var totalAmount = 233.55;
            var orgName = "Birtenshaw";
            var laName = "Bolton";
            var esfaReferenceUpin = "122211";
            var regionName = "Northern England";
            var urn = "140294";

            switch (ukprn)
            {
                case "10000552":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Barton Peveril College";
                    laName = "Hampshire";
                    esfaReferenceUpin = "108437";
                    regionName = "South and South-West";
                    urn = "140294";

                    break;
                case "10034690":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 72634;
                    orgName = "The Stourport High School and Sixth Form College";
                    laName = "Worcestershire";
                    esfaReferenceUpin = "120162";
                    regionName = "Midlands and East of England";
                    urn = "230294";

                    break;
                case "10030654":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 22.44;
                    orgName = "Aylward Academy";
                    laName = "Enfield";
                    esfaReferenceUpin = "119211";
                    regionName = "London and South-East";
                    urn = "563432";

                    break;
                case "10001929":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 87346;
                    orgName = "Derwen College";
                    laName = "Shropshire";
                    esfaReferenceUpin = "114847";
                    regionName = "Midlands and East of England";
                    urn = "87345";

                    break;
                case "10000866":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 323.77;
                    orgName = "Brentside High School";
                    laName = "Ealing";
                    esfaReferenceUpin = "113774";
                    regionName = "London and South-East";
                    urn = "134987";

                    break;
                case "10006247":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 23234.44;
                    orgName = "St Paul's Catholic College";
                    laName = "Surrey";
                    esfaReferenceUpin = "113066";
                    regionName = "London and South-East";
                    urn = "083333";

                    break;
                case "10040630":
                    orgName = "Haringey Sixth Form College";
                    laName = "Haringey";
                    urn = "864266";
                    break;
                case "10016972":
                    dateOpened = new DateTime(2000, 1, 1);
                    orgName = "St Catherine's School";
                    laName = "Isle of Wight";
                    urn = "118226";
                    filename = "NMSS_Static_DecreaseInStudentNumbers";
                    break;
                case "10030456":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Sidney Stringer Academy";
                    laName = "Coventry";
                    esfaReferenceUpin = "108437";
                    regionName = "West Middlands";
                    urn = "136126";
                    filename = "NMSS_Static_Adjusted_DBF_Above_Upper_Limit";

                    break;
                case "10054167":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Barr's Hill School and Community College";
                    laName = "Coventry";
                    esfaReferenceUpin = "108437";
                    regionName = "West Middlands";
                    urn = "142339";
                    filename = "NMSS_Static_Adjusted_DBF_Below_Lower_Limit";

                    break;
                case "10033567":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Blue Coat Church of England School and Music College";
                    laName = "Coventry";
                    esfaReferenceUpin = "108437";
                    regionName = "West Middlands";
                    urn = "137272";
                    filename = "NMSS_Static_Adjusted_DBF_Below_500";

                    break;
                case "10061616":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Hearsall Community Academy";
                    laName = "Coventry";
                    esfaReferenceUpin = "108437";
                    regionName = "West Middlands";
                    urn = "143535";
                    filename = "NMSS_Static_Adjusted_DBF_In_Limit";

                    break;
                case "10001232":
                    dateOpened = new DateTime(2000, 1, 1);
                    orgName = "St Vincent's School - A Specialist School for Sensory Impairment and Other Needs";
                    laName = "Liverpool";
                    regionName = "Merseyside";
                    urn = "104734";
                    filename = "NMSS_Static_IdenticalStudentNumbers";

                    break;
            }

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");
            var fundingValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents);
            fundingValue.Add("totalValue", totalAmount);

            var loopCount = 0;
            return new ProviderFundingApiSearchResponse
            {
                ProviderFunding = new List<FundingApiSearchProviderFunding>
                {
                    new FundingApiSearchProviderFunding
                    {
                        FundingPeriodCode = "AY-2122",
                        FundingStreamCode = "NMSS",
                        FundingVersion = "1_0",
                        ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                        StatementChannelVersion = 1,
                        IsFirstStatementChannelVersion = true,
                        SchemaVersion = "1.0",
                        TemplateVersion = "1.0",
                        FundingValue = JsonConvert.SerializeObject(fundingValue),
                        TotalAmount = totalAmount,
                        OrganisationName = orgName,
                        OrganisationUkprn = ukprn,
                        ProviderUpin = esfaReferenceUpin,
                        Id = $"{loopCount++}-NMSS-{ukprn}",
                        ParentProviderType = parentGroupingType ?? GroupingType.LocalAuthority,
                        GroupingReason = parentGroupingType == GroupingType.AcademyTrust ? GroupingReason.Payment : GroupingReason.Information,
                        DateOpened = dateOpened,
                        ParentName = laName,
                        RegionName = regionName,
                        ProviderType = "None maintained special school",
                        ProviderSubType = "21NMS",
                        StatusChangedDate = new DateTime(2021, 1, 10),
                        ProviderUrn = urn
                    }
                }
            };
        }

        /// <summary>
        /// An HTTP api service to fetch funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A funding collection.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IFundingApiSearchFunding> GetFunding(string id)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException("GetFunding not needed for NMSS");
        }

        /// <summary>
        /// An HTTP api service to fetch provider funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A provider funding collection.</returns>
        public async Task<IFundingApiSearchProviderFunding> GetProviderFunding(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var ukprn = "10015031";

            var dateOpened = new DateTime(2000, 1, 1);
            var filename = "NMSS_Static";
            var totalAmount = 44.44;
            var orgName = "Birtenshaw";
            var laName = "Bolton";
            var esfaReferenceUpin = "122211";
            var regionName = "Northern England";
            var urn = "140294";

            switch (id)
            {
                case "NMSS-AC-2122-10000552-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Barton Peveril College";
                    laName = "Hampshire";
                    esfaReferenceUpin = "108437";
                    ukprn = "10000552";
                    regionName = "South and South-West";
                    urn = "140294";

                    break;
                case "NMSS-AC-2122-10034690-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "The Stourport High School and Sixth Form College";
                    laName = "Worcestershire";
                    esfaReferenceUpin = "120162";
                    ukprn = "10034690";
                    regionName = "Midlands and East of England";
                    urn = "230294";

                    break;
                case "NMSS-AC-2122-10030654-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Aylward Academy";
                    laName = "Enfield";
                    esfaReferenceUpin = "119211";
                    ukprn = "10030654";
                    regionName = "London and South-East";
                    urn = "563432";

                    break;
                case "NMSS-AC-2122-10001929-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Derwen College";
                    laName = "Shropshire";
                    esfaReferenceUpin = "114847";
                    ukprn = "10001929";
                    regionName = "Midlands and East of England";
                    urn = "87345";

                    break;
                case "NMSS-AC-2122-10004756-1_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Abbey Hill Academy";
                    laName = "Stockton-on-Tees";
                    esfaReferenceUpin = "114847";
                    ukprn = "10004756";
                    regionName = "Midlands and East of England";
                    urn = "635435";

                    break;
                case "NMSS-AC-2122-10000866-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Brentside High School";
                    laName = "Ealing";
                    esfaReferenceUpin = "113774";
                    ukprn = "10000866";
                    regionName = "London and South-East";
                    urn = "134987";

                    break;
                case "NMSS-AC-2122-10006247-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "St Paul's Catholic College";
                    laName = "Surrey";
                    esfaReferenceUpin = "113066";
                    ukprn = "10006247";
                    regionName = "London and South-East";
                    urn = "083333";

                    break;
                case "NMSS-AC-2122-10040630-1_0":
                    orgName = "Haringey Sixth Form College";
                    laName = "Haringey";
                    ukprn = "10040630";
                    urn = "864266";

                    break;
                case "NMSS-FY-2021-10040631-1_0":
                    orgName = "Haringey Sixth Form College";
                    laName = "Haringey";
                    ukprn = "10040631";
                    urn = "864266";

                    break;

                case "NMSS-AY-2223-10040631-1_0":
                    orgName = "Haringey Sixth Form College";
                    laName = "Haringey";
                    ukprn = "10040631";
                    urn = "864266";
                    filename = "NMSS_Static_WithDefaultStudentProportions";

                    break;

                case "NMSS-AY-2223-10034690-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "The Stourport High School and Sixth Form College";
                    laName = "Worcestershire";
                    esfaReferenceUpin = "120162";
                    ukprn = "10034690";
                    regionName = "Midlands and East of England";
                    urn = "230294";
                    filename = "NMSS_Static_AgreedChanges_HighNeedsPlaces";

                    break;

                case "NMSS-AY-2324-10034690-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "The Stourport High School and Sixth Form College";
                    laName = "Worcestershire";
                    esfaReferenceUpin = "120162";
                    ukprn = "10034690";
                    regionName = "Midlands and East of England";
                    urn = "230294";
                    filename = "NMSS_Static_AgreedChanges_BaselineTransitionValue";

                    break;
            }

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");
            var fundingValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents);
            fundingValue.Add("totalValue", totalAmount);

            return new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AY-2122",
                FundingStreamCode = "NMSS",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = JsonConvert.SerializeObject(fundingValue),
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = ukprn,
                ProviderUpin = esfaReferenceUpin,
                Id = $"0-NMSS-{ukprn}",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                DateOpened = dateOpened,
                ParentName = laName,
                RegionName = regionName,
                ProviderType = "None maintained special school",
                ProviderSubType = "21NMS",
                StatusChangedDate = new DateTime(2021, 1, 10),
                ProviderUrn = urn
            };
        }

        private async Task<List<IFundingApiSearchProviderFunding>> GetNProviderFunding(int n, List<string> ukprns)
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

                var parentGroupingTypes = new string[3] { GroupingType.AcademyTrust, GroupingType.LocalAuthorityMaintained, GroupingType.LocalAuthority };

                foreach (var parentGroupingType in parentGroupingTypes)
                {
                    var cacheKey = $"{ukprn}{parentGroupingType}";

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
                            }, parentGroupingType)).ProviderFunding.First();

                        lookupCache.Add(cacheKey, providerFunding);
                    }

                    var serialised = JsonConvert.SerializeObject(providerFunding);
                    providerFunding = JsonConvert.DeserializeObject<FundingApiSearchProviderFunding>(serialised);

                    providerFunding.Id += $"-{idx}-{parentGroupingType}";

                    returnList.Add(providerFunding);
                }
            }

            return returnList;
        }
    }
}