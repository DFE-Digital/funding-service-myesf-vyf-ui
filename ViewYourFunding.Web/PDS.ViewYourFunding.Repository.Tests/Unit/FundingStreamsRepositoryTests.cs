using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Implementations;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Tests.Unit
{
    [TestClass]
    public class FundingStreamsRepositoryTests : DataContextTestBase
    {
        [TestMethod, TestCategory("Unit")]
        public async Task GetFundingStreams_GetExpectedResult()
        {
            // Arrange
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.SaveChangesAsync();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var repository = new FundingStreamRepository(context, null);

                // Act
                var actual = await repository.GetAllFundingStreams(true);

                // Assert
                actual.Should().HaveCount(2);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreateFundingStream_Returns_ExpectedResult()
        {
            //Arrange
            var databaseName = GetAsyncMethodName();
            var model = new FundingStream();
            model.LastUpdatedBy = string.Empty;

            // Act
            using (var context = GetContext(databaseName))
            {
                // Act
                var fundingStreamRepository = new FundingStreamRepository(context, null);

                var actual = await fundingStreamRepository.CreateFundingStream(model);

                //Assert
                actual.Should().BeEquivalentTo(model);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateFundingStream_Returns_ExpectedResult()
        {
            // Arrange
            var fundingStream = GetFundingStream();

            var databaseName = GetAsyncMethodName();
            FundingStream fundingStreamToUpdate;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStream);
                await context.SaveChangesAsync();

                fundingStreamToUpdate = context.FundingStreams.First();
            }

            const string newFundingStreamName = "test2";

            fundingStreamToUpdate.FundingStreamName = newFundingStreamName;

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var fundingStreamRepository = new FundingStreamRepository(context, null);

                // Act
                var actual = await fundingStreamRepository.UpdateFundingStream(fundingStreamToUpdate);

                // Assert
                actual.Should().BeTrue();

                var updatedFundingStream = context.FundingStreams.First(x => x.Id == fundingStreamToUpdate.Id);
                updatedFundingStream.FundingStreamName.Should().Be(newFundingStreamName);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteFundingStream_Returns_ExpectedResult()
        {
            // Arrange
            var fundingStream = GetFundingStream();
            var databaseName = GetAsyncMethodName();
            FundingStream fundingStreamToDelete;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStream);
                await context.SaveChangesAsync();

                fundingStreamToDelete = context.FundingStreams.First();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var fundingStreamRepository = new FundingStreamRepository(context, null);

                // Act
                var actual = await fundingStreamRepository.DeleteFundingStream(fundingStreamToDelete);

                // Assert
                actual.Should().BeTrue();
            }
        }

        private static FundingStream GetFundingStream()
        {
            return new FundingStream
            {
                FundingStreamCode = "test",
                FundingStreamName = "test",
                Id = 1,
                LastUpdatedBy = string.Empty
            };
        }
    }
}