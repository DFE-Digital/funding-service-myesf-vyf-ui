using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Extensions;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class NextPaymentTypeControllerUnitTests
    {
        #region Private Fields

        private readonly Mock<IAdminSettingsService> _mockAdminSettingsService;
        private readonly IMapper _mapper = null;
        private readonly Mock<IClaimsBasedIdentityService> _mockSecurityService;
        private readonly Mock<INextPaymentTypeService> _nextPaymentTypeService;
        private readonly string _fundingStreamName = null;

        private int _fundingStreamId, _nextPaymentTypeId;

        #endregion

        #region Mock Services
        private void SetupMockSettingsServices()
        {
            var mockFundingStream = GetMockFundingStream();

            _mockAdminSettingsService.Setup(x => x.GetSettingById(It.IsAny<int>(), It.IsAny<int>()))
             .ReturnsAsync(new Services.Models.SettingValue());

            _mockAdminSettingsService.Setup(x => x.GetAllFundingStreams(It.IsAny<Repositories.Enums.FetchData[]>()))
               .ReturnsAsync(new List<Services.Models.FundingStream>
               {
                   mockFundingStream
               });

            _mockAdminSettingsService
                .Setup(s => s.GetFundingStreamById(It.IsAny<int>(), It.IsAny<Repositories.Enums.FetchData[]>()))
                .ReturnsAsync(mockFundingStream)
                .Verifiable();

            _mockSecurityService
               .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
               .ReturnsAsync(LoggedInUser);

            _nextPaymentTypeService.Setup(s => s.GetNextPaymentTypes(It.IsAny<int>())).ReturnsAsync(new List<Services.Models.NextPaymentType>()
            {
            });
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentTypeControllerUnitTests"/> class.
        /// </summary>
        public NextPaymentTypeControllerUnitTests()
        {
            _mapper = GetMapper();
            _mockAdminSettingsService = new Mock<IAdminSettingsService>();
            _mockSecurityService = new Mock<IClaimsBasedIdentityService>();
            _nextPaymentTypeService = new Mock<INextPaymentTypeService>();
        }

        #endregion


        #region Action Tests

        /// <summary>
        /// Index action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Index_ResultExpected()
        {
            //Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentTypeController();

            //Act
            var actual = await controller.Index(_fundingStreamId);

            //Assert
            actual
            .Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<NextPaymentTypesIndexViewModel>();

            _mockAdminSettingsService.Verify(x => x.GetFundingStreamById(It.IsAny<int>(), It.IsAny<Repositories.Enums.FetchData[]>()), Times.Once);
        }

        /// <summary>
        /// Index action on a non zero invalid funding stream identifier gets result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Index_NonZero_Invalid_FundingStreamId_ResultExpected()
        {
            //Arrange
            _fundingStreamId = 999;
            _mockAdminSettingsService.Setup(x => x.GetAllFundingStreams())
              .ReturnsAsync(new List<Services.Models.FundingStream>
              {
                    new Services.Models.FundingStream()
              });
            var controller = GetNextPaymentTypeController();

            //Act
            var actual = await controller.Index(_fundingStreamId);

            //Assert
            actual
             .Should().BeOfType<RedirectToRouteResult>()
             .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsFundingStream);

            _mockAdminSettingsService.Verify(x => x.GetFundingStreamById(It.IsAny<int>(), It.IsAny<Repositories.Enums.FetchData[]>()), Times.Once);
        }

        /// <summary>
        /// Add action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_AddActionMode_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentTypeController();
            var expectedViewModel = new NextPaymentTypeViewModel
            {
                ActionMode = ActionMode.Add,
                FundingStreamId = _fundingStreamId,
                FundingStreamName = "test",
                NextPaymentType = new NextPaymentType(),
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
            var actual = await controller.Action(_nextPaymentTypeId, _fundingStreamId, ActionMode.Add);

            // Assert
            actual
               .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NextPaymentTypeViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
        }

        /// <summary>
        /// Delete action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_DeleteActionMode_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentTypeController();
            var expectedViewModel = GetNextPaymentTypeViewModel(ActionMode.Delete);

            // Act
            var actual = await controller.Action(_nextPaymentTypeId, _fundingStreamId, ActionMode.Delete);

            // Assert
            actual
                 .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsHome);


            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Edit action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_EditActionMode_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentTypeController();
            var expectedViewModel = GetNextPaymentTypeViewModel(ActionMode.Edit);

            // Act
            var actual = await controller.Action(_nextPaymentTypeId, _fundingStreamId, ActionMode.Edit);

            // Assert
            actual
                 .Should().BeOfType<RedirectToRouteResult>()
                 .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsHome);


            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Invalid next payment type id gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_Invalid_NextPaymentTypeId_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            _nextPaymentTypeId = _fundingStreamId = 1;
            var controller = GetNextPaymentTypeController();

            // Act
            var actual = await controller.Action(_nextPaymentTypeId, _fundingStreamId, ActionMode.Delete);

            // Assert
            actual
               .Should().BeOfType<RedirectToRouteResult>()
              .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsFundingStream);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Invalid funding stream identifier gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_Invalid_FundingStreamId_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            _nextPaymentTypeId = 1;
            var controller = GetNextPaymentTypeController();

            // Act
            var actual = await controller.Action(_nextPaymentTypeId, _fundingStreamId, ActionMode.Delete);

            // Assert
            actual
              .Should().BeOfType<RedirectToRouteResult>()
             .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsHome);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
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
            var controller = GetNextPaymentTypeController();
            var postedModel = GetNextPaymentTypeViewModel(ActionMode.Edit);
            var expectedViewModel = GetNextPaymentTypeViewModel(ActionMode.Edit);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            var actual = await controller.Action(postedModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NextPaymentTypeViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// A valid model gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_ValidModel_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentTypeController();
            var postedModel = GetNextPaymentTypeViewModel(ActionMode.Edit);
            var expectedViewModel = GetNextPaymentTypeViewModel(ActionMode.Edit);

            // Act
            var actual = await controller.Action(postedModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NextPaymentTypeViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Are you sure action gets the result expected.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void AreYouSure_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentTypeController();
            var viewModel = GetNextPaymentTypeViewModel(ActionMode.Edit);
            var expectedViewModel = GetNextPaymentTypeViewModel(ActionMode.Edit);

            // Act
            var actual = controller.AreYouSure(viewModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NextPaymentTypeViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Saves the changes and adds model gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Add_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentTypeController();
            var viewModel = GetNextPaymentTypeViewModel(ActionMode.Add);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
          .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminNextPaymentTypeConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
        }

        /// <summary>
        /// Saves the changes for the edit action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Edit_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentTypeController();
            var viewModel = GetNextPaymentTypeViewModel(ActionMode.Edit);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
           .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminNextPaymentTypeConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Saves the changes on delete and gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Delete_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentTypeController();
            var viewModel = GetNextPaymentTypeViewModel(ActionMode.Delete);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
           .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminNextPaymentTypeConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Confirmation on failure gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Failure_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentTypeController();
            var expectedViewModel = new NextPaymentTypeViewModel
            {
                ActionMode = ActionMode.Delete,
                SubmittedAtDisplayDate = string.Empty
            };

            // Act
            var actual = await controller.Confirmation(_nextPaymentTypeId, _fundingStreamId, _fundingStreamName, ActionMode.Delete, false);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NextPaymentTypeViewModel>();

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(It.IsAny<Repositories.Enums.FetchData[]>()), Times.Never);
            _mockAdminSettingsService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        #endregion


        #region Private Methods

        private static IMapper GetMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureWebMappings();
            return new Mapper(config);
        }

        private static Services.Models.FundingStream GetMockFundingStream()
        {
            return new Services.Models.FundingStream()
            {
                Publications = new List<Services.Models.Publication>()
                {
                    new Services.Models.Publication()
                    {
                        FundingStream = new Services.Models.FundingStream()
                        {
                            Id = 1, SettingValues = new List<Services.Models.SettingValue>()
                            {
                                new Services.Models.SettingValue()
                                {
                                    SettingId = 1,
                                    Setting = new Services.Models.SettingType()
                                    {
                                        ValueDataType = SettingValueDataType.Int
                                    }
                                }
                            }
                        },
                        PublishedDate = new DateTime(1, 1, 1)
                    }
                },
                SettingValues = new List<Services.Models.SettingValue>()
                {
                    new Services.Models.SettingValue()
                    {
                        SettingId = 1,
                        Setting = new Services.Models.SettingType()
                        {
                            ValueDataType = SettingValueDataType.Int
                        }
                    }
                },
                FundingStreamName = "test"
            };
        }

        /// <summary>
        /// Gets the view your funding next payment controller.
        /// </summary>
        /// <returns>ViewYourFundingSettingsController.</returns>
        private NextPaymentTypeController GetNextPaymentTypeController()
        {
            return new NextPaymentTypeController(
            _nextPaymentTypeService.Object,
            _mockAdminSettingsService.Object,
            _mapper,
            _mockSecurityService.Object,
            GetMockConfigService().Object,
            GetLogger().Object);
        }

        private Mock<ILoggerAdapter<NextPaymentTypeActionBase>> GetLogger()
        {
            var loggerService = new Mock<ILoggerAdapter<NextPaymentTypeActionBase>>();

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

        private NextPaymentTypeViewModel GetNextPaymentTypeViewModel(ActionMode actionMode)
        {
            var expectedViewModel = new NextPaymentTypeViewModel
            {
                NextPaymentType = new NextPaymentType(),
                ActionMode = actionMode,
                FundingStreamId = _fundingStreamId,
                FundingStreamName = "test",
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
        #endregion
    }
}