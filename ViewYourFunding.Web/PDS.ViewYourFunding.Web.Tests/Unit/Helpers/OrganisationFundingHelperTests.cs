using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Interfaces.Models;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models;
using PDS.ViewYourFunding.Web.Helpers;
using System;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Helpers
{
    /// <summary>
    /// The OrganisationFundingHelper Tests class.
    /// </summary>
    [TestClass]
    public class OrganisationFundingHelperTests
    {
        [TestMethod, TestCategory("Unit")]
        public void GetFundingsGroupedByPeriod_ForMultipleFundingsFromDifferentYears_ReturnsFundingsGroupedByPeriod()
        {
            //arrange
            var fundings = new List<IFundingApiSearchFunding>
            {
                new FundingApiSearchFunding
                {
                    FundingPeriodCode = "AS-2122",
                    FundingStreamCode = "1619",
                    GroupingReason = "Payment",
                    StatusChangedDate = new DateTime(2021, 10, 01)
                },
                new FundingApiSearchFunding
                {
                    FundingPeriodCode = "AY-2122",
                    FundingStreamCode = "1619",
                    GroupingReason = "Information",
                    StatusChangedDate = new DateTime(2021, 07, 01)
                },
                new FundingApiSearchFunding
                {
                    FundingPeriodCode = "AS-2122",
                    FundingStreamCode = "1619",
                    GroupingReason = "Information",
                    StatusChangedDate = new DateTime(2021, 06, 01)
                },
                new FundingApiSearchFunding
                {
                    FundingPeriodCode = "AS-2021",
                    FundingStreamCode = "1619",
                    GroupingReason = "Payment",
                    StatusChangedDate = new DateTime(2020, 09, 01)
                },
                new FundingApiSearchFunding
                {
                    FundingPeriodCode = "AY-2021",
                    FundingStreamCode = "1619",
                    GroupingReason = "Information",
                    StatusChangedDate = new DateTime(2020, 04, 01)
                },
                new FundingApiSearchFunding
                {
                    FundingPeriodCode = "AS-2021",
                    FundingStreamCode = "1619",
                    GroupingReason = "Information",
                    StatusChangedDate = new DateTime(2020, 03, 01)
                }
            };

            var fundingStream = new FundingStream
            {
                FundingStreamCode = "1619",
                FundingStreamName = "16 to 19 funding",
                FundingStreamNameWithinSentence = "16 to 19 funding",
                RelevantForProviders_LoggedIn = true,
                RelevantForOrganisations_LoggedIn = true,
                RelevantForOrganisations_Public = true,
                HistoryIndependentOfPublications = true,
                Active = true,
                SettingValues = new List<SettingValue>
                    {
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "AcademyAndSchoolAcademicYear"
                            },
                            Value = "202122"
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "UseStaticData"
                            },
                            Value = "True",
                        },
                        new SettingValue
                        {
                            Setting = new SettingType
                            {
                                SettingName = "ParentProviderType"
                            },
                            Value = "LocalAuthority",
                        }
                    },
                NextPayments = new List<NextPayment>(),
                Publications = new List<Publication>
                    {
                        new Publication
                        {
                            PublishedDate = new DateTime(2030, 1, 1),
                            FundingPeriodCode = "AS-2122",
                            IsLatest = true,
                            Status = (Services.Enums.PublicationStatus)PublicationStatus.Published
                        }
                    }
            };

            var expectedResult = new List<KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>>
            {
                new KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>((2021, 2022), new List<LocalAuthorityFundingViewModel>
                {
                    new LocalAuthorityFundingViewModel
                    {
                        FundingPeriodCode = "AS-2122",
                        StatusChangedDate = new DateTime(2021, 10, 01),
                        VariationReason = "Revised allocation.",
                        IsLatest = true,
                        IsFinal = false
                    },
                    new LocalAuthorityFundingViewModel
                    {
                        FundingPeriodCode = "AS-2122",
                        StatusChangedDate = new DateTime(2021, 06, 01),
                        VariationReason = "Initial allocation.",
                        IsLatest = false,
                        IsFinal = false
                    }
                }),
                new KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>((2020, 2021), new List<LocalAuthorityFundingViewModel>
                {
                   new LocalAuthorityFundingViewModel
                    {
                        FundingPeriodCode = "AS-2021",
                        StatusChangedDate = new DateTime(2020, 09, 01),
                        VariationReason = "Revised allocation.",
                        IsLatest = false,
                        IsFinal = true
                    },
                   new LocalAuthorityFundingViewModel
                    {
                        FundingPeriodCode = "AS-2021",
                        StatusChangedDate = new DateTime(2020, 03, 01),
                        VariationReason = "Initial allocation.",
                        IsLatest = false,
                        IsFinal = false
                    }
                })
            };


            //act
            var result = OrganisationFundingHelper.GetFundingsGroupedByPeriod(fundings, fundingStream, true);

            //assert
            result.Should().BeOfType<List<KeyValuePair<(int yearFrom, int yearTo), List<LocalAuthorityFundingViewModel>>>>().And.BeEquivalentTo(expectedResult);
        }

        [TestMethod, TestCategory("Unit")]
        public void AsWebModel_ForApiSearchFunding_ReturnsLocalAuthorityFundingViewModel()
        {
            //arrange
            var apiSearchFunding = new FundingApiSearchFunding
            {
                FundingPeriodCode = "AS-2122",
                FundingStreamCode = "1619",
                GroupingReason = "Payment",
                StatusChangedDate = new DateTime(2021, 10, 01),
                VariationReason = "Initial funding."
            };

            var expectedResult = new LocalAuthorityFundingViewModel
            {
                FundingPeriodCode = "AS-2122",
                StatusChangedDate = new DateTime(2021, 10, 01),
                VariationReason = "Initial funding."
            };

            //act
            var result = OrganisationFundingHelper.AsWebModel(apiSearchFunding);

            //assert
            result.Should().BeOfType<LocalAuthorityFundingViewModel>().And.BeEquivalentTo(expectedResult);
        }
    }
}
