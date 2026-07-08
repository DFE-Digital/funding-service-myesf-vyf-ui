using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStreamSetting;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using ViewYourFunding.Services.Models;
using AdminEnums = PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using Service = PDS.ViewYourFunding.Services.Models;
using SettingValueDataType = PDS.ViewYourFunding.Services.Enums.SettingValueDataType;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers.Admin
{
    /// <summary>
    /// The Funding stream setting controller tests.
    /// </summary>
    [TestClass]
    public class FundingStreamSettingControllerUnitTests
    {
        #region Private Fields

        private readonly Mock<IAdminSettingsService> _mockAdminSettingsService;
        private readonly IMapper _mapper;
        private readonly Mock<IClaimsBasedIdentityService> _mockSecurityService;
        private readonly Mock<IAdminFundingStreamSettingService> _mockFundingStreamSettingService;
        private readonly Mock<ISettingTypeService> _mockSettingTypeService;
        private readonly Mock<ILayoutManagementService> _mockLayoutManagementService;
        private readonly Mock<IPublicationSpreadsheetMetaDataService> _mockPublicationSpreadsheetMetaDataService;
        private int _fundingStreamId, _settingValueId;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamSettingControllerUnitTests"/> class.
        /// </summary>
        public FundingStreamSettingControllerUnitTests()
        {
            _mapper = GetMapper();
            _mockAdminSettingsService = new Mock<IAdminSettingsService>(MockBehavior.Strict);
            _mockSecurityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            _mockFundingStreamSettingService = new Mock<IAdminFundingStreamSettingService>(MockBehavior.Strict);
            _mockSettingTypeService = new Mock<ISettingTypeService>(MockBehavior.Strict);
            _mockPublicationSpreadsheetMetaDataService = new Mock<IPublicationSpreadsheetMetaDataService>(MockBehavior.Strict);
            _mockLayoutManagementService = new Mock<ILayoutManagementService>(MockBehavior.Strict);
            SetupDefaultMockServicesBehaviour();
        }

        #endregion


        #region Action Tests

        [TestMethod, TestCategory("Unit")]
        public async Task Index_ResultExpected()
        {
            // Arrange
            var controller = GetFundingStreamSettingController();
            _mockAdminSettingsService.Setup(service => service.GetAllFundingStreams(It.IsAny<FetchData[]>()))
               .ReturnsAsync(new List<Service.FundingStream>());

            // Act
            var actual = await controller.Index();

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamsListViewModel>();

            _mockAdminSettingsService.Verify(service => service.GetAllFundingStreams(It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task FundingStream_ReturnsExpectedView()
        {
            // Arrange
            _mockAdminSettingsService.Setup(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(new Service.FundingStream()
                {
                    Id = 1,
                    Publications = new List<Publication>(),
                    SettingValues = new List<SettingValue>()
                });

            var controller = GetFundingStreamSettingController();

            // Act
            var actual = await controller.FundingStream(_fundingStreamId);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamSettingsViewModel>();

            _mockSettingTypeService.Verify(mock => mock.GetAvailableSettingTypes(It.IsAny<IEnumerable<int>>()), Times.Once);
            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
            _mockAdminSettingsService.Verify(service => service.GetAllFundingStreams(), Times.Never);
            _mockPublicationSpreadsheetMetaDataService.Verify(
                service => service.GetPublicationSpreadsheetMetaDataAsync(It.IsAny<Publication>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task FundingStream_RedirectsTo_AdminSettingsHomePage()
        {
            // Arrange
            _mockAdminSettingsService.Setup(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(new Service.FundingStream());
            var controller = GetFundingStreamSettingController();

            // Act
            var actual = await controller.FundingStream(_fundingStreamId);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsHome);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
            _mockSettingTypeService.Verify(mock => mock.GetAvailableSettingTypes(It.IsAny<IEnumerable<int>>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ActionEdit_ReturnsExpectedResult()
        {
            // Arrange
            _fundingStreamId = _settingValueId = 1;
            var controller = GetFundingStreamSettingController();
            var expectedViewModel = GetExpectedFundingStreamSettingActionViewModel(FundingStreamSettingAction.Edit);

            // Act
            var actual = await controller.Action(_fundingStreamId, _settingValueId, FundingStreamSettingAction.Edit);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamSettingActionViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockFundingStreamSettingService.Verify(service => service.GetFirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ActionEdit_InvalidFundingStream_ReturnsExpectedResult()
        {
            // Arrange
            _mockFundingStreamSettingService.Setup(service => service.GetFirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((SettingValue)null);
            var controller = GetFundingStreamSettingController();

            // Act
            var actual = await controller.Action(_fundingStreamId, _settingValueId, FundingStreamSettingAction.Edit);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsFundingStream);

            _mockFundingStreamSettingService.Verify(service => service.GetFirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ActionDelete_ReturnsExpectedResult()
        {
            // Arrange
            _fundingStreamId = _settingValueId = 1;
            var controller = GetFundingStreamSettingController();
            var expectedViewModel = GetExpectedFundingStreamSettingAreYouSureViewModel(FundingStreamSettingAction.Delete);

            // Act
            var actual = await controller.Action(_fundingStreamId, _settingValueId, FundingStreamSettingAction.Delete);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamSettingAreYouSureViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockFundingStreamSettingService.Verify(service => service.GetFirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SetSettingType_ReturnsExpectedResult()
        {
            // Arrange
            _fundingStreamId = _settingValueId = 1;

            _mockAdminSettingsService.Setup(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(new Service.FundingStream()
                {
                    Id = 1,
                    Publications = new List<Publication>(),
                    SettingValues = new List<SettingValue>()
                });

            var controller = GetFundingStreamSettingController();
            var expectedViewModel = GetExpectedSetSettingTypeViewModel(_fundingStreamId);

            // Act
            var actual = await controller.SetSettingType(_fundingStreamId);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<SetSettingTypeViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SetSettingType_Invalid_FundingStreamId_ReturnsExpectedResult()
        {
            // Arrange
            _fundingStreamId = _settingValueId = 1;
            _mockAdminSettingsService.Setup(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>())).ReturnsAsync(new Service.FundingStream());
            var controller = GetFundingStreamSettingController();

            // Act
            var actual = await controller.SetSettingType(_fundingStreamId);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsHome);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ActionEdit_Post_Invalid_ReturnsExpectedResult()
        {
            // Arrange
            _mockAdminSettingsService.Setup(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync((Service.FundingStream)null);
            _fundingStreamId = _settingValueId = 1;
            var controller = GetFundingStreamSettingController();
            controller.ModelState.AddModelError("Test", "test");
            var model = GetPostFundingStreamSettingActionViewModel(FundingStreamSettingAction.Edit);
            model.SettingValueDataType = AdminEnums.SettingValueDataType.Int;

            // Act
            var actual = await controller.Action(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamSettingActionViewModel>()
                .Which.Should().BeEquivalentTo(model);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ActionEdit_Post_ReturnsExpectedResult()
        {
            // Arrange
            _fundingStreamId = _settingValueId = 1;
            var controller = GetFundingStreamSettingController();
            var model = GetPostFundingStreamSettingActionViewModel(FundingStreamSettingAction.Edit);

            var expectedViewModel = GetExpectedFundingStreamSettingAreYouSureViewModel(FundingStreamSettingAction.Edit);

            // Act
            var actual = await controller.Action(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamSettingAreYouSureViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ActionAdd_NationalLayout_Post_ReturnsExpectedResult()
        {
            // Arrange
            _fundingStreamId = _settingValueId = 1;
            var controller = GetFundingStreamSettingController();
            var model = GetPostFundingStreamSettingActionViewModel(FundingStreamSettingAction.Add);
            model.SettingValueDataType = AdminEnums.SettingValueDataType.NationalLayout;
            model.NewNationalLayoutIdValue = "Value";

            var expectedViewModel = GetExpectedFundingStreamSettingAreYouSureViewModel(FundingStreamSettingAction.Add);
            expectedViewModel.IsNationalLayoutSetting = true;
            expectedViewModel.NationalLayoutFriendlyName = "Value";

            // Act
            var actual = await controller.Action(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamSettingAreYouSureViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ActionAdd_Post_ReturnsExpectedResult()
        {
            // Arrange
            _fundingStreamId = _settingValueId = 1;
            var controller = GetFundingStreamSettingController();
            var model = GetPostFundingStreamSettingActionViewModel(FundingStreamSettingAction.Add);

            var expectedViewModel = GetExpectedFundingStreamSettingAreYouSureViewModel(FundingStreamSettingAction.Add);

            // Act
            var actual = await controller.Action(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamSettingAreYouSureViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Update_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetFundingStreamSettingController();
            var model = GetExpectedFundingStreamSettingAreYouSureViewModel(FundingStreamSettingAction.Edit);

            // Act
            var actual = await controller.SaveChanges(model);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminFundingStreamSettingConfirmation);

            var resultDictionary = GetRouteValueResultDictionary();
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            _mockFundingStreamSettingService.Verify(service => service.UpdateAsync(It.IsAny<SettingValue>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Add_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetFundingStreamSettingController();
            var model = GetExpectedFundingStreamSettingAreYouSureViewModel(FundingStreamSettingAction.Add);

            // Act
            var actual = await controller.SaveChanges(model);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminFundingStreamSettingConfirmation);

            var resultDictionary = GetRouteValueResultDictionary(fundingStreamSettingAction: FundingStreamSettingAction.Add);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            _mockFundingStreamSettingService.Verify(service => service.AddAsync(It.IsAny<SettingValue>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Add_Fails_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetFundingStreamSettingController();
            _mockFundingStreamSettingService.Setup(service => service.AddAsync(It.IsAny<SettingValue>())).ReturnsAsync(new SettingValue());
            var model = GetExpectedFundingStreamSettingAreYouSureViewModel(FundingStreamSettingAction.Add);

            // Act
            var actual = await controller.SaveChanges(model);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminFundingStreamSettingConfirmation);

            var resultDictionary = GetRouteValueResultDictionary(false, FundingStreamSettingAction.Add);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            _mockFundingStreamSettingService.Verify(service => service.AddAsync(It.IsAny<SettingValue>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Delete_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetFundingStreamSettingController();
            var model = GetExpectedFundingStreamSettingAreYouSureViewModel(FundingStreamSettingAction.Delete);

            // Act
            var actual = await controller.SaveChanges(model);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminFundingStreamSettingConfirmation);

            var resultDictionary = GetRouteValueResultDictionary(fundingStreamSettingAction: FundingStreamSettingAction.Delete);

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            _mockFundingStreamSettingService.Verify(service => service.DeleteAsync(It.IsAny<SettingValue>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Update_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetFundingStreamSettingController();

            // Act
            var actual = await controller.Confirmation(_fundingStreamId, true, FundingStreamSettingAction.Edit);

            // Assert
            actual
              .Should().BeOfType<ViewResult>()
              .Which.Model.Should().BeOfType<FundingStreamSettingConfirmationViewModel>();

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Add_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetFundingStreamSettingController();
            var expectedViewModel =
                GetExpectedFundingStreamSettingActionViewModel(FundingStreamSettingAction.Add);
            expectedViewModel.FundingStreamName = null;
            expectedViewModel.FundingStreamId = 1;
            expectedViewModel.CurrentValue = null;
            expectedViewModel.SettingValueId = 0;

            _mockAdminSettingsService.Setup(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(new Service.FundingStream()
                {
                    Id = 1,
                    Publications = new List<Publication>(),
                    SettingValues = new List<SettingValue>()
                });

            var postModel = new SetSettingTypeViewModel
            {
                FundingStreamId = _fundingStreamId,
                FundingStreamName = nameof(Service.FundingStream.FundingStreamName),
                SettingTypeId = 1
            };

            // Act
            var actual = await controller.Add(postModel);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamSettingActionViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Add_ReturnsExpectedRedirectResult()
        {
            // Arrange
            _mockAdminSettingsService.Setup(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>())).ReturnsAsync((Service.FundingStream)null);
            var controller = GetFundingStreamSettingController();

            // Act
            var actual = await controller.Add(new SetSettingTypeViewModel());

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsHome);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_ReturnsExpectedRedirectResult()
        {
            // Arrange
            _mockAdminSettingsService.Setup(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>())).ReturnsAsync((Service.FundingStream)null);
            var controller = GetFundingStreamSettingController();

            // Act
            var actual = await controller.Confirmation(1, false, FundingStreamSettingAction.Add);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsHome);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Add_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetFundingStreamSettingController();
            var expectedViewModel =
                GetExpectedFundingStreamSettingConfirmationViewModel(FundingStreamSettingAction.Add);

            // Act
            var actual = await controller.Confirmation(_fundingStreamId, true, FundingStreamSettingAction.Add);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamSettingConfirmationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Delete_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetFundingStreamSettingController();

            var expectedViewModel =
                GetExpectedFundingStreamSettingConfirmationViewModel(FundingStreamSettingAction.Delete);

            // Act
            var actual = await controller.Confirmation(_fundingStreamId, true, FundingStreamSettingAction.Delete);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<FundingStreamSettingConfirmationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Update_Fails_ReturnsExpectedResult()
        {
            // Arrange
            var controller = GetFundingStreamSettingController();

            var expectedViewModel =
                GetExpectedFundingStreamSettingConfirmationViewModel(FundingStreamSettingAction.Edit, false);

            // Act
            var actual = await controller.Confirmation(_fundingStreamId, false, FundingStreamSettingAction.Edit);

            // Assert
            actual
              .Should().BeOfType<ViewResult>()
              .Which.Model.Should().BeOfType<FundingStreamSettingConfirmationViewModel>()
              .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockAdminSettingsService.Verify(service => service.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        #endregion


        #region Private Methods

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new WebAutoMapperProfile())).CreateMapper();
        }

        private static Service.FundingStream GetFundingStreamData()
        {
            return new Service.FundingStream
            {
                Publications = new List<Publication>
                {
                    new Publication
                    {
                        FundingStream = new Service.FundingStream
                        {
                            Id = 1, SettingValues = new List<SettingValue>
                            {
                                new SettingValue
                                {
                                    SettingId = 1,
                                    Setting = new SettingType
                                    {
                                        ValueDataType = SettingValueDataType.Int
                                    }
                                }
                            }
                        },
                        PublishedDate = new DateTime(1, 1, 1)
                    }
                },
                SettingValues = new List<SettingValue>
                {
                    new SettingValue
                    {
                        SettingId = 1,
                        Id = 1,
                        Setting = new SettingType
                        {
                            ValueDataType = SettingValueDataType.Int
                        },
                        FundingStream = new Service.FundingStream
                        {
                            FundingStreamName = nameof(Service.FundingStream.FundingStreamName)
                        },
                        FundingStreamId = 1,
                        Value = nameof(SettingValue.Value)
                    }
                },
                FundingStreamName = nameof(Service.FundingStream.FundingStreamName),
                Id = 1
            };
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
            LastName = nameof(User.LastName)
        };

        private FundingStreamSettingActionViewModel GetPostFundingStreamSettingActionViewModel(FundingStreamSettingAction actionMode)
        {
            var model = new FundingStreamSettingActionViewModel
            {
                FundingStreamId = _fundingStreamId,
                FundingStreamName = nameof(Service.FundingStream.FundingStreamName),
                ActionMode = actionMode,
                SettingValueId = 1,
                SettingId = 1,
                CurrentValue = nameof(SettingValue.Value),
                NewStringValue = nameof(SettingValue.Value)
            };
            return model;
        }

        private FundingStreamSettingAreYouSureViewModel GetExpectedFundingStreamSettingAreYouSureViewModel(FundingStreamSettingAction actionMode)
        {
            var expectedViewModel = new FundingStreamSettingAreYouSureViewModel
            {
                FundingStreamId = _fundingStreamId,
                FundingStreamName = nameof(Service.FundingStream.FundingStreamName),
                ActionMode = actionMode,
                NewValue = nameof(SettingValue.Value),
                CurrentValue = nameof(SettingValue.Value),
                SettingValueId = _settingValueId,
                SettingId = 1,
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

        private FundingStreamSettingConfirmationViewModel GetExpectedFundingStreamSettingConfirmationViewModel(FundingStreamSettingAction actionMode, bool success = true)
        {
            var expectedViewModel = new FundingStreamSettingConfirmationViewModel
            {
                FundingStreamId = _fundingStreamId,
                FundingStreamName = nameof(Service.FundingStream.FundingStreamName),
                ActionMode = actionMode,
                ChangesSaved = success,
                SubmittedAtDisplayDate = DateTime.Now.ToDateTimeDisplayWithAt(),
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

        private SetSettingTypeViewModel GetExpectedSetSettingTypeViewModel(int fundingStreamId)
        {
            var expectedViewModel = new SetSettingTypeViewModel
            {
                FundingStreamId = _fundingStreamId,
                SettingTypes = new List<SelectListItem> { new SelectListItem { Value = "0" } },
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

        private Dictionary<string, object> GetRouteValueResultDictionary(bool success = true, FundingStreamSettingAction fundingStreamSettingAction = FundingStreamSettingAction.Edit)
        {
            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "success", success
                },
                {
                    "actionMode", fundingStreamSettingAction
                },
                {
                    "fundingStreamId", _fundingStreamId
                }
            };
            return resultDictionary;
        }

        private FundingStreamSettingActionViewModel GetExpectedFundingStreamSettingActionViewModel(
            FundingStreamSettingAction actionMode)
        {
            var expectedViewModel = new FundingStreamSettingActionViewModel
            {
                FundingStreamId = _fundingStreamId,
                FundingStreamName = nameof(Service.FundingStream.FundingStreamName),
                ActionMode = actionMode,
                SettingValueDataType = AdminEnums.SettingValueDataType.Int,
                CurrentValue = nameof(SettingValue.Value),
                SettingValueId = 1,
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

        private FundingStreamSettingController GetFundingStreamSettingController()
        {
            return new FundingStreamSettingController(
                _mockAdminSettingsService.Object,
                _mockFundingStreamSettingService.Object,
                _mockSettingTypeService.Object,
                _mockLayoutManagementService.Object,
                _mockSecurityService.Object,
                _mapper,
                _mockPublicationSpreadsheetMetaDataService.Object,
                GetMockConfigService().Object,
                GetMockCacheService().Object);
        }

        private Mock<ICacheService> GetMockCacheService()
        {
            var mockCachingService = new Mock<ICacheService>(MockBehavior.Strict);
            mockCachingService.Setup(service => service.ClearCache());

            return mockCachingService;
        }

        private Mock<IOptions<ApplicationConfiguration>> GetMockConfigService()
        {
            var mockConfigService = new Mock<IOptions<ApplicationConfiguration>>();

            mockConfigService.Setup(service => service.Value)
                .Returns(new ApplicationConfiguration
                {
                    ContactUsLink = "http://www.vyf-contactus.com/contactus",
                    FeedbackLink = "http://www.vyf-survey.com/survey",
                    ViewYourFundingApiBaseAddress = nameof(ApplicationConfiguration.ViewYourFundingApiBaseAddress)
                });

            return mockConfigService;
        }

        #endregion


        #region Mock Services

        private void SetupDefaultMockServicesBehaviour()
        {
            var mockFundingStream = GetFundingStreamData();

            _mockAdminSettingsService.Setup(service => service.GetSettingById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new SettingValue());

            _mockFundingStreamSettingService.Setup(service => service.UpdateAsync(It.IsAny<SettingValue>()))
                .ReturnsAsync(true);
            _mockFundingStreamSettingService.Setup(service => service.DeleteAsync(It.IsAny<SettingValue>()))
                .ReturnsAsync(true);
            _mockFundingStreamSettingService.Setup(service => service.AddAsync(It.IsAny<SettingValue>()))
                .ReturnsAsync(GetFundingStreamData().SettingValues.First);

            _mockAdminSettingsService.Setup(service => service.GetAllFundingStreams())
                .ReturnsAsync(new List<Service.FundingStream>
                {
                    mockFundingStream
                });

            _mockAdminSettingsService
                .Setup(s => s.GetFundingStreamById(It.IsAny<int>()))
                .ReturnsAsync(mockFundingStream)
                .Verifiable();

            _mockSecurityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(LoggedInUser);

            _mockPublicationSpreadsheetMetaDataService.Setup(service =>
                    service.GetPublicationSpreadsheetMetaDataAsync(It.IsAny<Publication>()))
                .ReturnsAsync(new PublicationSpreadsheetMetaData());

            _mockFundingStreamSettingService
                .Setup(service => service.GetFirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(GetFundingStreamData().SettingValues.First());

            _mockSettingTypeService.Setup(service => service.GetAvailableSettingTypes(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(
                    GetFundingStreamData().SettingValues.Select(settingValue => settingValue.Setting).ToList());

            _mockSettingTypeService.Setup(service => service.GetSettingTypeById(It.IsAny<int>()))
                .ReturnsAsync(
                    GetFundingStreamData().SettingValues.First().Setting);

            _mockLayoutManagementService.Setup(mock => mock.GetLayoutAsync(It.IsAny<string>())).ReturnsAsync(new LayoutModel
            {
                LayoutName = "Value"
            });

            _mockLayoutManagementService.Setup(mock => mock.GetAllLayoutsAsync(It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()))
                .ReturnsAsync(new List<LayoutModel>());
        }

        #endregion
    }
}