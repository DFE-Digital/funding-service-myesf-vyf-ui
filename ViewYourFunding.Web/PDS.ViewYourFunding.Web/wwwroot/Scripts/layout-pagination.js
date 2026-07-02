$(function () {
    BindEventHandlers();
});

function GetLayouts(pageNumber) {
    var basePath = window.location.pathname.includes("/view-latest-funding") ? "/view-latest-funding" : "/single-funding-statement/latest";
    var url = basePath + "/admin/layout/getlayouts/" + pageNumber + "/" + GetFundingStreamIds()
        + "/" + GetFundingViewTypeIds() + "/" + GetFundingViewScopeIds();

    $.get(url, function (data) {
        var paginationData = JSON.parse(data);
        var layouts = paginationData.Layouts;
        var pagination = paginationData.Pagination;

        BuildLayoutHTML(layouts);
        ShowHidePreviousPage(pagination.HasPreviousPage);
        ShowHideNextPage(pagination.HasNextPage);
        ShowCurrentPage(pagination.ResultCount, pagination.PageSize);
        ShowHideBackButton(layouts.length, pagination.PageSize);
        BuildPaginationLinks(pagination.FirstPage, pagination.LastPage, pagination.PageNumber, pagination.ResultCount, pagination.PageSize);
       
        $("#pgSummary").text("Showing " + pagination.FirstRecordNo + " to "
            + pagination.LastRecordNo + " of " + pagination.ResultCount + " layouts");
        $("#currPage").attr('value', pagination.PageNumber);

        if (pagination.ResultCount === 0) {
            $(".no-records").removeClass("hidden");
        }
    });
}

function BindEventHandlers() {
    $("a.app-c-back-to-top").bind("click",
        function () {
            window.scrollTo(0, 0);
        });

    $("#prevPage").bind("click",
        function () {
            pageNumber = GetPageNumber();
            GetLayouts(--pageNumber);
            return false;
        });

    $("#nextPage").bind("click",
        function () {
            pageNumber = GetPageNumber();
            GetLayouts(++pageNumber);
            return false;
        });

    $("#ResetSearch").bind("click",
        function () {
            ClearFilters();
            GetLayouts(1);
            return false;
        });

        $(".chk-funding-streams").change(function () {
            GetLayouts(1);
            return false;
        });


    $(".chk-funding-view-types, .chk-funding-view-scopes").change(function () {
        GetLayouts(1);
        return false;
    });

    $("#postButton").addClass("hidden");

    $(document).on("click", "input.linkPage", function () {
        var page = $(this).val();
        GetLayouts(page);
        return false;
    });

    $(document).on("click", "input.currentLink", function () {
        return false;
    });

}

function ClearFilters() {
    $('.chk-funding-streams').removeAttr('checked');
    $('.chk-funding-view-types').removeAttr('checked');
    $('.chk-funding-view-scopes').removeAttr('checked');
}

function GetPageNumber() {
    return parseInt($("#currPage").attr('value'));
}

function GetFundingStreamIds() {
    if ($('.chk-funding-streams:checked').length > 0) {
        return $('.chk-funding-streams:checked').map(function () {
            return ($(this).val());
        }).toArray();
    }
    else {
        return null;
    }
}

function GetFundingViewTypeIds() {
    var key = ".chk-funding-view-types:checked";

    if ($(key).length > 0) {
        return $(key).map(function () {
            return ($(this).val());
        }).toArray();
    }
    else {
        return null;
    }
}

function GetFundingViewScopeIds() {
    var key = ".chk-funding-view-scopes:checked";

    if ($(key).length > 0) {
        return $(key).map(function () {
            return ($(this).val());
        }).toArray();
    }
    else {
        return null;
    }
}

function ShowHidePreviousPage(ShowPreviousPage) {
    if (ShowPreviousPage) {
        $("#prevPage").removeClass("hidden");
    }
    else {
        $("#prevPage").addClass("hidden");
    }
}

