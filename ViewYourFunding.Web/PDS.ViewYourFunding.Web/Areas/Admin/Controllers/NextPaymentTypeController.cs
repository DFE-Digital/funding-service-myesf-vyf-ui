using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NextPaymentType = PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPaymentType.NextPaymentType;
using Service = PDS.ViewYourFunding.Services.Models;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The UI controller for managing the funding stream next Payment Types in the View Your Funding area.
    /// </summary>
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [Area("Admin")]
    public class NextPaymentTypeController : BaseController
    {
        #region Private fields

        /// <summary>
        /// The view your funding next Payment Type service.
        /// </summary>
        private readonly INextPaymentTypeService _viewYourFundingNextPaymentTypeService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The next payment action strategy.
        /// </summary>
        private readonly NextPaymentActionStrategy _nextPaymentActionStrategy;

        /// <summary>
        /// The view your funding next Payment Type service.
        /// </summary>
        private readonly IAdminSettingsService _adminSettingsService;


        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NextPaymentTypeController"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding settings.</param>
        /// <param name="viewYourFundingNextPaymentTypeService">The view your funding next Payment type service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        /// <param name="logger">The logger service.</param>
        public NextPaymentTypeController(
            INextPaymentTypeService viewYourFundingNextPaymentTypeService,
            IAdminSettingsService adminSettingsService,
            IMapper mapper,
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            ILoggerAdapter<NextPaymentTypeActionBase> logger)
           : base(securityService, applicationConfigurationOptions.Value.IsProductionEnvironment)
        {
            _viewYourFundingNextPaymentTypeService = viewYourFundingNextPaymentTypeService;
            _adminSettingsService = adminSettingsService;
            _mapper = mapper;
            _nextPaymentActionStrategy = NextPaymentStrategyFactory.GetNextPaymentTypeActionStrategy(
                _adminSettingsService,
                viewYourFundingNextPaymentTypeService,
                mapper,
                logger);
        }

        #endregion


        #region Actions

        /// <summary>
        /// Get the next payment types by funding stream id.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <returns>The MVC view result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentTypeIndex, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentTypeIndex)]
        public virtual async Task<IActionResult> Index(int fundingStreamId)
        {
            var fundingStream = await _adminSettingsService.GetFundingStreamById(fundingStreamId, FetchData.NextPaymentTypes_NextPayments);

            if (fundingStream != null)
            {
                var viewModel = await GetBasePageViewModel<NextPaymentTypesIndexViewModel>();
                viewModel.NextPaymentTypes = GetNextPaymentTypes(fundingStream.NextPaymentTypes);
                viewModel.FundingStreamId = fundingStream.Id;
                viewModel.FundingStreamName = fundingStream.FundingStreamName;
                return View(viewModel);
            }

            return FallBackActionResult(fundingStreamId);
        }

        /// <summary>Get the view result for the given next Payment Type action.</summary>
        /// <param name="nextPaymentTypeId">The next Payment Type identifier.</param>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>The View result.</returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentTypeAction, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentTypeAction)]
        public virtual async Task<IActionResult> Action(int nextPaymentTypeId, int fundingStreamId, ActionMode actionMode)
        {
            var viewModel = await _nextPaymentActionStrategy.NextPaymentTypeActions.First(x => x.AppliesTo(actionMode))
                .Action(fundingStreamId, nextPaymentTypeId);

            if (viewModel != null)
            {
                var baseViewModel = await GetBasePageViewModel<NextPaymentTypeViewModel>();
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
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentTypeAction, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentTypeAction)]
        public virtual async Task<ActionResult> Action(NextPaymentTypeViewModel model)
        {
            var baseViewModel = await GetBasePageViewModel<NextPaymentTypeViewModel>();
            model.CurrentUser = baseViewModel.CurrentUser;
            if (ModelState.IsValid)
            {
                return AreYouSure(model);
            }

            return View(model);
        }

        /// <summary>
        /// The 'Are you sure?' page for confirming the action on the next Payment Type.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The MVC view result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentTypeAreYouSure, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentTypeAreYouSure)]
        public virtual ActionResult AreYouSure(NextPaymentTypeViewModel model)
        {
            model.IsAreYouSurePage = true;
            return View("AreYouSure", model);
        }

        /// <summary>
        /// Persist the nextPaymentType action.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The redirect to the confirmation page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentTypeSaveChanges, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentTypeSaveChanges)]
        public virtual async Task<IActionResult> SaveChanges(NextPaymentTypeViewModel model)
        {
            var baseViewModel = await GetBasePageViewModel<NextPaymentTypeViewModel>();
            model.NextPaymentType.LastUpdatedBy = baseViewModel.CurrentUser?.FullName;

            var changesSaved = await _nextPaymentActionStrategy.NextPaymentTypeActions.First(x => x.AppliesTo(model.ActionMode))
                .SaveChanges(model);

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminNextPaymentTypeConfirmation, new
            {
                fundingStreamId = model.FundingStreamId,
                fundingStreamName = model.FundingStreamName,
                actionMode = model.ActionMode,
                nextPaymentTypeId = model.NextPaymentType.Id,
                changesSaved
            });
        }

        /// <summary>
        /// Confirmation of the save operation.
        /// </summary>
        /// <param name="nextPaymentTypeId">The next Payment Type identifier.</param>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="changesSaved">True is changes are saved.</param>
        /// <returns>The MVC view result.</returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentTypeConfirmation, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentTypeConfirmation)]
        public virtual async Task<IActionResult> Confirmation(int nextPaymentTypeId, int fundingStreamId, string fundingStreamName, ActionMode actionMode, bool changesSaved)
        {
            var viewModel = await GetBasePageViewModel<NextPaymentTypeViewModel>();

            var submittedAtDisplayDate = string.Empty;
            if (changesSaved && actionMode != ActionMode.Delete)
            {
                var nextPaymentTypes = await _viewYourFundingNextPaymentTypeService.GetNextPaymentTypes(fundingStreamId);
                submittedAtDisplayDate = nextPaymentTypes.FirstOrDefault(x => x.Id == nextPaymentTypeId)?.LastUpdatedAt
                    .ToGmtStandardTime().ToDateTimeDisplayWithAt();
            }

            viewModel.FundingStreamId = fundingStreamId;
            viewModel.FundingStreamName = fundingStreamName;
            viewModel.ActionMode = actionMode;
            viewModel.ChangesSaved = changesSaved;
            viewModel.SubmittedAtDisplayDate = submittedAtDisplayDate;

            return View(viewModel);
        }

        #endregion


        #region Private Helpers

        /// <summary>
        /// Gets the next payment types.
        /// </summary>
        /// <param name="nextPaymentTypes">The next payment types.</param>
        /// <returns>The next payment type list.</returns>
        private IEnumerable<NextPaymentType> GetNextPaymentTypes(IEnumerable<Service.NextPaymentType> nextPaymentTypes)
        {
            return _mapper.Map<IEnumerable<NextPaymentType>>(nextPaymentTypes);
        }

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