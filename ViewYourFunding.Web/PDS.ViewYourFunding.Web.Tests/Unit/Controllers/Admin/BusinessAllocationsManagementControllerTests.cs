using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Pds.Core.Common.Identity.Enums;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.BusinessAllocationsManagement;
using PDS.ViewYourFunding.Web.Exceptions;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers.Admin
{
    [TestClass]
    [TestCategory("Unit")]
    public class BusinessAllocationsManagementControllerTests : BaseControllerUnitTests
    {
        public BusinessAllocationsManagementControllerTests()
        {
            new HttpClient(Handler);
        }

        public BusinessAllocationsManagementController GetBusinessAllocationsManagementController(string httpUrl = null)
        {
            SetupMockServices();
            SetupApplicationConfigurationOptions(httpUrl);
            return new BusinessAllocationsManagementController(
                 SecurityService,
                 ApplicationConfigurationOptions,
                 HttpClientFactory,
                 AdminSettingsService,
                 BackgroundTaskQueue,
                 AuditService,
                 ProviderFundingService,
                 PdfGenerationActionsControllerLogger);
        }

        [TestMethod]
        public async Task Index_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var expectedViewModel = new BusinessAllocationsManagementHomeViewModel
            {
                CurrentUser = GetCurrentUserViewModel()
            };

            // Act
            var actual = await controller.Index();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<BusinessAllocationsManagementHomeViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task RunFeedReader_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var expectedViewModel = new RunFeedReaderViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamCodesSource = new List<SelectListItem>
                {
                    new SelectListItem("14 to 16 funding (14-16)", "1416")
                }
            };

            // Act
            var actual = await controller.RunFeedReader();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RunFeedReaderViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task RunFeedReader_WhenSfsAdmin_ResultsHasAllFundingStreams()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            LoggedInUser.Roles.Add(nameof(UserRole.SfsAdmin));

            var fundingStreamCodesSource = new List<SelectListItem>
                {
                    new SelectListItem("14 to 16 funding (14-16)", "1416"),
                    new SelectListItem("16 to 19 funding (16-19)", "1619")
                };

            // Act
            var actual = await controller.RunFeedReader();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RunFeedReaderViewModel>()
                .Which.FundingStreamCodesSource.Should().BeEquivalentTo(fundingStreamCodesSource);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task FeedReaderLastRun_FromHomePage_ReuturnsSuccessfully()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var expectedViewModel = new FeedReaderLastRunViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                Audit = new Dictionary<string, DataImportAuditModel>
                {
                    {
                        "14 to 16 funding (14-16)", new DataImportAuditModel
                            {
                                EndDateTime = new DateTime(2021, 07, 06, 13, 00, 00),
                                StartDateTime = new DateTime(2021, 07, 06, 12, 00, 00),
                                FundingUri = nameof(DataImportAuditModel.FundingUri),
                                Status = nameof(DataImportAuditModel.Status)
                            }
                    }
                },
                FundingStreamCodes = new List<string> { "1416" },
                ShowAllFundingStreams = true
            };

            // Act
            var actual = await controller.FeedReaderLastRun();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FeedReaderLastRunViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);


            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task FeedReaderLastRun_WhenNoFundingStreams_DoesnotProceed()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var expectedViewModel = new RunFeedReaderViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamCodesSource = new List<SelectListItem>
                {
                    new SelectListItem("14 to 16 funding (14-16)", "1416")
                }
            };

            // Act
            var actual = await controller.FeedReaderLastRun(expectedViewModel);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RunFeedReaderViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task FeedReaderLastRun_WhenFundingStreamsSelected_Proceeds()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var viewModel = new RunFeedReaderViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamCodesSource = new List<SelectListItem>
                {
                    new SelectListItem("14 to 16 funding (14-16)", "1416")
                },
                FundingStreamCodes = new List<string> { "1416" }
            };

            var expectedViewModel = new FeedReaderLastRunViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                Audit = new Dictionary<string, DataImportAuditModel>
                {
                    {
                        "14 to 16 funding (14-16)", new DataImportAuditModel
                            {
                                EndDateTime = new DateTime(2021, 07, 06, 13, 00, 00),
                                StartDateTime = new DateTime(2021, 07, 06, 12, 00, 00),
                                FundingUri = nameof(DataImportAuditModel.FundingUri),
                                Status = nameof(DataImportAuditModel.Status)
                            }
                    }
                },
                FundingStreamCodes = viewModel.FundingStreamCodes
            };

            // Act
            var actual = await controller.FeedReaderLastRun(viewModel);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FeedReaderLastRunViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public void CallFeedReader_FeedReaderUrlNotSet()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            // Act
            Action act = () =>
            {
                controller.ConfirmRunFeedReader(new FeedReaderLastRunViewModel());
            };

            // Assert
            act.Should().Throw<Exception>().WithMessage("Feed reader url not provided");
        }

        [TestMethod]
        public void ConfirmRunFeedReader_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController("http://www.feed-reader-function.com");

            Mock.Get(Handler)
                  .Protected()
                  .Setup<Task<HttpResponseMessage>>(
                      "SendAsync",
                      ItExpr.IsAny<HttpRequestMessage>(),
                      ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                  .Verifiable();

            // Act
            var actual = controller.ConfirmRunFeedReader(new FeedReaderLastRunViewModel());

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_BusinessAllocationsActionsConfirmation);

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "success", true
                },
                {
                    "BusinessAllocationsAction", BusinessAllocationsAction.RunFeedReader
                }
            };

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions), Mock.Get(BackgroundTaskQueue));
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task Confirmation_ResultExpected(bool success)
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();
            var fundingStreamCodeAndPeriodCode = "1416";
            var expectedFundingStreamAndPeriodCode = "14 to 16 funding (14-16)";
            string expectedSourceFolder = null;
            string expectedTargetFolder = null;

            var expectedViewModel = new BusinessAllocationsConfirmationViewModel
            {
                Success = success,
                CurrentUser = GetCurrentUserViewModel(),
                ContactUsLink = ApplicationConfigurationOptions.Value.ContactUsLink,
                FundingStreamCodeAndPeriodCode = expectedFundingStreamAndPeriodCode,
                SelectedFundingStreamCodes = fundingStreamCodeAndPeriodCode,
                SourceFolder = null,
                TargetFolder = null
            };

            // Act
            var actual = await controller.Confirmation(success, BusinessAllocationsAction.RunFeedReader, fundingStreamCodeAndPeriodCode, expectedSourceFolder, expectedTargetFolder);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<BusinessAllocationsConfirmationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task ConfirmComparison_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();
            var fundingStreamCodeAndPeriodCode = "1416:AS-2122";
            string expectedSourceFolder = "sourcefolder";
            string expectedTargetFolder = "targetFolder";

            var expectedViewModel = new RunPdfConfirmComparisonViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamCodeAndPeriodCode = fundingStreamCodeAndPeriodCode,
                FundingStreamCodesAndPeriodCodes = new List<SelectListItem>
                {
                    new SelectListItem("1416:", "1416:")
                },
                SourceFolder = expectedSourceFolder,
                TargetFolder = expectedTargetFolder
            };

            // Act
            var actual = await controller.RunPdfComparison(fundingStreamCodeAndPeriodCode, expectedSourceFolder, expectedTargetFolder);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RunPdfConfirmComparisonViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task ConfirmComparison_RequiredParameterNotSet()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();
            var fundingStreamCodeAndPeriodCode = "1416:AS-2122";

            // Act
            var actual = await controller.RunPdfComparison(fundingStreamCodeAndPeriodCode, null, null);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_BusinessAllocationsRunPdfComparison);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task ConfirmDataRequest_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();
            var fundingStreamCodeAndPeriodCode = "1416";

            var expectedViewModel = new FeedReaderLastRunViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                SelectedFundingStreamCodes = fundingStreamCodeAndPeriodCode,
                FundingStreamCodes = new List<string> { fundingStreamCodeAndPeriodCode },
                Audit = new Dictionary<string, DataImportAuditModel>
                {
                    {
                        "14 to 16 funding (14-16)", new DataImportAuditModel
                            {
                                EndDateTime = new DateTime(2021, 07, 06, 13, 00, 00),
                                StartDateTime = new DateTime(2021, 07, 06, 12, 00, 00),
                                FundingUri = nameof(DataImportAuditModel.FundingUri),
                                Status = nameof(DataImportAuditModel.Status)
                            }
                    }
                }
            };

            // Act
            var actual = await controller.FeedReaderLastRun(fundingStreamCodeAndPeriodCode);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FeedReaderLastRunViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task RunPdfComparison_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var expectedViewModel = new RunPdfComparisonViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamCodesAndPeriodCodes = new List<SelectListItem>
                {
                    new SelectListItem("1416:", "1416:")
                }
            };

            // Act
            var actual = await controller.RunPdfComparison();

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RunPdfComparisonViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public void ConfirmPdfComparison_DocumentGeneratorPdfComparerUrlNotSet()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            // Act
            Action act = () =>
            {
                controller.ConfirmRunPdfComparison(new RunPdfComparisonViewModel());
            };

            // Assert
            act.Should().Throw<RequestException>().WithMessage("Document Generator PDF Comparer url not provided");
        }

        [TestMethod]
        public void ConfirmPdfComparison_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController("http://www.run-pdf-compare-function.com");

            Mock.Get(Handler)
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            // Act
            var actual = controller.ConfirmRunPdfComparison(GetValidRunPdfComparisonViewModel());

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_BusinessAllocationsActionsConfirmation);

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "success", true
                },
                {
                    "BusinessAllocationsAction", PdfGenerationAction.PdfComparison
                },
                {
                    "FundingStreamCodeAndPeriodCode", "1416:AC-2122"
                },
                {
                    "SourceFolder", "SourceFolder"
                },
                {
                    "TargetFolder", "DestinationFolder"
                }
            };

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions), Mock.Get(BackgroundTaskQueue));
        }

        [TestMethod]
        public async Task SelectFundingStream_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "fundingStreamCode", "1416"
                }
            };

            // Act
            var actual = await controller.SelectFundingStream();

            // Assert
            actual
               .Should().BeOfType<RedirectToRouteResult>()
               .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYear);

            actual
                 .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task SelectFundingStream_SelectedFundingStream_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var selectFundingStreamViewModel = new SelectFundingStreamViewModel
            {
                FundingStreamCodesSource = new List<SelectListItem>
                {
                    new SelectListItem("2018/19 academic year (AY-1819)", "AY-1819")
                },
                SelectedFundingStreamCode = "1416"
            };

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "fundingStreamCode", selectFundingStreamViewModel.SelectedFundingStreamCode
                }
            };

            // Act
            var actual = await controller.SelectFundingStream(selectFundingStreamViewModel);

            // Assert
            actual
               .Should().BeOfType<RedirectToRouteResult>()
               .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYear);

            actual
                 .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public async Task SelectFundingStream_SelectedFundingStream_NullOrEmpty(string selectedFundingStreamCode)
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var selectFundingStreamViewModel = new SelectFundingStreamViewModel
            {
                SelectedFundingStreamCode = selectedFundingStreamCode
            };

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "error", true
                }
            };

            // Act
            var actual = await controller.SelectFundingStream(selectFundingStreamViewModel);

            // Assert
            actual
               .Should().BeOfType<RedirectToRouteResult>()
               .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_BusinessAllocationsSelectFundingStream);

            actual
                 .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task SearchByProviderAndYear_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var expectedViewModel = new SearchByProviderAndYearViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamPeriodCodes = new List<SelectListItem>
                {
                    new SelectListItem("2018/19 academic year (AY-1819)", "AY-1819")
                },
                FundingStreamCode = "1416"
            };

            // Act
            var actual = await controller.SearchByProviderAndYear("1416");

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<SearchByProviderAndYearViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task SearchByProviderAndYear_SelectedFundingPeriodCode_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var searchByProviderAndYearViewModel = new SearchByProviderAndYearViewModel
            {
                SelectedFundingPeriodCode = "AY-1819",
                FundingStreamCode = "1416",
                Ukprn = "10039379"
            };

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "fundingStreamCode", searchByProviderAndYearViewModel.FundingStreamCode
                },
                {
                    "ukprn", searchByProviderAndYearViewModel.Ukprn
                },
                {
                    "fundingPeriodCode", searchByProviderAndYearViewModel.SelectedFundingPeriodCode
                }
            };

            // Act
            var actual = await controller.SearchByProviderAndYear(searchByProviderAndYearViewModel);

            // Assert
            actual
               .Should().BeOfType<RedirectToRouteResult>()
               .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYearDetails);

            actual
                 .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public async Task SearchByProviderAndYear_SelectedFundingPeriodCode_NullOrEmpty(string selectedFundingPeriodCode)
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var searchByProviderAndYearViewModel = new SearchByProviderAndYearViewModel
            {
                SelectedFundingPeriodCode = selectedFundingPeriodCode,
                FundingStreamCode = "1416",
                Ukprn = "10039379"
            };

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "fundingStreamCode", searchByProviderAndYearViewModel.FundingStreamCode
                },
                {
                    "error", true
                }
            };

            // Act
            var actual = await controller.SearchByProviderAndYear(searchByProviderAndYearViewModel);

            // Assert
            actual
               .Should().BeOfType<RedirectToRouteResult>()
               .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_BusinessAllocationsSearchByProviderAndYear);

            actual
                 .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task SearchByProviderAndYearDetails_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            var expectedViewModel = new SearchByProviderAndYearDetailsViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                ProviderFundings = new List<ProviderFundingModel>
                {
                    new ProviderFundingModel { FundingStreamCode = "PSG", PartitionKey = "10039379", FundingVersion = "3_0" }
                },
                Ukprn = "10039379",
                SelectedFundingPeriodCode = "AY-1920",
                FormattedFundingPeriodCode = "2019/20 academic year",
                FundingStreamCode = "PSG",
                FundingStreamBusinessAllocationName = "PE and sport grant"
            };

            // Act
            var actual = await controller.SearchByProviderAndYearDetails("PSG", "10039379", "AY-1920");

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<SearchByProviderAndYearDetailsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task SearchByProviderDataResult_ResultExpected()
        {
            // Arrange
            var controller = GetBusinessAllocationsManagementController();

            string expectedFormattedDateValue = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "{\n  \"id\": \"testId\",\n  \"fundingVersion\": \"testFundingVersion\"\n}" : "{\r\n  \"id\": \"testId\",\r\n  \"fundingVersion\": \"testFundingVersion\"\r\n}";

            var expectedViewModel = new SearchByProviderDataResultViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                ProviderFunding = new ProviderFundingModel { FundingStreamCode = "PSG", PartitionKey = "10039379", FundingVersion = "3_0", FundingPeriodId = "AY-1920", Provider = new ProviderModel { Name = "Test Provider Name" } },
                DataValue = "[{\"id\": \"testId\", \"fundingVersion\": \"testFundingVersion\"}]",
                FormattedDataValue = expectedFormattedDateValue,
                FundingStreamBusinessAllocationName = "PE and sport grant",
                FormattedFundingPeriodCode = "2019/20 academic year"
            };

            // Act
            var actual = await controller.SearchByProviderDataResult("PSG-AY-1920-10039379-3_0");

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<SearchByProviderDataResultViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        private static RunPdfComparisonViewModel GetValidRunPdfComparisonViewModel()
        {
            return new RunPdfComparisonViewModel
            {
                FundingStreamCodeAndPeriodCode = "1416:AC-2122",
                SourceFolder = "SourceFolder",
                TargetFolder = "DestinationFolder",
                FundingStreamCodesAndPeriodCodes = new List<SelectListItem>()
            };
        }

        private void SetupApplicationConfigurationOptions(
                string httpUrl)
        {
            Mock.Get(ApplicationConfigurationOptions)
               .Setup(x => x.Value)
              .Returns(new ApplicationConfiguration
              {
                  ContactUsLink = "http://www.vyf-contactus.com/contactus",
                  FeedbackLink = "http://www.vyf-survey.com/survey",
                  FeedReaderUrl = httpUrl,
                  DocumentGeneratorPdfComparerUrl = httpUrl,
                  DocumentGeneratorUrl = httpUrl,
                  DocumentGeneratorFundingReportsUrl = httpUrl,
                  DocumentGeneratorRerunUrl = httpUrl,
                  ViewYourFundingApiBaseAddress = nameof(ApplicationConfiguration.ViewYourFundingApiBaseAddress)
              });
        }

        private void SetupMockServices(bool returnSfsAdminRole = false)
        {
            LoggedInUser.Roles = new List<string> { "BusinessAllocations_1416" };

            Mock.Get(SecurityService)
               .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
               .ReturnsAsync(LoggedInUser);

            Mock.Get(AdminSettingsService)
                .Setup(s => s.GetAllFundingStreams(It.IsAny<FetchData[]>()))
                .ReturnsAsync(new List<FundingStream>
                {
                    new FundingStream { FundingStreamCode = "1416", FundingStreamName = "14 to 16 funding", FundingStreamBusinessAllocationName = "14 to 16 funding (14-16)", Publications = new List<Publication> { new Publication { Status = Services.Enums.PublicationStatus.Published } } },
                    new FundingStream { FundingStreamCode = "1619", FundingStreamName = "16 to 19 funding", FundingStreamBusinessAllocationName = "16 to 19 funding (16-19)", Publications = new List<Publication> { new Publication { Status = Services.Enums.PublicationStatus.Published } } }
                });

            Mock.Get(AdminSettingsService)
               .Setup(s => s.GetFundingStream("1416"))
               .ReturnsAsync(new FundingStream { FundingStreamCode = "1416", FundingStreamName = "14 to 16 funding", FundingStreamNameWithinSentence = "14 to 16 funding", FundingStreamBusinessAllocationName = "14 to 16 funding (14-16)" });

            Mock.Get(AdminSettingsService)
              .Setup(s => s.GetFundingStream("1619"))
              .ReturnsAsync(new FundingStream { FundingStreamCode = "1619", FundingStreamName = "16 to 19 funding", FundingStreamNameWithinSentence = "16 to 19 funding", FundingStreamBusinessAllocationName = "16 to 19 funding (16-19)" });

            Mock.Get(AdminSettingsService)
              .Setup(s => s.GetPublications("1416"))
              .ReturnsAsync(new List<Publication>
              {
                 new Publication
                 {
                     FundingStream = new FundingStream { FundingStreamCode = "1416" },
                     FundingPeriodCode = "AY-1819"
                 }
              });

            Mock.Get(AdminSettingsService)
                .Setup(x => x.GetFundingStream("PSG"))
                .ReturnsAsync(new FundingStream
                {
                    FundingStreamBusinessAllocationName = "PE and sport grant"
                });

            Mock.Get(BackgroundTaskQueue)
                .Setup(x => x.QueueTask(It.IsAny<Func<CancellationToken, Task>>()));

            Mock.Get(PdfGenerationActionsControllerLogger)
                .Setup(x => x.LogDebug(It.IsAny<string>()));

            Mock.Get(HttpClientFactory)
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(SetupHttpClient(false));

            Mock.Get(AuditService)
                .Setup(x => x.GetDataImportAudit("1416"))
                .ReturnsAsync(new List<DataImportAuditModel>
                {
                    new DataImportAuditModel
                    {
                        EndDateTime = new DateTime(2021, 07, 06, 12, 00, 00),
                        StartDateTime = new DateTime(2021, 07, 06, 11, 00, 00),
                        FundingUri = nameof(DataImportAuditModel.FundingUri),
                        Status = nameof(DataImportAuditModel.Status)
                    }
                });

            Mock.Get(AuditService)
               .Setup(x => x.GetDataImportAudit("1619"))
               .ReturnsAsync(new List<DataImportAuditModel>
               {
                    new DataImportAuditModel
                    {
                        EndDateTime = new DateTime(2020, 07, 06, 12, 00, 00),
                        StartDateTime = new DateTime(2020, 07, 06, 11, 00, 00),
                        FundingUri = nameof(DataImportAuditModel.FundingUri),
                        Status = nameof(DataImportAuditModel.Status)
                    }
               });

            Mock.Get(ProviderFundingService)
               .Setup(x => x.GetProviderFundings("PSG", "10039379", "AY-1920"))
               .ReturnsAsync(new List<ProviderFundingModel>
               {
                    new ProviderFundingModel
                    {
                        PartitionKey = "10039379",
                        FundingStreamCode = "PSG",
                        FundingVersion = "3_0"
                    }
               });

            Mock.Get(ProviderFundingService)
               .Setup(x => x.GetProviderFundingsById("PSG-AY-1920-10039379-3_0"))
               .ReturnsAsync(new List<ProviderFundingModel>
               {
                    new ProviderFundingModel
                    {
                        PartitionKey = "10039379",
                        FundingStreamCode = "PSG",
                        Provider = new ProviderModel { Name = "Test Provider Name" },
                        FundingVersion = "3_0",
                        FundingPeriodId = "AY-1920"
                    }
               });

            Mock.Get(ProviderFundingService)
               .Setup(x => x.GetProviderFundingFromAllById("PSG-AY-1920-10039379-3_0"))
               .ReturnsAsync("[{\"id\": \"testId\", \"fundingVersion\": \"testFundingVersion\"}]");
        }

        private HttpClient SetupHttpClient(bool exception)
        {
            if (exception)
            {
                mockHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("bad request") })
                    .Verifiable();
            }
            else
            {
                mockHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("\"2020-10-21T00:00:00\"")
                    })
                    .Verifiable();
            }

            return new HttpClient(mockHandler.Object);
        }
    }
}