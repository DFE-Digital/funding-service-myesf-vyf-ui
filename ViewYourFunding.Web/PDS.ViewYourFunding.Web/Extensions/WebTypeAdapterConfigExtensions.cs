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
    /// <summary>
    /// Provides extension methods for configuring Mapster mappings
    /// used by the Web layer.
    /// </summary>
    public static class WebTypeAdapterConfigExtensions
    {
        /// <summary>
        /// Registers mappings between service models, view models,
        /// area models, and request models.
        /// </summary>
        /// <param name="config">
        /// The <see cref="TypeAdapterConfig"/> instance to configure.
        /// </param>
        public static void ConfigureWebMappings(this TypeAdapterConfig config)
        {
                TypeAdapterConfig.GlobalSettings.AllowImplicitSourceInheritance = true;

                config.Default.AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);
                config.Default.PreserveReference(true);

                config.NewConfig<NextPaymentType, Area.Models.NextPaymentType.NextPaymentType>()
                    .Map(
                        dest => dest.IsNextPaymentTypeInUse,
                        src => src.NextPayments != null ? src.NextPayments.Any() : false)
                    .TwoWays();

                config.NewConfig<Services.Models.SettingType, SettingType>()
                .Map(
                    dest => dest.IsSettingTypeInUse,
                    src => src.SettingValues != null ? src.SettingValues.Any() : false)
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