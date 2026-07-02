using AutoMapper;

namespace PDS.ViewYourFunding.Repositories.Config
{
    /// <summary>
    /// The Repositories AutoMapper Profile.
    /// </summary>
    /// <seealso cref="Profile" />
    public class RepositoriesAutoMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoriesAutoMappingProfile"/> class.
        /// </summary>
        public RepositoriesAutoMappingProfile()
        {
            //CreateMap<DataModels.FundingStream, FundingStream>().ReverseMap();
            //CreateMap<DataModels.SettingValue, SettingValue>().ReverseMap();
            //CreateMap<DataModels.Setting, Setting>().ReverseMap();
            //CreateMap<DataModels.Publication, Publication>().ReverseMap();
            //CreateMap<NextPayment, DataModels.NextPayment>().ReverseMap()
            //    .ForMember(dest => dest.NextPaymentTypeDescription, opt => opt.MapFrom(src => src.NextPaymentType.Description))
            //    .ForMember(dest => dest.NextPaymentTypeCode, opt => opt.MapFrom(src => src.NextPaymentType.TypeCode));
        }
    }
}