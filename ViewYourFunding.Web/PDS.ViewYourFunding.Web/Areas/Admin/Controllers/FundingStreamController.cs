using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.FundingStream;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStream;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The UI controller for managing the funding streams in the View Your Funding area.
    /// </summary>
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [Area("Admin")]
    public class FundingStreamController : BaseController
    {
        #region Private fields

        /// <summary>
        /// The view your funding admin settings  service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;

        /// <summary>
        /// The publication action strategy.
        /// </summary>
        private readonly FundingStreamActionStrategy _fundingStreamActionStrategy;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingStreamController"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding admin settings.</param>
        /// <param name="fundingStreamService">The view your funding stream settings.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        /// <param name="logger">The logger service.</param>
        public FundingStreamController(
            IAdminSettingsService adminSettingsService,
            IFundingStreamService fundingStreamService,
            IMapper mapper,
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            ILoggerAdapter<FundingStreamActionBase> logger)
           : base(securityService, applicationConfigurationOptions.Value.IsProductionEnvironment)
        {
            _adminSettingsService = adminSettingsService;
            _fundingStreamActionStrategy = FundingStreamStrategyFactory.GetFundingStreamActionStrategy(
               _adminSettingsService,
               fundingStreamService,
               mapper,
               logger);
        }

        #endregion


        #region Actions

        /// <summary>Get the view result for the given funding stream action.</summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>The View result.</returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminFundingStreamAction,
            Name = ViewYourFundingConstants.RouteName_AdminFundingStreamAction)]
        public virtual async Task<IActionResult> Action(int fundingStreamId, ActionMode actionMode)
        {
            var viewModel = await _fundingStreamActionStrategy.FundingStreamActions
                .First(fsAction => fsAction.AppliesTo(actionMode))
                .Action(fundingStreamId);

            if (viewModel != null)
            {
                var baseViewModel = await GetBasePageViewModel<FundingStreamViewModel>();
                viewModel.CurrentUser = baseViewModel.CurrentUser;
                if (actionMode == ActionMode.Delete)
                {
                    return View("AreYouSure", viewModel);
                }

                return View(viewModel);
            }

            return FallBackActionResult(fundingStreamId);
        }

        /// <summary>
        /// Process the submitted action model.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The Are you Sure page view result if the model is valid.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(ViewYourFundingConstants.Route_AdminFundingStreamAction, Name = ViewYourFundingConstants.RouteName_AdminFundingStreamAction)]
        public virtual async Task<ActionResult> Action(FundingStreamViewModel model)
        {
            var viewModel = await GetBasePageViewModel<FundingStreamViewModel>();

            model.CurrentUser = viewModel.CurrentUser;
            if (ModelState.IsValid)
            {
                return AreYouSure(model);
            }

            return View(model);
        }

        /// <summary>
        /// The 'Are you sure?' page for confirming the action on the funding stream.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The MVC view result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminFundingStreamAreYouSure, Name = ViewYourFundingConstants.RouteName_AdminFundingStreamAreYouSure)]
        public virtual ActionResult AreYouSure(FundingStreamViewModel model)
        {
            model.IsAreYouSurePage = true;
            return View("AreYouSure", model);
        }

        /// <summary>
        /// Persist the funding stream action.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The redirect to the confirmation page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(ViewYourFundingConstants.Route_AdminFundingStreamSaveChanges, Name = ViewYourFundingConstants.RouteName_AdminFundingStreamSaveChanges)]
        public virtual async Task<IActionResult> SaveChanges(FundingStreamViewModel model)
        {
            var baseViewModel = await GetBasePageViewModel<FundingStreamViewModel>();
            model.FundingStream.LastUpdatedBy = baseViewModel.CurrentUser?.FullName;

            var changesSaved = await _fundingStreamActionStrategy.FundingStreamActions.First(x => x.AppliesTo(model.ActionMode))
                .SaveChanges(model);

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminFundingStreamConfirmation, new
            {
                fundingStreamId = model.FundingStream.Id,
                fundingStreamName = model.FundingStream.FundingStreamName,
                actionMode = model.ActionMode,
                changesSaved
            });
        }


        /// <summary>
        /// Confirmation of the save operation.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="changesSaved">Set to true if the changes have been saved.</param>
        /// <returns>The MVC view result.</returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_AdminFundingStreamConfirmation, Name = ViewYourFundingConstants.RouteName_AdminFundingStreamConfirmation)]
        public virtual async Task<ActionResult> Confirmation(int fundingStreamId, string fundingStreamName, ActionMode actionMode, bool changesSaved)
        {
            var viewModel = await GetBasePageViewModel<FundingStreamViewModel>();

            var submittedAtDisplayDate = string.Empty;
            if (changesSaved && actionMode != ActionMode.Delete)
            {
                var fundingStream = await _adminSettingsService.GetFundingStreamById(fundingStreamId);
                submittedAtDisplayDate = fundingStream?.LastUpdatedAt.ToGmtStandardTime().ToDateTimeDisplayWithAt();
            }

            viewModel.FundingStream = new FundingStream
            {
                Id = fundingStreamId,
                FundingStreamName = fundingStreamName
            };

            viewModel.ActionMode = actionMode;
            viewModel.ChangesSaved = changesSaved;
            viewModel.SubmittedAtDisplayDate = submittedAtDisplayDate;

            return View(viewModel);
        }

        #endregion


        #region Private Helpers

        /// <summary>Returns the fall back action result.</summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <returns>The Redirect Action Result.</returns>
        private ActionResult FallBackActionResult(int fundingStreamId)
        {
            if (fundingStreamId != 0)
            {
                return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminSettingsFundingStream, new { fundingStreamId });
            }

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminSettingsHome);
        }

        #endregion
    }
}