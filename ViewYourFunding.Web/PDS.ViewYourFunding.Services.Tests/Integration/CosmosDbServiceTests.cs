using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Implementations;
using PDS.ViewYourFunding.Services.Tests.Config;
using PDS.ViewYourFunding.Services.Tests.Integration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Services.Tests.Integration
{
    [TestClass]
    public class CosmosDbServiceTests
    {
        private const string GetAllQuery = "SELECT * FROM c";

        private readonly CosmosClient _cosmosClient;
        private readonly Container _cosmosContainer;
        private readonly Mock<IOptions<CosmosDbConfiguration>> _cosmosDbConfiguration = new Mock<IOptions<CosmosDbConfiguration>>();
        private readonly ILoggerAdapter<CosmosDbService<TestCosmosDocument>> _logger = null;
        private readonly TestCosmosDocument _firstDocument;
        private readonly TestCosmosDocument _secondDocument;

        public CosmosDbServiceTests()
        {
            _cosmosClient = InitializeCosmosClientInstanceAsync().GetAwaiter().GetResult();
            _cosmosDbConfiguration.Setup(mock => mock.Value).Returns(new CosmosDbConfiguration());
            _firstDocument = new TestCosmosDocument
            {
                Description = $"first - {nameof(TestCosmosDocument.Description)}",
                Id = Guid.NewGuid().ToString()
            };

            _secondDocument = new TestCosmosDocument
            {
                Description = $"second - {nameof(TestCosmosDocument.Description)}",
                Id = Guid.NewGuid().ToString()
            };
            _cosmosContainer = _cosmosClient.GetContainer(_cosmosDbConfiguration.Object.Value.DatabaseName, _firstDocument.CollectionName);
            SeedCosmosDb().GetAwaiter().GetResult();
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetAllAsync_ReturnsExpected()
        {
            // Arrange
            await ClearCosmosDb();
            await SeedCosmosDb();
            var service = GetCosmosDbService();
            var expectedResult = new List<TestCosmosDocument> { _firstDocument, _secondDocument };

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetAsync_ReturnsExpected()
        {
            // Arrange
            var service = GetCosmosDbService();

            // Act
            var result = await service.GetAsync(_firstDocument.Id);

            // Assert
            result.Should().BeEquivalentTo(_firstDocument);
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetAsync_EmptyCollection_ReturnsExpected()
        {
            // Arrange
            await ClearCosmosDb();
            var service = GetCosmosDbService();

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetAsync_NotFound_ReturnsExpected()
        {
            // Arrange
            var service = GetCosmosDbService();

            // Act
            var result = await service.GetAsync(nameof(GetAsync_NotFound_ReturnsExpected));

            // Assert
            result.Should().BeNull();
        }

        [TestMethod, TestCategory("Integration")]
        public async Task UpdateAsync_ReturnsExpected()
        {
            // Arrange
            await ClearCosmosDb();
            await SeedCosmosDb();
            var service = GetCosmosDbService();
            _firstDocument.Description = "updated";

            // Act
            var result = await service.UpdateAsync(_firstDocument.Id, _firstDocument);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod, TestCategory("Integration")]
        public async Task AddAsync_ReturnsExpected()
        {
            // Arrange
            var service = GetCosmosDbService();
            var newDocument = new TestCosmosDocument
            {
                Id = Guid.NewGuid().ToString()
            };

            // Act
            var result = await service.AddAsync(newDocument);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod, TestCategory("Integration")]
        public async Task UpdateAsync_NotFound_ReturnsExpected()
        {
            // Arrange
            await ClearCosmosDb();
            var service = GetCosmosDbService();
            _firstDocument.Description = "updated";

            // Act
            var result = await service.UpdateAsync(_firstDocument.Id, _firstDocument);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod, TestCategory("Integration")]
        public async Task DeleteAsync_ReturnsExpected()
        {
            // Arrange
            await ClearCosmosDb();
            await SeedCosmosDb();
            var service = GetCosmosDbService();

            // Act
            var result = await service.DeleteAsync(_firstDocument.Id, _firstDocument);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod, TestCategory("Integration")]
        public async Task DeleteAsync_NotFound_ReturnsExpected()
        {
            // Arrange
            await ClearCosmosDb();
            var service = GetCosmosDbService();

            // Act
            var result = await service.DeleteAsync(_firstDocument.Id, _firstDocument);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetCountAsync_ReturnsExpected()
        {
            // Arrange
            await ClearCosmosDb();
            await SeedCosmosDb();
            var service = GetCosmosDbService();

            // Act
            var result = await service.GetCountAsync();

            // Assert
            Assert.IsTrue(result == 2);
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetCountAsync_ReturnsNotExpected()
        {
            // Arrange
            await ClearCosmosDb();
            await SeedCosmosDb();
            var service = GetCosmosDbService();

            // Act
            var result = await service.GetCountAsync(new List<Expression<Func<TestCosmosDocument, bool>>>
            {
                expression => expression.Id == "xyz"
            });

            // Assert
            Assert.IsTrue(result == 0);
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetCountAsync_ReturnsRecordExpected()
        {
            // Arrange
            await ClearCosmosDb();
            await SeedCosmosDb();
            var service = GetCosmosDbService();

            // Act
            var result = await service.GetCountAsync(new List<Expression<Func<TestCosmosDocument, bool>>>
            {
                expression => expression.Id == _firstDocument.Id
            });

            // Assert
            Assert.IsTrue(result == 1);
        }

        [TestMethod, TestCategory("Integration")]
        public async Task GetPagination_ReturnsRecordExpected()
        {
            // Arrange
            await ClearCosmosDb();
            await SeedCosmosDb();
            var service = GetCosmosDbService();
            var expectedResult = new List<TestCosmosDocument> { _secondDocument };

            // Act
            var result = await service.GetPagination(1, 10);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        private CosmosDbService<TestCosmosDocument> GetCosmosDbService()
        {
            return new CosmosDbService<TestCosmosDocument>(_cosmosClient, _cosmosDbConfiguration.Object, _logger);
        }

        private async Task<CosmosClient> InitializeCosmosClientInstanceAsync()
        {
            var appConfig = ConfigHelper.GetApplicationConfiguration();
            var clientBuilder =
                new Microsoft.Azure.Cosmos.Fluent.CosmosClientBuilder(appConfig.CosmosDbConfiguration.ConnectionString);
            CosmosClient client;
            if (appConfig.CosmosDbConfiguration.CosmosConnectionMode == "Gateway")
            {
                client = clientBuilder
                        .WithConnectionModeGateway()
                        .Build();
            }
            else
            {
                client = clientBuilder
                        .WithConnectionModeDirect()
                        .Build();
            }

            var database = await client.CreateDatabaseIfNotExistsAsync(appConfig.CosmosDbConfiguration.DatabaseName);
            await database.Database.CreateContainerIfNotExistsAsync("fundingUiIntegrationTests", "/id");

            return client;
        }

        private async Task SeedCosmosDb()
        {
            var results = await GetAllTestCosmosDocuments();
            if (!results.Contains(_firstDocument))
            {
                await _cosmosContainer.CreateItemAsync(_firstDocument);
            }

            if (!results.Contains(_secondDocument))
            {
                await _cosmosContainer.CreateItemAsync(_secondDocument);
            }
        }

        private async Task ClearCosmosDb()
        {
            var results = await GetAllTestCosmosDocuments();

            foreach (var result in results)
            {
                await _cosmosContainer.DeleteItemAsync<TestCosmosDocument>(result.Id, new PartitionKey(result.Id));
            }
        }

        private async Task<List<TestCosmosDocument>> GetAllTestCosmosDocuments()
        {
            var query = _cosmosContainer.GetItemQueryIterator<TestCosmosDocument>(new QueryDefinition(GetAllQuery));
            var results = new List<TestCosmosDocument>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}