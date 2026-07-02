using Moq;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Web.Tests.Helpers
{
    public static class CommonMocks
    {
        public static Mock<IGlobalSettingService> GlobalSettingService()
        {
            var returnObject = new Mock<IGlobalSettingService>(MockBehavior.Strict);
            returnObject.Setup(s => s.GetFirstOrDefault(9)).ReturnsAsync(new GlobalSetting { Value = "False" });
            returnObject.Setup(s => s.GetFirstOrDefault(13)).ReturnsAsync(new GlobalSetting { Value = "False" });
            returnObject.Setup(s => s.GetFirstOrDefault(14)).ReturnsAsync(new GlobalSetting { Value = "True" });

            return returnObject;
        }
    }
}