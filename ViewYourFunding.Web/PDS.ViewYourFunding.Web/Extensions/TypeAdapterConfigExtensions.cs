using Mapster;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Areas.LoggedIn.Models.Requests;
using PDS.VYF.Services.Models.RequestModels.ViewDataRequestModels;
using System.Linq;

using Area = PDS.ViewYourFunding.Web.Areas.Admin;
using GlobalSetting = PDS.ViewYourFunding.Web.Areas.Admin.Models.GlobalSetting.GlobalSetting;
using Pagination = PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement.Pagination;
using SettingType = PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType.SettingType;
using SettingValue = PDS.ViewYourFunding.Web.Areas.Admin.Models.SettingType.SettingValue;

namespace PDS.ViewYourFunding.Web.Extensions
{
    public static class TypeAdapterConfigExtensions
    {
        public static void Configure(this TypeAdapterConfig config)
        {
            config.NewConfig<Repositories.DataModels.FundingStream, FundingStream>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.PublicationLayout, PublicationLayout>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.Publication, Publication>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.Setting, Setting>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.SettingValue, SettingValue>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.NextPaymentType, NextPaymentType>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.GlobalSetting, GlobalSetting>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.Setting, SettingType>()
                .TwoWays();

            config.NewConfig<Repositories.DataModels.NextPayment, NextPayment>()
                .Map(
                    dest => dest.NextPaymentTypeDescription,
                    src => src.NextPaymentType.Description)
                .Map(
                    dest => dest.NextPaymentTypeCode,
                    src => src.NextPaymentType.TypeCode)
                .TwoWays();

            config.NewConfig<Publication, PublicationViewModel>()
                .TwoWays();

            config.NewConfig<NextPaymentType, Area.Models.NextPaymentType.NextPaymentType>()
                .Map(
                    dest => dest.IsNextPaymentTypeInUse,
                    src => src.NextPayments.Any())
                .TwoWays();

            config.NewConfig<NextPayment, Area.Models.NextPayment.NextPayment>()
                .TwoWays();

            config.NewConfig<Services.Models.GlobalSetting, GlobalSetting>()
                .TwoWays();

            config.NewConfig<Services.Models.GlobalSetting, Models.GlobalSetting.GlobalSetting>()
                .TwoWays();

            config.NewConfig<LayoutImportViewModel, LayoutFileImportViewModel>()
                .TwoWays();

            config.NewConfig<FundingStream, Area.Models.FundingStream.FundingStream>()
                .TwoWays();

            config.NewConfig<FundingStream, Web.Models.FundingStream.FundingStream>()
                .TwoWays();

            config.NewConfig<Services.Models.SettingType, SettingType>()
                .Map(
                    dest => dest.IsSettingTypeInUse,
                    src => src.SettingValues.Any())
                .TwoWays();

            config.NewConfig<Services.Models.SettingValue, SettingValue>()
                .TwoWays();

            config.NewConfig<Services.Models.Pagination, Pagination>()
                .TwoWays();

            config.NewConfig<ChildDetailedViewDataRequestModel, ProviderFundingBreakdownRequest>()
                .Map(
                    dest => dest.Tab,
                    src => src.SelectedTab)
                .Map(
                    dest => dest.Ukprn,
                    src => src.UkprnFromRoute)
                .TwoWays();
        }
    }
}