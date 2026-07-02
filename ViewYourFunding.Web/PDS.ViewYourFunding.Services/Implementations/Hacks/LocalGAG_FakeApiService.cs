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
    /// A short term use implementation of funding service to get mock GAG data.
    /// </summary>
    public class LocalGAG_FakeApiService : BaseFakeApiService, IFundingApiService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalGAG_FakeApiService"/> class.
        /// Create a new instance of a LocalGAG_FakeApiService.
        /// </summary>
        /// <param name="httpApiService">The HTTP service to use.</param>
        public LocalGAG_FakeApiService(IHttpApiService httpApiService)
            : base(httpApiService)
        {
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="requestObj">An FundingApiSearchRequestObject object.</param>
        /// <returns>Nothing - throws an error.</returns>
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
        /// <returns>A matched provider funding.</returns>
        public async Task<IFundingApiSearchResponseProviderFunding> SearchProviderFunding(FundingApiSearchRequestObject request, bool getLatest = false)
        {
            if (string.IsNullOrEmpty(request.SearchTerm)
                && request.FundingStreams.Any(fs =>
                    fs?.Filters?.Any(fil =>
                        fil.PropertyValue.Contains("10035320") ||
                        fil.PropertyValue.Contains("10063493") ||
                        fil.PropertyValue.Contains("10041006")) == true))
            {
                return new ProviderFundingApiSearchResponse
                {
                    ProviderFunding = await GetMatProviderFundings()
                };
            }

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

            if (request.FundingStreams.Any(fs => fs?.Filters?.Any(fil => fil.PropertyValue == "10081072") == true) == true)
            {
                parentType = GroupingType.AcademyTrust;
            }

            switch (searchTerm)
            {
                case "10087061":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary_SpecialFreeSchool", null, "Indicative allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary_SpecialFreeSchool_Allocation_1", "2021-07-06", "Revised indicative allocation.");
                    orgName = "Alanbrooke School";
                    dateOpened = new DateTime(2019, 11, 01);
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;
                    providerStatus = ProviderStatus.Open;
                    groupingReason = GroupingReason.Information;

                    break;
                case "10086776":
                    phaseOfEducation = "Secondary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Secondary", variationReason: "Indicative allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Secondary_Allocation_1", "2021-07-06", "Revised indicative allocation.");
                    orgName = "Chamberlayne Academy";
                    dateOpened = new DateTime(2019, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademySponsored;
                    providerStatus = ProviderStatus.IndicativeStatuses.First();
                    groupingReason = GroupingReason.Indicative;

                    break;
                case "10039896":
                    phaseOfEducation = "Secondary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Secondary", "2021-09-01", "Revised allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Secondary_Allocation_1", "2021-07-06", "Initial allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Secondary_Allocation_2", "2021-01-06", "Revised indicative allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Secondary_Allocation_3", "2020-07-06", "Indicative allocation.");
                    orgName = "Abbeywood Community School";
                    dateOpened = new DateTime(2019, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademySponsored;
                    providerStatus = ProviderStatus.Open;
                    groupingReason = GroupingReason.Information;

                    break;

                case "10034857":
                    phaseOfEducation = "Secondary";
                    orgName = "St Aidan's Church of England High School";
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.AcademySpecialSponsoreLed;
                    dateOpened = new DateTime(2021, 11, 01);
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Secondary", "2021-06-01", "Initial allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Secondary_Allocation_1", "2021-06-06", "Revised allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Secondary_Allocation_2", "2021-06-12", "Revised allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Secondary", "2020-06-01", "Initial allocation.", "AC-2021");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Secondary_Allocation_1", "2020-06-06", "Revised allocation.", "AC-2021");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Secondary_Allocation_2", "2020-06-12", "Revised allocation.", "AC-2021");

                    break;
                case "10063152":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary_SpecialFreeSchool", null, "Initial allocation.");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary_SpecialFreeSchool_Allocation_1", "2021-06-06", "Revised allocation.");
                    orgName = "Drake Primary Academy";
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;

                    break;
                case "10038354":
                    phaseOfEducation = "Secondary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Secondary");
                    orgName = "Bowland High";
                    laName = "Lancashire";
                    laeStab = "8884041";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademyConverter;

                    break;
                case "10034949":
                    phaseOfEducation = "All-through";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Allthrough");
                    orgName = "Simon Balle All-Through School";
                    laName = "Hertfordshire";
                    laeStab = "9194067";
                    dateOpened = new DateTime(2013, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademyConverter;
                    upin = "121196";
                    urn = "140294";

                    break;
                case "10043073":
                    // Previously Hollickwood Primary School - 10069594
                    phaseOfEducation = "All-through";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Allthrough_WithSparsity");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Allthrough_WithSparsity", statusChangedDate: "2022-06-01", fundingPeriodCode: "AC-2223");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Allthrough_WithSparsity", statusChangedDate: "2023-06-01", fundingPeriodCode: "AC-2324");
                    orgName = "Hollingworth Academy";
                    laName = "Rochdale";
                    laeStab = "3545401";
                    dateOpened = new DateTime(2013, 11, 01);
                    upin = "121196";
                    urn = "140091";

                    break;
                case "10068124":
                    phaseOfEducation = "All-through";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Allthrough_ExceptionalAdjustments");
                    orgName = "Kents Hill Park all-through school";
                    laName = "Milton Keynes";
                    laeStab = "9194067";
                    dateOpened = new DateTime(2013, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademyConverter;

                    break;
                case "10086304":
                    // Previously St Monica's RC Primary School 10071644
                    phaseOfEducation = "All-through";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Allthrough_PostApril");
                    orgName = "St Monica's RC High School, a Voluntary Academy";
                    laName = "Bury";
                    laeStab = "3514006";
                    dateOpened = new DateTime(2021, 06, 01);
                    providerType = ProviderTypeExternal.Academy;
                    upin = "121196";
                    urn = "148050";

                    break;
                case "10039385":
                    // Previously Lickey End First School 10080153
                    phaseOfEducation = "All-through";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Allthrough_PostApril_WithSparsity");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Allthrough_PostApril_WithSparsity", statusChangedDate: "2022-06-01", fundingPeriodCode: "AC-2223");
                    orgName = "Rickley Park Primary School";
                    laName = "Milton Keynes";
                    laeStab = "8265208";
                    dateOpened = new DateTime(2021, 06, 01);
                    providerType = ProviderTypeExternal.Academy;
                    upin = "121196";
                    urn = "138933";

                    break;
                case "10045706":
                    // Previously St Gabriel's Roman Catholic Primary School, Rochdale 10070726
                    phaseOfEducation = "All-through";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Allthrough_PostSept");
                    orgName = "Saint Gabriel's Catholic Voluntary Primary Academy";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;
                    laName = "Redcar and Cleveland";
                    laeStab = "8073387";
                    dateOpened = new DateTime(2020, 09, 01);
                    upin = "121196";
                    urn = "140750";

                    break;
                case "10034574":
                    // Previously Lickey Hills Primary School and Nursery 10077246
                    phaseOfEducation = "All-through";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Allthrough_PostSept_WithSparsity");
                    orgName = "Pickhurst Academy";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;
                    laName = "Bromley";
                    laeStab = "3052018";
                    dateOpened = new DateTime(2020, 09, 01);
                    upin = "121196";
                    urn = "137070";

                    break;
                case "10039388":
                    // Previously Calder Vale St John Church of England Primary School 10072060
                    phaseOfEducation = "All-through";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Allthrough_MFGTotalOverZero");
                    orgName = "Abington Vale Primary School";
                    laName = "Northamptonshire";
                    laeStab = "9282177";
                    dateOpened = new DateTime(2013, 11, 01);
                    upin = "121196";
                    urn = "138952";

                    break;
                case "10021055":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear");
                    orgName = "Bradford Academy";
                    laName = "West Yorkshire";
                    laeStab = "3806906";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.FreeSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "10044483":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_NegativeAvgPerPupilRate");
                    orgName = "Dodworth St John";
                    laName = "Barnsley";
                    laeStab = "3806906";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.FreeSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "10078119":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear");
                    orgName = "Brinscall Primary School";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;

                    break;
                case "10067061":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Free_School");
                    orgName = "Cam Woodfield Junior School";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "10067283":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Primary");
                    orgName = "All Saints CofE Academy Denstone";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademyConverter;

                    break;
                case "10081419":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Primary");
                    orgName = "Springfield Primary Academy";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademySponsored;

                    break;
                case "10047466":
                    // Previously Wombridge Primary School 10072950
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Primary_ZeroStartUpFunding");
                    orgName = "Wombourne High School";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademySponsored;

                    break;
                case "10061450":
                    phaseOfEducation = "Secondary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Secondary");
                    orgName = "Great Barr Academy";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademySponsored;

                    break;
                case "10021072":
                    phaseOfEducation = "Secondary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Secondary_ZeroStartUpFunding");
                    orgName = "Belvedere Academy";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademySponsored;

                    break;
                case "10047220":
                    phaseOfEducation = "All-through";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Allthrough");
                    orgName = "Harris Academy Tottenham";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademySponsored;

                    break;
                case "10084320":
                    // Previously Shrewsbury High School 10008489
                    phaseOfEducation = "All-through";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_InYear_Allthrough_ZeroStartUpFunding");
                    orgName = "Shrewsbury Academy";
                    dateOpened = new DateTime(2021, 11, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademySponsored;

                    break;
                case "10038487":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary_YesYearGroupsWithoutPupils");
                    orgName = "Goldsmith Primary";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "10085813":
                    // Previously Bollington St John's CofE Primary School 10070464
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary_NoYearGroupsWithoutPupils");
                    orgName = "Little Bollington CofE Primary School";
                    laName = "Cheshire East";
                    laeStab = "8953108";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "10082238":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary_NoYearGroupsWithoutPupilsLowerCap");
                    orgName = "Bollington St John";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "10063751":
                    // Previously Fitzjohn's Primary School 10079731
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary");
                    orgName = "Fitzwilliam Primary School";
                    laName = "Wakefield";
                    laeStab = "3842197";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;

                    break;
                case "10084554":
                    // Previously Ladysmith Infant & Nursery School 10069684
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary_TotalPostOpeningGrantAboveZero");
                    orgName = "Co-op Academy Smithies Moor";
                    laName = "Kirklees";
                    laeStab = "3822059";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;

                    break;
                case "10074599":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary_TotalPostOpeningGrantAboveZero");
                    orgName = "Oliver Goldsmith";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;

                    break;
                case "10045843":
                    // Previously Rivermead Primary School 10068956
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Special");
                    orgName = "Fowey River Academy";
                    laName = "Cornwall";
                    laeStab = "9084001";
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;
                    upin = "121196";
                    urn = "140836";

                    break;
                case "10042434":
                    // Previously Cranbury College 10016947
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_IYO_Special_PostApril");
                    orgName = "Cranberry Academy";
                    laName = "Cheshire East";
                    laeStab = "8952000";
                    dateOpened = new DateTime(2021, 06, 01);
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;
                    upin = "121196";
                    urn = "139910";

                    break;
                case "10048870":
                    // Previously St Joseph's Roman Catholic Voluntary Aided Primary School, Sunderland 10074425
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_IYO_Special_PostSept");
                    orgName = "Our Lady & St. Joseph Catholic Academy";
                    laName = "Warwickshire";
                    laeStab = "9373584";
                    dateOpened = new DateTime(2020, 10, 01);
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;
                    upin = "121196";
                    urn = "141823";

                    break;
                case "10035674":
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_IYO_Special_PostSept");
                    orgName = "Lickhill Primary School";
                    laName = "Worcestershire";
                    laeStab = "8852904";
                    dateOpened = new DateTime(2020, 10, 01);
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.AcademySpecialConverter;
                    upin = "121196";
                    urn = "140294";

                    break;
                case "10072811":
                default:
                    phaseOfEducation = "Primary";
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary");
                    BuildProviderFundingAllocations(providerFundingAllocations, "GAG_Static_Existing_Primary", statusChangedDate: "2023-06-01", fundingPeriodCode: "AC-2324");
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
                        FundingStreamCode = "GAG",
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
                        Id = $"{loopCount++}-GAG-Id30",
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

        /// <summary>
        /// An HTTP api service to fetch funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A funding collection.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IFundingApiSearchFunding> GetFunding(string id)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException("GetFunding not needed for GAG");
        }

        /// <summary>
        /// An HTTP api service to fetch provider funding.
        /// </summary>
        /// <param name="id">The id to search for.</param>
        /// <returns>A provider funding.</returns>
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
                case "GAG-FY-2021-10034949-1_0":
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_Existing_Allthrough";
                    orgName = "Simon Balle All-Through School";
                    laName = "Hertfordshire";
                    laeStab = "9194067";
                    dateOpened = new DateTime(2013, 11, 01);
                    ukprn = "10034949";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademyConverter;
                    upin = "121196";
                    urn = "140294";

                    break;
                case "GAG-FY-2021-10043073-1_0":
                    // Previously Hollickwood Primary School 10069594
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_Existing_Allthrough_WithSparsity";
                    orgName = "Hollingworth Academy";
                    laName = "Rochdale";
                    laeStab = "3545401";
                    dateOpened = new DateTime(2013, 11, 01);
                    ukprn = "10043073";
                    upin = "121196";
                    urn = "140091";

                    break;
                case "GAG-FY-2021-10086304-2_0":
                    // Previously St Monica's RC Primary School 10071644
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_InYear_Allthrough_PostApril";
                    orgName = "St Monica's RC High School, a Voluntary Academy";
                    laName = "Bury";
                    laeStab = "3514006";
                    dateOpened = new DateTime(2021, 04, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;
                    ukprn = "10086304";
                    upin = "121196";
                    urn = "148050";

                    break;
                case "GAG-FY-2021-10039385-1_0":
                    // Previously Lickey End First School 10080153
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_Existing_Allthrough_WithSparsity";
                    orgName = "Rickley Park Primary School";
                    laName = "Milton Keynes";
                    laeStab = "8265208";
                    fundingPeriodCode = "AC-2223";
                    dateOpened = new DateTime(2011, 04, 01);
                    providerType = ProviderTypeExternal.Academy;
                    ukprn = "10039385";
                    upin = "121196";
                    urn = "138933";

                    break;

                case "GAG-FY-2021-10040626-1_0":
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_InYear_Allthrough_PostApril_WithSparsity";
                    orgName = "Asfordby Hill Primary School";
                    laName = "Leicestershire";
                    laeStab = "8552004";
                    dateOpened = new DateTime(2021, 06, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademyConverter;
                    ukprn = "10040626";
                    upin = "121196";
                    urn = "138933";


                    break;

                case "GAG-FY-2021-10040622-1_0":
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_Existing_Allthrough_WithSparsityAdjusted";
                    orgName = "Asfordby Hill Primary School";
                    laName = "Leicestershire";
                    laeStab = "8552004";
                    fundingPeriodCode = "AC-2223";
                    dateOpened = new DateTime(2011, 04, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademyConverter;
                    ukprn = "10040626";
                    upin = "121196";
                    urn = "138933";

                    break;

                case "GAG-FY-2526-10040622-1_0":
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_Calculation_Baseline_MFG_ScalingCapping";
                    orgName = "Asfordby Hill Primary School";
                    laName = "Leicestershire";
                    laeStab = "8552004";
                    fundingPeriodCode = "AC-2526";
                    dateOpened = new DateTime(2024, 04, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademyConverter;
                    ukprn = "10040626";
                    upin = "121196";
                    urn = "138933";

                    break;

                case "GAG-FY-2021-10040623-2_0":
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_IYO_Special_PostApril";
                    orgName = "Asfordby Hill Primary School";
                    laName = "Leicestershire";
                    laeStab = "8552004";
                    fundingPeriodCode = "AC-2223";
                    dateOpened = new DateTime(2022, 04, 06);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademyConverter;
                    ukprn = "10040623";
                    upin = "121196";
                    urn = "138933";

                    break;


                case "GAG-FY-2324-10040623-2_0":
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_IYO_Special_PostApril";
                    orgName = "Asfordby Hill Primary School";
                    laName = "Leicestershire";
                    laeStab = "8552004";
                    fundingPeriodCode = "AC-2324";
                    dateOpened = new DateTime(2023, 04, 06);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.AcademyConverter;
                    ukprn = "10040623";
                    upin = "121196";
                    urn = "138933";

                    break;

                case "GAG-FY-2021-10056888-1_0":
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_InYear_Allthrough_PostApril_WithSparsity";
                    orgName = "Badger Hill Primary Academy";
                    laName = "York";
                    laeStab = "8162431";
                    dateOpened = new DateTime(2021, 04, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;
                    ukprn = "10056888";
                    upin = "121196";
                    urn = "138933";

                    break;
                case "GAG-FY-2021-10045706-1_0":
                    // Previously St Gabriel's Roman Catholic Primary School, Rochdale 10070726
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_InYear_Allthrough_PostSept";
                    orgName = "Saint Gabriel's Catholic Voluntary Primary Academy";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;
                    laName = "Redcar and Cleveland";
                    laeStab = "8073387";
                    dateOpened = new DateTime(2020, 09, 01);
                    ukprn = "10045706";
                    upin = "121196";
                    urn = "140750";

                    break;
                case "GAG-FY-2021-10034574-2_0":
                    // Previously Lickey Hills Primary School and Nursery 10077246
                    phaseOfEducation = "All-through";
                    filename = "GAG_Static_InYear_Allthrough_PostSept_WithSparsity";
                    orgName = "Pickhurst Academy";
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;
                    laName = "Bromley";
                    laeStab = "3052018";
                    dateOpened = new DateTime(2020, 10, 01);
                    ukprn = "10034574";
                    upin = "121196";
                    urn = "137070";

                    break;
                case "GAG-FY-2021-10045843-1_0":
                    // Previously Rivermead Primary School 10068956
                    phaseOfEducation = "Primary";
                    filename = "GAG_Static_Existing_Special";
                    orgName = "Fowey River Academy";
                    laName = "Cornwall";
                    laeStab = "9084001";
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;
                    ukprn = "10045843";
                    upin = "121196";
                    urn = "140836";

                    break;
                case "GAG-FY-2021-10042434-2_0":
                    // Prevously Cranbury College 10016947
                    phaseOfEducation = "Primary";
                    filename = "GAG_Static_IYO_Special_PostApril";
                    orgName = "Cranberry Academy";
                    laName = "Cheshire East";
                    laeStab = "8952000";
                    dateOpened = new DateTime(2021, 06, 01);
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;
                    ukprn = "10042434";
                    upin = "121196";
                    urn = "139910";

                    break;
                case "GAG-FY-2021-10048870-1_0":
                    // Previously St Joseph's Roman Catholic Voluntary Aided Primary School, Sunderland 10074425
                    phaseOfEducation = "Primary";
                    filename = "GAG_Static_IYO_Special_PostSept";
                    orgName = "Our Lady & St. Joseph Catholic Academy";
                    laName = "Warwickshire";
                    laeStab = "9373584";
                    dateOpened = new DateTime(2020, 10, 01);
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;
                    ukprn = "10048870";
                    upin = "121196";
                    urn = "141823";

                    break;

                case "GAG-FY-2021-10086304-1_0":
                    phaseOfEducation = "Secondary";
                    filename = "GAG_Static_IYO_Special_PostSept";
                    orgName = "St Monica's RC High School, a Voluntary Academy";
                    laName = "Bury";
                    laeStab = "9373520";
                    dateOpened = new DateTime(2020, 09, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;
                    ukprn = "10086304";
                    upin = "121196";
                    urn = "148050";

                    break;

                case "GAG-FY-2021-10035674-2_0":
                    phaseOfEducation = "Primary";
                    filename = "GAG_Static_IYO_Special_PostSept";
                    orgName = "Lickhill Primary School";
                    laName = "Worcestershire";
                    laeStab = "8852904";
                    dateOpened = new DateTime(2020, 10, 01);
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.AcademySpecialConverter;
                    ukprn = "10035674";
                    upin = "121196";
                    urn = "140294";

                    break;
                case "GAG-FY-2021-10042437-2_0":
                    // Prevously Cranbury College 10016947
                    phaseOfEducation = "Primary";
                    filename = "GAG_Static_IYO_Special_PostApril";
                    orgName = "Cranberry Academy";
                    laName = "Cheshire East";
                    laeStab = "8952000";
                    dateOpened = new DateTime(2021, 04, 06);
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;
                    ukprn = "10042437";
                    upin = "121196";
                    urn = "139910";

                    break;
                case "GAG-FY-2021-10042438-1_0":
                    // Prevously Cranbury College 10016947
                    phaseOfEducation = "Primary";
                    filename = "GAG_Static_IYO_Special_PostApril";
                    orgName = "Cranberry Academy";
                    laName = "Cheshire East";
                    laeStab = "8952000";
                    dateOpened = new DateTime(2022, 06, 01);
                    providerType = ProviderTypeExternal.SpecialSchool;
                    providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;
                    ukprn = "10042438";
                    upin = "121196";
                    urn = "139910";

                    break;

                case "GAG-AC-2223-10086304-1_0":
                    phaseOfEducation = "Secondary";
                    filename = "GAG_Static_IYO_Special_PostSept";
                    orgName = "St Monica's RC High School, a Voluntary Academy";
                    laName = "Bury";
                    laeStab = "9373520";
                    dateOpened = new DateTime(2021, 09, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;
                    ukprn = "10086304";
                    upin = "121196";
                    urn = "148050";

                    break;

                case "GAG-AC-2324-10086304-1_0":
                    phaseOfEducation = "Primary";
                    filename = "GAG_Static_IYO_Special_PostSept";
                    orgName = "St Monica's RC High School, a Voluntary Academy";
                    laName = "Bury";
                    laeStab = "9373520";
                    dateOpened = new DateTime(2021, 09, 01);
                    providerType = ProviderTypeExternal.Academy;
                    providerSubType = ProviderSubTypeExternal.FreeSchool;
                    ukprn = "10086304";
                    upin = "121196";
                    urn = "148050";

                    break;

                case "GAG-FY-2021-10072811-1_0":
                default:
                    phaseOfEducation = "Primary";
                    filename = "GAG_Static_Existing_Primary";
                    orgName = "Abbey View Primary Academy";
                    laName = "Buckinghamshire";
                    laeStab = "8252042";
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
                FundingStreamCode = "GAG",
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
                Id = $"{totalAmount}-GAG-Id1",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                LocalAuthorityName = laName,
                OrganisationDfeNumber = laeStab,
                ProviderUpin = upin,
                ProviderUrn = urn
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

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();
            var fetchTasks = new List<Task<List<FundingApiSearchProviderFunding>>>
            {
                GetMatProviderFundings_1(),
                GetMatProviderFundings_2(),
                GetMatProviderFundings_3(),
                GetMatProviderFundings_4(),
                GetMatProviderFundings_5(),
                GetMatProviderFundings_6(),
                GetMatProviderFundings_7(),
                GetMatProviderFundings_8(),
                GetMatProviderFundings_9(),
                GetMatProviderFundings_10(),
                GetMatProviderFundings_11(),
                GetMatProviderFundings_12(),
                GetMatProviderFundings_13()
            };

            foreach (var fetchTask in fetchTasks)
            {
                var providerFundings = await fetchTask;
                returnList.AddRange(providerFundings);
            }

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_1()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            string phaseOfEducation, filename, orgName, providerType, providerSubType;
            var dateOpened = new DateTime(2000, 1, 1);

            phaseOfEducation = "Secondary";
            orgName = "St Aidan's Church of England High School";
            providerType = ProviderTypeExternal.SpecialSchool;
            providerSubType = ProviderSubTypeExternal.AcademySpecialSponsoreLed;

            var totalAmount = 2740800.00;
            filename = "GAG_Static_InYear_Secondary";
            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10034857",
                ProviderUrn = "10034857",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id2",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10034857",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id3",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                ParentName = "Southwark",
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            totalAmount = 2738800.00;
            filename = "GAG_Static_InYear_Secondary_Allocation_1";
            fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");
            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10034857",
                ProviderUrn = "10034857",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id4",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-06")
            });

            totalAmount = 2730800.00;
            filename = "GAG_Static_InYear_Secondary_Allocation_2";
            fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");
            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10034857",
                ProviderUrn = "10034857",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id4",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-12")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_2()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            var totalAmount = 2740800.00;
            string phaseOfEducation, filename, orgName, providerType, providerSubType;
            var dateOpened = new DateTime(2000, 1, 1);
            providerType = ProviderTypeExternal.SpecialSchool;
            providerSubType = ProviderSubTypeExternal.AcademySpecialSponsoreLed;

            phaseOfEducation = "Secondary";
            filename = "GAG_Static_Existing_Secondary";
            orgName = "Bowland High";

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10038354",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id4",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_3()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            string phaseOfEducation, filename, orgName, providerType, providerSubType;
            var dateOpened = new DateTime(2000, 1, 1);
            providerType = ProviderTypeExternal.SpecialSchool;
            providerSubType = ProviderSubTypeExternal.AcademySpecialSponsoreLed;

            phaseOfEducation = "All-through";
            filename = "GAG_Static_Existing_Allthrough";
            var totalAmount = 7312439.57;
            orgName = "Simon Balle All-Through School";

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10034949",
                ProviderUrn = "10034949",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id5",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_4()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            var phaseOfEducation = "Primary";
            var filename = "GAG_Static_InYear";
            var totalAmount = 413390.84;
            var orgName = "Bradford Academy";
            var dateOpened = new DateTime(2021, 11, 01);
            var providerType = ProviderTypeExternal.FreeSchool;
            var providerSubType = ProviderSubTypeExternal.FreeSchool;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10021055",
                ProviderUrn = "10021055",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id6",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_5()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            var phaseOfEducation = "Primary";
            var filename = "GAG_Static_InYear";
            var orgName = "Brinscall Primary School";
            var dateOpened = new DateTime(2021, 11, 01);
            var providerType = ProviderTypeExternal.Academy;
            var providerSubType = ProviderSubTypeExternal.FreeSchoolSpecial;
            var totalAmount = 413390.84;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10078119",
                ProviderUrn = "10078119",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id7",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_6()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            string phaseOfEducation, filename, orgName, providerType, providerSubType;

            phaseOfEducation = "Primary";
            filename = "GAG_Static_InYear_Primary";
            var totalAmount = 413390.84;
            orgName = "All Saints CofE Academy Denstone";
            var dateOpened = new DateTime(2021, 11, 01);
            providerType = ProviderTypeExternal.Academy;
            providerSubType = ProviderSubTypeExternal.AcademyConverter;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10067283",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id8",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_7()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            string phaseOfEducation, filename, orgName, providerType, providerSubType;

            phaseOfEducation = "Primary";
            filename = "GAG_Static_InYear_Primary";
            var totalAmount = 413390.84;
            orgName = "Springfield Primary Academy";
            var dateOpened = new DateTime(2021, 11, 01);
            providerType = ProviderTypeExternal.Academy;
            providerSubType = ProviderSubTypeExternal.AcademySponsored;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10081419",
                ProviderUrn = "10081419",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id9",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_8()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            string phaseOfEducation, filename, orgName, providerType, providerSubType;

            phaseOfEducation = "Primary";
            filename = "GAG_Static_InYear_Primary_ZeroStartUpFunding";
            var totalAmount = 413390.84;
            orgName = "Wombourne High School";
            var dateOpened = new DateTime(2021, 11, 01);
            providerType = ProviderTypeExternal.Academy;
            providerSubType = ProviderSubTypeExternal.AcademySponsored;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            // Previously Wombridge Primary School
            var ukrpn = "10047466";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = ukrpn,
                ProviderUrn = ukrpn,
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id10",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_9()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            string phaseOfEducation, filename, orgName, providerType, providerSubType;

            phaseOfEducation = "Secondary";
            filename = "GAG_Static_InYear_Secondary";
            var totalAmount = 413390.84;
            orgName = "Great Barr Academy";
            var dateOpened = new DateTime(2021, 11, 01);
            providerType = ProviderTypeExternal.Academy;
            providerSubType = ProviderSubTypeExternal.AcademySponsored;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10061450",
                ProviderUrn = "10061450",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id11",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_10()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            string phaseOfEducation, filename, orgName, providerType, providerSubType;

            phaseOfEducation = "Secondary";
            filename = "GAG_Static_InYear_Secondary_ZeroStartUpFunding";
            var totalAmount = 413390.84;
            orgName = "Belvedere Academy";
            var dateOpened = new DateTime(2021, 11, 01);
            providerType = ProviderTypeExternal.Academy;
            providerSubType = ProviderSubTypeExternal.AcademySponsored;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10021072",
                ProviderUrn = "10021072",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id12",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_11()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            string phaseOfEducation, filename, orgName, providerType, providerSubType;

            phaseOfEducation = "All-through";
            filename = "GAG_Static_InYear_Allthrough";
            var totalAmount = 7312439.57;
            orgName = "Harris Academy Tottenham";
            var dateOpened = new DateTime(2021, 11, 01);
            providerType = ProviderTypeExternal.Academy;
            providerSubType = ProviderSubTypeExternal.AcademySponsored;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10047220",
                ProviderUrn = "10047220",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id13",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_12()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            string phaseOfEducation, filename, orgName, providerType, providerSubType;

            phaseOfEducation = "All-through";
            filename = "GAG_Static_InYear_Allthrough_ZeroStartUpFunding";
            var totalAmount = 7312439.57;
            orgName = "Shrewsbury Academy";
            var dateOpened = new DateTime(2021, 11, 01);
            providerType = ProviderTypeExternal.Academy;
            providerSubType = ProviderSubTypeExternal.AcademySponsored;

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            // Previously Shrewsbury High School
            var ukrpn = "10084320";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = ukrpn,
                ProviderUrn = ukrpn,
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id14",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }

        private async Task<List<FundingApiSearchProviderFunding>> GetMatProviderFundings_13()
        {
            var returnList = new List<FundingApiSearchProviderFunding>();

            string phaseOfEducation, filename, orgName, providerType, providerSubType;

            phaseOfEducation = "Primary";
            filename = "GAG_Static_Existing_Primary";
            var totalAmount = 436228.22;
            orgName = "Abbey View Primary Academy";
            providerType = ProviderTypeExternal.Academy;
            providerSubType = ProviderSubTypeExternal.FreeSchool;
            var dateOpened = new DateTime(2021, 11, 01);

            var fileContents = await System.IO.File.ReadAllTextAsync(@$"Areas/LoggedIn/Data/{filename}.json");

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072811",
                ProviderUrn = "10072811",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id15",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 2";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072812",
                ProviderUrn = "10072812",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id16",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 3";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072813",
                ProviderUrn = "10072813",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id17",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 4";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072814",
                ProviderUrn = "10072814",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id18",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 5";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072815",
                ProviderUrn = "10072815",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id19",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 6";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072816",
                ProviderUrn = "10072816",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id20",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 7";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072817",
                ProviderUrn = "10072817",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id21",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 8";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072818",
                ProviderUrn = "10072818",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id22",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 9";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072819",
                ProviderUrn = "10072819",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id23",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 10";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072820",
                ProviderUrn = "10072815",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id24",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 11";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072821",
                ProviderUrn = "10072821",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id25",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 12";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072822",
                ProviderUrn = "10072822",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id26",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 13";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072823",
                ProviderUrn = "10072823",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id27",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 14";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072824",
                ProviderUrn = "10072824",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id28",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            orgName = "Abbey View Primary Academy 15";

            returnList.Add(new FundingApiSearchProviderFunding
            {
                FundingPeriodCode = "AC-2122",
                FundingStreamCode = "GAG",
                FundingVersion = "1_0",
                ChannelVersions = FundingVersionHelper.BuildChannelVersions(1),
                StatementChannelVersion = 1,
                IsFirstStatementChannelVersion = true,
                SchemaVersion = "1.0",
                TemplateVersion = "1.0",
                FundingValue = fileContents,
                TotalAmount = totalAmount,
                OrganisationName = orgName,
                OrganisationUkprn = "10072825",
                ProviderUrn = "10072825",
                ParentProviderType = GroupingType.AcademyTrust,
                GroupingReason = GroupingReason.Information,
                PhaseOfEducation = phaseOfEducation,
                DateOpened = dateOpened,
                Id = $"{totalAmount}-GAG-Id29",
                ProviderType = providerType,
                ProviderSubType = providerSubType,
                SearchableOrganisationName = orgName,
                StatusChangedDate = Convert.ToDateTime("2021-06-01")
            });

            return returnList;
        }
    }
}