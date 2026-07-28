using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pds.Core.Common.Identity.Enums;
using Pds.Core.Identity.Claims.Interfaces;
using PDS.ViewYourFunding.Core.Configuration;
using PDS.ViewYourFunding.Services.Constants;
using PDS.ViewYourFunding.Services.Enums;
using PDS.ViewYourFunding.Services.Helper;
using PDS.ViewYourFunding.Services.Interfaces;
using PDS.ViewYourFunding.Services.Models;
using PDS.ViewYourFunding.Web.Areas.Admin.Enums;
using PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.FundingStreamSettings;
using PDS.ViewYourFunding.Web.Areas.Admin.Strategies.LayoutManagement;
using PDS.ViewYourFunding.Web.Controllers;
using PDS.ViewYourFunding.Web.Models.ViewYourFunding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filter = PDS.ViewYourFunding.Web.Areas.Admin.Models.LayoutManagement.Filter;

namespace PDS.ViewYourFunding.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// The UI controller for managing the layout settings.
    /// </summary>
    /// <seealso cref="BaseController" />
    [Authorize(Policy = nameof(UserRole.SfsAdmin))]
    [Area("Admin")]
    public class LayoutManagementController : BaseController
    {
        #region Private Fields

        private const int PageSize = 10;
        private readonly IAdminSettingsService _adminSettingsService;
        private readonly IAdminPublicationService _publicationService;
        private readonly IMapper _mapper;
        private readonly ILayoutManagementService _layoutManagementService;
        private readonly PreviewLayoutActionStrategy _previewLayoutActionStrategy;

        private IReadOnlyList<FundingStream> _fundingStreamList = new List<FundingStream>();
        private List<Publication> _publications = new List<Publication>();

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutManagementController"/> class.
        /// </summary>
        /// <param name="adminSettingsService">The admin settings service.</param>
        /// <param name="publicationService">The publication service.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="layoutManagementService">The layout management service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="applicationConfigurationOptions">The application configuration options.</param>
        public LayoutManagementController(
            IAdminSettingsService adminSettingsService,
            IAdminPublicationService publicationService,
            IMapper mapper,
            ILayoutManagementService layoutManagementService,
            IClaimsBasedIdentityService securityService,
            IOptions<ApplicationConfiguration> applicationConfigurationOptions)
            : base(securityService, applicationConfigurationOptions.Value.IsProductionEnvironment)
        {
            _adminSettingsService = adminSettingsService;
            _publicationService = publicationService;
            _mapper = mapper;
            _layoutManagementService = layoutManagementService;
            _previewLayoutActionStrategy =
                PreviewLayoutStrategyFactory.GetFundingViewScopeActionStrategy(adminSettingsService);
        }

        #endregion


        #region Actions

        /// <summary>
        /// The start action for layout management pages.
        /// </summary>
        /// <param name="layoutFilter">layout Filters.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [Route(
            ViewYourFundingConstants.Route_AdminLayoutManagementHome,
            Name = ViewYourFundingConstants.RouteName_AdminLayoutManagementHome)]
        public virtual async Task<IActionResult> Index(LayoutFilter layoutFilter)
        {
            var viewModel = await GetBasePageViewModel<LayoutManagementHomePageViewModel>();

            _fundingStreamList = await GetAllFundingStreams();
            _fundingStreamList.ToList().ForEach(fs => _publications.AddRange(fs.Publications));

            var fundingStreamsIds = layoutFilter?.FundingStreams?.Select(fundingStream => fundingStream.Id).ToList();

            var fundingViewTypes = layoutFilter?.FundingViewTypes?
                .Select(fundingViewType => ((FundingViewType)fundingViewType.Id).ToString()).ToList();

            var fundingViewScopes = layoutFilter?.FundingViewScopes?
                .Select(fundingViewScope => ((FundingViewType)fundingViewScope.Id).ToString()).ToList();

            var paginationResult = await _layoutManagementService.GetPaginationResultWithFilters(
                layoutFilter.PageNumber,
                PageSize,
                fundingStreamsIds,
                fundingViewTypes,
                fundingViewScopes);

            viewModel.Pagination = MapPaginationDetail(paginationResult.PaginationDetail);
            viewModel.LayoutModels = GetLayoutUiModels(paginationResult.LayoutModels.ToList());
            viewModel.LayoutFilter = GetUiFilterTypes(paginationResult.FilterOptions, layoutFilter);
            return View(viewModel);
        }


        /// <summary>
        /// The action method to GetLayouts from javascript get method.
        /// </summary>
        /// <param name="pageNumber">The page no. passed from Pagination.</param>
        /// <param name="fundingStreamIds">The comma separated funding stream ids.</param>
        /// <param name="fundingViewTypeIds">The comma separated funding view type ids.</param>
        /// <param name="fundingViewScopeIds">The comma separated funding view scope ids.</param>
        /// <returns>
        /// The data for the layouts and pagination details.
        /// </returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminLayoutManagementGetLayouts,
            Name = ViewYourFundingConstants.RouteName_AdminLayoutManagementGetLayouts)]
        public virtual async Task<string> GetLayouts(
            int? pageNumber,
            string fundingStreamIds,
            string fundingViewTypeIds,
            string fundingViewScopeIds)
        {
            var fundingStreamIdList = (fundingStreamIds == "null") ? new List<int>() : fundingStreamIds.Split(',').Select(int.Parse).ToList();
            var fundingViewTypes = (fundingViewTypeIds == "null") ? new List<string>() : fundingViewTypeIds.Split(',').Select(fundingViewTypeId => ((FundingViewType)Convert.ToInt32(fundingViewTypeId)).ToString()).ToList();
            var fundingViewScopes = (fundingViewScopeIds == "null") ? new List<string>() : fundingViewScopeIds.Split(',').Select(fundingViewScopeId => ((FundingViewScope)Convert.ToInt32(fundingViewScopeId)).ToString()).ToList();
            var pageNo = pageNumber ?? 1;

            _fundingStreamList = await GetAllFundingStreams();
            var paginationResult = await _layoutManagementService.GetPaginationResult(pageNo, PageSize, fundingStreamIdList, fundingViewTypes, fundingViewScopes);

            _publications = (await _publicationService.GetAll()).ToList();

            var layoutUiModels = GetLayoutUiModels(paginationResult.LayoutModels.ToList());
            var layouts = layoutUiModels.Select(layout =>

             new
             {
                 LayoutName = layout.LayoutName.ToString(),
                 layout.LayoutId,
                 LastModifiedDateTime = layout.LastModifiedDateTime.ToDateTimeDisplayWithAt(),
                 FundingViewType = layout.FundingViewTypeName,
                 FundingViewTypeValue = layout.FundingViewType.ToString(),
                 FundingViewScope = layout.FundingViewScopeName,
                 FundingViewScopeValue = layout.FundingViewScope.ToString(),
                 layout.FundingStreamName,
                 layout.FundingStreamId,
                 Status = layout.Status.ToString(),
                 StatusName = layout.StatusName,
                 ShowDeleteLink = layout.Status != LayoutStatus.Published,
                 ShowPreviewLink = layout.FundingViewType != FundingViewType.Spreadsheet
             }).ToList();

            return JsonConvert.SerializeObject(new { Layouts = layouts, Pagination = paginationResult.PaginationDetail });
        }

        /// <summary>
        /// The Import layout page action.
        /// </summary>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminLayoutImport,
            Name = ViewYourFundingConstants.RouteName_AdminLayoutImport)]
        public virtual async Task<IActionResult> ImportLayout()
        {
            var viewModel = await GetBasePageViewModel<LayoutImportViewModel>();

            viewModel.FundingStreams = await GetFundingStreamSelectList();

            return View(viewModel);
        }

        /// <summary>
        /// The Import layout page post action.
        /// </summary>
        /// <param name="viewModel">The view model for the page.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpPost]
        [Route(
            ViewYourFundingConstants.Route_AdminLayoutImport,
            Name = ViewYourFundingConstants.RouteName_AdminLayoutImport)]
        public virtual async Task<IActionResult> ImportLayout(LayoutImportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var importFileViewModel = _mapper.Map<LayoutFileImportViewModel>(viewModel);
                await SetLayoutImportFileViewModelProperties(viewModel, importFileViewModel);
                return await ImportLayoutFile(importFileViewModel);
            }

            var baseViewModel = await GetBasePageViewModel<LayoutImportViewModel>();
            viewModel.CurrentUser = baseViewModel.CurrentUser;
            viewModel.FundingStreams = await GetFundingStreamSelectList();

            return View(viewModel);
        }

        /// <summary>
        /// The layout file import page action.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The MVC View Result.</returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.RouteName_AdminLayoutFileImport,
            Name = ViewYourFundingConstants.RouteName_AdminLayoutFileImport)]
        public virtual async Task<IActionResult> ImportLayoutFile(LayoutFileImportViewModel viewModel)
        {
            var baseViewModel = await GetBasePageViewModel<LayoutFileImportViewModel>();
            viewModel.CurrentUser = baseViewModel.CurrentUser;
            return View(nameof(ImportLayoutFile), viewModel);
        }


        /// <summary>
        /// The Import layout file upload action.
        /// </summary>
        /// <param name="viewModel">The view model for the page.</param>
        /// <returns>
        /// The MVC view result.
        /// </returns>
        [HttpPost]
        [Route(
            ViewYourFundingConstants.Route_AdminLayoutFileImportUpload,
            Name = ViewYourFundingConstants.RouteName_AdminLayoutFileImportUpload)]
        public virtual async Task<IActionResult> ImportLayoutFileUpload(LayoutFileImportViewModel viewModel)
        {
            var baseViewModel = await GetBasePageViewModel<LayoutFileImportViewModel>();
            viewModel.CurrentUser = baseViewModel.CurrentUser;
            if (ModelState.IsValid)
            {
                var content = await ReadAsStringAsync(viewModel.FileUpload);
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

                var layoutModel = new LayoutModel
                {
                    Data = data,
                    FundingViewType = viewModel.FundingViewType.ToString(),
                    FundingViewScope = viewModel.FundingViewScope.ToString(),
                    FundingStreamId = viewModel.FundingStreamId,
                    LastModifiedBy = viewModel.CurrentUser?.FullName,
                    LayoutName = viewModel.LayoutName,
                    Id = viewModel.LayoutModelId
                };

                if (viewModel.LayoutAction == LayoutAction.Update)
                {
                    var updateResult = await _layoutManagementService.UpdateLayoutAsync(layoutModel);

                    if (!updateResult)
                    {
                        viewModel.LayoutModelId = Guid.Empty.ToString();
                    }
                }
                else
                {
                    var result = await _layoutManagementService.AddLayoutAsync(layoutModel);
                    viewModel.LayoutModelId = result.ToString();
                }

                return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminLayoutImportConfirmation, new
                {
                    layoutId = viewModel.LayoutModelId,
                    viewModel.LayoutAction
                });
            }

            return View(nameof(ImportLayoutFile), viewModel);
        }

        /// <summary>
        /// Confirmation of the import operation.
        /// </summary>
        /// <param name="layoutId">The layout identifier.</param>
        /// <param name="layoutAction">The layout action.</param>
        /// <param name="changesSaved">True is changes are saved.</param>
        /// <returns>The MVC view result.</returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminLayoutImportConfirmation,
            Name = ViewYourFundingConstants.RouteName_AdminLayoutImportConfirmation)]
        public virtual async Task<ActionResult> Confirmation(Guid layoutId, LayoutAction layoutAction, bool changesSaved = true)
        {
            var viewModel = await GetBasePageViewModel<LayoutConfirmationViewModel>();
            viewModel.LayoutAction = layoutAction;
            _fundingStreamList = await GetAllFundingStreams();
            var layout = await _layoutManagementService.GetLayoutAsync(layoutId.ToString());
            if (!string.IsNullOrWhiteSpace(layout?.Id))
            {
                viewModel.LayoutUiModel = MapLayoutUiModel(layout, false);
                var submittedAtDisplayDate = viewModel.LayoutUiModel.LastModifiedDateTime.ToDateTimeDisplayWithAt();
                viewModel.SubmittedDisplayDate = $"on {submittedAtDisplayDate}";
                viewModel.ChangesSaved = changesSaved;
            }

            return View(viewModel);
        }

        /// <summary>
        /// Downloads the layout file.
        /// </summary>
        /// <param name="layoutId">The layout identifier.</param>
        /// <returns>The layout file to download.</returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminLayoutDownloadFile,
            Name = ViewYourFundingConstants.RouteName_AdminLayoutDownloadFile)]
        public virtual async Task<IActionResult> DownloadLayoutFile(Guid layoutId)
        {
            var layout = await _layoutManagementService.GetLayoutAsync(layoutId.ToString());
            if (string.IsNullOrWhiteSpace(layout?.Id))
            {
                return NotFound();
            }

            var data = !string.IsNullOrEmpty(layout.LayoutJsonData) ? layout.LayoutJsonData : JsonConvert.SerializeObject(layout.Data);
            var fileBytes = Encoding.UTF8.GetBytes(data);

            return new FileContentResult(
                fileBytes,
                FundingDocumentFileType.LayoutFile_JsonContentType)
            {
                FileDownloadName = $"{layout.LayoutName.Replace(" ", "_")}.json"
            };
        }

        /// <summary>
        /// The 'Are you sure?' page for confirming the delete of layout.
        /// </summary>
        /// <param name="layoutId">The layout identifier.</param>
        /// <returns>returns Are you sure view.</returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminLayoutAreYouSure,
            Name = ViewYourFundingConstants.RouteName_AdminLayoutAreYouSure)]
        public virtual async Task<IActionResult> AreYouSure(Guid layoutId)
        {
            var viewModel = await GetBasePageViewModel<LayoutAreYouSureViewModel>();
            var layout = await _layoutManagementService.GetLayoutAsync(layoutId.ToString());
            _fundingStreamList = await GetAllFundingStreams();
            _fundingStreamList.ToList().ForEach(fs => _publications.AddRange(fs.Publications));
            viewModel.LayoutUiModel = MapLayoutUiModel(layout);
            return View(viewModel);
        }

        /// <summary>
        /// Action to delete a layout.
        /// </summary>
        /// <param name="layoutId">The layout identifier.</param>
        /// <returns>returns to confirmation view.</returns>
        [HttpPost]
        [Route(
            ViewYourFundingConstants.Route_AdminDeleteLayout,
            Name = ViewYourFundingConstants.RouteName_AdminDeleteLayout)]
        public virtual async Task<IActionResult> DeleteLayout(Guid layoutId)
        {
            var layout = await _layoutManagementService.GetLayoutAsync(layoutId.ToString());
            bool deleted = await _layoutManagementService.DeleteLayoutAsync(layout);
            return RedirectToRoute(ViewYourFundingConstants.RouteName_AdminLayoutImportConfirmation, new
            {
                layoutId,
                layoutAction = LayoutAction.Delete,
                deleted
            });
        }

        /// <summary>
        /// Previews the layout.
        /// </summary>
        /// <param name="layoutId">The layout identifier.</param>
        /// <param name="fundingViewScope">The funding view scope.</param>
        /// <param name="fundingStreamId">The funding stream identifier.</param>
        /// <param name="fundingViewtype">The funding view type.</param>
        /// <returns>The MVC view result.</returns>
        [HttpGet]
        [Route(
            ViewYourFundingConstants.Route_AdminPreviewLayout,
            Name = ViewYourFundingConstants.RouteName_AdminPreviewLayout)]
        public virtual async Task<IActionResult> PreviewLayout(
            Guid layoutId,
            FundingViewScope fundingViewScope,
            int fundingStreamId,
            FundingViewType fundingViewtype)
        {
            var viewModel = await GetBasePageViewModel<PreviewLayoutViewModel>();
            viewModel.FundingStreamId = fundingStreamId;
            viewModel.FundingViewScope = fundingViewScope;
            viewModel.LayoutId = layoutId;
            viewModel.FundingViewType = fundingViewtype;

            if (viewModel.FundingViewScope == FundingViewScope.National)
            {
                return await RedirectToPreviewPage(viewModel);
            }

            return View(viewModel);
        }

        /// <summary>
        /// Previews the layout.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The MVC view result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(
            ViewYourFundingConstants.Route_AdminPreviewLayout,
            Name = ViewYourFundingConstants.RouteName_AdminPreviewLayout)]
        public virtual async Task<IActionResult> PreviewLayout(PreviewLayoutViewModel viewModel)
        {
            ClearAndClearModelErrors(viewModel);
            if (ModelState.IsValid)
            {
                return await RedirectToPreviewPage(viewModel);
            }

            var baseViewModel = await GetBasePageViewModel<PreviewLayoutViewModel>();
            viewModel.CurrentUser = baseViewModel.CurrentUser;
            return View(viewModel);
        }

        #endregion


        #region Private Helpers

        private static string GetDisplayAttribute(Enum value)
        {
            return !(value
                .GetType()?
                .GetField(value?.ToString())?
                .GetCustomAttributes(typeof(DisplayAttribute), false)?
                .SingleOrDefault() is DisplayAttribute attribute) ? value.ToString() : attribute.Name;
        }

        private static async Task<string> ReadAsStringAsync(IFormFile file)
        {
            var result = new StringBuilder();
            if (file.Length > 0)
            {
                using var reader = new StreamReader(file.OpenReadStream());
                while (reader.Peek() >= 0)
                {
                    result.AppendLine(await reader.ReadLineAsync());
                }
            }

            return result.ToString();
        }

        private static FundingViewType GetFundingViewType(string layout)
        {
            Enum.TryParse<FundingViewType>(layout, out var result);

            return result;
        }

        private static FundingViewScope GetFundingViewScope(string layout)
        {
            Enum.TryParse<FundingViewScope>(layout, out var result);

            return result;
        }

        private static void CheckLayoutExists(
            LayoutImportViewModel viewModel,
            LayoutFileImportViewModel importFileViewModel,
            IReadOnlyList<LayoutModel> dbLayouts)
        {
            var existingLayout = dbLayouts.FirstOrDefault(layout =>
                !string.IsNullOrWhiteSpace(layout.LayoutName) &&
                layout.LayoutName.Equals(viewModel.LayoutName, StringComparison.InvariantCultureIgnoreCase) &&
                layout.FundingStreamId == viewModel.FundingStreamId &&
                IsEnumEqualToString(layout.FundingViewType, viewModel.FundingViewType.ToString()) &&
                IsEnumEqualToString(layout.FundingViewScope, viewModel.FundingViewScope.ToString()));

            if (existingLayout != null)
            {
                importFileViewModel.LayoutAction = LayoutAction.Update;
                importFileViewModel.LayoutModelId = existingLayout.Id;
            }
        }

        private static bool IsEnumEqualToString(string actual, string desired)
        {
            return (actual ?? "0") == desired;
        }

        private async Task SetLayoutImportFileViewModelProperties(
            LayoutImportViewModel viewModel,
            LayoutFileImportViewModel importFileViewModel)
        {
            var fundingStream = await _adminSettingsService.GetFundingStreamById(viewModel.FundingStreamId);
            importFileViewModel.FundingStreamName = fundingStream.FundingStreamName;
            var dbLayouts = await _layoutManagementService.GetAllLayoutsAsync();
            CheckLayoutExists(viewModel, importFileViewModel, dbLayouts);
        }

        private IReadOnlyList<LayoutUiModel> GetLayoutUiModels(IReadOnlyList<LayoutModel> layoutModels)
        {
            return layoutModels.Select(layout => MapLayoutUiModel(layout)).ToList();
        }

        private LayoutUiModel MapLayoutUiModel(LayoutModel layoutModel, bool getFundingStreamName = true)
        {
            var fundingViewType = GetFundingViewType(layoutModel.FundingViewType);
            return new LayoutUiModel
            {
                LayoutName = layoutModel.LayoutName,
                LayoutId = layoutModel.Id,
                FundingStreamId = layoutModel.FundingStreamId,
                LastModifiedDateTime = layoutModel.LastModifiedDateTime.ConvertUtcDateTimeToGmtDateTime(),
                FundingViewType = fundingViewType,
                FundingViewScope = GetFundingViewScope(layoutModel.FundingViewScope),
                FundingStreamName = getFundingStreamName ? _fundingStreamList?
                    .FirstOrDefault(fundingStream => fundingStream.Id == layoutModel.FundingStreamId)?.FundingStreamName : string.Empty,
                Status = GetLayoutStatus(layoutModel.Id)
            };
        }

        private LayoutStatus GetLayoutStatus(string layoutId)
        {
            Guid.TryParse(layoutId, out var layoutGuid);
            var assignedPublication = _publications.FirstOrDefault(p => p.PublicationLayouts.Any(pl => pl.LayoutId == layoutId));

            if (assignedPublication == null)
            {
                return LayoutStatus.NotAssigned;
            }

            return assignedPublication.Status == PublicationStatus.Published
                ? LayoutStatus.Published
                : assignedPublication.Status == PublicationStatus.Preview ? LayoutStatus.Preview : LayoutStatus.Disabled;
        }

        private LayoutFilter GetUiFilterTypes(FilterOptions filterOptions, LayoutFilter filterType)
        {
            var model = new LayoutFilter
            {
                FundingStreams = GetFundingStreamsFilterList(filterOptions?.FundingStreamsIds),
                FundingViewTypes = GetFundingViewTypeFilterList(filterOptions?.FundingViewTypes),
                FundingViewScopes = GetFundingViewScopeFilterList(filterOptions?.FundingViewScopes)
            };

            if (filterType?.FundingViewTypes != null)
            {
                model.FundingViewTypes
                   .Where(fundingViewType => filterType.FundingViewTypes.Select(fundingViewTypeInner => fundingViewTypeInner.Id).ToList()
                   .Contains(fundingViewType.Id)).ToList().ForEach(fundingViewType => fundingViewType.Selected = true);
            }

            if (filterType?.FundingViewScopes != null)
            {
                model.FundingViewScopes
                   .Where(fundingViewScope => filterType.FundingViewScopes.Select(fundingViewScopeInner => fundingViewScopeInner.Id).ToList()
                   .Contains(fundingViewScope.Id)).ToList().ForEach(fundingViewScope => fundingViewScope.Selected = true);
            }

            if (filterType?.FundingStreams != null)
            {
                model.FundingViewScopes
                   .Where(fundingViewScope => filterType.FundingViewScopes.Select(fundingViewScopeInner => fundingViewScopeInner.Id).ToList()
                   .Contains(fundingViewScope.Id)).ToList().ForEach(fundingViewScope => fundingViewScope.Selected = true);
            }

            return model;
        }

        private List<Filter> GetFundingViewTypeFilterList(List<string> fundingViewTypes)
        {
            return fundingViewTypes?.Select(fundingViewType => new Filter
            {
                Id = (int)GetFundingViewType(fundingViewType),
                Name = GetDisplayAttribute(GetFundingViewType(fundingViewType))
            }).ToList();
        }

        private List<Filter> GetFundingViewScopeFilterList(List<string> fundingViewScopes)
        {
            return fundingViewScopes?.Select(fundingViewScope => new Filter
            {
                Id = (int)GetFundingViewScope(fundingViewScope),
                Name = GetDisplayAttribute(GetFundingViewScope(fundingViewScope))
            }).ToList();
        }

        private async Task<IReadOnlyList<FundingStream>> GetAllFundingStreams()
        {
            return await _adminSettingsService.GetAllFundingStreams(Repositories.Enums.FetchData.Publications, Repositories.Enums.FetchData.Publications_PublicationLayouts);
        }

        private async Task<IEnumerable<SelectListItem>> GetFundingStreamSelectList()
        {
            var fundingStreams = await _adminSettingsService.GetAllFundingStreams();
            return fundingStreams.Select(fundingStream => new SelectListItem
            {
                Text = fundingStream.FundingStreamName,
                Value = fundingStream.Id.ToString()
            });
        }

        private Models.LayoutManagement.Pagination MapPaginationDetail(Services.Models.Pagination pagination)
        {
            return _mapper.Map<Models.LayoutManagement.Pagination>(pagination);
        }

        private List<Filter> GetFundingStreamsFilterList(List<int> fundingStreamIds)
        {
            return _fundingStreamList.Where(fundingStream => fundingStreamIds.Contains(fundingStream.Id))
                .Select(fundingStream => new Filter
                {
                    Id = fundingStream.Id,
                    Name = fundingStream.FundingStreamName
                }).ToList();
        }

        private void ClearAndClearModelErrors(PreviewLayoutViewModel model)
        {
            if (model.IsOrganisationViewScope)
            {
                ModelState.Remove(nameof(PreviewLayoutViewModel.OrganisationUkprn));
            }
            else
            {
                ModelState.Remove(nameof(PreviewLayoutViewModel.LocalAuthorityCode));
            }
        }

        private async Task<IActionResult> RedirectToPreviewPage(PreviewLayoutViewModel viewModel)
        {
            var (routeName, routeValues) = await _previewLayoutActionStrategy.PreviewLayoutActions
                .First(fundingViewScopeAction => fundingViewScopeAction.AppliesTo(viewModel.FundingViewScope))
                .GetRouteNameAndValues(viewModel);

            return RedirectToRoute(routeName, routeValues);
        }

        #endregion
    }
}