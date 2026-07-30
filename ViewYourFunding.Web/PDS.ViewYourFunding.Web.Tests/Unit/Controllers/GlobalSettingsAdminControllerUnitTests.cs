using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.DataTypeEdit;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSettingAdmin;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Extensions;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GlobalSetting = PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSetting.GlobalSetting;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class GlobalSettingsAdminControllerUnitTests
    {
        #region Private fields

        private const int BoolTypeId = 1;
        private const int DateTimeTypeId = 6;
        private const int StringTypeId = 4;
        private const int DateTypeId = 2;
        private const int TimeTypeId = 3;
        private const int IntTypeId = 5;

        #endregion


        #region Mock Services

        private readonly Mock<IGlobalSettingService> _mockGlobalSettingService;

        private readonly IMapper _mapper;

        private readonly Mock<IClaimsBasedIdentityService> _mockSecurityService;

        #endregion


        #region Constructor

        public GlobalSettingsAdminControllerUnitTests()
        {
            _mapper = GetMapper();
            _mockGlobalSettingService = new Mock<IGlobalSettingService>(MockBehavior.Strict);
            _mockSecurityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            SetupMockServices();
        }

        #endregion


        #region Action Tests

        [TestMethod, TestCategory("Unit")]
        public async Task Index_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();
            var expectedViewModel = new GlobalSettingListViewModel
            {
                GlobalSettings = GetWebGlobalSettings().ToList(),
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
            var actual = await controller.Index();

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<GlobalSettingListViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.GetAllGlobalSettings(), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Post_StringEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            var model = new StringTypeEdit
            {
                DataTypeId = 1,
                NewValue = nameof(DataTypeBaseEdit.NewValueString)
            };
            var expectedViewModel = new StringTypeEdit
            {
                DataTypeId = 1,
                NewValueString = nameof(DataTypeBaseEdit.NewValueString),
                NewValue = nameof(DataTypeBaseEdit.NewValueString),
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
            var actual = await controller.Edit(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<StringTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.GetAllGlobalSettings(), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Post_ModelStateError_StringEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            var model = new StringTypeEdit
            {
                DataTypeId = 1,
                NewValue = nameof(DataTypeBaseEdit.NewValueString)
            };

            controller.ModelState.AddModelError("string", "Invalid error");

            // Act
            var actual = await controller.Edit(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<StringTypeEdit>()
                .Which.Should().BeEquivalentTo(model);

            _mockGlobalSettingService.Verify(x => x.GetAllGlobalSettings(), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Post_IntTypeEdit_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            var model = new IntTypeEdit
            {
                DataTypeId = 1,
                NewValue = 1,
                SettingEditType = SettingEditType.Int
            };
            var expectedViewModel = new IntTypeEdit
            {
                DataTypeId = 1,
                NewValue = 1,
                NewValueString = "1",
                SettingEditType = SettingEditType.Int,
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
            var actual = await controller.Edit(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<IntTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.GetAllGlobalSettings(), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Post_DateEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            var model = new DateTypeEdit
            {
                DataTypeId = 1,
                Day = "1",
                Month = "1",
                Year = "2020",
                SettingEditType = SettingEditType.Date
            };
            var expectedViewModel = new DateTypeEdit
            {
                DataTypeId = 1,
                NewValueString = "01/01/2020",
                Day = "1",
                Month = "1",
                Year = "2020",
                SettingEditType = SettingEditType.Date,
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
            var actual = await controller.Edit(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<DateTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.GetAllGlobalSettings(), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Post_BoolEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            var model = new BoolTypeEdit
            {
                DataTypeId = 1,
                NewValue = true,
                SettingEditType = SettingEditType.Bool
            };
            var expectedViewModel = new BoolTypeEdit
            {
                DataTypeId = 1,
                NewValue = true,
                NewValueString = "TRUE",
                SettingEditType = SettingEditType.Bool,
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
            var actual = await controller.Edit(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<BoolTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.GetAllGlobalSettings(), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Post_DateTimeEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            var model = new DateTimeTypeEdit
            {
                DataTypeId = 1,
                Day = "1",
                Month = "1",
                Year = "2020",
                Hour = "1",
                Minute = "1",
                SettingEditType = SettingEditType.DateTime
            };
            var expectedViewModel = new DateTimeTypeEdit
            {
                DataTypeId = 1,
                NewValueString = "01/01/2020 01:01",
                Day = "1",
                Month = "1",
                Year = "2020",
                Hour = "1",
                Minute = "1",
                SettingEditType = SettingEditType.DateTime,
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
            var actual = await controller.Edit(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<DateTimeTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.GetAllGlobalSettings(), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Post_TimeEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            var model = new TimeTypeEdit
            {
                DataTypeId = 1,
                Hour = "1",
                Minute = "1",
                SettingEditType = SettingEditType.Time
            };
            var expectedViewModel = new TimeTypeEdit
            {
                DataTypeId = 1,
                NewValueString = "01:01",
                Hour = "1",
                Minute = "1",
                SettingEditType = SettingEditType.Time,
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
            var actual = await controller.Edit(model);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.ViewName.Should().Be("AreYouSure");

            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<TimeTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.GetAllGlobalSettings(), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Get_TimeEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            _mockGlobalSettingService
                .Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync(GetDbGlobalSettings().First(x => x.Id == TimeTypeId));

            var globalSetting = GetWebGlobalSettings().First(x => x.Id == TimeTypeId);

            var expectedViewModel = new TimeTypeEdit
            {
                DataTypeId = TimeTypeId,
                Hour = "12",
                Minute = "00",
                SettingEditType = SettingEditType.Time,
                Description = globalSetting.Description,
                CurrentValue = globalSetting.Value,
                EditTemplateName = nameof(TimeTypeEdit),
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
                IsCurrentPageEditPage = true
            };

            // Act
            var actual = await controller.Edit(TimeTypeId);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<TimeTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Get_StringEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            _mockGlobalSettingService
                .Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync(GetDbGlobalSettings().First(x => x.Id == StringTypeId));

            var globalSetting = GetWebGlobalSettings().First(x => x.Id == StringTypeId);

            var expectedViewModel = new StringTypeEdit
            {
                DataTypeId = StringTypeId,
                NewValue = globalSetting.Value,
                SettingEditType = SettingEditType.String,
                Description = globalSetting.Description,
                CurrentValue = globalSetting.Value,
                EditTemplateName = nameof(StringTypeEdit),
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
                IsCurrentPageEditPage = true
            };

            // Act
            var actual = await controller.Edit(StringTypeId);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<StringTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Get_IntEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            _mockGlobalSettingService
                .Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync(GetDbGlobalSettings().First(x => x.Id == IntTypeId));

            var globalSetting = GetWebGlobalSettings().First(x => x.Id == IntTypeId);

            var expectedViewModel = new IntTypeEdit
            {
                DataTypeId = IntTypeId,
                NewValue = 0,
                SettingEditType = SettingEditType.Int,
                Description = globalSetting.Description,
                CurrentValue = globalSetting.Value,
                EditTemplateName = nameof(IntTypeEdit),
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
                IsCurrentPageEditPage = true
            };

            // Act
            var actual = await controller.Edit(IntTypeId);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<IntTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Get_DateEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            var globalSetting = GetWebGlobalSettings().First(x => x.Id == DateTypeId);

            _mockGlobalSettingService
                .Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync(GetDbGlobalSettings().First(x => x.Id == DateTypeId));
            var expectedViewModel = new DateTypeEdit
            {
                DataTypeId = DateTypeId,
                Day = "12",
                Month = "12",
                Year = "2002",
                SettingEditType = SettingEditType.Date,
                Description = globalSetting.Description,
                CurrentValue = globalSetting.Value,
                EditTemplateName = nameof(DateTypeEdit),
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
                IsCurrentPageEditPage = true
            };

            // Act
            var actual = await controller.Edit(DateTypeId);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<DateTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Get_DateTimeEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            _mockGlobalSettingService
                .Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync(GetDbGlobalSettings().First(x => x.Id == DateTimeTypeId));

            var globalSetting = GetWebGlobalSettings().First(x => x.Id == DateTimeTypeId);
            var expectedViewModel = new DateTimeTypeEdit
            {
                DataTypeId = DateTimeTypeId,
                Hour = "12",
                Minute = "45",
                Day = "12",
                Month = "12",
                Year = "2008",
                SettingEditType = SettingEditType.DateTime,
                Description = globalSetting.Description,
                CurrentValue = globalSetting.Value,
                EditTemplateName = nameof(DateTimeTypeEdit),
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
                IsCurrentPageEditPage = true
            };

            // Act
            var actual = await controller.Edit(DateTimeTypeId);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<DateTimeTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Edit_Get_BoolEditType_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            _mockGlobalSettingService
                .Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync(GetDbGlobalSettings().First(x => x.Id == BoolTypeId));
            var globalSetting = GetWebGlobalSettings().First(x => x.Id == BoolTypeId);

            var expectedViewModel = new BoolTypeEdit
            {
                DataTypeId = BoolTypeId,
                NewValue = Convert.ToBoolean(globalSetting.Value),
                SettingEditType = SettingEditType.Bool,
                Description = globalSetting.Description,
                CurrentValue = globalSetting.Value,
                EditTemplateName = nameof(BoolTypeEdit),
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
                IsCurrentPageEditPage = true
            };

            // Act
            var actual = await controller.Edit(BoolTypeId);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<BoolTypeEdit>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Success_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();
            var viewModel = new DataTypeBaseEdit
            {
                DataTypeId = 1,
                NewValueString = nameof(DataTypeBaseEdit.NewValueString)
            };
            _mockGlobalSettingService.Setup(x => x.UpdateAsync(It.IsAny<Services.Models.GlobalSetting>())).ReturnsAsync(true);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminGeneralSettingConfirmation);

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "changesSaved", true
                },
                {
                    "globalSettingId", 1
                }
            };

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);

            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
            _mockGlobalSettingService.Verify(x => x.UpdateAsync(It.IsAny<Services.Models.GlobalSetting>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task SaveChanges_Failure_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();
            var viewModel = new DataTypeBaseEdit
            {
                DataTypeId = 1,
                NewValueString = nameof(DataTypeBaseEdit.NewValueString)
            };
            _mockGlobalSettingService.Setup(x => x.UpdateAsync(It.IsAny<Services.Models.GlobalSetting>())).ReturnsAsync(false);

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminGeneralSettingConfirmation);

            var resultDictionary = new Dictionary<string, object>
            {
                {
                    "changesSaved", false
                },
                {
                    "globalSettingId", 1
                }
            };

            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteValues.Should().BeEquivalentTo(resultDictionary);
            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
            _mockGlobalSettingService.Verify(x => x.UpdateAsync(It.IsAny<Services.Models.GlobalSetting>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Success_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();
            var expectedViewModel = new GlobalSettingConfirmationViewModel
            {
                ChangesSaved = true,
                SubmittedAtDisplayDate = GetWebGlobalSettings().First(x => x.Id == 1).UpdatedAt.ToGmtStandardTime().ToDateTimeDisplayWithAt(),
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
                GlobalSetting = GetWebGlobalSettings().First(x => x.Id == 1)
            };

            // Act
            var actual = await controller.Confirmation(1, true);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<GlobalSettingConfirmationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Failure_OnSave_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();
            var expectedViewModel = new GlobalSettingConfirmationViewModel
            {
                SubmittedAtDisplayDate = string.Empty,
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
                GlobalSetting = GetWebGlobalSettings().First(x => x.Id == 1)
            };

            // Act
            var actual = await controller.Confirmation(1, false);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<GlobalSettingConfirmationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_Failure_Invalid_GlobalSettingId_ResultExpected()
        {
            // Arrange
            var controller = GetGlobalSettingAdminController();

            _mockGlobalSettingService
                .Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync(new Services.Models.GlobalSetting());

            // Act
            var actual = await controller.Confirmation(10, false);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminGeneralSettingHome);

            _mockGlobalSettingService.Verify(x => x.Get(It.IsAny<int>()), Times.Once);
        }
        #endregion


        #region Private Data helpers

        private IEnumerable<GlobalSetting> GetWebGlobalSettings()
        {
            return _mapper.Map<IEnumerable<GlobalSetting>>(GetDbGlobalSettings());
        }

        private IEnumerable<Services.Models.GlobalSetting> GetDbGlobalSettings()
        {
            yield return new Services.Models.GlobalSetting
            {
                Id = BoolTypeId,
                Type = 1,
                Description = "Is View Your Funding External area available",
                EditType = Services.Models.GlobalSetting.SettingEditType.Bool,
                ReadOnly = false,
                Value = "TRUE",
                CreatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                UpdatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };

            yield return new Services.Models.GlobalSetting
            {
                Id = DateTypeId,
                Type = 2,
                Description = "Demo date",
                EditType = Services.Models.GlobalSetting.SettingEditType.Date,
                ReadOnly = false,
                Value = "12/12/2002",
                CreatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                UpdatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };

            yield return new Services.Models.GlobalSetting
            {
                Id = TimeTypeId,
                Type = 3,
                Description = "Demo time",
                EditType = Services.Models.GlobalSetting.SettingEditType.Time,
                ReadOnly = false,
                Value = "12:00",
                CreatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                UpdatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };

            yield return new Services.Models.GlobalSetting
            {
                Id = StringTypeId,
                Type = 4,
                Description = "Demo string",
                EditType = Services.Models.GlobalSetting.SettingEditType.String,
                ReadOnly = false,
                Value = nameof(GlobalSetting.Value),
                CreatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                UpdatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };

            yield return new Services.Models.GlobalSetting
            {
                Id = IntTypeId,
                Type = 5,
                Description = "Demo int",
                EditType = Services.Models.GlobalSetting.SettingEditType.Int,
                ReadOnly = false,
                Value = "12",
                CreatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                UpdatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };

            yield return new Services.Models.GlobalSetting
            {
                Id = DateTimeTypeId,
                Type = 6,
                Description = "Demo Date Time",
                EditType = Services.Models.GlobalSetting.SettingEditType.DateTime,
                ReadOnly = false,
                Value = "12/12/2008 12:45",
                CreatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                UpdatedAt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
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
            LastName = nameof(User.LastName),
        };

        #endregion


        #region Private helpers

        private static IMapper GetMapper()
        {
            var config = new TypeAdapterConfig();
            config.ConfigureWebMappings();
            return new Mapper(config);
        }

        private GlobalSettingAdminController GetGlobalSettingAdminController()
        {
            var controller = new GlobalSettingAdminController(
                _mockGlobalSettingService.Object,
                _mapper,
                _mockSecurityService.Object,
                new MemoryCacheService(null, 0),
                GetMockConfigService().Object);

            return controller;
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

        private void SetupMockServices()
        {
            _mockSecurityService
                .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(LoggedInUser);

            _mockGlobalSettingService
                .Setup(s => s.GetAllGlobalSettings())
                .ReturnsAsync(GetDbGlobalSettings().ToList);

            _mockGlobalSettingService
                .Setup(s => s.Get(It.IsAny<int>()))
                .ReturnsAsync(GetDbGlobalSettings().First());
        }

        #endregion
    }
}