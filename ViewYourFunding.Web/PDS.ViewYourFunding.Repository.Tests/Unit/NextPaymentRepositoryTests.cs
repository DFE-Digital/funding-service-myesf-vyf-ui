using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.ViewYourFunding.Repositories.DataModels;
using PDS.ViewYourFunding.Repositories.Implementations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Repositories.Tests.Unit
{
    [TestClass]
    public class NextPaymentRepositoryTests : DataContextTestBase
    {
        [TestMethod, TestCategory("Unit")]
        public async Task CreateNextPayment_Returns_ExpectedResult()
        {
            //Arrange
            var databaseName = GetAsyncMethodName();
            var model = new NextPayment();
            model.LastUpdatedBy = string.Empty;

            // Act
            using (var context = GetContext(databaseName))
            {
                // Act
                var nextPaymentRepository = new NextPaymentRepository(context, null);

                var actual = await nextPaymentRepository.CreateNextPayment(model);

                //Assert
                actual.Should().BeEquivalentTo(model);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task UpdateNextPayment_Returns_ExpectedResult()
        {
            // Arrange
            var nextPayment = GetNextPayment();

            var databaseName = GetAsyncMethodName();
            NextPayment nextPaymentToUpdate;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(nextPayment);
                await context.SaveChangesAsync();

                nextPaymentToUpdate = context.NextPayments.First();
            }

            const string newFundingPeriodCode = "FY-2222";

            nextPaymentToUpdate.FundingPeriodCode = newFundingPeriodCode;

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var nextPaymentRepository = new NextPaymentRepository(context, null);

                // Act
                var actual = await nextPaymentRepository.UpdateNextPayment(nextPaymentToUpdate);

                // Assert
                actual.Should().BeTrue();

                var updatedNextPayment = context.NextPayments.First(x => x.Id == nextPaymentToUpdate.Id);
                updatedNextPayment.FundingPeriodCode.Should().Be(newFundingPeriodCode);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task GetNextPayments_Returns_ExpectedResult()
        {
            // Arrange
            var nextPayment = GetNextPayment();
            var fundingStreams = GetFundingStreamData();

            var databaseName = GetAsyncMethodName();

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(fundingStreams);
                await context.AddRangeAsync(nextPayment);
                await context.SaveChangesAsync();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var nextPaymentRepository = new NextPaymentRepository(context, null);

                // Act
                var actual = await nextPaymentRepository.GetNextPayments(nextPayment.FundingStreamId);

                // Assert
                actual.Should().HaveCount(1);
            }
        }

        [TestMethod, TestCategory("Unit")]
        public async Task DeleteNextPayment_Returns_ExpectedResult()
        {
            // Arrange
            var nextPayment = GetNextPayment();

            var databaseName = GetAsyncMethodName();
            NextPayment nextPaymentToUpDelete;

            // Create in-memory database.
            using (var context = GetContext(databaseName))
            {
                await context.AddRangeAsync(nextPayment);
                await context.SaveChangesAsync();

                nextPaymentToUpDelete = context.NextPayments.First();
            }

            // Act/Assert
            // create new context and run test to ensure data persisted in database and not in dbContext
            using (var context = GetContext(databaseName))
            {
                var nextPaymentRepository = new NextPaymentRepository(context, null);

                // Act
                var actual = await nextPaymentRepository.DeleteNextPayment(nextPaymentToUpDelete);

                // Assert
                actual.Should().BeTrue();
            }
        }

        private static NextPayment GetNextPayment()
        {
            return new NextPayment
            {
                NextPaymentDate = new DateTime(1, 1, 1),
                FundingStreamId = 1,
                NextPaymentTypeId = 1,
                Active = true,
                LastUpdatedBy = "test"
            };
        }
    }
}