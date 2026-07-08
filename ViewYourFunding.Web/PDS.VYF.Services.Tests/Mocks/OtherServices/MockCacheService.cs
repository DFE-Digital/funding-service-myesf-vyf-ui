using Moq;
using PDS.ViewYourFunding.Services.Cache;
using PDS.ViewYourFunding.Services.Interfaces;

namespace PDS.VYF.Services.Tests.Mocks.OtherServices
{
    public class MockCacheService : MockBase<ICacheService>
    {
        public MockCacheService()
            : base(false)
        {
        }

        public override void SetupDefault()
        {
            throw new NotImplementedException();
        }

        public MockCacheService SetupAddOrGetExistingResultAsync<T>(string cacheKey, CacheExpirationPolicy cacheExpirationPolicy, T returnValue)
        {
            var exp = this.CallTo(a => a.AddOrGetExistingResultAsync<T>(cacheKey, It.IsAny<Func<Task<T>>>(), cacheExpirationPolicy, It.IsAny<TimeSpan?>()));

            this.Setup(exp)
                .ReturnsAsync(returnValue);

            return this;
        }

        public MockCacheService VerifyAddOrGetExistingResultAsync<T>(string cacheKey, CacheExpirationPolicy cacheExpirationPolicy, int callCount)
        {
            var exp = this.CallTo(a => a.AddOrGetExistingResultAsync<T>(cacheKey, It.IsAny<Func<Task<T>>>(), cacheExpirationPolicy, It.IsAny<TimeSpan?>()));

            this.Verify(exp, Times.Exactly(callCount));

            return this;
        }
    }
}
