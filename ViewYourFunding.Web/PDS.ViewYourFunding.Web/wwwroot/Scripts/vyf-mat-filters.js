"use strict";

$(function () {
    var resultsList = $("#list-items-container .govuk-accordion__section");
    var allFilters = $(".filter-value-input");
    var alldata = [];

    var allDataNameOfFiltes = {
        "Filter.academy": "academy",
        "Filter.localAuthority": "localauthority",
        "Filter.fundingType": "fundingtype"
    };

    var allRelatedFilters = {
        "Filter.academy": ["Filter.localAuthority", "Filter.fundingType"],
        "Filter.localAuthority": ["Filter.academy", "Filter.fundingType"],
        "Filter.fundingType": ["Filter.academy", "Filter.localAuthority"]
    };

    var SetAllData = () => {
        resultsList.each((index, item) => {
            alldata.push({
                academy: $(item).data("academy"),
                fundingtype: $(item).data("fundingtype"),
                localauthority: $(item).data("localauthority"),
            });
        });
    };

    var filterClickedEventHandler = (evt) => {
        var currentFilterName = evt.currentTarget.name;
        var allCheckedItemsInCurrentFilter = $('.filter-value-input[name="' + currentFilterName + '"]:checked')
                                            .map(function () { return this.value; })
                                            .toArray();
        
        var currentFilterDataName = allDataNameOfFiltes[currentFilterName];
        var relatedFilters = allRelatedFilters[currentFilterName];

        relatedFilters.forEach(relatedFilter => {

            var relatedFilterDataName = allDataNameOfFiltes[relatedFilter];

            var relatedFilterData = alldata
                .filter(data => allCheckedItemsInCurrentFilter.includes(data[currentFilterDataName]))
                .map(data => data[relatedFilterDataName]);

            $('.filter-value-input[name="' + relatedFilter + '"]').prop("checked", false);

            relatedFilterData.forEach(f => {
                var selector = '.filter-value-input[id="queryFilter-' + f.replace("'", "\\'") + '"][name="' + relatedFilter + '"]';
                $(selector).prop("checked", true);
            });
        });

        updateResults(evt);
    };
    var updateResults = function (evt) {
        var checkedFilters = $(".filter-value-input:checked");

        // if no filters selected => show everything.
        if (checkedFilters.length === 0) {
            resultsList.show();
            return;
        }

        resultsList.hide();

        var filterGroups = {};

        checkedFilters.each(function () {
            var filter = $(this);
            var filterTarget = filter.attr("name").slice(7).toLowerCase();
            var filterValue = filter.val();

            if (!filterGroups[filterTarget]) {
                filterGroups[filterTarget] = [];
            }

            filterGroups[filterTarget].push(filterValue);
        });

        var validResults = resultsList.filter(function (index, element) {
            var isValid = [];

            for (var group in filterGroups) {
                isValid.push(filterGroups[group].includes($(this).data(group).toString()));
            }

            return !isValid.includes(false);
        });

        if (!validResults || validResults.length === 0) {
            // display no results element.
            $(".govuk-accordion__controls").hide();
            $("#list-no-items").removeClass("hidden")
                .attr("aria-hidden", "false")
                .removeAttr("hidden");
        } else {
            $(".govuk-accordion__controls").show();
            $("#list-no-items").addClass("hidden")
                .attr("aria-hidden", "true")
                .attr("hidden", "hidden");
            validResults.show();
        }
    };

    var hideFilters = () => {
        var academysCount = $('#filter-by-academy > section > ul')[0].childElementCount;
        var fundingTypesCount = $('#filter-by-fundingType > section > ul')[0].childElementCount;
        var lasCount = $('#filter-by-localAuthority > section > ul')[0].childElementCount;

        if (academysCount <= 1) {
            $('#filter-academy').hide();
        }
        
        if (fundingTypesCount <= 1) {
            $('#filter-fundingType').hide();
        }
        
        if (lasCount <= 1) {
            $('#filter-localAuthority').hide();
        }

        if (academysCount <= 1 && fundingTypesCount <= 1 && lasCount <= 1) {
            $('#content > div:nth-child(3) > div:nth-child(2) > form > div.column-filter-one-third.column-third').hide();
            $('#allocation-statements-content').addClass('column-full').removeClass('column-two-thirds');
        }
    };

    var clearAll = function (evt) {
        evt.preventDefault();

        $(".filter-value-input:checked").prop('checked', false);

        resultsList.show();
    };

    var init = function () {
        allFilters.each(function (i, el) {
            $(this).on("click", filterClickedEventHandler);
        });

        $("#clearAllFilters a").on("click", clearAll);

        $("#updateFilterResults").hide();

        SetAllData();

        hideFilters();
    };

    var filtersExist = $("#updateFilterResults").length > 0;

    if (filtersExist) {
        init();
    }
});