using Moq;
using PDS.VYF.Services.Abstracts.InfraServices.DataApiClientServices;
using PDS.VYF.Services.Models.RequestModels.DataApiRequestModels;
using PDS.VYF.Services.Models.ResponseModels.DataApiResponseModels;

namespace PDS.VYF.Services.Tests.Mocks.InfraServices.DataApiClientServices
{
    public class MockParentApiClientServices : MockBase<IParentApiClientServices>
    {
        public MockParentApiClientServices()
            : base(false)
        {
        }

        public override void SetupDefault()
        {
            throw new NotImplementedException();
        }

        public MockParentApiClientServices SetupIsParent(string ukprn, bool output)
        {
            this
                .Setup(a => a.IsParent(ukprn, It.IsAny<bool>(), It.IsAny<List<string>?>()))
                .ReturnsAsync(output)
                .Verifiable();

            return this;
        }

        public MockParentApiClientServices SetupIsMyChild(string parentUkprn, string childUkprn, bool output)
        {
            this
                .Setup(a => a.IsMyChild(parentUkprn, childUkprn, It.IsAny<bool>(), It.IsAny<List<string>?>()))
                .ReturnsAsync(output)
                .Verifiable();

            return this;
        }

        public MockParentApiClientServices SetupSearchParent(List<LoggedInParentModel> output)
        {
            var exp = this.CallTo(a => a.SearchParent(It.IsAny<ParentSearchApiRequestModel>(), It.IsAny<bool>(), It.IsAny<List<string>?>()));

            this
                .Setup(exp)
                .ReturnsAsync(output)
                .Verifiable();

            return this;
        }

        public MockParentApiClientServices VerifySearchParent(int callCount)
        {
            var exp = this.CallTo(a => a.SearchParent(It.IsAny<ParentSearchApiRequestModel>(), It.IsAny<bool>(), It.IsAny<List<string>?>()));

            this
                .Verify(exp, Times.Exactly(callCount));

            return this;
        }
    }
}
