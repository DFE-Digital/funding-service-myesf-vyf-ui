using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Helpers;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Helpers
{
    /// <summary>
    /// The QueryFilterHelperTests class.
    /// </summary>
    [TestClass]
    public class LayoutHelperTests
    {
        private static string _firstLayoutId = Guid.NewGuid().ToString();
        private static string _secondLayoutId = Guid.NewGuid().ToString();

        #region Tests

        [TestMethod, TestCategory("Unit")]
        [DynamicData(nameof(MapLayoutUiModelShouldMatchExpectedSource))]
        public void MapLayoutUiModel_ShouldMatchExpected(LayoutModel input, LayoutUiModel expected)
        {
            // Act
            var actual = LayoutHelper.MapLayoutUiModel(input);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod, TestCategory("Unit")]
        [DynamicData(nameof(GetLayoutNameShouldMatchExpectedSource))]
        public void GetLayoutName_ShouldMatchExpected(IReadOnlyList<LayoutUiModel> input, string layoutId, string expected)
        {
            // Act
            var actual = input.GetLayoutName(layoutId);

            // Assert
            actual.Should().Be(expected);
        }

        [TestMethod, TestCategory("Unit")]
        [DynamicData(nameof(GetLayoutsAvailableShouldMatchExpectedSource))]
        public void GetLayoutsAvailable_ShouldMatchExpected(
            IReadOnlyList<LayoutUiModel> input,
            FundingViewType fundingViewType,
            FundingViewScope fundingViewScope,
            IEnumerable<SelectListItem> expected)
        {
            // Act
            var actual = input.GetLayoutsAvailable(fundingViewType, fundingViewScope);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        #endregion


        #region Mock Data Member Helpers

        private static IEnumerable<object[]> GetLayoutShouldMatchExpectedSource =>
            new List<object[]>
            {
                new object[]
                {
                    "NationalSpreadsheet",
                    FundingViewType.Spreadsheet,
                    FundingViewScope.National
                },
                new object[]
                {
                    "National",
                    FundingViewType.ViewData,
                    FundingViewScope.National
                },
                new object[]
                {
                    "dummyData",
                    FundingViewType.ViewData,
                    FundingViewScope.OrganisationSummary
                }
            };


        private static IEnumerable<object[]> GetLayoutNameShouldMatchExpectedSource =>
            new List<object[]>
            {
                new object[]
                {
                   GetLayoutUiModels(10).ToList(),
                   _firstLayoutId,
                   GetLayoutUiModels(10).FirstOrDefault(layout => layout.LayoutId == _firstLayoutId.ToString())?.LayoutName
                },
                new object[]
                {
                    GetLayoutUiModels(12).ToList(),
                    _secondLayoutId,
                    GetLayoutUiModels(8).FirstOrDefault(layout => layout.LayoutId == _secondLayoutId.ToString())?.LayoutName
                }
            };

        private static IEnumerable<object[]> MapLayoutUiModelShouldMatchExpectedSource =>
            new List<object[]>
            {
                new object[]
                {
                    new LayoutModel
                    {
                        FundingViewType = FundingViewType.Spreadsheet.ToString(),
                        FundingViewScope = FundingViewScope.Organisation.ToString()
                    },
                    new LayoutUiModel
                    {
                        FundingViewType = FundingViewType.Spreadsheet,
                        FundingViewScope = FundingViewScope.Organisation
                    }
                },
                new object[]
                {
                    new LayoutModel
                    {
                        FundingViewType = FundingViewType.ViewData.ToString(),
                        FundingViewScope = FundingViewScope.National.ToString()
                    },
                    new LayoutUiModel
                    {
                        FundingViewType = FundingViewType.ViewData,
                        FundingViewScope = FundingViewScope.National
                    }
                }
            };

        private static IEnumerable<object[]> GetLayoutsAvailableShouldMatchExpectedSource =>
            new List<object[]>
            {
                new object[]
                {
                    GetLayoutUiModels(4).ToList(),
                    FundingViewType.ViewData,
                    FundingViewScope.OrganisationSummary,
                    GetLayoutSelectItems(4)
                },
                new object[]
                {
                    GetLayoutUiModels(4).ToList(),
                    FundingViewType.Spreadsheet,
                    FundingViewScope.National,
                    new List<SelectListItem>
                    {
                        new SelectListItem("No Layouts set up yet", string.Empty)
                    }
                }
            };

        private static IEnumerable<LayoutUiModel> GetLayoutUiModels(int resultsCount)
        {
            foreach (var item in Enumerable.Range(0, resultsCount))
            {
                yield return new LayoutUiModel
                {
                    LayoutName = $"{nameof(LayoutUiModel.LayoutName)}{item}",
                    LayoutId = item == 1 ? _firstLayoutId.ToString() :
                        item == 2 ? _secondLayoutId.ToString() : item.ToString(),
                    FundingViewType = FundingViewType.ViewData,
                    FundingViewScope = FundingViewScope.OrganisationSummary
                };
            }
        }

        private static IEnumerable<SelectListItem> GetLayoutSelectItems(int resultsCount)
        {
            yield return new SelectListItem("--", string.Empty);

            foreach (var item in Enumerable.Range(0, resultsCount))
            {
                yield return new SelectListItem(
                    $"{nameof(LayoutUiModel.LayoutName)}{item}",
                    item == 1 ? _firstLayoutId.ToString() : item == 2 ? _secondLayoutId.ToString() : item.ToString());
            }
        }

        #endregion

    }
}