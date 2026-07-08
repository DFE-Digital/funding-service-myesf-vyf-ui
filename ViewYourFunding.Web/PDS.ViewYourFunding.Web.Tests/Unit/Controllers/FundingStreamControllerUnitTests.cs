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
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStream;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Service = PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class FundingStreamControllerUnitTests
    {
        #region Private Fields

        private readonly Mock<IAdminSettingsService> _mockAdminSettingsService;
        private readonly IMapper _mapper;
        private readonly Mock<IClaimsBasedIdentityService> _mockSecurityService;
        private readonly Mock<IFundingStreamService> _nextFundingStreamService;
        private readonly string _fundingStreamName = null;
        private readonly int _fundingStreamId = 0;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamControllerUnitTests"/> class.
        /// </summary>
        public FundingStreamControllerUnitTests()
        {
            _mapper = GetMapper();
            _mockAdminSettingsService = new Mock<IAdminSettingsService>();
            _mockSecurityService = new Mock<IClaimsBasedIdentityService>();
            _nextFundingStreamService = new Mock<IFundingStreamService>();
        }

        #endregion


        #region Action Tests

        /// <summary>
        /// Add action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_AddActionMode_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetFundingStreamController();
            var expectedViewModel = new FundingStreamViewModel
            {
                ActionMode = ActionMode.Add,
                FundingStream = new FundingStream()
            };

            // Act
            var actual = await controller.Action(_fundingStreamId, ActionMode.Add);

            // Assert
            actual
               .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(It.IsAny<FetchData[]>()), Times.Never);
            _nextFundingStreamService.Verify(x => x.CreateFundingStream(It.IsAny<Service.FundingStream>()), Times.Never);
        }

        /// <summary>
        /// Delete action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_DeleteActionMode_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetFundingStreamController();

            // Act
            var actual = await controller.Action(_fundingStreamId, ActionMode.Delete);

            actual
              .Should().BeOfType<ViewResult>()
              .Which.Model.Should().BeOfType<FundingStreamViewModel>();

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _nextFundingStreamService.Verify(x => x.DeleteFundingStream(It.IsAny<Service.FundingStream>()), Times.Never);
        }


        /// <summary>
        /// Edit action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_EditActionMode_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetFundingStreamController();
            var expectedViewModel = GetFundingStreamViewModel(ActionMode.Edit);

            // Act
            var actual = await controller.Action(_fundingStreamId, ActionMode.Edit);

            // Assert
            actual
               .Should().BeOfType<ViewResult>()
               .Which.Model.Should().BeOfType<FundingStreamViewModel>();

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _nextFundingStreamService.Verify(x => x.UpdateFundingStream(It.IsAny<Service.FundingStream>()), Times.Never);
        }

        /// <summary>
        /// Invalid funding stream identifier gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_Invalid_FundingStreamId_ResultExpected()
        {
            // Arrange
            var controller = GetFundingStreamController();
            _mockSecurityService.Setup(x => x.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            // Act
            var actual = await controller.Action(_fundingStreamId, ActionMode.Delete);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsHome);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _nextFundingStreamService.Verify(x => x.UpdateFundingStream(It.IsAny<Service.FundingStream>()), Times.Never);
        }

        /// <summary>
        /// Invalid model gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_InvalidModel_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetFundingStreamController();
            var postedModel = GetFundingStreamViewModel(ActionMode.Edit);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            var actual = await controller.Action(postedModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamViewModel>();

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _nextFundingStreamService.Verify(x => x.UpdateFundingStream(It.IsAny<Service.FundingStream>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Action_ValidModel_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetFundingStreamController();
            var postedModel = GetFundingStreamViewModel(ActionMode.Edit);

            // Act
            var actual = await controller.Action(postedModel);

            // Assert
            actual
            .Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<FundingStreamViewModel>();

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _nextFundingStreamService.Verify(x => x.UpdateFundingStream(It.IsAny<Service.FundingStream>()), Times.Never);
        }

        /// <summary>
        /// Are you sure action gets the result expected.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void AreYouSure_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetFundingStreamController();
            var viewModel = new FundingStreamViewModel();
            var expectedViewModel = new FundingStreamViewModel
            {
                IsAreYouSurePage = true
            };

            // Act
            var actual = controller.AreYouSure(viewModel);

            // Assert
            actual.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _nextFundingStreamService.Verify(x => x.UpdateFundingStream(It.IsAny<Service.FundingStream>()), Times.Never);
        }

        /// <summary>
        /// Saves the changes and adds model gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Add_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetFundingStreamController();
            var viewModel = GetFundingStreamViewModel(ActionMode.Add);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
          .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminFundingStreamConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
        }

        /// <summary>
        /// Saves the changes for the edit action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Edit_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetFundingStreamController();
            var viewModel = GetFundingStreamViewModel(ActionMode.Edit);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
           .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminFundingStreamConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Saves the changes on delete and gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Delete_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetFundingStreamController();
            var viewModel = GetFundingStreamViewModel(ActionMode.Delete);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
           .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminFundingStreamConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Confirmation on failure gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Failure_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetFundingStreamController();
            var expectedViewModel = new FundingStreamViewModel
            {
                ActionMode = ActionMode.Delete,
                SubmittedAtDisplayDate = string.Empty
            };

            // Act
            var actual = await controller.Confirmation(_fundingStreamId, _fundingStreamName, ActionMode.Delete, false);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamViewModel>();

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        #endregion


        #region Mock Services

        private static Services.Models.FundingStream GetMockFundingStream()
        {
            return new Services.Models.FundingStream()
            {
                FundingStreamName = "test",
                FundingStreamCode = "test",
                Id = 1,
                Active = true
            };
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new WebAutoMapperProfile())).CreateMapper();
        }

        private void SetupMockSettingsServices()
        {
            var mockFundingStream = GetMockFundingStream();

            _mockAdminSettingsService.Setup(x => x.GetSettingById(It.IsAny<int>(), It.IsAny<int>()))
             .ReturnsAsync(new Services.Models.SettingValue());

            _mockAdminSettingsService.Setup(x => x.GetAllFundingStreams())
               .ReturnsAsync(new List<Services.Models.FundingStream>
               {
                   mockFundingStream
               });

            _mockAdminSettingsService
                .Setup(s => s.GetFundingStreamById(It.IsAny<int>()))
                .ReturnsAsync(mockFundingStream)
                .Verifiable();
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Gets the view your funding stream controller.
        /// </summary>
        /// <returns>FundingStreamController.</returns>
        private FundingStreamController GetFundingStreamController()
        {
            return new FundingStreamController(
             _mockAdminSettingsService.Object,
             _nextFundingStreamService.Object,
             _mapper,
             _mockSecurityService.Object,
             GetMockConfigService().Object,
             GetLogger().Object);
        }

        private Mock<ILoggerAdapter<FundingStreamActionBase>> GetLogger()
        {
            var loggerService = new Mock<ILoggerAdapter<FundingStreamActionBase>>();

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

        private FundingStreamViewModel GetFundingStreamViewModel(ActionMode actionMode)
        {
            var expectedViewModel = new FundingStreamViewModel
            {
                FundingStream = new FundingStream
                {
                    Id = _fundingStreamId,
                    FundingStreamCode = "test",
                    FundingStreamName = "test"
                },
                ActionMode = actionMode,
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
            return expectedViewModel;
        }

        #endregion
    }
}
