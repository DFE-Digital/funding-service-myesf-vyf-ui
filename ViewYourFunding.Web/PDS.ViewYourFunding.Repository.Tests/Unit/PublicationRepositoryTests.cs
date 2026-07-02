using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Repositories.Implementations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Tests.Unit
{
    [TestClass]
    public class PublicationRepositoryTests : DataContextTestBase
    {
        [TestMethod, TestCategory("Unit")]
        public async Task GetPublications_GetExpectedResult()
        {
            // Arrange
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();

            int fundingStreamId;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.SaveChangesAsync();

                fundingStreamId = context.FundingStreams.First().Id;
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var repository = new PublicationRepository(context, null);

                // Act
                var actual = await repository.GetPublications(fundingStreamId);

                // Assert
                actual.Should().HaveCount(2);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdatePublication_GetExpectedResult()
        {
            // Arrange
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();
            Publication publicationToUpdate;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.SaveChangesAsync();

                publicationToUpdate = context.Publications.First();
            }

            const string newFundingPeriodCode = "FY-2222";

            publicationToUpdate.FundingPeriodCode = newFundingPeriodCode;

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var repository = new PublicationRepository(context, null);

                // Act
                var actual = await repository.UpdatePublication(publicationToUpdate);

                // Assert
                actual.Should().BeTrue();

                var updatedPublication = context.Publications.First(x => x.Id == publicationToUpdate.Id);
                updatedPublication.FundingPeriodCode.Should().Be(newFundingPeriodCode);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeletePublication_GetExpectedResult()
        {
            // Arrange
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();
            Publication publicationToDelete;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.SaveChangesAsync();

                publicationToDelete = context.Publications.First();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var repository = new PublicationRepository(context, null);

                // Act
                var actual = await repository.DeletePublication(publicationToDelete);

                // Assert
                actual.Should().BeTrue();
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task CreatePublication_GetExpectedResult()
        {
            //Arrange
            var databaseName = GetAsyncMethodName();
            var publication = GetPublication();
            var fundingStreams = GetFundingStreamData();

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = GetContext(databaseName))
            {
                // Act
                var repository = new PublicationRepository(context, null);

                var actual = await repository.CreatePublication(publication);

                //Assert
                actual.Should().BeEquivalentTo(publication);
            }
        }

        private static Publication GetPublication()
        {
            return new Publication
            {
                Description = "Publication1",
                PublishedDate = new DateTime(2019, 10, 10),
                CutOffDate = new DateTime(2020, 01, 01),
                FundingPeriodCode = "AY-1920",
                SpreadsheetModelVersion = 2,
                UIModelVersion = 2,
                Status = PublicationStatus.Published,
                FundingStreamId = 1
            };
        }
    }
}