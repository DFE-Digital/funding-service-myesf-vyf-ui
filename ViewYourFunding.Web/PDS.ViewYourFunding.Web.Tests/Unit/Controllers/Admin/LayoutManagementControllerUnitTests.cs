using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Web.Models;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Controllers;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Config;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Pagination = PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement.Pagination;
using PublicationStatus = PDS.ViewYourFunding.Services.Enums.PublicationStatus;
using Service = PDS.ViewYourFunding.Services.Models;
using SettingValueDataType = PDS.ViewYourFunding.Services.Enums.SettingValueDataType;

namespace PDS.ViewYourFunding.Web.Tests.Unit.Controllers.Admin
{
    [TestClass]
    public class LayoutManagementControllerUnitTests
    {
        #region Mock Services

        private readonly Mock<IOptions<ApplicationConfiguration>> _mockConfigurationService;
        private readonly Mock<IClaimsBasedIdentityService> _mockSecurityService;
        private readonly Mock<IAdminSettingsService> _mockAdminSettingsService;
        private readonly Mock<IAdminPublicationService> _mockAdminPublicationService;
        private readonly IMapper _mapper;
        private readonly Mock<ILayoutManagementService> _mockLayoutManagementService;

        #endregion


        #region Constructor

        public LayoutManagementControllerUnitTests()
        {
            _mockConfigurationService = new Mock<IOptions<ApplicationConfiguration>>(MockBehavior.Strict);
            _mockSecurityService = new Mock<IClaimsBasedIdentityService>(MockBehavior.Strict);
            _mockAdminSettingsService = new Mock<IAdminSettingsService>(MockBehavior.Strict);
            _mockLayoutManagementService = new Mock<ILayoutManagementService>(MockBehavior.Strict);
            _mockAdminPublicationService = new Mock<IAdminPublicationService>(MockBehavior.Strict);
            _mapper = GetMapper();
        }

        #endregion


        #region Action Tests

        [TestMethod, TestCategory("Unit")]
        public async Task Index_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var model = new LayoutFilter
            {
                PageNumber = 1,
                FundingStreams = new List<Filter>
                {
                    new Filter
                    {
                        Id = 1,
                        Name = "PE & Sports"
                    }
                },
                FundingViewTypes = new List<Filter>
                {
                    new Filter
                    {
                        Id = 1,
                        Name = "Provider Summary"
                    }
                },
                FundingViewScopes = new List<Filter>
                {
                    new Filter
                    {
                        Id = 1,
                        Name = "Provider Summary"
                    }
                }
            };

            var expectedViewModel = GetExpectedHomeViewModel();

            _mockAdminSettingsService.Setup(mock => mock.GetAllFundingStreams(It.IsAny<FetchData[]>())).ReturnsAsync(
                new List<FundingStream>()
                {
                    new FundingStream
                    {
                        Id = 1,
                        FundingStreamCode = "PSG",
                        FundingStreamName = "PE & sports",
                        Publications = new List<Publication>()
                    },
                    new FundingStream
                    {
                        Id = 2,
                        FundingStreamCode = "DSG",
                        FundingStreamName = "Dedicated school grant",
                        Publications = new List<Publication>()
                    }
                });


            _mockLayoutManagementService.Setup(mock => mock.GetPaginationResultWithFilters(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>(), It.IsAny<List<string>>(), It.IsAny<List<string>>())).ReturnsAsync(
                new PaginationResult
                {
                    TotalCount = 1,
                    PaginationDetail = new Service.Pagination()
                    {
                        TotalPages = 1,
                        ResultCount = 1,
                        FirstRecordNo = 1,
                        LastRecordNo = 1,
                        PageNumber = 1
                    },
                    LayoutModels = new List<LayoutModel>()
                    {
                        new LayoutModel
                        {
                            LayoutName = nameof(LayoutModel.LayoutName),
                            Id = "xx",
                            FundingStreamId = 1
                        }
                    },
                    FilterOptions = new FilterOptions
                    {
                        FundingStreamsIds = new List<int>() { 1, 2 },
                        FundingViewTypes = new List<string> { "ViewData" },
                        FundingViewScopes = new List<string>() { "ProviderSummary" }
                    }
                });

