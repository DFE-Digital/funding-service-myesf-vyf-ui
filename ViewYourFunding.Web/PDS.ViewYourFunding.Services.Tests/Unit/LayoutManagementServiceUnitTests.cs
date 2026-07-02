using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Unit
{
    [TestClass]
    public class LayoutManagementServiceUnitTests
    {
        private readonly LayoutModel _inputLayoutModel = GetLayoutModel(string.Empty);
        private readonly Mock<ICosmosDbService<LayoutModel>> _mockCosmosDbService;

        public LayoutManagementServiceUnitTests()
        {
            _mockCosmosDbService = new Mock<ICosmosDbService<LayoutModel>>(MockBehavior.Strict);
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetAllLayoutsAsync_ReturnsExpected()
        {
            // Arrange
            var service = GetLayoutManagementService();
            _ = _mockCosmosDbService.Setup(
                mock => mock.GetAllAsync(It.IsAny<List<Expression<Func<LayoutModel, bool>>>>())).ReturnsAsync(GetLayoutModels);

            // Act
            var layoutModels = await service.GetAllLayoutsAsync();

            // Assert
            layoutModels.Should().BeEquivalentTo(GetLayoutModels());

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetAsync_ReturnsExpected()
        {
            // Arrange
            var service = GetLayoutManagementService();
            _mockCosmosDbService.Setup(
                mock => mock.GetAsync("1")).ReturnsAsync(GetLayoutModel("1"));

            // Act
            var layoutModel = await service.GetLayoutAsync("1");

            // Assert
            layoutModel.Should().BeEquivalentTo(GetLayoutModel("1"));

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetAsync_Fails_ReturnsExpected()
        {
            // Arrange
            var service = GetLayoutManagementService();
            _mockCosmosDbService.Setup(
                mock => mock.GetAsync("12")).ReturnsAsync(new LayoutModel());

            // Act
            var layoutModels = await service.GetLayoutAsync("12");

            // Assert
            layoutModels.Should().BeEquivalentTo(new LayoutModel());

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AddAsync_ReturnsExpected()
        {
            // Arrange
            var service = GetLayoutManagementService();
            _mockCosmosDbService.Setup(
                mock => mock.AddAsync(_inputLayoutModel)).ReturnsAsync(true);

            // Act
            var result = await service.AddLayoutAsync(_inputLayoutModel);

            // Assert
            result.Should().NotBeEmpty();
            _inputLayoutModel.Id.Should().NotBeNullOrEmpty();
            _inputLayoutModel.CreatedDate.Should().NotBe(DateTime.MinValue);
            _inputLayoutModel.LastModifiedDateTime.Should().NotBe(DateTime.MinValue);

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task AddAsync_Fails_ReturnsExpected()
        {
            // Arrange
            var service = GetLayoutManagementService();
            _mockCosmosDbService.Setup(
                mock => mock.AddAsync(_inputLayoutModel)).ReturnsAsync(false);

            // Act
            var result = await service.AddLayoutAsync(_inputLayoutModel);

            // Assert
            result.Should().BeEmpty();
            _inputLayoutModel.Id.Should().NotBeNullOrEmpty();

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateAsync_ReturnsExpected()
        {
            // Arrange
            var service = GetLayoutManagementService();
            _mockCosmosDbService.Setup(
                mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<LayoutModel>())).ReturnsAsync(true);

            // Act
            var result = await service.UpdateLayoutAsync(_inputLayoutModel);

            // Assert
            result.Should().BeTrue();
            _inputLayoutModel.LastModifiedDateTime.Should().NotBe(DateTime.MinValue);

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateAsync_Fails_ReturnsExpected()
        {
            // Arrange
            var service = GetLayoutManagementService();
            _mockCosmosDbService.Setup(
                mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<LayoutModel>())).ReturnsAsync(false);

            // Act
            var result = await service.UpdateLayoutAsync(_inputLayoutModel);

            // Assert
            result.Should().BeFalse();
            _inputLayoutModel.LastModifiedDateTime.Should().NotBe(DateTime.MinValue);

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteAsync_ReturnsExpected()
        {
            // Arrange
            var service = GetLayoutManagementService();
            _mockCosmosDbService.Setup(
                mock => mock.DeleteAsync(It.IsAny<string>(), It.IsAny<LayoutModel>())).ReturnsAsync(true);

            // Act
            var result = await service.DeleteLayoutAsync(_inputLayoutModel);

            // Assert
            result.Should().BeTrue();
            _inputLayoutModel.DeletedDateTime.Should().NotBe(DateTime.MinValue);

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteAsync_Fails_ReturnsExpected()
        {
            // Arrange
            var service = GetLayoutManagementService();
            _mockCosmosDbService.Setup(
                mock => mock.DeleteAsync(It.IsAny<string>(), It.IsAny<LayoutModel>())).ReturnsAsync(false);

            // Act
            var result = await service.DeleteLayoutAsync(_inputLayoutModel);

            // Assert
            result.Should().BeFalse();
            _inputLayoutModel.DeletedDateTime.Should().NotBe(DateTime.MinValue);

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetPaginationResultWithFilters_ReturnsResultExpected()
        {
            // Arrange
            List<int> lst = new List<int>() { 1, 2 };
            var fundingViewTypes = new List<string>() { "ViewData" };
            var fundingViewScopes = new List<string>() { "ProviderSummary" };

            var expectedModel = new PaginationResult()
            {
                TotalCount = 1,
                PaginationDetail = new Pagination()
                {
                    TotalPages = 1,
                    ResultCount = 1,
                    FirstRecordNo = 1,
                    LastRecordNo = 1,
                    PageNumber = 1,
                    FirstPage = 1,
                    LastPage = 1,
                    PageSize = 1
                },
                LayoutModels = new List<LayoutModel>()
                    {
                        new LayoutModel()
                    },
                FilterOptions = new FilterOptions
                {
                    FundingStreamsIds = new List<int>() { 1 },
                    FundingViewTypes = new List<string>() { null },
                    FundingViewScopes = new List<string>() { null }
                }
            };
            var model = new List<LayoutModel>() { new LayoutModel() };
            var service = GetLayoutManagementService();

            _mockCosmosDbService.Setup(
                mock => mock.GetPagination(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()))
                .ReturnsAsync(model);

            _mockCosmosDbService.Setup(
               mock => mock.GetCountAsync(
                   It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()))
               .ReturnsAsync(1);


            _mockCosmosDbService.Setup(mock => mock.GetAllAsync(It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()))
                .ReturnsAsync(new List<LayoutModel>
                {
                    new LayoutModel
                    {
                        FundingStreamId = 1,
                        LayoutName = "ProviderSummary"
                    }
                });

            // Act
            var result = await service.GetPaginationResultWithFilters(1, 1, lst, fundingViewTypes, fundingViewScopes);

            // Assert
            result.Should().BeEquivalentTo(expectedModel);

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetPaginationResuls_ReturnsResultExpected()
        {
            // Arrange
            List<int> lst = new List<int>() { 1, 2 };
            var fundingViewTypes = new List<string>() { "ViewData" };
            var fundingViewScopes = new List<string>() { "ProviderSummary" };

            var expectedModel = new PaginationResult()
            {
                TotalCount = 1,
                PaginationDetail = new Pagination()
                {
                    TotalPages = 1,
                    ResultCount = 1,
                    FirstRecordNo = 1,
                    LastRecordNo = 1,
                    PageNumber = 1,
                    FirstPage = 1,
                    LastPage = 1,
                    PageSize = 1
                },
                LayoutModels = new List<LayoutModel>()
                    {
                        new LayoutModel()
                    }
            };
            var model = new List<LayoutModel>() { new LayoutModel() };
            var service = GetLayoutManagementService();

            _mockCosmosDbService.Setup(
                mock => mock.GetPagination(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()))
                .ReturnsAsync(model);

            _mockCosmosDbService.Setup(
               mock => mock.GetCountAsync(
                   It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()))
               .ReturnsAsync(1);


            _mockCosmosDbService.Setup(mock => mock.GetAllAsync(null))
                .ReturnsAsync(new List<LayoutModel>
                {
                    new LayoutModel
                    {
                        FundingStreamId = 1,
                        LayoutName = "ProviderSummary"
                    }
                });

            // Act
            var result = await service.GetPaginationResult(1, 1, lst, fundingViewTypes, fundingViewScopes);

            // Assert
            result.Should().BeEquivalentTo(expectedModel);

            _mockCosmosDbService.Verify();
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetPaginationResult_ReturnsResultNotExpected()
        {
            // Arrange
            List<LayoutModel> model = null;
            List<int> lst = new List<int>() { 1, 2 };
            var fundingViewTypes = new List<string>() { "ViewData" };
            var fundingViewScopes = new List<string>() { "ProviderSummary" };

            var service = GetLayoutManagementService();
            _mockCosmosDbService.Setup(
               mock => mock.GetPagination(
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()))
               .ReturnsAsync(model);

            _mockCosmosDbService.Setup(
              mock => mock.GetCountAsync(
                  It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()))
              .ReturnsAsync(1);

            _mockCosmosDbService.Setup(mock => mock.GetAllAsync(It.IsAny<List<Expression<Func<LayoutModel, bool>>>>()))
              .ReturnsAsync(new List<LayoutModel>
              {
                    new LayoutModel
                    {
                        FundingStreamId = 1,
                        LayoutName = "ProviderSummary"
                    }
              });

            // Act
            Func<Task> act = async () => await service.GetPaginationResult(1, 1, lst, fundingViewTypes, fundingViewScopes);

            // Assert
            await act.Should().NotThrowAsync();

            _mockCosmosDbService.Verify();
        }

        private static List<LayoutModel> GetLayoutModels()
        {
            return Enumerable.Range(1, 4).Select(id => GetLayoutModel(id.ToString())).ToList();
        }

        private static LayoutModel GetLayoutModel(string id)
        {
            return new LayoutModel
            {
                Id = id,
                LayoutName = nameof(LayoutModel.LayoutName),
                FundingViewType = nameof(LayoutModel.FundingViewType),
                FundingViewScope = nameof(LayoutModel.FundingViewScope),
                FundingStreamId = 1,
                LayoutJsonData = nameof(LayoutModel.LayoutJsonData)
            };
        }

        private LayoutManagementService GetLayoutManagementService()
        {
            return new LayoutManagementService(_mockCosmosDbService.Object, null);
        }
    }
}