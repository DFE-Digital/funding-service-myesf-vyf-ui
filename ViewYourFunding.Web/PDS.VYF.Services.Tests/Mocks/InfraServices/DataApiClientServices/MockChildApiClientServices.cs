using Moq;
using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

namespace PDS.VYF.Services.Tests.Mocks.InfraServices.DataApiClientServices
{
    public class MockChildApiClientServices : MockBase<IChildApiClientServices>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockChildApiClientServices"/> class.
        /// </summary>
        public MockChildApiClientServices()
            : base(false)
        {
        }

        public override void SetupDefault()
        {
            throw new NotImplementedException();
        }

        public MockChildApiClientServices SetupSearchChild(List<LoggedInChildModel> mockReturnValue)
        {
            var exp = this.CallTo(a => a.SearchChild(It.IsAny<ChildSearchApiRequestModel>(), It.IsAny<bool>(), It.IsAny<List<string>?>()));

            this
                .Setup(exp)
                .ReturnsAsync(mockReturnValue)
                .Verifiable();

            return this;
        }

        public MockChildApiClientServices VerifySearchChild(int callCount)
        {
            var exp = this.CallTo(a => a.SearchChild(It.IsAny<ChildSearchApiRequestModel>(), It.IsAny<bool>(), It.IsAny<List<string>?>()));

            this
                .Verify(exp, Times.Exactly(callCount));

            return this;
        }

        public MockChildApiClientServices SetupLatestFundingPeriod(List<string> mockReturnValue)
        {
            var exp = this.CallTo(a => a.LatestFundingPeriod(It.IsAny<ChildSearchApiRequestModel>()));

            this
                .Setup(exp)
                .ReturnsAsync(mockReturnValue)
                .Verifiable();

            return this;
        }
    }
}
