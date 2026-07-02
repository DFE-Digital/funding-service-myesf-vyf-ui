using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Constants;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.PdfGenerationActions;
using PDS.ViewYourFunding.Web.Exceptions;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers.Admin
{
    [TestClass]
    [TestCategory("Unit")]
    public class PdfGenerationActionsControllerTests : BaseControllerUnitTests
    {
        private const string DummyHttpUrl = "https://www.feed-reader-function.com?code=test";

        public PdfGenerationActionsControllerTests()
        {
            new HttpClient(Handler);
        }

        public PdfGenerationActionsController GetViewYourFundingSettingsController(string httpUrl = null)
        {
            SetupMockServices();
            SetupApplicationConfigurationOptions(httpUrl);
            return new PdfGenerationActionsController(
                SecurityService,
                ApplicationConfigurationOptions,
                HttpClientFactory,
                AdminSettingsService,
                BackgroundTaskQueue,
                AuditService,
                PdfGenerationActionsControllerLogger);
        }

        [TestMethod]
        public async Task Index_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            var expectedViewModel = new PdfGenerationActionsListViewModel
            {
                CurrentUser = GetCurrentUserViewModel()
            };

            // Act
            var actual = await controller.Index();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PdfGenerationActionsListViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task RunFeedReader_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            var expectedViewModel = new RunFeedReaderViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamCodesSource = new List<SelectListItem>()
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
        public async Task RunPdfComparison_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            var expectedViewModel = new RunPdfComparisonViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamCodesAndPeriodCodes = new List<SelectListItem>()
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
        public async Task GenerateFundingReports_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            var expectedViewModel = new GenerateFundingReportsViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                ReportTypes = LocalAuthorityReports.GetReportTypes(),
                FundingPeriodCodes = new List<SelectListItem>()
            };

            // Act
            var actual = await controller.GenerateFundingReports();

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<GenerateFundingReportsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public void GenerateFundingReportPost_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            var expectedViewModel = new GenerateFundingReportsViewModel
            {
                CurrentUser = GetCurrentUserViewModel()
            };

            // Act
            var actual = controller.GenerateFundingReports(expectedViewModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<GenerateFundingReportsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task ReRunPdfGeneration_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController(DummyHttpUrl);

            var expectedViewModel = new RerunPdfGenerationViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamCodes = new List<SelectListItem>()
            };

            // Act
            var actual = await controller.RerunPdfGeneration();

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RerunPdfGenerationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task ReRunPdfGenerationPost_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController(DummyHttpUrl);
            RerunPdfGenerationViewModel expectedViewModel = GetValidReRunPdfGenerationViewModel();

            // Act
            var actual = await controller.RerunPdfGeneration(expectedViewModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RerunPdfGenerationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public void ConfirmReRunPdfGeneration_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController(DummyHttpUrl);

            Mock.Get(Handler)
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            // Act
            var actual = controller.ConfirmRerunPdfGeneration(GetValidReRunPdfGenerationViewModel());

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation);

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "success", true
                },
                {
                    "PdfGenerationAction", PdfGenerationAction.RerunPdfGeneration
                }
            };

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions), Mock.Get(BackgroundTaskQueue));
        }

        [TestMethod]
        public void ConfirmReRunPdfGenerationPost_DocumentGeneratorRerunUrlNotSet_ThrowsExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();
            var expectedViewModel = new RerunPdfGenerationViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamCode = "GAG",
                ResetFunding = false,
                ResetProviderFunding = false,
                EndDateTime = "2021-05-19T18:03:56.4119848+01:00",
                SinceCreatedDate = "2021-05-18T18:03:56.4119848+01:00",
                FundingStreamCodes = new List<SelectListItem>()
            };

            // Act
            Action act = () =>
            {
                controller.ConfirmRerunPdfGeneration(expectedViewModel);
            };

            // Assert
            act.Should().Throw<RequestException>().WithMessage("Document Generator Rerun url not provided.");
            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task ReRunPdfGenerationPost_RequiredFieldsNotSet_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            controller.ModelState.AddModelError("SinceCreatedDate", "Required");
            controller.ModelState.AddModelError("EndDateTime", "Required");
            controller.ModelState.AddModelError("FundingStreamCode", "Required");

            var expectedViewModel = new RerunPdfGenerationViewModel
            {
                CurrentUser = GetCurrentUserViewModel()
            };

            // Act
            var actual = await controller.RerunPdfGeneration(expectedViewModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RerunPdfGenerationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task RunPdfComparisonPost_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            var expectedViewModel = new RunPdfComparisonViewModel
            {
                CurrentUser = GetCurrentUserViewModel()
            };

            // Act
            var actual = await controller.RunPdfComparison(expectedViewModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RunPdfComparisonViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task RunPdfComparisonPost_Invalid_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            controller.ModelState.AddModelError("SourceFolder", "Required");

            var expectedViewModel = GetValidRunPdfComparisonViewModel();

            // Act
            var actual = await controller.RunPdfComparison(GetValidRunPdfComparisonViewModel());

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RunPdfComparisonViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public void CallFeedReader_FeedReaderUrlNotSet()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            // Act
            Action act = () =>
            {
                controller.ConfirmRunFeedReader(new RunFeedReaderViewModel());
            };

            // Assert
            act.Should().Throw<Exception>().WithMessage("Feed reader url not provided");
        }

        [TestMethod]
        public void ConfirmPdfComparison_DocumentGeneratorPdfComparerUrlNotSet()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

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
            var controller = GetViewYourFundingSettingsController(DummyHttpUrl);

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
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation);

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "success", true
                },
                {
                    "PdfGenerationAction", PdfGenerationAction.PdfComparison
                }
            };

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions), Mock.Get(BackgroundTaskQueue));
        }

        [TestMethod]
        public void ConfirmPdfFundingReport_DocumentGeneratorFundingReportsUrlNotSet()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            // Act
            Action act = () =>
            {
                controller.GenerateFundingReport(new GenerateFundingReportsViewModel());
            };

            // Assert
            act.Should().Throw<RequestException>().WithMessage("Document Generator Funding Reports url not provided");
        }

        [TestMethod]
        [DataRow(LocalAuthorityReports.SixthFormMssValue)]
        [DataRow(LocalAuthorityReports.SixthFormValue)]
        [DataRow(LocalAuthorityReports.StudentNumbersValue)]
        public void GenerateFundingReport_ResultExpected(string reportType)
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController(DummyHttpUrl);

            Mock.Get(Handler)
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();
            var viewModel = GetValidGenerateFundingReportsViewModel(reportType);

            // Act
            var actual = controller.GenerateFundingReport(viewModel);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation);

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "success", true
                },
                {
                    "PdfGenerationAction", PdfGenerationAction.PdfGenerateFundingReport
                }
            };

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions), Mock.Get(BackgroundTaskQueue));
        }

        [TestMethod]
        public void ConfirmRunFeedReader_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController("http://www.feed-reader-function.com");

            Mock.Get(Handler)
                  .Protected()
                  .Setup<Task<HttpResponseMessage>>(
                      "SendAsync",
                      ItExpr.IsAny<HttpRequestMessage>(),
                      ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                  .Verifiable();

            // Act
            var actual = controller.ConfirmRunFeedReader(new RunFeedReaderViewModel { ByPassBookmark = true });

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation);

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "success", true
                },
                {
                    "PdfGenerationAction", PdfGenerationAction.RunFeedReader
                }
            };

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions), Mock.Get(BackgroundTaskQueue));
        }

        [TestMethod]
        public async Task CallFeedReader_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController("http://www.feed-reader-function.com");

            var expectedViewModel = new RunFeedReaderViewModel
            {
                ByPassBookmark = true,
                FundingStreamCodes = new[] { "gag" }
            };

            // Act
            var actual = await controller.RunFeedReader(expectedViewModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RunFeedReaderViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task CallFeedReader_NoSelected_FundingStreamCodes_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController("http://www.feed-reader-function.com");

            var expectedViewModel = new RunFeedReaderViewModel
            {
                ByPassBookmark = true
            };

            // Act
            var actual = await controller.RunFeedReader(expectedViewModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<RunFeedReaderViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            actual
                .Should().BeOfType<ViewResult>();

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task FeedReaderLastRun_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController("http://www.feed-reader-function.com");

            var expectedViewModel = new FeedReaderLastRunViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                Audit = new DataImportAuditModel
                {
                    EndDateTime = new DateTime(2021, 07, 06, 13, 00, 00),
                    StartDateTime = new DateTime(2021, 07, 06, 12, 00, 00),
                    FundingUri = nameof(DataImportAuditModel.FundingUri),
                    Status = nameof(DataImportAuditModel.Status)
                }
            };

            // Act
            var actual = await controller.FeedReaderLastRun();

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FeedReaderLastRunViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }


        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task Confirmation_ResultExpected(bool success)
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            var expectedViewModel = new PdfGenerationOperationConfirmationViewModel
            {
                Success = success,
                CurrentUser = GetCurrentUserViewModel()
            };

            // Act
            var actual = await controller.Confirmation(success, PdfGenerationAction.RunFeedReader);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PdfGenerationOperationConfirmationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(SecurityService), Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task GenerateSinglePdf_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            var expectedViewModel = new GenerateSinglePdfViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                ProviderTypes = GetProviderTypesAndSubTypes(),
                ProviderSubTypes = GetProviderTypesAndSubTypes(),
                FundingStreamCodeAndPeriodCodes = new List<SelectListItem>()
            };

            // Act
            var actual = await controller.GenerateSinglePdf();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                 .Which.Model.Should().BeOfType<GenerateSinglePdfViewModel>()
                 .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod]
        public async Task GenerateSinglePdfPost_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            var expectedViewModel = GetValidGenerateSinglePdfViewModel();

            // Act
            var actual = await controller.GenerateSinglePdf(expectedViewModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<GenerateSinglePdfViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task GenerateSinglePdfPost_Invalid_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            controller.ModelState.AddModelError("ProviderFundingId", "Required");
            controller.ModelState.AddModelError("Ukprn", "Required");
            controller.ModelState.AddModelError("CutOffDate", "Required");

            var expectedViewModel = new GenerateSinglePdfViewModel
            {
                CurrentUser = GetCurrentUserViewModel()
            };

            // Act
            var actual = await controller.GenerateSinglePdf(expectedViewModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<GenerateSinglePdfViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public async Task GenerateSinglePdfPost_Invalid_Uprn_And_CutOffDate_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            controller.ModelState.AddModelError("Ukprn", "Please enter a valid Provider UKPRN");
            controller.ModelState.AddModelError("CutOffDate", "Please enter a valid date: yyyy/mm/dd");

            var expectedViewModel = new GenerateSinglePdfViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                ProviderFundingId = "id",
                Ukprn = "ukprn",
                CutOffDate = "cutOffDate",
            };

            // Act
            var actual = await controller.GenerateSinglePdf(expectedViewModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<GenerateSinglePdfViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        [TestMethod]
        public void ConfirmGenerateSinglePdf_DocumentGeneratorUrlNotSet()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController();

            // Act
            Action act = () =>
            {
                controller.ConfirmGenerateSinglePdf(new GenerateSinglePdfViewModel());
            };

            // Assert
            act.Should().Throw<RequestException>().WithMessage("Document Generator url not provided");
        }

        [TestMethod]
        public void ConfirmGenerateSinglePdf_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingSettingsController(DummyHttpUrl);

            Mock.Get(Handler)
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            var expectedViewModel = GetValidGenerateSinglePdfViewModel();

            // Act
            var actual = controller.ConfirmGenerateSinglePdf(expectedViewModel);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_PdfGenerationActionsConfirmation);

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "success", true
                },
                {
                    "PdfGenerationAction", PdfGenerationAction.GenerateSinglePdf
                }
            };

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            Mock.VerifyAll(Mock.Get(ApplicationConfigurationOptions));
        }

        private static RerunPdfGenerationViewModel GetValidReRunPdfGenerationViewModel()
        {
            return new RerunPdfGenerationViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                FundingStreamCode = "GAG",
                ResetFunding = false,
                ResetProviderFunding = false,
                EndDateTime = "2021-05-19T18:03:56.4119848+01:00",
                SinceCreatedDate = "2021-05-18T18:03:56.4119848+01:00",
                FundingStreamCodes = new List<SelectListItem>()
            };
        }

        private static RunPdfComparisonViewModel GetValidRunPdfComparisonViewModel()
        {
            return new RunPdfComparisonViewModel
            {
                FundingStreamCodeAndPeriodCode = "GAG:AC-2122",
                SourceFolder = "SourceFolder",
                DestinationFolder = "DestinationFolder",
                FundingStreamCodesAndPeriodCodes = new List<SelectListItem>()
            };
        }

        private static GenerateSinglePdfViewModel GetValidGenerateSinglePdfViewModel()
        {
            return new GenerateSinglePdfViewModel
            {
                CurrentUser = GetCurrentUserViewModel(),
                ProviderFundingId = "ProviderFundingId",
                Ukprn = "12345678",
                CutOffDate = "2021-05-26",
                ProviderTypes = GetProviderTypesAndSubTypes(),
                ProviderSubTypes = GetProviderTypesAndSubTypes(),
                FundingStreamCodeAndPeriodCodes = new List<SelectListItem>(),
                FundingStreamCodeAndPeriodCode = "GAG:AC-2122",
            };
        }

        private static IEnumerable<SelectListItem> GetProviderTypesAndSubTypes()
        {
            return new List<SelectListItem>
            {
                 new SelectListItem { Text = "General", Value = "General" },
                 new SelectListItem { Text = "Indicative", Value = "Indicative" }
            };
        }

        private static GenerateFundingReportsViewModel GetValidGenerateFundingReportsViewModel(string reportType)
        {
            return new GenerateFundingReportsViewModel
            {
                FundingPeriodCode = "AC-2122",
                ReportType = reportType
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

        private void SetupMockServices()
        {
            Mock.Get(SecurityService)
               .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
               .ReturnsAsync(LoggedInUser);

            Mock.Get(AdminSettingsService)
                .Setup(s => s.GetAllFundingStreams(It.IsAny<FetchData[]>()))
                .ReturnsAsync(new List<FundingStream>());

            Mock.Get(BackgroundTaskQueue)
                .Setup(x => x.QueueTask(It.IsAny<Func<CancellationToken, Task>>()));

            Mock.Get(PdfGenerationActionsControllerLogger)
                .Setup(x => x.LogDebug(It.IsAny<string>()));

            Mock.Get(HttpClientFactory)
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(SetupHttpClient(false));

            Mock.Get(AuditService)
                .Setup(x => x.GetDataImportAudit())
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