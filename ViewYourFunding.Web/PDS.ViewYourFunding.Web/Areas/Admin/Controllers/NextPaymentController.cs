using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using Pds.Core.Logging;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Repositories.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.NextPayment;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.Publication;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.NextPayments;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The UI controller for managing the funding stream next Payment Types in the View Your Funding area.
    /// </summary>
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [Area("Admin")]
    public class NextPaymentController : BaseController
    {
        #region Private fields

        /// <summary>
        /// The view your funding next Payment service.
        /// </summary>
        private readonly INextPaymentService _nextPaymentService;

        /// <summary>
        /// The view your funding next Payment Type service.
        /// </summary>
        private readonly INextPaymentTypeService _nextPaymentTypeService;

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
        /// Initializes a new instance of the <see cref="NextPaymentController"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The view your funding settings.</param>
        /// <param name="nextPaymentService">The view your funding next Payment service.</param>
        /// <param name="nextPaymentTypeService">The view your funding next Payment type service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        /// <param name="logger">The logger service.</param>
        public NextPaymentController(
            IAdminSettingsService adminSettingsService,
            INextPaymentService nextPaymentService,
            INextPaymentTypeService nextPaymentTypeService,
            IMapper mapper,
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions,
            ILoggerAdapter<NextPaymentActionBase> logger)
           : base(securityService, applicationConfigurationOptions.Value.IsProductionEnvironment)
        {
            _adminSettingsService = adminSettingsService;
            _nextPaymentService = nextPaymentService;
            _nextPaymentTypeService = nextPaymentTypeService;
            _nextPaymentActionStrategy = NextPaymentStrategyFactory.GetNextPaymentActionStrategy(
                _adminSettingsService,
                _nextPaymentService,
                _nextPaymentTypeService,
                mapper,
                logger);
        }

        #endregion


        #region Actions

        /// <summary>
        /// Get the next payments for the funding stream id.
        /// </summary>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <returns>The view result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentIndex, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentIndex)]
        public virtual async Task<IActionResult> Index(int fundingStreamId)
        {
            var fundingStream = await _adminSettingsService.GetFundingStreamById(fundingStreamId, FetchData.NextPayments_NextPaymentType, FetchData.NextPaymentTypes_NextPayments);

            if (fundingStream != null)
            {
                var viewModel = await GetBasePageViewModel<NextPaymentsIndexViewModel>();
                viewModel.NextPayments = GetNextPayments(fundingStream.NextPayments);
                viewModel.FundingStreamId = fundingStream.Id;
                viewModel.FundingStreamName = fundingStream.FundingStreamName;
                return View(viewModel);
            }

            return FallBackActionResult(fundingStreamId);
        }

        /// <summary>Get the view result for the given next Payment action.</summary>
        /// <param name="nextPaymentId">The next Payment identifier.</param>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <returns>The View result.</returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentAction, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentAction)]
        public virtual async Task<IActionResult> Action(int nextPaymentId, int fundingStreamId, ActionMode actionMode)
        {
            var viewModel = await _nextPaymentActionStrategy.NextPaymentActions.First(x => x.AppliesTo(actionMode))
                .Action(fundingStreamId, nextPaymentId);

            if (viewModel != null)
            {
                var baseViewModel = await GetBasePageViewModel<NextPaymentViewModel>();
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
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentAction, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentAction)]
        public virtual async Task<IActionResult> Action(NextPaymentViewModel model)
        {
            var baseViewModel = await GetBasePageViewModel<NextPaymentViewModel>();
            model.CurrentUser = baseViewModel.CurrentUser;
            if (ModelState.IsValid)
            {
                model.NextPayment = await GetNextPayment(model.NextPayment);
                return AreYouSure(model);
            }

            model.NextPayment.NextPaymentTypes = await GetNextPaymentTypes();
            return View(model);
        }

        /// <summary>
        /// The 'Are you sure?' page for confirming the action on the next Payment.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The MVC view result.</returns>
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentAreYouSure, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentAreYouSure)]
        public virtual ActionResult AreYouSure(NextPaymentViewModel model)
        {
            model.IsAreYouSurePage = true;
            return View("AreYouSure", model);
        }

        /// <summary>
        /// Persist the next Payment action.
        /// </summary>
        /// <param name="model">The view model for the page.</param>
        /// <returns>The redirect to the confirmation page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentSaveChanges, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentSaveChanges)]
        public virtual async Task<IActionResult> SaveChanges(NextPaymentViewModel model)
        {
            var baseViewModel = await GetBasePageViewModel<NextPaymentViewModel>();
            model.NextPayment.LastUpdatedBy = baseViewModel.CurrentUser?.FullName;

            var changesSaved = await _nextPaymentActionStrategy.NextPaymentActions.First(x => x.AppliesTo(model.ActionMode))
                .SaveChanges(model);

            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminNextPaymentConfirmation, new
            {
                fundingStreamId = model.FundingStreamId,
                fundingStreamName = model.FundingStreamName,
                actionMode = model.ActionMode,
                nextPaymentId = model.NextPayment.Id,
                changesSaved
            });
        }

        /// <summary>
        /// Confirmation of the save operation.
        /// </summary>
        /// <param name="nextPaymentId">The next Payment identifier.</param>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="fundingStreamName">The funding stream name.</param>
        /// <param name="actionMode">The action mode.</param>
        /// <param name="changesSaved">Set to true if the changes have been saved.</param>
        /// <returns>The MVC view result.</returns>
        [HttpGet]
        [Route(ViewYourFundingConstants.Route_AdminNextPaymentConfirmation, Name = ViewYourFundingConstants.RouteName_AdminNextPaymentConfirmation)]
        public virtual async Task<ActionResult> Confirmation(int nextPaymentId, int fundingStreamId, string fundingStreamName, ActionMode actionMode, bool changesSaved)
        {
            var viewModel = await GetBasePageViewModel<NextPaymentViewModel>();

            var submittedAtDisplayDate = string.Empty;
            if (changesSaved && actionMode != ActionMode.Delete)
            {
                var nextPayments = await _nextPaymentService.GetNextPayments(fundingStreamId);
                submittedAtDisplayDate = nextPayments.FirstOrDefault(x => x.Id == nextPaymentId)?.LastUpdatedAt
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
        /// Gets the next payments.
        /// </summary>
        /// <param name="nextPayments">The next payments.</param>
        /// <returns>A list of The next payments.</returns>
        private static IEnumerable<NextPayment> GetNextPayments(IEnumerable<Services.Models.NextPayment> nextPayments)
        {
            return nextPayments.OrderBy(x => x.NextPaymentDate).Select(nextPayment => new NextPayment
            {
                FundingStreamId = nextPayment.FundingStreamId,
                NextPaymentDateDay = nextPayment.NextPaymentDate.Day.ToString(),
                NextPaymentDateMonth = nextPayment.NextPaymentDate.Month.ToString(),
                NextPaymentDateYear = nextPayment.NextPaymentDate.Year.ToString(),
                NextPaymentTypeCode = nextPayment.NextPaymentType.TypeCode,
                FundingPeriodCode = nextPayment.FundingPeriodCode,
                NextPaymentTypeDescription = nextPayment.NextPaymentType.Description,
                NextPaymentTypeId = nextPayment.NextPaymentTypeId,
                Active = nextPayment.Active,
                Id = nextPayment.Id
            });
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

        /// <summary>
        /// Gets the next payment.
        /// </summary>
        /// <param name="modelNextPayment">The model next payment.</param>
        /// <returns>The next payment.</returns>
        private async Task<NextPayment> GetNextPayment(NextPayment modelNextPayment)
        {
            var nextPaymentTypes = await _nextPaymentTypeService
                .GetAllNextPaymentTypes();

            var nextPaymentType = nextPaymentTypes.FirstOrDefault(x => x.Id == modelNextPayment.NextPaymentTypeId);

            modelNextPayment.NextPaymentTypeCode = nextPaymentType?.TypeCode;
            modelNextPayment.NextPaymentTypeDescription = nextPaymentType?.Description;
            return modelNextPayment;
        }

        /// <summary>
        /// Gets the next payment types.
        /// </summary>
        /// <returns>
        /// The select list of Next Payment types.
        /// </returns>
        private async Task<IEnumerable<SelectListItem>> GetNextPaymentTypes()
        {
            var nextPaymentTypes = await _nextPaymentTypeService.GetAllNextPaymentTypes();
            return nextPaymentTypes.Select(nextPaymentType => new SelectListItem
            {
                Text = $@"{nextPaymentType.TypeCode} - {nextPaymentType.Description}",
                Value = nextPaymentType.Id.ToString()
            });
        }
        #endregion

    }
}