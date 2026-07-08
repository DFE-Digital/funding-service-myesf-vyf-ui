using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.Publications;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ViewYourFunding.Services.Models;
using FundingStream = PDS.ViewYourFunding.Services.Models.FundingStream;
using Publication = PDS.ViewYourFunding.Services.Models.Publication;
using SettingValue = PDS.ViewYourFunding.Services.Models.SettingValue;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers.Admin
{
    [TestClass]
    public class PublicationControllerUnitTests
    {
        #region Private Fields

        private readonly Mock<IAdminSettingsService> _mockAdminSettingsService;

        private readonly Mock<IAdminPublicationService> _mockAdminPublicationService;

        private readonly Mock<IGenerateSpreadsheetService> _mockGenerateSpreadsheetService;

        private readonly Mock<IFundingUiModelDetailsService> _mockFundingUiModelDetailsService;

        private readonly IMapper _mapper;

        private readonly Mock<IClaimsBasedIdentityService> _mockSecurityService;

        private readonly Mock<IPublicationSpreadsheetMetaDataService> _mockPublicationSpreadSheetService;

        private readonly Mock<IBackgroundTaskQueue> _mockBackgroundTaskQueue;

        private readonly Mock<ILayoutManagementService> _mockLayoutManagementService;

        private readonly string _fundingStreamName = null;

        private int _fundingStreamId, _publicationId;
        private Publication _mockPublication;

        #endregion


        #region Mock Services

        private void SetupMockViewYourFundingServices()
        {
            var mockFundingStream = GetMockFundingStream();

            _mockPublication = new Publication
            {
                FundingStreamId = _fundingStreamId,
                FundingStream = mockFundingStream
            };

            _mockAdminSettingsService.Setup(x => x.GetAllFundingStreams(It.IsAny<FetchData[]>()))
                .ReturnsAsync(new List<FundingStream>
                {
                   mockFundingStream
                });

            _mockAdminSettingsService
                .Setup(s => s.GetSettingsById(It.IsAny<int>()))
                .ReturnsAsync(GetFundingSettingValues().ToList())
                .Verifiable();

            _mockAdminSettingsService
                .Setup(s => s.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(mockFundingStream)
                .Verifiable();

            _mockAdminPublicationService.Setup(x => x.GetPublications(It.IsAny<int>())).ReturnsAsync(new List<Publication>
            {
                _mockPublication
            });

            _mockAdminPublicationService.Setup(x => x.UpdatePublication(It.IsAny<Publication>()));
            _mockAdminPublicationService.Setup(x => x.DeletePublication(It.IsAny<Publication>())).ReturnsAsync(true);
            _mockAdminPublicationService.Setup(x => x.CreatePublication(It.IsAny<Publication>())).ReturnsAsync(_mockPublication);

            _mockGenerateSpreadsheetService
                .Setup(s => s.GenerateFundingStreamSpreadSheetByUrlAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new GenerateSpreadsheetResult { Success = true })
                .Verifiable();

            _mockFundingUiModelDetailsService
                .Setup(s => s.GetMaximumUiAndSpreadsheetVersionNumbersByUrl(It.IsAny<string>()))
                .ReturnsAsync(new MaximumUiSpreadsheetVersion())
                .Verifiable();

            _mockGenerateSpreadsheetService
                .Setup(s => s.GenerateFundingStreamSpreadSheetByUrlAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new GenerateSpreadsheetResult { Success = true })
                .Verifiable();

            _mockSecurityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(LoggedInUser);

            _mockLayoutManagementService.Setup(mock => mock.GetAllLayoutsAsync(It.IsAny<List<Expression<Func<LayoutModel, bool>>>>())).ReturnsAsync(new List<LayoutModel>());
        }

        #endregion


        #region Constructor

        public PublicationControllerUnitTests()
        {
            _mockPublicationSpreadSheetService = new Mock<IPublicationSpreadsheetMetaDataService>(MockBehavior.Strict);
            _mockBackgroundTaskQueue = new Mock<IBackgroundTaskQueue>(MockBehavior.Strict);
            _mapper = GetMapper();
            _mockAdminSettingsService = new Mock<IAdminSettingsService>(MockBehavior.Strict);
            _mockAdminPublicationService = new Mock<IAdminPublicationService>(MockBehavior.Strict);
            _mockGenerateSpreadsheetService = new Mock<IGenerateSpreadsheetService>(MockBehavior.Strict);
            _mockFundingUiModelDetailsService = new Mock<IFundingUiModelDetailsService>(MockBehavior.Strict);
            _mockSecurityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            _mockLayoutManagementService = new Mock<ILayoutManagementService>(MockBehavior.Strict);
            SetupMockViewYourFundingServices();
        }

        #endregion


        #region Action Tests

        [TestMethod, TestCategory("Unit")]
        public async Task Action_AddActionMode_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var expectedViewModel = new PublicationActionViewModel
            {
                ActionMode = ActionMode.Add,
                FundingStreamId = _fundingStreamId,
                FundingPublication = new PublicationViewModel(),
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = LoggedInUser.Ukprn,
                    ProviderName = LoggedInUser.ProviderName,
                    IsExternalUser = LoggedInUser.IsExternalUser,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName,
                    IsLoggedIn = LoggedInUser.IsAuthenticated
                }
            };

            // Act
            var actual = await controller.Action(_publicationId, _fundingStreamId, ActionMode.Add);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PublicationActionViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Action_DeleteActionMode_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var expectedViewModel = GetViewYourFundingPublicationViewModel(ActionMode.Delete);
            expectedViewModel.IsAreYouSurePage = false;

            // Act
            var actual = await controller.Action(_publicationId, _fundingStreamId, ActionMode.Delete);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PublicationActionViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Once);
            _mockFundingUiModelDetailsService.Verify(x => x.GetMaximumUiAndSpreadsheetVersionNumbersByUrl(It.IsAny<string>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Action_Invalid_PublicationId_ResultExpected()
        {
            // Arrange
            _publicationId = _fundingStreamId = 1;
            var controller = GetViewYourFundingPublicationController();

            // Act
            var actual = await controller.Action(_publicationId, _fundingStreamId, ActionMode.Delete);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsFundingStream);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Once);
            _mockFundingUiModelDetailsService.Verify(x => x.GetMaximumUiAndSpreadsheetVersionNumbersByUrl(It.IsAny<string>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Action_Invalid_FundingStreamId_ResultExpected()
        {
            // Arrange
            _publicationId = 1;
            var controller = GetViewYourFundingPublicationController();

            // Act
            var actual = await controller.Action(_publicationId, _fundingStreamId, ActionMode.Delete);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsHome);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Once);
            _mockFundingUiModelDetailsService.Verify(x => x.GetMaximumUiAndSpreadsheetVersionNumbersByUrl(It.IsAny<string>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Action_EditActionMode_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var expectedViewModel = GetViewYourFundingPublicationViewModel(ActionMode.Edit);
            expectedViewModel.IsAreYouSurePage = false;

            // Act
            var actual = await controller.Action(_publicationId, _fundingStreamId, ActionMode.Edit);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PublicationActionViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Once);
            _mockFundingUiModelDetailsService.Verify(x => x.GetMaximumUiAndSpreadsheetVersionNumbersByUrl(It.IsAny<string>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Action_InvalidModel_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var postedModel = GetViewYourFundingPublicationViewModel(ActionMode.Edit);
            var expectedViewModel = GetViewYourFundingPublicationViewModel(ActionMode.Edit);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            var actual = await controller.Action(postedModel);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PublicationActionViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Never);
            _mockFundingUiModelDetailsService.Verify(x => x.GetMaximumUiAndSpreadsheetVersionNumbersByUrl(It.IsAny<string>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Action_ValidModel_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var postedModel = GetViewYourFundingPublicationViewModel(ActionMode.Edit);
            var expectedViewModel = GetViewYourFundingPublicationViewModel(ActionMode.Edit);

            // Act
            var actual = await controller.Action(postedModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PublicationActionViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            actual
                 .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Never);
            _mockFundingUiModelDetailsService.Verify(x => x.GetMaximumUiAndSpreadsheetVersionNumbersByUrl(It.IsAny<string>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public void AreYouSure_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var viewModel = GetViewYourFundingPublicationViewModel(ActionMode.Edit);
            var expectedViewModel = GetViewYourFundingPublicationViewModel(ActionMode.Edit);

            // Act
            var actual = controller.AreYouSure(viewModel);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PublicationActionViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Add_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var viewModel = GetViewYourFundingPublicationViewModel(ActionMode.Add);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminPublicationConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminPublicationService.Verify(x => x.CreatePublication(It.IsAny<Publication>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Edit_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var viewModel = GetViewYourFundingPublicationViewModel(ActionMode.Edit);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminPublicationConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Once);
            _mockAdminPublicationService.Verify(x => x.UpdatePublication(It.IsAny<Publication>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Delete_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var viewModel = GetViewYourFundingPublicationViewModel(ActionMode.Delete);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminPublicationConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(It.IsAny<FetchData[]>()), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Once);
            _mockAdminPublicationService.Verify(x => x.DeletePublication(It.IsAny<Publication>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Success_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var expectedViewModel = new PublicationActionViewModel
            {
                ChangesSaved = true,
                ActionMode = ActionMode.Add,
                SubmittedAtDisplayDate = "01 January 0001 at 12:00am",
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = 12345678,
                    ProviderName = "Test Provider",
                    IsExternalUser = true,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName,
                    IsLoggedIn = true
                }
            };

            // Act
            var actual = await controller.Confirmation(_publicationId, _fundingStreamId, _fundingStreamName, ActionMode.Add, true, null);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PublicationActionViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Failure_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            var expectedViewModel = new PublicationActionViewModel
            {
                ActionMode = ActionMode.Delete,
                SubmittedAtDisplayDate = string.Empty,
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = 12345678,
                    ProviderName = "Test Provider",
                    IsExternalUser = true,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName,
                    IsLoggedIn = true
                }
            };

            // Act
            var actual = await controller.Confirmation(_publicationId, _fundingStreamId, _fundingStreamName, ActionMode.Delete, false, null);

            // Assert
            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PublicationActionViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(It.IsAny<FetchData[]>()), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GenerateSpreadsheet_Failure_ResultExpected()
        {
            // Arrange
            _publicationId = 1;
            var controller = GetViewYourFundingPublicationController();

            // Act
            var actual = await controller.GenerateSpreadsheet(_fundingStreamId, _publicationId);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminPublicationConfirmation);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("changesSaved", null);

            _mockGenerateSpreadsheetService.Verify(x => x.GenerateFundingStreamSpreadSheetByUrlAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GenerateSpreadsheet_Success_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            _mockBackgroundTaskQueue.Setup(x => x.QueueTask(It.IsAny<Func<CancellationToken, Task>>()));

            // Act
            var actual = await controller.GenerateSpreadsheet(_fundingStreamId, _publicationId);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminPublicationConfirmation);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().Contain("changesSaved", true);

            _mockBackgroundTaskQueue.Verify(x => x.QueueTask(It.IsAny<Func<CancellationToken, Task>>()), Times.Once);
            _mockAdminPublicationService.Verify(x => x.GetPublications(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetSpreadsheetCreatedDate_Success_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            _mockPublicationSpreadSheetService.Setup(x => x.GetPublicationSpreadsheetMetaDataAsync(It.IsAny<Publication>()))
                .ReturnsAsync(new PublicationSpreadsheetMetaData
                {
                    CreatedDateTime = DateTime.Now
                });

            // Act
            var actual = await controller.GetSpreadsheetCreateDate(It.IsAny<string>(), It.IsAny<string>(), "12-12-2019", It.IsAny<string>());

            // Assert
            actual
                .Should().NotBeEmpty();

            _mockPublicationSpreadSheetService.Verify(x => x.GetPublicationSpreadsheetMetaDataAsync(It.IsAny<Publication>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetSpreadsheetCreatedDate_Failure_ResultExpected()
        {
            // Arrange
            var controller = GetViewYourFundingPublicationController();
            _mockPublicationSpreadSheetService.Setup(x => x.GetPublicationSpreadsheetMetaDataAsync(It.IsAny<Publication>()))
                .ReturnsAsync(new PublicationSpreadsheetMetaData());

            // Act
            var actual = await controller.GetSpreadsheetCreateDate(It.IsAny<string>(), It.IsAny<string>(), "12-12-2020", It.IsAny<string>());

            // Assert
            actual
                .Should().BeNullOrEmpty();

            _mockPublicationSpreadSheetService.Verify(x => x.GetPublicationSpreadsheetMetaDataAsync(It.IsAny<Publication>()), Times.Once);
        }

        #endregion


        #region Private helpers

        protected IEnumerable<SettingValue> GetFundingSettingValues(int count = 2)
        {
            for (var itemIndex = 0; itemIndex < count; itemIndex++)
            {
                yield return new SettingValue
                {
                    Value = nameof(SettingValue.Value),
                    Setting = new SettingType()
                    {
                        SettingName = itemIndex == 0 ? ServiceConstants.UpdateSpreadsheetUrlSettingName : $"setting{itemIndex}"
                    },
                    FundingStream = new FundingStream
                    {
                        FundingStreamName = nameof(FundingStream.FundingStreamName)
                    }
                };
            }
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new WebAutoMapperProfile())).CreateMapper();
        }

        private static readonly User LoggedInUser = new User
        {
            Ukprn = 12345678,
            ProviderName = "Test Provider",
            Email = "test@test.com",
            IsAuthenticated = true,
            IsExternalUser = true,
            FirstName = nameof(User.FirstName),
            FullName = nameof(User.FullName),
            LastName = nameof(User.LastName),
        };

        private static FundingStream GetMockFundingStream()
        {
            return new FundingStream
            {
                Id = 1
            };
        }

        private PublicationController GetViewYourFundingPublicationController()
        {
            var controller = new PublicationController(
                _mockAdminSettingsService.Object,
                _mockAdminPublicationService.Object,
                _mockFundingUiModelDetailsService.Object,
                _mockGenerateSpreadsheetService.Object,
                _mockPublicationSpreadSheetService.Object,
                _mockBackgroundTaskQueue.Object,
                _mockLayoutManagementService.Object,
                null,
                _mapper,
                _mockSecurityService.Object,
                GetMockConfigService().Object,
                null,
                GetLogger().Object);

            return controller;
        }

        private PublicationActionViewModel GetViewYourFundingPublicationViewModel(ActionMode actionMode)
        {
            var expectedViewModel = new PublicationActionViewModel
            {
                ActionMode = actionMode,
                FundingStreamId = _fundingStreamId,
                FundingPublication = new PublicationViewModel
                {
                    PublishedDateDay = "1",
                    PublishedDateMonth = "1",
                    PublishedDateYear = "1"
                },
                CurrentUser = new CurrentUserViewModel
                {
                    Ukprn = LoggedInUser.Ukprn,
                    ProviderName = LoggedInUser.ProviderName,
                    IsExternalUser = LoggedInUser.IsExternalUser,
                    FirstName = LoggedInUser.FirstName,
                    LastName = LoggedInUser.LastName,
                    FullName = LoggedInUser.FullName,
                    IsLoggedIn = LoggedInUser.IsAuthenticated
                },
                IsAreYouSurePage = true
            };
            return expectedViewModel;
        }

        private Mock<ILoggerAdapter<PublicationActionBase>> GetLogger()
        {
            var loggerService = new Mock<ILoggerAdapter<PublicationActionBase>>();

            loggerService.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));

            return loggerService;
        }

        private Mock<IOptions<ApplicationConfiguration>> GetMockConfigService()
        {
            var mockConfigService = new Mock<IOptions<ApplicationConfiguration>>();

            mockConfigService.Setup(x => x.Value)
                .Returns(new ApplicationConfiguration
                {
                    ContactUsLink = "http://www.vyf-contactus.com/contactus",
                    FeedbackLink = "http://www.vyf-survey.com/survey",
                    ViewYourFundingApiBaseAddress = nameof(ApplicationConfiguration.ViewYourFundingApiBaseAddress)
                });

            return mockConfigService;
        }

        #endregion
    }
}