            _mockAdminSettingsService.Setup(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(new FundingStream());

            // Act
            var actual = await controller.Index(model);

            // Assert
            actual
                 .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LayoutManagementHomePageViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);

            _mockLayoutManagementService.Verify(mock => mock.GetPaginationResultWithFilters(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()), Times.Once);
            _mockAdminSettingsService.Verify(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Never);
            _mockAdminPublicationService.Verify(mock => mock.GetAll(), Times.Never);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Getlayouts_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();

            _mockLayoutManagementService.Setup(mock => mock.GetPaginationResult(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>(), It.IsAny<List<string>>(), It.IsAny<List<string>>())).ReturnsAsync(
                new PaginationResult
                {
                    TotalCount = 1,
                    PaginationDetail = new Service.Pagination()
                    {
                        TotalPages = 1,
                        ResultCount = 1,
                        FirstRecordNo = 1,
                        LastRecordNo = 1,
                        PageNumber = 1
                    },
                    LayoutModels = new List<LayoutModel>()
                    {
                        new LayoutModel
                        {
                            LayoutName = nameof(LayoutModel.LayoutName),
                            Id = "xx",
                            FundingStreamId = 1
                        }
                    }
                });

            _mockAdminSettingsService.Setup(mock => mock.GetAllFundingStreams(It.IsAny<FetchData[]>())).ReturnsAsync(new List<FundingStream>());
            _mockAdminSettingsService.Setup(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(new FundingStream());

            var fundingstreamIds = "1";
            var fundingViewTypeIds = "0";
            var fundingViewScopeIds = "0";

            _mockAdminPublicationService.Setup(mock => mock.GetAll()).ReturnsAsync(new List<Publication>());

            // Act
            var actual = await controller.GetLayouts(1, fundingstreamIds, fundingViewTypeIds, fundingViewScopeIds);

            // Assert
            actual
                 .Should().BeOfType<string>();

            _mockLayoutManagementService.Verify(mock => mock.GetPaginationResult(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()), Times.Once);
            _mockAdminSettingsService.Verify(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Never);
            _mockAdminPublicationService.Verify(mock => mock.GetAll(), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ImportLayout_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var expectedViewModel = GetExpectedLayoutImportViewModel();
            _mockAdminSettingsService.Setup(mock => mock.GetAllFundingStreams()).ReturnsAsync(new List<FundingStream>());

            // Act
            var actual = await controller.ImportLayout();

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LayoutImportViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            _mockAdminSettingsService.Verify(mock => mock.GetAllFundingStreams(), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ImportLayout_Post_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var expectedViewModel = GetExpectedLayoutFileImportViewModel();
            _mockLayoutManagementService.Setup(mock => mock.GetAllLayoutsAsync(It.IsAny<List<Expression<Func<LayoutModel, bool>>>>())).ReturnsAsync(new List<LayoutModel>());
            _mockAdminSettingsService.Setup(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>())).ReturnsAsync(new FundingStream());

            // Act
            var actual = await controller.ImportLayout(new LayoutImportViewModel());

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LayoutFileImportViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            _mockLayoutManagementService.Verify(mock => mock.GetAllLayoutsAsync(It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()), Times.Once);
            _mockAdminSettingsService.Verify(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ImportLayout_AlreadyExists_Post_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var expectedViewModel = GetExpectedLayoutFileImportViewModel();
            expectedViewModel.LayoutName = nameof(LayoutModel.LayoutName);
            expectedViewModel.LayoutAction = LayoutAction.Update;
            expectedViewModel.FundingViewType = FundingViewType.Other;

            _mockLayoutManagementService.Setup(mock => mock.GetAllLayoutsAsync(It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()))
                .ReturnsAsync(new List<LayoutModel>
            {
                new LayoutModel
                {
                    LayoutName = nameof(LayoutModel.LayoutName),
                    FundingViewType = "Other",
                    FundingViewScope = "Unknown"
                }
            });
            _mockAdminSettingsService.Setup(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(new FundingStream());

            // Act
            var actual = await controller.ImportLayout(new LayoutImportViewModel
            {
                LayoutName = nameof(LayoutModel.LayoutName),
                FundingViewType = FundingViewType.Other,
                FundingViewScope = FundingViewScope.Unknown
            });

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LayoutFileImportViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            _mockLayoutManagementService.Verify(mock => mock.GetAllLayoutsAsync(It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()), Times.Once);
            _mockAdminSettingsService.Verify(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ImportLayout_Post_Invalid_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var expectedViewModel = GetExpectedLayoutImportViewModel();
            _mockAdminSettingsService.Setup(mock => mock.GetAllFundingStreams()).ReturnsAsync(new List<FundingStream>());

            // Act
            controller.ModelState.AddModelError("key", "error message");
            var actual = await controller.ImportLayout(new LayoutImportViewModel());

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LayoutImportViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            _mockAdminSettingsService.Verify(mock => mock.GetAllFundingStreams(), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ImportLayoutFileUpload_AddLayout_Invalid_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            _mockLayoutManagementService.Setup(mock => mock.AddLayoutAsync(It.IsAny<LayoutModel>())).ReturnsAsync(Guid.Empty);

            // Act
            var actual = await controller.ImportLayoutFileUpload(new LayoutFileImportViewModel
            {
                FileUpload = new Mock<IFormFile>().Object
            });

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminLayoutImportConfirmation);

            _mockLayoutManagementService.Verify(mock => mock.AddLayoutAsync(It.IsAny<LayoutModel>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ImportLayoutFileUpload_LayoutAdd_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            _mockLayoutManagementService.Setup(mock => mock.AddLayoutAsync(It.IsAny<LayoutModel>())).ReturnsAsync(default(Guid));

            // Act
            var actual = await controller.ImportLayoutFileUpload(new LayoutFileImportViewModel { FileUpload = new Mock<IFormFile>().Object });

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminLayoutImportConfirmation);

            _mockLayoutManagementService.Verify(mock => mock.AddLayoutAsync(It.IsAny<LayoutModel>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task ImportLayoutFileUpload_LayoutUpdate_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            _mockLayoutManagementService.Setup(mock => mock.UpdateLayoutAsync(It.IsAny<LayoutModel>())).ReturnsAsync(true);

            // Act
            var actual = await controller.ImportLayoutFileUpload(new LayoutFileImportViewModel
            {
                FileUpload = new Mock<IFormFile>().Object,
                LayoutAction = LayoutAction.Update
            });

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminLayoutImportConfirmation);

            _mockLayoutManagementService.Verify(mock => mock.UpdateLayoutAsync(It.IsAny<LayoutModel>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_LayoutAdd_Fail_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var expectedViewModel = GetExpectedConfirmationViewModel(false);
            _mockLayoutManagementService.Setup(mock => mock.GetLayoutAsync(It.IsAny<string>())).ReturnsAsync(new LayoutModel());
            _mockAdminSettingsService.Setup(mock => mock.GetAllFundingStreams(It.IsAny<FetchData[]>())).ReturnsAsync(
               new List<FundingStream>()
               {
                    new FundingStream
                    {
                        Id = 1,
                        FundingStreamCode = "PSG",
                        FundingStreamName = "PE & sports"
                    },
                    new FundingStream
                    {
                        Id = 2,
                        FundingStreamCode = "DSG",
                        FundingStreamName = "Dedicated school grant"
                    }
               });

            // Act
            var actual = await controller.Confirmation(Guid.NewGuid(), LayoutAction.Add);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LayoutConfirmationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            _mockLayoutManagementService.Verify(mock => mock.GetLayoutAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_LayoutUpdate_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var expectedViewModel = GetExpectedConfirmationViewModel(layoutAction: LayoutAction.Update);
            _mockLayoutManagementService.Setup(mock => mock.GetLayoutAsync(It.IsAny<string>())).ReturnsAsync(new LayoutModel { Id = nameof(LayoutModel.Id) });
            _mockAdminSettingsService.Setup(mock => mock.GetAllFundingStreams(It.IsAny<FetchData[]>())).ReturnsAsync(
               new List<FundingStream>()
               {
                    new FundingStream
                    {
                        Id = 1,
                        FundingStreamCode = "PSG",
                        FundingStreamName = "PE & sports"
                    },
                    new FundingStream
                    {
                        Id = 2,
                        FundingStreamCode = "DSG",
                        FundingStreamName = "Dedicated school grant"
                    }
               });

            // Act
            var actual = await controller.Confirmation(Guid.NewGuid(), LayoutAction.Update);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LayoutConfirmationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            _mockLayoutManagementService.Verify(mock => mock.GetLayoutAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task Confirmation_LayoutAdd_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var expectedViewModel = GetExpectedConfirmationViewModel();
            _mockLayoutManagementService.Setup(mock => mock.GetLayoutAsync(It.IsAny<string>())).ReturnsAsync(new LayoutModel { Id = nameof(LayoutModel.Id) });
            _mockAdminSettingsService.Setup(mock => mock.GetAllFundingStreams(It.IsAny<FetchData[]>())).ReturnsAsync(
               new List<FundingStream>()
               {
                    new FundingStream
                    {
                        Id = 1,
                        FundingStreamCode = "PSG",
                        FundingStreamName = "PE & sports"
                    },
                    new FundingStream
                    {
                        Id = 2,
                        FundingStreamCode = "DSG",
                        FundingStreamName = "Dedicated school grant"
                    }
               });

            // Act
            var actual = await controller.Confirmation(Guid.NewGuid(), LayoutAction.Add);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LayoutConfirmationViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
            _mockLayoutManagementService.Verify(mock => mock.GetLayoutAsync(It.IsAny<string>()), Times.Once);
        }


        [TestMethod, TestCategory("Unit")]
        [DataRow("gag local authority summary layout", "gag_local_authority_summary_layout.json")]
        [DataRow("dsg provider detailed layout", "dsg_provider_detailed_layout.json")]
        [DataRow("psg summary layout", "psg_summary_layout.json")]
        public async Task DownloadLayoutFile_Success_ResultExpected(string layoutName, string expectedFileName)
        {
            // Arrange
            var controller = GetLayoutManagementController();
            _mockLayoutManagementService.Setup(mock => mock.GetLayoutAsync(It.IsAny<string>())).ReturnsAsync(new LayoutModel
            {
                LayoutName = layoutName,
                LayoutJsonData = nameof(LayoutModel.LayoutJsonData),
                Id = nameof(LayoutModel.Id)
            });

            // Act
            var actual = await controller.DownloadLayoutFile(Guid.NewGuid());

            // Assert
            actual
                .Should().BeOfType<FileContentResult>()
                .Which.FileDownloadName.Should().Be(expectedFileName);

            _mockLayoutManagementService.Verify(mock => mock.GetLayoutAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DownloadLayoutFile_NotFoundResult_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            _mockLayoutManagementService.Setup(mock => mock.GetLayoutAsync(It.IsAny<string>())).ReturnsAsync(new LayoutModel());

            // Act
            var actual = await controller.DownloadLayoutFile(Guid.NewGuid());

            // Assert
            actual
                .Should().BeOfType<NotFoundResult>();

            _mockLayoutManagementService.Verify(mock => mock.GetLayoutAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AreYouSure_Returns_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            _mockLayoutManagementService.Setup(mock => mock.GetLayoutAsync(It.IsAny<string>())).ReturnsAsync(
                        new LayoutModel
                        {
                            LayoutName = nameof(LayoutModel.LayoutName),
                            Id = "xx",
                            FundingStreamId = 1
                        });
            _mockAdminPublicationService.Setup(mock => mock.GetAll()).ReturnsAsync(new List<Publication>());
            _mockAdminSettingsService.Setup(mock => mock.GetAllFundingStreams(It.IsAny<FetchData[]>())).ReturnsAsync(
                  new List<FundingStream>()
                  {
                    new FundingStream
                    {
                        Id = 1,
                        FundingStreamCode = "PSG",
                        FundingStreamName = "PE & sports",
                        Publications = new List<Publication>()
                    },
                    new FundingStream
                    {
                        Id = 2,
                        FundingStreamCode = "DSG",
                        FundingStreamName = "Dedicated school grant",
                        Publications = new List<Publication>()
                    }
                  });

            // Act
            var actual = await controller.AreYouSure(Guid.NewGuid());

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<LayoutAreYouSureViewModel>();

            _mockLayoutManagementService.Verify(mock => mock.GetLayoutAsync(It.IsAny<string>()), Times.Once);
            _mockAdminSettingsService.Verify(mock => mock.GetAllFundingStreams(It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteLayout_Returns_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            _mockLayoutManagementService.Setup(mock => mock.GetLayoutAsync(It.IsAny<string>())).ReturnsAsync(new LayoutModel());
            _mockLayoutManagementService.Setup(mock => mock.DeleteLayoutAsync(It.IsAny<LayoutModel>())).ReturnsAsync(true);

            // Act
            var actual = await controller.DeleteLayout(Guid.NewGuid());

            // Assert
            actual
                 .Should().BeOfType<RedirectToRouteResult>()
                 .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_AdminLayoutImportConfirmation);

            _mockLayoutManagementService.Verify(mock => mock.GetLayoutAsync(It.IsAny<string>()), Times.Once);
            _mockLayoutManagementService.Verify(mock => mock.DeleteLayoutAsync(It.IsAny<LayoutModel>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(FundingViewScope.OrganisationHistorySingleYear, FundingViewType.ViewData, ViewYourFundingConstants.RouteName_LocalAuthorityHistorySingleYear)]
        [DataRow(FundingViewScope.OrganisationSummary, FundingViewType.ViewData, ViewYourFundingConstants.RouteName_LocalAuthorityStatement)]
        [DataRow(FundingViewScope.Organisation, FundingViewType.ViewData, ViewYourFundingConstants.RouteName_LocalAuthorityFundingBreakdown)]
        [DataRow(FundingViewScope.OrganisationHistory, FundingViewType.ViewData, ViewYourFundingConstants.RouteName_LocalAuthorityHistory)]
        [DataRow(FundingViewScope.ProviderSummary, FundingViewType.ViewData, ViewYourFundingConstants.RouteName_ProviderStatement)]
        [DataRow(FundingViewScope.ProviderHistorySingleYear, FundingViewType.ViewData, ViewYourFundingConstants.RouteName_ProviderHistorySingleYear)]
        [DataRow(FundingViewScope.ProviderHistory, FundingViewType.ViewData, ViewYourFundingConstants.RouteName_ProviderHistory)]
        public async Task PreviewLayout_Returns_ResultExpected(FundingViewScope fundingViewScope, FundingViewType fundingViewType, string routeName)
        {
            // Arrange
            var controller = GetLayoutManagementController();
            _mockAdminSettingsService.Setup(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(GetFundingStreamData);

            // Act
            var actual = await controller.PreviewLayout(new PreviewLayoutViewModel { FundingViewScope = fundingViewScope, FundingViewType = fundingViewType });

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(routeName);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(FundingViewScope.Provider, FundingViewType.Spreadsheet, ViewYourFundingConstants.RouteName_ProviderSpreadsheetDownload)]
        [DataRow(FundingViewScope.Organisation, FundingViewType.Spreadsheet, ViewYourFundingConstants.RouteName_LocalAuthoritySpreadsheetDownload)]
        [DataRow(FundingViewScope.National, FundingViewType.Spreadsheet, ViewYourFundingConstants.RouteName_PreviewNationalSpreadsheetDownload)]
        public async Task PreviewSpreadsheet_Post_Returns_ResultExpected(FundingViewScope fundingViewScope, FundingViewType fundingViewType, string routeName)
        {
            // Arrange
            var controller = GetLayoutManagementController();
            _mockAdminSettingsService.Setup(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(GetFundingStreamData);

            // Act
            var actual = await controller.PreviewLayout(new PreviewLayoutViewModel { FundingViewScope = fundingViewScope, FundingViewType = fundingViewType });

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(routeName);
        }


        [TestMethod, TestCategory("Unit")]
        public async Task PreviewLayout_National_Returns_ResultExpected()
        {
            // Arrange
            var controller = GetLayoutManagementController();
            _mockAdminSettingsService.Setup(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
                .ReturnsAsync(GetFundingStreamData);

            // Act
            var actual = await controller.PreviewLayout(Guid.NewGuid(), FundingViewScope.National, 1, FundingViewType.ViewData);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_NationalFundingAllocation);

            _mockAdminSettingsService.Verify(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(FundingViewScope.OrganisationHistorySingleYear, FundingViewType.ViewData)]
        [DataRow(FundingViewScope.Organisation, FundingViewType.ViewData)]
        [DataRow(FundingViewScope.OrganisationHistory, FundingViewType.ViewData)]
        [DataRow(FundingViewScope.ProviderSummary, FundingViewType.ViewData)]
        [DataRow(FundingViewScope.ProviderHistorySingleYear, FundingViewType.ViewData)]
        public async Task PreviewLayout_Post_Returns_ResultExpected(FundingViewScope fundingViewScope, FundingViewType fundingViewType)
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var expectedViewModel = GetExpectedPreviewLayoutViewModel(fundingViewScope, fundingViewType);

            // Act
            var actual = await controller.PreviewLayout(Guid.Empty, fundingViewScope, 1, fundingViewType);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PreviewLayoutViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(FundingViewScope.Provider, FundingViewType.Spreadsheet)]
        [DataRow(FundingViewScope.Organisation, FundingViewType.Spreadsheet)]
        public async Task PreviewSpreadsheet_Returns_ResultExpected(FundingViewScope fundingViewScope, FundingViewType fundingViewType)
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var expectedViewModel = GetExpectedPreviewLayoutViewModel(fundingViewScope, fundingViewType);

            // Act
            var actual = await controller.PreviewLayout(Guid.Empty, fundingViewScope, 1, fundingViewType);

            // Assert
            actual
                .Should().BeOfType<ViewResult>()
                .Which.Model.Should().BeOfType<PreviewLayoutViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [TestMethod, TestCategory("Unit")]
        [DataRow(FundingViewScope.National, FundingViewType.Spreadsheet)]
        public async Task PreviewNationalSpreadsheet_Returns_ResultExpected(FundingViewScope fundingViewScope, FundingViewType fundingViewType)
        {
            // Arrange
            var controller = GetLayoutManagementController();
            var expectedViewModel = GetExpectedPreviewLayoutViewModel(fundingViewScope, fundingViewType);
            _mockAdminSettingsService.Setup(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()))
               .ReturnsAsync(GetFundingStreamData);

            // Act
            var actual = await controller.PreviewLayout(Guid.Empty, fundingViewScope, 1, fundingViewType);

            // Assert
            actual
                .Should().BeOfType<RedirectToRouteResult>()
                .Which.RouteName.Should().Be(ViewYourFundingConstants.RouteName_PreviewNationalSpreadsheetDownload);

            _mockAdminSettingsService.Verify(mock => mock.GetFundingStreamById(It.IsAny<int>(), It.IsAny<FetchData[]>()), Times.Once);
        }

        #endregion


        #region Private helpers

        private static PreviewLayoutViewModel GetExpectedPreviewLayoutViewModel(FundingViewScope fundingViewScope, FundingViewType fundingViewType)
        {
            return new PreviewLayoutViewModel
            {
                FundingViewScope = fundingViewScope,
                FundingViewType = fundingViewType,
                FundingStreamId = 1,
                CurrentUser = new CurrentUserViewModel()
            };
        }

        private static LayoutManagementHomePageViewModel GetExpectedHomeViewModel()
        {
            var expectedViewModel = new LayoutManagementHomePageViewModel
            {
                CurrentUser = new CurrentUserViewModel(),
                LayoutModels = new List<LayoutUiModel>
                {
                    new LayoutUiModel
                    {
                        LayoutName = nameof(LayoutModel.LayoutName),
                        FundingStreamName = "PE & sports",
                        LayoutId = "xx",
                        FundingStreamId = 1,
                        Status = LayoutStatus.NotAssigned
                    }
                },
                Pagination = new Pagination
                {
                    FirstRecordNo = 1,
                    PageIndex = 0,
                    PageNumber = 1,
                    ResultCount = 1,
                    LastRecordNo = 1,
                    TotalPages = 1
                },
                LayoutFilter = new LayoutFilter
                {
                    FundingStreams = new List<Filter>()
                    {
                        new Filter
                        {
                            Id = 1,
                            Name = "PE & sports",
                            Selected = false
                        },
                        new Filter
                        {
                            Id = 2,
                            Name = "Dedicated school grant",
                            Selected = false
                        }
                    },
                    FundingViewTypes = new List<Filter>
                    {
                        new Filter
                        {
                            Id = 2,
                            Name = "UI view",
                            Selected = false
                        }
                    },
                    FundingViewScopes = new List<Filter>
                    {
                        new Filter
                        {
                            Id = 6,
                            Name = "Provider summary",
                            Selected = false
                        }
                    },
                    PageNumber = 0
                }
            };
            return expectedViewModel;
        }

        private static LayoutConfirmationViewModel GetExpectedConfirmationViewModel(
            bool savedChanges = true,
            LayoutAction layoutAction = LayoutAction.Add)
        {
            return savedChanges
                ? new LayoutConfirmationViewModel
                {
                    CurrentUser = new CurrentUserViewModel(),
                    LayoutUiModel = new LayoutUiModel
                    {
                        FundingStreamName = string.Empty,
                        LayoutId = "Id",
                        Status = LayoutStatus.NotAssigned
                    },
                    ChangesSaved = true,
                    SubmittedDisplayDate = "on 01 January 0001 at 12:00am",
                    LayoutAction = layoutAction,
                }
                : new LayoutConfirmationViewModel
                {
                    CurrentUser = new CurrentUserViewModel(),
                    LayoutAction = layoutAction,
                };
        }

        private static LayoutImportViewModel GetExpectedLayoutImportViewModel()
        {
            var expectedViewModel = new LayoutImportViewModel
            {
                CurrentUser = new CurrentUserViewModel(),
                FundingStreams = new List<SelectListItem>()
            };
            return expectedViewModel;
        }

        private static LayoutFileImportViewModel GetExpectedLayoutFileImportViewModel()
        {
            var expectedViewModel = new LayoutFileImportViewModel
            {
                CurrentUser = new CurrentUserViewModel(),
                FundingStreams = new List<SelectListItem>()
            };
            return expectedViewModel;
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(x => x.AddProfile(new WebAutoMapperProfile())).CreateMapper();
        }

        private static FundingStream GetFundingStreamData()
        {
            return new FundingStream
            {
                Publications = new List<Publication>
                {
                    new Publication
                    {
                        FundingStream = new FundingStream
                        {
                            Id = 1, SettingValues = new List<SettingValue>
                            {
                                new SettingValue
                                {
                                    SettingId = 1,
                                    Setting = new SettingType
                                    {
                                        ValueDataType = SettingValueDataType.Int,
                                        SettingName = nameof(SettingType.SettingName)
                                    }
                                }
                            }
                        },
                        PublishedDate = new DateTime(1, 1, 1),
                        FundingPeriodCode = "AY-1920",
                        Status = PublicationStatus.Published
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
                            ValueDataType = SettingValueDataType.Int,
                            SettingName = SettingName.NextAllocationPaymentDate
                        },
                        FundingStream = new FundingStream
                        {
                            FundingStreamName = nameof(FundingStream.FundingStreamName)
                        },
                        FundingStreamId = 1,
                        Value = nameof(SettingValue.Value)
                    },
                    new SettingValue
                    {
                        SettingId = 2,
                        Id = 2,
                        Setting = new SettingType
                        {
                            ValueDataType = SettingValueDataType.Int,
                            SettingName = SettingName.AcademicYear
                        },
                        FundingStream = new FundingStream
                        {
                            FundingStreamName = nameof(FundingStream.FundingStreamName)
                        },
                        FundingStreamId = 1,
                        Value = "201920",
                    }
                },
                FundingStreamName = nameof(FundingStream.FundingStreamName),
                Id = 1
            };
        }

        private LayoutManagementController GetLayoutManagementController()
        {
            SetupMockServices();
            var controller = new LayoutManagementController(
                _mockAdminSettingsService.Object,
                _mockAdminPublicationService.Object,
                _mapper,
                _mockLayoutManagementService.Object,
                _mockSecurityService.Object,
                _mockConfigurationService.Object);

            return controller;
        }

        private void SetupMockServices()
        {
            _mockConfigurationService.Setup(x => x.Value)
                .Returns(new ApplicationConfiguration());

            _mockSecurityService.Setup(x => x.GetUserFromClaims(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new Pds.Core.Common.Identity.Models.User());
        }

        #endregion
    }
}