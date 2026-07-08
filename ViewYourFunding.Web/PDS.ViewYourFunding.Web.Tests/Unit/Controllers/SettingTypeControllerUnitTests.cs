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
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.SettingTypes;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    /// <summary>
    /// SettingTypeControllerUnitTests.
    /// </summary>
    [TestClass]
    public class SettingTypeControllerUnitTests
    {
        #region Private Fields

        private readonly Mock<IAdminSettingsService> _mockAdminSettingsService;
        private readonly IMapper _mapper;
        private readonly Mock<IClaimsBasedIdentityService> _mockSecurityService;
        private readonly Mock<ISettingTypeService> _mockSettingTypesService;
        private readonly int _settingTypeId = 0;
        private readonly string _settingTypeName = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingTypeControllerUnitTests"/> class.
        /// </summary>
        public SettingTypeControllerUnitTests()
        {
            _mapper = GetMapper();
            _mockAdminSettingsService = new Mock<IAdminSettingsService>();
            _mockSecurityService = new Mock<IClaimsBasedIdentityService>();
            _mockSettingTypesService = new Mock<ISettingTypeService>();
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
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();
            var expectedSettingTypesIndexViewModel = new SettingTypesIndexViewModel
            {
                SettingTypes = new List<SettingType>
                {
                    new SettingType
                    {
                        Id = 1,
                        SettingName = "Setting Name",
                        SettingDescription = "Setting Description",
                        ValueDataType = SettingValueDataType.String,
                        SettingValues = new List<SettingValue>()
                    }
                }
            };

            //Act
            var actual = await controller.Index();

            //Assert
            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Once);

            actual.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<SettingTypesIndexViewModel>()
                .Which.Should().BeEquivalentTo(expectedSettingTypesIndexViewModel);

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Once);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        /// <summary>
        /// Add action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_AddActionMode_ResultExpected()
        {
            // Arrange
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();
            var expectedViewModel = new SettingTypeViewModel
            {
                ActionMode = ActionMode.Add,
                SettingType = new SettingType()
            };

            // Act
            var actual = await controller.Action(_settingTypeId, false, ActionMode.Add);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<SettingTypeViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        /// <summary>
        /// Delete action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_DeleteActionMode_ResultExpected()
        {
            // Arrange
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();

            // Act
            var actual = await controller.Action(_settingTypeId, false, ActionMode.Delete);

            actual
              .Should().BeOfType<ViewResult>()
              .Which.Model.Should().BeOfType<SettingTypeViewModel>();

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Once);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        /// <summary>
        /// Edit action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_EditActionMode_ResultExpected()
        {
            // Arrange
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();
            var expectedViewModel = GetSettingTypeViewModel(ActionMode.Edit);

            // Act
            var actual = await controller.Action(_settingTypeId, false, ActionMode.Edit);

            // Assert
            actual
               .Should().BeOfType<ViewResult>()
               .Which.Model.Should().BeOfType<SettingTypeViewModel>();

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Once);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        /// <summary>
        /// Invalid SettingType identifier gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_Invalid_SettingTypeId_ResultExpected()
        {
            // Arrange
            var controller = GetSettingTypeController();
            _mockSecurityService.Setup(x => x.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new User());

            // Act
            var actual = await controller.Action(_settingTypeId, false, ActionMode.Delete);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingTypesHome);

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Once);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        /// <summary>
        /// Invalid model gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_InvalidModel_ResultExpected()
        {
            // Arrange
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();
            var postedModel = GetSettingTypeViewModel(ActionMode.Edit);
            controller.ModelState.AddModelError("key", "error message");

            // Act
            var actual = await controller.Action(postedModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<SettingTypeViewModel>();

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        /// <summary>
        /// Valid model gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_ValidModel_ResultExpected()
        {
            // Arrange
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();
            var postedModel = GetSettingTypeViewModel(ActionMode.Edit);

            // Act
            var actual = await controller.Action(postedModel);

            // Assert
            controller.ModelState.IsValid.Should().BeTrue();

            actual
            .Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<SettingTypeViewModel>();

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        /// <summary>
        /// Are you sure action gets the result expected.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void AreYouSure_ResultExpected()
        {
            // Arrange
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();
            var viewModel = new SettingTypeViewModel();
            var expectedViewModel = new SettingTypeViewModel
            {
                IsAreYouSurePage = true
            };

            // Act
            var actual = controller.AreYouSure(viewModel);

            // Assert
            actual.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<SettingTypeViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        /// <summary>
        /// Saves the changes and adds model gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Add_ResultExpected()
        {
            // Arrange
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();
            var viewModel = GetSettingTypeViewModel(ActionMode.Add);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
            .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingTypeConfirmation);

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Once);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        /// <summary>
        /// Saves the changes for the edit action gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Edit_ResultExpected()
        {
            // Arrange
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();
            var viewModel = GetSettingTypeViewModel(ActionMode.Edit);


            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
            .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingTypeConfirmation);

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Once);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Once);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        /// <summary>
        /// Saves the changes on delete and gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Delete_ResultExpected()
        {
            // Arrange
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();
            var viewModel = GetSettingTypeViewModel(ActionMode.Delete);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
            .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingTypeConfirmation);

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Once);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Once);
        }

        /// <summary>
        /// Confirmation on failure gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/>Representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Failure_ResultExpected()
        {
            // Arrange
            SetupMockSettingTypeService();
            var controller = GetSettingTypeController();
            var expectedViewModel = new SettingTypeViewModel
            {
                ActionMode = ActionMode.Delete,
                SubmittedAtDisplayDate = string.Empty
            };

            // Act
            var actual = await controller.Confirmation(_settingTypeId, _settingTypeName, ActionMode.Delete, false);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<SettingTypeViewModel>();

            _mockSettingTypesService.Verify(x => x.GetAllSettingTypes(), Times.Never);
            _mockSettingTypesService.Verify(x => x.GetSettingTypeById(It.IsAny<int>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
            _mockSettingTypesService.Verify(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()), Times.Never);
        }

        #endregion


        #region Mock Services

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <returns>IMapper.</returns>
        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new WebAutoMapperProfile())).CreateMapper();
        }


        /// <summary>
        /// Gets the mock SettingType.
        /// </summary>
        /// <returns>SettingType.</returns>
        private static Services.Models.SettingType GetMockSettingType()
        {
            return new Services.Models.SettingType()
            {
                Id = 1,
                SettingName = "Setting Name",
                SettingDescription = "Setting Description",
                ValueDataType = Services.Enums.SettingValueDataType.String,
            };
        }

        /// <summary>
        /// Setup the mock SettingType service.
        /// </summary>
        private void SetupMockSettingTypeService()
        {
            // GetAllSettingTypes
            _mockSettingTypesService.Setup(x => x.GetAllSettingTypes())
                .ReturnsAsync(new List<Services.Models.SettingType>
                {
                    GetMockSettingType()
                });

            // GetSettingTypeById
            _mockSettingTypesService.Setup(x => x.GetSettingTypeById(It.IsAny<int>()))
                .ReturnsAsync(GetMockSettingType);

            // CreateSettingType
            _mockSettingTypesService.Setup(x => x.CreateSettingType(It.IsAny<Services.Models.SettingType>()))
                .ReturnsAsync(new Services.Models.SettingType());

            // UpdateSettingType
            _mockSettingTypesService.Setup(x => x.UpdateSettingType(It.IsAny<Services.Models.SettingType>()))
                .ReturnsAsync(true);

            // DeleteSettingType
            _mockSettingTypesService.Setup(x => x.DeleteSettingType(It.IsAny<Services.Models.SettingType>()))
                .ReturnsAsync(true);
        }

        #endregion Mock Services


        #region Private Methods

        /// <summary>
        /// Gets the SettingType controller.
        /// </summary>
        /// <returns>ViewYourFundingSettingTypeController.</returns>
        private SettingTypeAdminController GetSettingTypeController()
        {
            return new SettingTypeAdminController(
            _mockSettingTypesService.Object,
            _mockAdminSettingsService.Object,
            _mapper,
            _mockSecurityService.Object,
            GetMockConfigService().Object,
            GetLogger().Object);
        }


        /// <summary>
        /// Gets the mock logging service.
        /// </summary>
        /// <returns>ILoggerAdapter.</returns>
        private Mock<ILoggerAdapter<SettingTypeActionBase>> GetLogger()
        {
            var loggerService = new Mock<ILoggerAdapter<SettingTypeActionBase>>(MockBehavior.Strict);

            loggerService.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));

            return loggerService;
        }

        /// <summary>
        /// Gets the mock config service.
        /// </summary>
        /// <returns>ApplicationConfiguration.</returns>
        private Mock<IOptions<ApplicationConfiguration>> GetMockConfigService()
        {
            var mockConfigService = new Mock<IOptions<ApplicationConfiguration>>(MockBehavior.Strict);

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

        /// <summary>
        /// Gets the SettingType view model.
        /// </summary>
        /// <param name="actionMode">The actionmode.</param>
        /// <returns>SettingTypeViewModel.</returns>
        private SettingTypeViewModel GetSettingTypeViewModel(ActionMode actionMode)
        {
            var expectedViewModel = new SettingTypeViewModel
            {
                SettingType = new SettingType
                {
                    SettingName = "TestName",
                    SettingDescription = "TestDescription",
                    ValueDataType = SettingValueDataType.String,
                    SettingValues = new List<SettingValue>()
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

        #endregion Private Methods
    }
}