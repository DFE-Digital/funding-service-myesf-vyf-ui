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
    /// A short term use implementation of funding service to get mock 1619 data.
    /// </summary>
    public class Local1619_FakeApiService : BaseFakeApiService, IFundingApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Local1619_FakeApiService"/> class.
        /// Create a new instance of a LocalNMSS_FakeApiService.
        /// </summary>
        /// <param name="httpApiService">The HTTP service to use.</param>
        public Local1619_FakeApiService(IHttpApiService httpApiService)
            : base(httpApiService)
        {
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="requestObj">An FundingApiSearchRequestObject object.</param>
        /// <returns>Nothing - throws an error.</returns>
        public async Task<IFundingApiSearchResponseFunding> SearchFunding(FundingApiSearchRequestObject requestObj)
        {
            // for 2122
            var forContract = (FundingApiSearchFunding)await GetFunding("ForContract");
            var infoLA = (FundingApiSearchFunding)await GetFunding("1619-AS-2122-Information-LocalAuthority-341-1_0");
            infoLA.StatusChangedDate = new DateTime(2021, 01, 08);
            var sixthSpecial = (FundingApiSearchFunding)await GetFunding("SixthSpecial");
            sixthSpecial.StatusChangedDate = new DateTime(2021, 01, 02);

            //for 2021
            var forContract20 = (FundingApiSearchFunding)await GetFunding("ForContract");
            forContract20.StatusChangedDate = new DateTime(2020, 01, 10);
            forContract20.FundingPeriodCode = "AS-2021";
            var infoLA20 = (FundingApiSearchFunding)await GetFunding("1619-AS-2122-Information-LocalAuthority-341-1_0");
            infoLA20.StatusChangedDate = new DateTime(2020, 06, 01);
            infoLA20.FundingPeriodCode = "AS-2021";
            var sixthSpecial20 = (FundingApiSearchFunding)await GetFunding("SixthSpecial");
            sixthSpecial20.StatusChangedDate = new DateTime(2020, 02, 01);
            sixthSpecial20.FundingPeriodCode = "AS-2021";


            return new FundingApiSearchResponse
            {
                Funding = new List<FundingApiSearchFunding>
                {
                    forContract,
                    infoLA,
                    sixthSpecial,
                    forContract20,
                    infoLA20,
                    sixthSpecial20
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

            string filename;
            double total;
            var groupingReason = GroupingReason.Information;
            var groupingType = GroupingType.LocalAuthority;

            if (id == "1619-AS-2122-Information-LocalAuthority-341-1_0")
            {
                filename = "1619_Static_LA_Student";
                total = 1.23;
            }
            else if (id == "SixthSpecial")
            {
                filename = "1619_Static_LA_Student";
                total = 1.23;
                groupingReason = GroupingReason.Contracting;
                groupingType = GroupingType.LocalAuthority;
            }
            else
            {
                filename = "1619_Static_LA_SixthForm";
                total = 1.23;
                groupingReason = GroupingReason.Contracting;
                groupingType = GroupingType.LocalAuthoritySsf;
            }

            if (id == "1619-AS-2122-Contracting-LocalAuthoritySsf-10001464-1_0")
            {
                groupingType = GroupingType.LocalAuthorityMaintained;
                groupingReason = GroupingReason.Information;
            }

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");
            var fundingValue = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileContents);
            fundingValue.Add("totalValue", total);

            return new FundingApiSearchFunding
            {
                Id = id,
                FundingPeriodCode = "AS-2122",
                FundingStreamCode = "1619",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = JsonConvert.SerializeObject(fundingValue),
                GroupingReason = groupingReason,
                GroupingType = groupingType,
                TotalAmount = total,
                GroupName = "Hertfordshire",
                GroupCode = "919",
                RegionName = "London and South-East",
                StatusChangedDate = new DateTime(2021, 1, 10),
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

            var schemaVersion = "1.0";
            var ukprn = "10007063";
            var openReason = string.Empty;

            var dateOpened = new DateTime(2000, 1, 1);
            var filename = "1619_Static_FE_With";
            var totalAmount = 1.23;
            var orgName = "Truro and Penwith College";
            var laName = "Cornwall";
            var esfaReferenceUpin = "108441";
            var regionName = "South and South-West";
            var providerType = "Furth";
            var providerSubType = "11ACA";
            var urn = "140294";

            switch (id)
            {
                case "1619-AS-2122-10000552-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "1619_Static_FE_Without";
                    totalAmount = 1.23;
                    orgName = "Barton Peveril College";
                    laName = "Hampshire";
                    esfaReferenceUpin = "108437";
                    ukprn = "10000552";
                    regionName = "South and South-West";
                    providerType = "Furth";
                    urn = "140294";

                    break;
                case "1619-AS-2122-10034690-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "1619_Static_Academies_With";
                    totalAmount = 1.23;
                    orgName = "The Stourport High School and Sixth Form College";
                    laName = "Worcestershire";
                    esfaReferenceUpin = "120162";
                    ukprn = "10034690";
                    regionName = "Midlands and East of England";
                    providerType = "Acade";
                    urn = "230294";

                    break;
                case "1619-AS-2122-10034690-3_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "1619_Static_Academies_With_PhasedClosure";
                    totalAmount = 1.23;
                    orgName = "The Stourport High School and Sixth Form College";
                    laName = "Worcestershire";
                    esfaReferenceUpin = "120162";
                    ukprn = "10034690";
                    regionName = "Midlands and East of England";
                    providerType = "Acade";
                    urn = "230294";

                    break;
                case "1619-AS-2122-10030654-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "1619_Static_Academies_Without";
                    totalAmount = 1.23;
                    orgName = "Aylward Academy";
                    laName = "Enfield";
                    esfaReferenceUpin = "119211";
                    ukprn = "10030654";
                    regionName = "London and South-East";
                    providerType = "Acade";
                    urn = "563432";

                    break;
                case "1619-AS-2122-10001929-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "1619_Static_SpecialPost16";
                    totalAmount = 1.23;
                    orgName = "Derwen College";
                    laName = "Shropshire";
                    esfaReferenceUpin = "114847";
                    ukprn = "10001929";
                    regionName = "Midlands and East of England";
                    providerSubType = "18ISP";
                    urn = "87345";

                    break;
                case "1619-AS-2122-10004756-1_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "1619_Static_SpecialAcademy";
                    totalAmount = 1.23;
                    orgName = "Abbey Hill Academy";
                    laName = "Stockton-on-Tees";
                    esfaReferenceUpin = "114847";
                    ukprn = "10004756";
                    regionName = "Midlands and East of England";
                    providerType = "Acade";
                    providerSubType = "17NMF";
                    urn = "635435";

                    break;
                case "1619-AS-2122-10000866-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "1619_Static_SixthForm_Without";
                    totalAmount = 1.23;
                    orgName = "Brentside High School";
                    laName = "Ealing";
                    esfaReferenceUpin = "113774";
                    ukprn = "10000866";
                    regionName = "London and South-East";
                    providerType = "Schoo";
                    urn = "134987";

                    break;
                case "1619-AS-2122-10006247-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "1619_Static_SixthForm_With";
                    totalAmount = 1.23;
                    orgName = "St Paul's Catholic College";
                    laName = "Surrey";
                    esfaReferenceUpin = "113066";
                    ukprn = "10006247";
                    regionName = "London and South-East";
                    providerType = "Schoo";
                    urn = "083333";

                    break;
                case "1619-AS-2122-10040630-1_0":
                    filename = "1619_Static_SixthForm_WithFreeMealsLine";
                    orgName = "Haringey Sixth Form College";
                    laName = "Haringey";
                    providerType = "Academ";
                    ukprn = "10040630";
                    urn = "864266";

                    break;
                case "1619-FY-2021-10040631-1_0":
                    filename = "1619_Static_SixthForm_WithHighValueCoursesForSchoolAndCollegeLeavers_And_SUP_AND_POG";
                    orgName = "Haringey Sixth Form College";
                    laName = "Haringey";
                    providerType = "Acade";
                    providerSubType = "FS1619";
                    ukprn = "10040631";
                    urn = "864266";

                    break;

                case "1619-AS-2223-10040631-1_0":
                    filename = "1619_Static_SixthForm_WithHighValueCoursesForSchoolAndCollegeLeavers_And_SUP_AND_POG";
                    orgName = "Haringey Sixth Form College";
                    laName = "Haringey";
                    providerType = "Acade";
                    providerSubType = "FS1619";
                    ukprn = "10040631";

                    break;

                case "1619-AS-2122-10047244-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "1619_Static_TuitionFunding_AndMathsTopUp";
                    totalAmount = 1.23;
                    orgName = "University Technical College Norfolk";
                    laName = "Norfolk";
                    esfaReferenceUpin = "114847";
                    ukprn = "10047244";
                    regionName = "Midlands and East of England";
                    providerSubType = "15UTC";
                    urn = "141086";
                    schemaVersion = "1.2";

                    break;

                case "1619-AS-2122-10064744-2_0":
                    dateOpened = new DateTime(2000, 1, 1);
                    filename = "1619_Static_TuitionFunding_NoMathsTopUp";
                    totalAmount = 1.23;
                    orgName = "Notton House Academy";
                    laName = "Bristol City of";
                    esfaReferenceUpin = "114847";
                    ukprn = "10064744";
                    regionName = "Midlands and East of England";
                    providerSubType = "17NMF";
                    urn = "144286";
                    schemaVersion = "1.2";

                    break;

                case "1619-AS-2122-10088096-2_0":
                    dateOpened = new DateTime(2021, 11, 1);
                    filename = "1619_Static_AcademyConverterInYearOpener";
                    totalAmount = 1.23;
                    orgName = "St Mary's Catholic School";
                    laName = "Hertfordshire";
                    esfaReferenceUpin = "114847";
                    ukprn = "10064744";
                    regionName = "Midlands and East of England";
                    providerSubType = "11ACA    ";
                    urn = "148499";
                    schemaVersion = "1.2";
                    openReason = "Academy Converter";

                    break;

                case "1619-AS-2324-10088096-2_0":
                    dateOpened = new DateTime(2023, 11, 1);
                    filename = "1619_Static_AcademyConverterInYearOpener";
                    totalAmount = 1.23;
                    orgName = "St Mary's Catholic School";
                    laName = "Hertfordshire";
                    esfaReferenceUpin = "114847";
                    ukprn = "10064744";
                    regionName = "Midlands and East of England";
                    providerSubType = "11ACA    ";
                    urn = "148499";
                    schemaVersion = "1.2";
                    openReason = "Academy Converter";

                    break;

                case "1619-AS-2223-10088096-2_0":
                    dateOpened = new DateTime(2021, 11, 1);
                    filename = "1619_Static_FE_With";
                    totalAmount = 1.23;
                    orgName = "St Mary's Catholic School";
                    laName = "Hertfordshire";
                    esfaReferenceUpin = "114847";
                    ukprn = "10064744";
                    regionName = "Midlands and East of England";
                    providerSubType = "11ACA    ";
                    urn = "148499";
                    schemaVersion = "1.0";
                    openReason = "Academy Converter";

                    break;

                case "1619-AS-2223-10088096-3_0":
                    dateOpened = new DateTime(2021, 11, 1);
                    filename = "1619_Static_FE_Without";
                    totalAmount = 1.23;
                    orgName = "St Mary's Catholic School";
                    laName = "Hertfordshire";
                    esfaReferenceUpin = "114847";
                    ukprn = "10064744";
                    regionName = "Midlands and East of England";
                    providerSubType = "11ACA    ";
                    urn = "148499";
                    schemaVersion = "1.0";
                    openReason = "Academy Converter";

                    break;


                case "1619-AS-2122-10088092-2_0":
                    dateOpened = new DateTime(2021, 11, 1);
                    filename = "1619_Static_WithHighValueCoursesLeavers";
                    totalAmount = 1.23;
                    orgName = "St Mary's Catholic School";
                    laName = "Hertfordshire";
                    esfaReferenceUpin = "114847";
                    ukprn = "10064744";
                    regionName = "Midlands and East of England";
                    providerSubType = "11ACA    ";
                    urn = "148499";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2324-99999001-1_0":
                    filename = "1619_Static_Adjustments_2324_SummaryTableBugFixTesting";
                    schemaVersion = "1.2";
                    openReason = "Academy Converter";
                    dateOpened = new DateTime(2018, 11, 1);
                    break;

                case "1619-AS-2324-99999002-1_0":
                    filename = "1619_Static_Adjustments_2324_SummaryTableBugFixTesting";
                    schemaVersion = "1.2";
                    openReason = "Academy Converter";
                    dateOpened = new DateTime(2023, 11, 1);
                    break;

                case "1619-AS-2324-10088092-2_0":
                    filename = "1619_Static_Adjustments_2324_ShowNone_0";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2324-10088092-3_0":
                    filename = "1619_Static_Adjustments_2324_ShowAll";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2324-10088092-4_0":
                    filename = "1619_Static_Adjustments_2324_ShowNone_null";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2324-10088092-5_0":
                    filename = "1619_Static_Adjustments_2324_IndustryPlacements_NonSpecialAcca_HasValues";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2324-10088092-6_0":
                    filename = "1619_Static_Adjustments_2324_IndustryPlacements_NonSpecialAcca_HasZero";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2324-10088092-7_0":
                    filename = "1619_Static_Adjustments_2324_IndustryPlacements_SpecialAcca_HasValues";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2324-10088092-8_0":
                    filename = "1619_Static_Adjustments_2324_IndustryPlacements_SpecialAcca_HasZero";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2324-10088092-9_0":
                    filename = "1619_Static_Adjustments_2324_BringTableUp_MathsTopUpAndTuitionFunding_Pos";
                    schemaVersion = "1.2";
                    providerType = "Acade";
                    providerSubType = "Free schools 16 to 19";
                    break;

                case "1619-AS-2324-10088092-10_0":
                    filename = "1619_Static_Adjustments_2324_BringTableUp_MathsTopUpAndTuitionFunding_Neg";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2324-10088092-11_0":
                    filename = "1619_Static_Adjustments_2324_BringTableUp_MathsTopUpAndTuitionFunding_Pos";
                    schemaVersion = "1.2";
                    providerType = "Acade";
                    break;

                case "1619-AS-2425-10088092-11_0":
                    filename = "1619_Static_Adjustments_2425_TBandVCShow";
                    schemaVersion = "1.2";
                    providerType = "Acade";
                    break;

                case "1619-AS-2425-10088092-12_0":
                    filename = "1619_Static_Adjustments_2425_TBandVCHide";
                    schemaVersion = "1.2";
                    providerType = "Acade";
                    break;

                case "1619-AS-2425-10088092-1_0":
                    filename = "1619_Static_Adjustments_2425_IndPlacementFundingTblHide";
                    schemaVersion = "1.2";
                    providerType = "Acade";
                    break;

                case "1619-AS-2425-10088092-2_0":
                    filename = "1619_Static_Adjustments_2425_IndPlacementFundingTblShow";
                    schemaVersion = "1.2";
                    providerType = "Acade";
                    break;

                case "1619-AS-2425-99999001-1_0":
                    filename = "1619_Static_Indicative_2425_WithTLevelIndPlacements";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2425-99999002-1_0":
                    filename = "1619_Static_Adjustments_2425_NS_SET2AllShow";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2425-99999003-1_0":
                    dateOpened = new DateTime(2024, 11, 1);
                    filename = "1619_Static_Adjustments_2425_NS_SET2AllShow";
                    totalAmount = 1.23;
                    orgName = "St Mary's Catholic School";
                    laName = "Hertfordshire";
                    esfaReferenceUpin = "114847";
                    ukprn = "99999003";
                    regionName = "Midlands and East of England";
                    providerSubType = "11ACA    ";
                    urn = "148499";
                    schemaVersion = "1.2";
                    openReason = "Academy Converter";
                    break;

                case "1619-AS-2425-99999004-1_0":
                    filename = "1619_Static_Adjustments_2425_S_SET2AllShow";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2425-99999005-1_0":
                    dateOpened = new DateTime(2024, 11, 1);
                    filename = "1619_Static_Adjustments_2425_S_SET2AllShow";
                    totalAmount = 1.23;
                    orgName = "St Mary's Catholic School";
                    laName = "Hertfordshire";
                    esfaReferenceUpin = "114847";
                    ukprn = "99999003";
                    regionName = "Midlands and East of England";
                    providerSubType = "11ACA    ";
                    urn = "148499";
                    schemaVersion = "1.2";
                    openReason = "Academy Converter";
                    break;
                case "1619-AS-2425-99999006-1_0":
                    filename = "1619_Static_Adjustments_2324_SummaryTableBugFixTesting";
                    schemaVersion = "1.2";
                    openReason = "Academy Converter";
                    dateOpened = new DateTime(2018, 11, 1);
                    break;

                case "1619-AS-2425-99999007-1_0":
                    filename = "1619_Static_Adjustments_2324_SummaryTableBugFixTesting";
                    schemaVersion = "1.2";
                    openReason = "Academy Converter";
                    dateOpened = new DateTime(2023, 11, 1);
                    break;

                case "1619-AS-2425-99999008-1_0":
                    filename = "1619_Static_2425_Residential_Support_Scheme";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2526-10007063-1_0":
                    filename = "1619_Static_Adjustments_2526_ShowAll";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2526-10007063-2_0":
                    filename = "1619_Static_Adjustments_2526_HideAll";
                    schemaVersion = "1.2";
                    break;

                case "1619-AS-2627-10007063-1_0":
                    filename = "1619_Static_PolicyChanges_2627_ShowAll";
                    schemaVersion = "1.2";
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
                FundingPeriodCode = "AS-2122",
                FundingStreamCode = "1619",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = schemaVersion,
                TemplateVersion = "1.0",
                FundingValue = JsonConvert.SerializeObject(fundingValue),
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = ukprn,
                ProviderUpin = esfaReferenceUpin,
                Id = $"0-1619-{ukprn}",
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
                            "10000552",
                            "10034690",
                            "10030654",
                            "10001929",
                            "10004756",
                            "10000866",
                            "10006247"
                        },
                        request)
                };
            }

            var providerFundingAllocations = new Dictionary<int, Dictionary<string, string>>();

            var dateOpened = new DateTime(2000, 1, 1);
            var totalAmount = 1.23;
            var orgName = "Truro and Penwith College";
            var laName = "Cornwall";
            var esfaReferenceUpin = "108441";
            var regionName = "South and South-West";
            var providerType = "Furth";
            var providerSubType = "11ACA";
            var urn = "140294";
            var providerStatus = ProviderStatus.Open;

            switch (ukprn)
            {
                case "10000552":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Barton Peveril College";
                    laName = "Hampshire";
                    esfaReferenceUpin = "108437";
                    regionName = "South and South-West";
                    providerType = "Furth";
                    urn = "140294";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_FE_Without");

                    break;
                case "10000055":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Abingdon and Witney College";
                    laName = "Oxfordshire";
                    esfaReferenceUpin = "108437";
                    regionName = "South east";
                    providerType = "Furth";
                    urn = "130793";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_FE_WithSeaFishing");

                    break;
                case "10034690":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "The Stourport High School and Sixth Form College";
                    laName = "Worcestershire";
                    esfaReferenceUpin = "120162";
                    regionName = "Midlands and East of England";
                    providerType = "Acade";
                    urn = "230294";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_Academies_With");

                    break;
                case "10015175":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Ark Burlington Danes Academy";
                    laName = "Hammersmith and Fulham ";
                    esfaReferenceUpin = "120162";
                    regionName = "London";
                    providerType = "Acade";
                    urn = "131752";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_Academies_WithSportingExcellence");

                    break;
                case "10030654":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Aylward Academy";
                    laName = "Enfield";
                    esfaReferenceUpin = "119211";
                    regionName = "London and South-East";
                    providerType = "Acade";
                    urn = "563432";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_Academies_Without");

                    break;
                case "10001929":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Derwen College";
                    laName = "Shropshire";
                    esfaReferenceUpin = "114847";
                    regionName = "Midlands and East of England";
                    providerSubType = "18ISP";
                    urn = "87345";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_SpecialPost16");

                    break;
                case "10004756":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Abbey Hill Academy";
                    laName = "Stockton-on-Tees";
                    esfaReferenceUpin = "114847";
                    ukprn = "10004756";
                    regionName = "Midlands and East of England";
                    providerType = "Acade";
                    providerSubType = "17NMF";
                    urn = "635435";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_SpecialAcademy");

                    break;
                case "10000866":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Brentside High School";
                    laName = "Ealing";
                    esfaReferenceUpin = "113774";
                    regionName = "London and South-East";
                    providerType = "Schoo";
                    urn = "134987";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_SixthForm_Without");

                    break;
                case "10006247":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "St Paul's Catholic College";
                    laName = "Surrey";
                    esfaReferenceUpin = "113066";
                    regionName = "London and South-East";
                    providerType = "Schoo";
                    urn = "083333";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_SixthForm_With");

                    break;
                case "10040630":
                    orgName = "Haringey Sixth Form College";
                    laName = "Haringey";
                    providerType = "Acade";
                    urn = "864266";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_SixthForm_WithFreeMealsLine");

                    break;
                case "10040776":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Abbey Hey Primary Academy";
                    laName = "Manchester";
                    esfaReferenceUpin = "114847";
                    regionName = "Midlands and East of England";
                    providerType = "Acade";
                    providerSubType = "FS1619";
                    urn = "635435";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_NotSchool_NotAcademy_NotSpecialAcademy");

                    break;
                case "10030456":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Sidney Stringer Academy";
                    laName = "Coventry";
                    esfaReferenceUpin = "108437";
                    regionName = "West Middlands";
                    providerType = "Acade";
                    urn = "136126";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_Adjusted_Above_Upper_Limit");

                    break;
                case "10054167":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Barr's Hill School and Community College";
                    laName = "Coventry";
                    esfaReferenceUpin = "108437";
                    regionName = "West Middlands";
                    providerType = "Acade";
                    urn = "142339";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_Adjusted_Below_Lower_Limit");

                    break;
                case "10033567":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Blue Coat Church of England School and Music College";
                    laName = "Coventry";
                    esfaReferenceUpin = "108437";
                    regionName = "West Middlands";
                    providerType = "Acade";
                    urn = "137272";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_Adjusted_Below_500");

                    break;
                case "10061616":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Hearsall Community Academy";
                    laName = "Coventry";
                    esfaReferenceUpin = "108437";
                    regionName = "West Middlands";
                    providerType = "Acade";
                    urn = "143535";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_Adjusted_In_Limit");
                    break;

                case "10053725":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Clifford Bridge Academy";
                    laName = "Coventry";
                    esfaReferenceUpin = "108437";
                    regionName = "West Middlands";
                    providerType = "Acade";
                    urn = "143535";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_Advanced_Maths_Premium_Is_0");
                    break;

                case "10037002":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Hearsall Community Academy";
                    laName = "Coventry";
                    esfaReferenceUpin = "108437";
                    regionName = "West Middlands";
                    providerType = "Acade";
                    urn = "143535";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_Advanced_Maths_Eligible_Students_Below_Baseline");
                    break;

                case "10047244":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "University Technical College Norfolk";
                    laName = "Norfolk";
                    esfaReferenceUpin = "9264014";
                    regionName = "Norwich";
                    providerType = "Acade";
                    urn = "141086";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_TuitionFunding_AndMathsTopUp");
                    break;

                case "10060613":
                    dateOpened = new DateTime(2000, 1, 1);
                    totalAmount = 1.23;
                    orgName = "Chamberlayne Academy";
                    laName = "Norfolk";
                    esfaReferenceUpin = "9264111";
                    regionName = "Norwich";
                    providerType = "Acade";
                    urn = "141086";
                    groupingReason = GroupingReason.Indicative;
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_FE_With", variationReason: "Indicative allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_FE_With-PREVIOUS", "2020-02-02", "Revised indicative allocation.");
                    break;

                default:
                    dateOpened = new DateTime(2000, 1, 1);
                    orgName = "Truro and Penwith College";
                    laName = "Cornwall";
                    esfaReferenceUpin = "108441";
                    regionName = "South and South-West";
                    providerType = "Furth";
                    providerSubType = "11ACA";
                    urn = "140294";
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_FE_With", "2021-07-01", "Revised allocation.", "AS-2122");
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_FE_With-PREVIOUS", "2021-01-01", "Initial allocation.", "AS-2122");
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_FE_With", "2020-06-01", "Revised allocation.", "AS-2021");
                    BuildProviderFundingAllocations(providerFundingAllocations, "1619_Static_FE_With-PREVIOUS", "2020-02-01", "Initial allocation.", "AS-2021");
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
                var totalValue = fundingValue.TotalValue ?? totalAmount;
                providerSubType ??= providerType;

                providerFunding.Add(
                    new FundingApiSearchProviderFunding
                    {
                        FundingPeriodCode = fundingPeriodCode,
                        FundingStreamCode = "1619",
                        FundingVersion = "1_0",
                        ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                        StatementChannelVersion = 1,
                        IsFirstStatementChannelVersion = true,
                        SchemaVersion = "1.0",
                        TemplateVersion = "1.0",
                        FundingValue = JsonConvert.SerializeObject(fundingValue),
                        TotalAmount = totalValue,
                        OrganisationName = orgName,
                        OrganisationUkprn = ukprn,
                        ProviderUpin = esfaReferenceUpin,
                        OrganisationDfeNumber = "1234",
                        Id = $"{loopCount++}-1619-{ukprn}",
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
                        ProviderStatus = variationReason.Contains("indicative", StringComparison.InvariantCultureIgnoreCase) ? ProviderStatus.IndicativeStatuses.First() : providerStatus
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
                { "fundingPeriodCode", fundingPeriodCode ?? "AS-2122" }
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

                var parentGroupingTypes = new string[4]
                {
                    GroupingType.AcademyTrust,
                    GroupingType.LocalAuthorityMaintained,
                    GroupingType.LocalAuthority,
                    GroupingType.LocalAuthoritySsf
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

                    returnList.Add(providerFunding);
                }
            }

            return returnList;
        }
    }
}