using AutoMapper;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests;
using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
using System.Linq;
using Area = PDS.ViewYourFunding.Web.Areas.Admin;
using GlobalSetting = PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSetting.GlobalSetting;
using SettingType = PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType.SettingType;
using SettingValue = PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType.SettingValue;

namespace PDS.ViewYourFunding.Web.Config
{
    /// <summary>
    /// The Web AutoMapper Profile.
    /// </summary>
    /// <seealso cref="Profile" />
    public class WebAutoMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebAutoMapperProfile"/> class.
        /// </summary>
        public WebAutoMapperProfile()
        {
            CreateMap<Publication, PublicationViewModel>()
                .ReverseMap();
            CreateMap<NextPaymentType, Area.Models.NextPaymentType.NextPaymentType>()
                .ForMember(x => x.IsNextPaymentTypeInUse, o => o.MapFrom(x => x.NextPayments.Any()))
                .ReverseMap();
            CreateMap<NextPayment, Area.Models.NextPayment.NextPayment>()
                .ReverseMap();
            CreateMap<Services.Models.GlobalSetting, GlobalSetting>().ReverseMap();
            CreateMap<Services.Models.GlobalSetting, Models.GlobalSetting.GlobalSetting>().ReverseMap();
            CreateMap<LayoutImportViewModel, LayoutFileImportViewModel>().ReverseMap();
            CreateMap<FundingStream, Area.Models.FundingStream.FundingStream>().ReverseMap();
            CreateMap<FundingStream, Web.Models.FundingStream.FundingStream>().ReverseMap();
            CreateMap<Services.Models.SettingType, SettingType>()
                .ForMember(settingType => settingType.IsSettingTypeInUse, o => o.MapFrom(settingType => settingType.SettingValues.Any()))
                .ReverseMap();
            CreateMap<Services.Models.SettingValue, SettingValue>().ReverseMap();
            CreateMap<Services.Models.Pagination, Area.Models.LayoutManagement.Pagination>().ReverseMap();
            CreateMap<ChildDetailedViewDataRequestModel, ProviderFundingBreakdownRequest>()
                .ForMember(a => a.Tab, o => o.MapFrom(serviceRequest => serviceRequest.SelectedTab))
                .ForMember(a => a.Ukprn, o => o.MapFrom(serviceRequest => serviceRequest.UkprnFromRoute))
                .ReverseMap();
        }
    }
}