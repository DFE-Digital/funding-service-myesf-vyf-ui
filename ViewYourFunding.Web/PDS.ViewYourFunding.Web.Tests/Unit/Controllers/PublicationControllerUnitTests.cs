using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class PublicationControllerUnitTests
    {
        private Mock<IUserJourneyService> _mockUserJourneyService;

        [TestMethod, TestCategory("Unit")]
        [DataRow("Funding Stream/Period with latest as Preview", "GAG", "AC-2425", "2024-08-01")]
        [DataRow("Funding Stream/Period with latest as Disabled", "GAG", "AC-2324", "2023-08-01")]
        [DataRow("Second Funding Stream/Period with latest as Disabled", "1619", "FY-2324", "2023-08-01")]
        [DataRow("Funding Stream/Period with latest as Published", "1619", "FY-2425", "2025-01-01")]
        [DataRow("Non existing funding streams", "Test", "FY-2425", "1900-01-01")]
        [DataRow("Non existing funding periods", "GAG", "FY-2526", "1900-01-01")]
        [DataRow("Funding Stream with empty Publications", "1416", "FY-2526", "1900-01-01")]
        [DataRow("Funding Stream with Null Publications", "PNA", "FY-2526", "1900-01-01")]
        [DataRow("Funding Stream with Settings are null", "UIFSM", "FY-2425", "1900-01-01")]
        public async Task GetLatestFundingStreamPublishedDate_ReturnsValidDateWhenFound(string testCaseName, string fundingStreamCode, string fundingPeriodId, string expectedValue)
        {
            // Arrange
            var controller = GetPublicationController();
            DateTime expectedDate = DateTime.Parse(expectedValue);

            // Act
            var actual = await controller.GetLatestFundingStreamPublishedDate(fundingStreamCode, fundingPeriodId);

            // Assert
            actual.Should().Be(expectedDate, testCaseName);
        }

        private PublicationController GetPublicationController()
        {
            _mockUserJourneyService = new Mock<IUserJourneyService>(MockBehavior.Strict);

            _mockUserJourneyService
                .Setup(a => a.GetFundingStreams())
                .ReturnsAsync(new List<FundingStream>()
                {
                    new ()
                    {
                        FundingStreamCode = "GAG",
                        Publications = new List<Publication>
                        {
                            new () { PublishedDate = new DateTime(2023, 6, 1), Status = PublicationStatus.Published, FundingPeriodCode = "AC-2324" },
                            new () { PublishedDate = new DateTime(2023, 8, 1), Status = PublicationStatus.Published, FundingPeriodCode = "AC-2324" },
                            new () { PublishedDate = new DateTime(2023, 12, 1), Status = PublicationStatus.Disabled, FundingPeriodCode = "AC-2324" },
                            new () { PublishedDate = new DateTime(2024, 6, 1), Status = PublicationStatus.Published, FundingPeriodCode = "AC-2425" },
                            new () { PublishedDate = new DateTime(2024, 8, 1), Status = PublicationStatus.Published, FundingPeriodCode = "AC-2425" },
                            new () { PublishedDate = new DateTime(2024, 12, 1), Status = PublicationStatus.Disabled, FundingPeriodCode = "AC-2425" },
                            new () { PublishedDate = new DateTime(2025, 1, 1), Status = PublicationStatus.Preview, FundingPeriodCode = "AC-2425" },
                        },
                        SettingValues = new List<SettingValue>()
                        {
                            new () { Id = 1, Value = "Test" },
                        },
                    },
                    new ()
                    {
                        FundingStreamCode = "1619",
                        Publications = new List<Publication>
                        {
                            new () { PublishedDate = new DateTime(2023, 6, 1), Status = PublicationStatus.Published, FundingPeriodCode = "FY-2324" },
                            new () { PublishedDate = new DateTime(2023, 8, 1), Status = PublicationStatus.Published, FundingPeriodCode = "FY-2324" },
                            new () { PublishedDate = new DateTime(2023, 12, 1), Status = PublicationStatus.Disabled, FundingPeriodCode = "FY-2324" },
                            new () { PublishedDate = new DateTime(2024, 6, 1), Status = PublicationStatus.Published, FundingPeriodCode = "FY-2425" },
                            new () { PublishedDate = new DateTime(2024, 8, 1), Status = PublicationStatus.Published, FundingPeriodCode = "FY-2425" },
                            new () { PublishedDate = new DateTime(2024, 12, 1), Status = PublicationStatus.Disabled, FundingPeriodCode = "FY-2425" },
                            new () { PublishedDate = new DateTime(2025, 1, 1), Status = PublicationStatus.Published, FundingPeriodCode = "FY-2425" },
                        },
                        SettingValues = new List<SettingValue>()
                        {
                            new () { Id = 1, Value = "Test" },
                        },
                    },
                    new ()
                    {
                        FundingStreamCode = "1416",
                        Publications = new List<Publication>(),
                        SettingValues = new List<SettingValue>()
                        {
                            new () { Id = 1, Value = "Test" },
                        },
                    },
                    new ()
                    {
                        FundingStreamCode = "PNA",
                        Publications = null,
                        SettingValues = new List<SettingValue>()
                        {
                            new () { Id = 1, Value = "Test" },
                        },
                    },
                    new ()
                    {
                        FundingStreamCode = "UIFSM",
                        Publications = new List<Publication>
                        {
                            new () { PublishedDate = new DateTime(2023, 6, 1), Status = PublicationStatus.Published, FundingPeriodCode = "FY-2324" },
                            new () { PublishedDate = new DateTime(2023, 8, 1), Status = PublicationStatus.Published, FundingPeriodCode = "FY-2324" },
                            new () { PublishedDate = new DateTime(2023, 12, 1), Status = PublicationStatus.Disabled, FundingPeriodCode = "FY-2324" },
                            new () { PublishedDate = new DateTime(2024, 6, 1), Status = PublicationStatus.Published, FundingPeriodCode = "FY-2425" },
                            new () { PublishedDate = new DateTime(2024, 8, 1), Status = PublicationStatus.Published, FundingPeriodCode = "FY-2425" },
                            new () { PublishedDate = new DateTime(2024, 12, 1), Status = PublicationStatus.Disabled, FundingPeriodCode = "FY-2425" },
                            new () { PublishedDate = new DateTime(2025, 1, 1), Status = PublicationStatus.Published, FundingPeriodCode = "FY-2425" },
                        },
                        SettingValues = null,
                    },
                });


            var controller = new PublicationController(
                null,
                null,
                null,
                _mockUserJourneyService.Object,
                null,
                null,
                null,
                null,
                null);

            return controller;
        }
    }
}
