using AutoMapper;
using PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Services.Config
{
    /// <summary>
    /// The Services Auto Mapper Profile.
    /// </summary>
    /// <seealso cref="Profile" />
    public class ServicesAutoMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServicesAutoMapperProfile"/> class.
        /// </summary>
        public ServicesAutoMapperProfile()
        {
            CreateMap<Repositories.DataModels.FundingStream, FundingStream>().ReverseMap();
            CreateMap<Repositories.DataModels.PublicationLayout, PublicationLayout>().ReverseMap();
            CreateMap<Repositories.DataModels.Publication, Publication>().ReverseMap();
            CreateMap<Repositories.DataModels.Setting, Setting>().ReverseMap();
            CreateMap<Repositories.DataModels.SettingValue, SettingValue>().ReverseMap();
            CreateMap<Repositories.DataModels.NextPayment, NextPayment>().ReverseMap();
            CreateMap<Repositories.DataModels.NextPaymentType, NextPaymentType>().ReverseMap();
            CreateMap<Repositories.DataModels.GlobalSetting, GlobalSetting>().ReverseMap();
            CreateMap<Repositories.DataModels.Setting, SettingType>().ReverseMap();
            CreateMap<NextPayment, Repositories.DataModels.NextPayment>().ReverseMap()
                .ForMember(dest => dest.NextPaymentTypeDescription, opt => opt.MapFrom(src => src.NextPaymentType.Description))
                .ForMember(dest => dest.NextPaymentTypeCode, opt => opt.MapFrom(src => src.NextPaymentType.TypeCode));
        }
    }
}