function ShowHideNextPage(ShowNextPage) {
    if (ShowNextPage) {
        $("#nextPage").show();
    }
    else {
        $("#nextPage").hide();
    }
}

function ShowCurrentPage(resultCount, pageSize) {

    if (resultCount < pageSize) {
        $("input.currentLink").hide();
    }
    else {
        $("input.currentLink").show();
    }
}

function ShowHideBackButton(resultCount, pageSize) {
    if (resultCount < pageSize) {
        $("a.app-c-back-to-top").hide();
    }
    else {
        $("a.app-c-back-to-top").show();
    }
}

function BuildPaginationLinks(startPage, endPage, currentPage, resultCount, pageSize) {
    var pageLinks = "";
    if (resultCount >= pageSize) {
        for (var page = startPage; page <= endPage; page++) {
            var className = (page == currentPage) ? "currentLink" : "linkButton linkPage";
            pageLinks += "<input type='submit' name='Pagination.PageNumber' value='" + page + "'  class='" + className + "' />";
        }
    }
    $("span.pages").html(pageLinks);
}

function BuildLayoutHTML(layoutJson) {
    var layoutHtml = "<tr><th>Layout Template</th><th>Last Updated</th><th>Actions</th></tr>";
    if (layoutJson.length === 0 )
        layoutHtml += "<tr><td colspan='3'> No layouts found. Please amend your criteria. </td></tr>";
    var basePath = window.location.pathname.includes("/view-latest-funding") ? "/view-latest-funding" : "/single-funding-statement/latest";

    $.each(layoutJson, function (index, value) {
        var areYouSureURL = basePath + "/admin/layout/areyousure/" + this.LayoutId;
        var previewURL = basePath + "/admin/layout/preview/" + this.LayoutId + "/" + this.FundingViewScopeValue + "/" + this.FundingStreamId + "/" + this.FundingViewTypeValue;
        var exportUrl = basePath + "/admin/layout/download/" + this.LayoutId;
        var hideLinkClass = this.ShowDeleteLink ? "" : "hidden";
        var statusClass = this.Status === "Disabled" ? "govuk-tag govuk-bgred grey-background" : this.Status === "Published" ? "govuk-tag govuk-bgred red-background" : "govuk-tag govuk-bgred";
        layoutHtml +=
            "<tr>" +
            "<td class='no-border-bottom no-padding-bottom'>" +
            "<div class='parent-wrapper'>" +
            "<p><span>Layout Name: " + this.LayoutName + "</span> </p>" +
            "<p><span>Funding stream: " + this.FundingStreamName + "</span></p>" +
            "<p> <span>Funding view type: " + this.FundingViewType + "</span></p>" +
            "<p> <span>Layout type: " + this.FundingViewScope + "</span></p>" + 
            "</div>" +
            "</td>" +
            "<td class='no-border-bottom no-padding-bottom'>" +
            "<div class='parent-wrapper'>" +
            "<span>" + this.LastModifiedDateTime + "</span>" +
            "</div>" +
            "</td>" +
            "<td class='no-border-bottom no-padding-bottom'>" +
            "<p class='parent-wrapper'><a href='" + exportUrl + "'>Export</a></p>" +
            "<p class='parent-wrapper'>" +
            "<a target='_blank' rel='external noopener noreferrer' href='" + previewURL + "'>Preview</a>" +
            "</p>" +
            "<p class='parent-wrapper'>" +
            "<a class='" + hideLinkClass + "' href='" + areYouSureURL + "'>Delete</a>" +
            "</p>" +
            "</td>" +
            "</tr>" +
            "<tr>" +
            "<td class='no-padding-top' colspan='3'><p><span>Status: <strong class='" + statusClass + "'>" + this.StatusName + "</strong></span></p></td>"
            "</tr>";
    });

    $("table.touch-table > tbody").html(layoutHtml);

}