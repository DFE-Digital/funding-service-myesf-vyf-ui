using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PDS.ViewYourFunding.Repositories.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Builder;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Rules;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Home.Tiles.Validators;
using PDS.ViewYourFunding.Web.Constants;
using System.Collections.Generic;

namespace PDS.ViewYourFunding.Web.Config
{
    /// <summary>
    /// The Container Tile Builder extensions class.
    /// </summary>
    public static class ContainerTileBuilderExtensions
    {
        /// <summary>
        /// Registers the tile dependencies.
        /// </summary>
        /// <param name="containerBuilder">The input container builder.</param>
        /// <returns>The container builder.</returns>
        public static ContainerBuilder RegisterTileDependencies(this ContainerBuilder containerBuilder)
        {
            // Register Sfs Admin rules
            containerBuilder.Register(c =>
                new UserMustHaveSfsAdminRoleRule())
                .Named<IRule>(TileBuilderConstants.SfsAdminRulesKey).As<IRule>().SingleInstance();

            // Register Digital Allocation Rules
            containerBuilder.Register(c =>
                new ProviderMustHaveUkprnRule())
                .Named<IRule>(TileBuilderConstants.DigitalAllocationsRulesKey).As<IRule>().SingleInstance();

            containerBuilder.Register(c =>
                new DigitalAllocationTileSettingMustBeEnabledRule(
                    c.Resolve<IDynamicSettingsService>()))
                .Named<IRule>(TileBuilderConstants.DigitalAllocationsRulesKey).As<IRule>().SingleInstance();

            // Register tiles
            containerBuilder.Register(c =>
            {
                var sfsAdminRules = c.ResolveNamed<IEnumerable<IRule>>(TileBuilderConstants.SfsAdminRulesKey);
                return new GlobalSettingsTile(
                    new TileDisplayRuleValidator(sfsAdminRules),
                    null,
                    c.Resolve<LinkGenerator>(),
                    c.Resolve<IHttpContextAccessor>());
            }).As<ITile>().SingleInstance();


            containerBuilder.Register(c =>
            {
                var sfsAdminRules = c.ResolveNamed<IEnumerable<IRule>>(TileBuilderConstants.SfsAdminRulesKey);
                return new SettingsTile(
                    new TileDisplayRuleValidator(sfsAdminRules),
                    null,
                    c.Resolve<LinkGenerator>(),
                    c.Resolve<IHttpContextAccessor>());
            }).As<ITile>().SingleInstance();

            containerBuilder.Register(c =>
            {
                var sfsAdminRules = c.ResolveNamed<IEnumerable<IRule>>(TileBuilderConstants.SfsAdminRulesKey);
                return new LayoutManagementTile(
                    new TileDisplayRuleValidator(sfsAdminRules),
                    null,
                    c.Resolve<LinkGenerator>(),
                    c.Resolve<IHttpContextAccessor>());
            }).As<ITile>().SingleInstance();

            containerBuilder.Register(c =>
            {
                var sfsAdminRules = c.ResolveNamed<IEnumerable<IRule>>(TileBuilderConstants.SfsAdminRulesKey);
                return new SettingTypesTile(
                    new TileDisplayRuleValidator(sfsAdminRules),
                    null,
                    c.Resolve<LinkGenerator>(),
                    c.Resolve<IHttpContextAccessor>());
            }).As<ITile>().SingleInstance();

            containerBuilder.Register(c =>
            {
                var sfsAdminRules = c.ResolveNamed<IEnumerable<IRule>>(TileBuilderConstants.SfsAdminRulesKey);
                return new PdfGenerationActionsTile(
                    new TileDisplayRuleValidator(sfsAdminRules),
                    null,
                    c.Resolve<LinkGenerator>(),
                    c.Resolve<IHttpContextAccessor>());
            }).As<ITile>().SingleInstance();

            // Register tile builder
            containerBuilder.Register(c => new HomeTileBuilder(
                c.Resolve<IEnumerable<ITile>>()))
            .As<ITileBuilder>().SingleInstance();

            return containerBuilder;
        }
    }
}