using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Common.Identity.Models;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using NextPayment = PDS.ViewYourFunding.Services.Models.NextPayment;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class NextPaymentControllerUnitTests
    {
        #region Private Fields

        private readonly Mock<IAdminSettingsService> _mockAdminSettingsService;
        private readonly IMapper _mapper;
        private readonly Mock<IClaimsBasedIdentityService> _mockSecurityService;
        private readonly Mock<INextPaymentService> _nextPaymentService;
        private readonly Mock<INextPaymentTypeService> _nextPaymentTypeService;
        private readonly string _fundingStreamName = null;

        private int _fundingStreamId, _nextPaymentId;

        #endregion


        #region Constructor

        public NextPaymentControllerUnitTests()
        {
            _mapper = GetMapper();
            _mockAdminSettingsService = new Mock<IAdminSettingsService>();
            _mockSecurityService = new Mock<IClaimsBasedIdentityService>();
            _nextPaymentTypeService = new Mock<INextPaymentTypeService>();
            _nextPaymentService = new Mock<INextPaymentService>();
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
            var controller = GetNextPaymentController();

            //Act
            var actual = await controller.Index(_fundingStreamId);

            //Assert
            actual.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<NextPaymentsIndexViewModel>();

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(It.IsAny<FetchData[]>()), Times.Never);
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
            _mockAdminSettingsService.Setup(x => x.GetAllFundingStreams(It.IsAny<FetchData[]>()))
              .ReturnsAsync(new List<Services.Models.FundingStream>
              {
                    new Services.Models.FundingStream()
              });
            var controller = GetNextPaymentController();

            //Act
            var actual = await controller.Index(_fundingStreamId);

            //Assert
            actual
             .Should().BeOfType<RedirectToRouteResult>()
           .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsFundingStream);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
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
            var newItem = new SelectListItem { Text = " - ", Value = "0" };
            var newList = new List<SelectListItem> { newItem };

            var controller = GetNextPaymentController();

            // Act
            var actual = await controller.Action(_nextPaymentId, _fundingStreamId, ActionMode.Add);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NextPaymentViewModel>();

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _nextPaymentTypeService.Verify(x => x.GetAllNextPaymentTypes(), Times.Once);
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
            var controller = GetNextPaymentController();

            // Act
            var actual = await controller.Action(_nextPaymentId, _fundingStreamId, ActionMode.Delete);

            // Assert
            actual
               .Should().BeOfType<ViewResult>()
               .Which.Model.Should().BeOfType<NextPaymentViewModel>();

            _nextPaymentTypeService.Verify(x => x.GetAllNextPaymentTypes(), Times.Once);
            _nextPaymentService.Verify(x => x.GetNextPayments(It.IsAny<int>()), Times.Once);
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
            var controller = GetNextPaymentController();

            // Act
            var actual = await controller.Action(_nextPaymentId, _fundingStreamId, ActionMode.Edit);

            // Assert
            actual
              .Should().BeOfType<ViewResult>()
              .Which.Model.Should().BeOfType<NextPaymentViewModel>();

            _nextPaymentTypeService.Verify(x => x.GetAllNextPaymentTypes(), Times.Once);
            _nextPaymentService.Verify(x => x.GetNextPayments(It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Invalid next payment id gets the result expected.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod, TestCategory("Unit")]
        public async Task Action_Invalid_nextPaymentId_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();

            _nextPaymentService.Setup(x => x.GetNextPayments(It.IsAny<int>()))
                .ReturnsAsync(new List<ViewYourFunding.Services.Models.NextPayment>());
            _mockSecurityService.Setup(x => x.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new Pds.Core.Common.Identity.Models.User());
            _nextPaymentId = 0;
            _fundingStreamId = 1;
            var controller = GetNextPaymentController();

            // Act
            var actual = await controller.Action(_nextPaymentId, _fundingStreamId, ActionMode.Delete);

            // Assert
            actual
                 .Should().BeOfType<RedirectToRouteResult>()
                  .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsFundingStream);

            _nextPaymentTypeService.Verify(x => x.GetAllNextPaymentTypes(), Times.Never);
            _nextPaymentService.Verify(x => x.GetNextPayments(It.IsAny<int>()), Times.Once);
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
            _nextPaymentId = 1;
            var controller = GetNextPaymentController();
            _mockSecurityService.Setup(x => x.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new Pds.Core.Common.Identity.Models.User());

            // Act
            var actual = await controller.Action(_nextPaymentId, _fundingStreamId, ActionMode.Delete);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminSettingsHome);

            _nextPaymentTypeService.Verify(x => x.GetAllNextPaymentTypes(), Times.Never);
            _nextPaymentService.Verify(x => x.GetNextPayments(It.IsAny<int>()), Times.Once);
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
            var controller = GetNextPaymentController();
            var model = new NextPaymentViewModel()
            {
                NextPayment = new PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment.NextPayment()
                {
                    NextPaymentTypeCode = "test",
                    NextPaymentTypeDescription = "test"
                },
                ActionMode = ActionMode.Edit
            };

            controller.ModelState.AddModelError("key", "error message");

            // Act
            var actual = await controller.Action(model);

            // Assert
            actual
               .Should().BeOfType<ViewResult>()
               .Which.Model.Should().BeOfType<NextPaymentViewModel>();

            _nextPaymentTypeService.Verify(x => x.GetAllNextPaymentTypes(), Times.Once);
            _nextPaymentService.Verify(x => x.GetNextPayments(It.IsAny<int>()), Times.Never);
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
            var controller = GetNextPaymentController();
            var model = new NextPaymentViewModel()
            {
                NextPayment = new PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment.NextPayment()
                {
                    NextPaymentTypeCode = "test",
                    NextPaymentTypeDescription = "test"
                },
                ActionMode = ActionMode.Edit
            };

            // Act
            var actual = await controller.Action(model);

            // Assert
            actual
            .Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<NextPaymentViewModel>();

            _nextPaymentTypeService.Verify(x => x.GetAllNextPaymentTypes(), Times.Once);
            _nextPaymentService.Verify(x => x.GetNextPayments(It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Are you sure action gets the result expected.
        /// </summary>
        [TestMethod, TestCategory("Unit")]
        public void AreYouSure_ResultExpected()
        {
            // Arrange
            SetupMockSettingsServices();
            var controller = GetNextPaymentController();
            var viewModel = new NextPaymentViewModel();
            var expectedViewModel = new NextPaymentViewModel
            {
                IsAreYouSurePage = true
            };

            // Act
            var actual = controller.AreYouSure(viewModel);

            // Assert
            actual.Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<NextPaymentViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _nextPaymentTypeService.Verify(x => x.GetAllNextPaymentTypes(), Times.Never);
            _nextPaymentService.Verify(x => x.GetNextPayments(It.IsAny<int>()), Times.Never);
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
            var controller = GetNextPaymentController();
            var viewModel = new NextPaymentViewModel()
            {
                NextPayment = new PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment.NextPayment()
                {
                    NextPaymentTypeCode = "test",
                    NextPaymentTypeDescription = "test"
                },
                ActionMode = ActionMode.Add
            };

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
          .Should().BeOfType<RedirectToRouteResult>()
          .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminNextPaymentConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _nextPaymentService.Verify(x => x.CreateNextPayment(It.IsAny<Services.Models.NextPayment>()), Times.Once);
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
            var controller = GetNextPaymentController();
            var viewModel = new NextPaymentViewModel()
            {
                NextPayment = new PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment.NextPayment()
                {
                    NextPaymentTypeCode = "test",
                    NextPaymentTypeDescription = "test"
                },
                ActionMode = ActionMode.Edit
            };

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
            .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminNextPaymentConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _nextPaymentService.Verify(x => x.GetNextPayments(It.IsAny<int>()), Times.Once);
            _nextPaymentService.Verify(x => x.UpdateNextPayment(It.IsAny<Services.Models.NextPayment>()), Times.Once);
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
            var controller = GetNextPaymentController();
            var viewModel = new NextPaymentViewModel()
            {
                NextPayment = new PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment.NextPayment()
                {
                    NextPaymentTypeCode = "test",
                    NextPaymentTypeDescription = "test"
                },
                ActionMode = ActionMode.Delete
            };

            // Act
            var actual = await controller.SaveChanges(viewModel);

            // Assert
            actual
            .Should().BeOfType<RedirectToRouteResult>()
            .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminNextPaymentConfirmation);

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(), Times.Never);
            _nextPaymentService.Verify(x => x.GetNextPayments(It.IsAny<int>()), Times.Once);
            _nextPaymentService.Verify(x => x.DeleteNextPayment(It.IsAny<Services.Models.NextPayment>()), Times.Once);
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
            var controller = GetNextPaymentController();

            // Act
            var actual = await controller.Confirmation(_nextPaymentId, _fundingStreamId, _fundingStreamName, ActionMode.Delete, false);

            // Assert
            actual
              .Should().BeOfType<ViewResult>()
              .Which.Model.Should().BeOfType<NextPaymentViewModel>();

            _mockAdminSettingsService.Verify(x => x.GetAllFundingStreams(It.IsAny<FetchData[]>()), Times.Never);
            _nextPaymentTypeService.Verify(x => x.GetNextPaymentTypes(It.IsAny<int>()), Times.Never);
        }

        #endregion

        #region Private Methods

        private static Services.Models.FundingStream GetMockFundingStream()
        {
            return new Services.Models.FundingStream
            {
                NextPayments = new List<NextPayment>
                {
                    new NextPayment
                    {
                        FundingPeriodCode = nameof(NextPayment.FundingPeriodCode)
                    }
                },
                FundingStreamName = "test"
            };
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new WebAutoMapperProfile())).CreateMapper();
        }

        /// <summary>
        /// Gets the view your funding next payment controller.
        /// </summary>
        /// <returns>ViewYourFundingSettingsController.</returns>
        private NextPaymentController GetNextPaymentController()
        {
            return new NextPaymentController(
             _mockAdminSettingsService.Object,
             _nextPaymentService.Object,
             _nextPaymentTypeService.Object,
             _mapper,
             _mockSecurityService.Object,
             GetMockConfigService().Object,
             GetLogger().Object);
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

        private Mock<ILoggerAdapter<NextPaymentActionBase>> GetLogger()
        {
            var loggerService = new Mock<ILoggerAdapter<NextPaymentActionBase>>();

            loggerService.Setup(x => x.LogError(It.IsAny<Exception>(), It.IsAny<string>()));

            return loggerService;
        }

        #region Mock Services
        private void SetupMockSettingsServices()
        {
            var mockFundingStream = GetMockFundingStream();

            _mockAdminSettingsService.Setup(x => x.GetSettingById(It.IsAny<int>(), It.IsAny<int>()))
             .ReturnsAsync(new Services.Models.SettingValue());

            _mockAdminSettingsService.Setup(x => x.GetAllFundingStreams(It.IsAny<FetchData[]>()))
               .ReturnsAsync(new List<Services.Models.FundingStream>
               {
                   mockFundingStream
               });

            _mockAdminSettingsService
                .Setup(s => s.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(mockFundingStream)
                .Verifiable();

            _mockAdminSettingsService
                .Setup(s => s.GetNextPayments(It.IsAny<int>()))
                .ReturnsAsync(
                new List<Services.Models.NextPayment>()
                    {
                        new Services.Models.NextPayment()
                        {
                            NextPaymentDate = new DateTime(1, 1, 1),
                            NextPaymentType = new Services.Models.NextPaymentType()
                            {
                                TypeCode = "test", Description = "test"
                            }
                        }
                    });

            _mockSecurityService
               .Setup(s => s.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
               .ReturnsAsync(LoggedInUser);

            _nextPaymentTypeService.Setup(s => s.GetAllNextPaymentTypes()).ReturnsAsync(new List<Services.Models.NextPaymentType>()
            {
                new Services.Models.NextPaymentType()
                    {
                        TypeCode = "test",
                        Description = "test"
                    }
            });

            _nextPaymentTypeService.Setup(s => s.GetNextPaymentTypes(It.IsAny<int>())).ReturnsAsync(new List<Services.Models.NextPaymentType>() { });

            _nextPaymentService.Setup(x => x.GetNextPayments(It.IsAny<int>())).ReturnsAsync(new List<Services.Models.NextPayment>()
            {
                new Services.Models.NextPayment()
                {
                    NextPaymentDate = new DateTime(1, 1, 1),
                    NextPaymentType = new Services.Models.NextPaymentType()
                    {
                        TypeCode = "test", Description = "test"
                    },
                    FundingStream = new Services.Models.FundingStream()
                }
            });

            _nextPaymentService.Setup(x => x.CreateNextPayment(It.IsAny<Services.Models.NextPayment>())).ReturnsAsync(new Services.Models.NextPayment());

            _nextPaymentService.Setup(x => x.UpdateNextPayment(It.IsAny<Services.Models.NextPayment>())).ReturnsAsync(true);

            _nextPaymentService.Setup(x => x.DeleteNextPayment(It.IsAny<Services.Models.NextPayment>())).ReturnsAsync(true);
        }

        #endregion


        #endregion
    }
}
