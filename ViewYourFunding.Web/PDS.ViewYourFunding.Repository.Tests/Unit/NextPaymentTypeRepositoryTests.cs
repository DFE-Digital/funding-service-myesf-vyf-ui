using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Implementations;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Tests.Unit
{
    [TestClass]
    public class NextPaymentTypeRepositoryTests : DataContextTestBase
    {
        [TestMethod, TestCategory("Unit")]
        public async Task CreateNextPaymentType_Returns_ExpectedResult()
        {
            //Arrange
            var databaseName = GetAsyncMethodName();
            var model = new NextPaymentType();
            model.LastUpdatedBy = string.Empty;
            model.TypeCode = string.Empty;

            // Act
            using (var context = GetContext(databaseName))
            {
                // Act
                var nextPaymentTypeRepository = new NextPaymentTypeRepository(context, null);

                var actual = await nextPaymentTypeRepository.CreateNextPaymentType(model);

                //Assert
                actual.Should().BeEquivalentTo(model);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateNextPaymentType_Returns_ExpectedResult()
        {
            // Arrange
            var nextPaymentType = GetNextPaymentType();

            var databaseName = GetAsyncMethodName();
            NextPaymentType nextPaymentTypeToUpdate;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(nextPaymentType);
                await context.SaveChangesAsync();

                nextPaymentTypeToUpdate = context.NextPaymentTypes.First();
            }

            const string newDescription = "FY-2222";

            nextPaymentTypeToUpdate.Description = newDescription;

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var nextPaymentTypeRepository = new NextPaymentTypeRepository(context, null);

                // Act
                var actual = await nextPaymentTypeRepository.UpdateNextPaymentType(nextPaymentTypeToUpdate);

                // Assert
                actual.Should().BeTrue();

                var updatedNextPaymentType = context.NextPaymentTypes.First(x => x.Id == nextPaymentTypeToUpdate.Id);
                updatedNextPaymentType.Description.Should().Be(newDescription);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetNextPaymentTypes_Returns_ExpectedResult()
        {
            // Arrange
            var nextPaymentType = GetNextPaymentType();
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.AddRangeAsync(nextPaymentType);
                await context.SaveChangesAsync();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var nextPaymentTypeRepository = new NextPaymentTypeRepository(context, null);

                // Act
                var actual = await nextPaymentTypeRepository.GetNextPaymentTypes(nextPaymentType.FundingStreamId);

                // Assert
                actual.Should().HaveCount(3);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteNextPaymentType_Returns_ExpectedResult()
        {
            // Arrange
            var nextPaymentType = GetNextPaymentType();

            var databaseName = GetAsyncMethodName();
            NextPaymentType nextPaymentTypeToUpDelete;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(nextPaymentType);
                await context.SaveChangesAsync();

                nextPaymentTypeToUpDelete = context.NextPaymentTypes.First();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var nextPaymentTypeRepository = new NextPaymentTypeRepository(context, null);

                // Act
                var actual = await nextPaymentTypeRepository.DeleteNextPaymentType(nextPaymentTypeToUpDelete);

                // Assert
                actual.Should().BeTrue();
            }
        }

        private static NextPaymentType GetNextPaymentType()
        {
            return new NextPaymentType
            {
                TypeCode = "test",
                Description = "test",
                FundingStreamId = 1,
                LastUpdatedBy = string.Empty
            };
        }
    }
}
