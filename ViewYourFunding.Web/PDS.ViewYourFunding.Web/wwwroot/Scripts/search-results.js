(function () {
    SetupAllFilters();
    SetupDynamicSearch();
    AddSearchBox();

    function AddSearchBox() {
        if ($('.form-group.direction-rtl').length) {
            $('.search-field-wrap').show();
        }

        var laEle = $(".LocalAuthorityFilter .filter-search");

        laEle.keyup(filterLaList);
        laEle.on("input", filterLaList);

        filterLaList();

        function filterLaList() {
            var val = laEle.val().toLowerCase();

            $(".LocalAuthorityFilter ul li").each(function () {
                var t = $(this).text().toLowerCase();

                if (t === "all") val === "" ? $(this).removeClass("hide") : $(this).addClass("hide");

                if (val === "" || t.indexOf(val) > -1) {
                    $(this).removeClass("hide");
                }
                else {
                    $(this).addClass("hide");
                }
            });
        }
    }

    function SetupDynamicSearch() {
        $("#UpdateResults").hide();
        $("form").submit(function (e) {
            updateResults();
            e.preventDefault();
        });

        var filtersEles = $(".filters input[type='checkbox']");
        filtersEles.change(updateResults);

        $("#ResetSearch").click(function (e) {
            filtersEles.prop('checked', false);
            updateResults();

            e.stopPropagation();
            e.preventDefault();
        });

        $(document).ready(updateResults);

        // Uncheck everything
        filtersEles.each(function () { $(this).prop("checked", false); });

        var urTimer = null;

        function updateResults() {
            if (urTimer) {
                clearTimeout(urTimer);
                urTimer = null;
            }

            urTimer = setTimeout(function () {
                var acceptablesEstablishmentTypes = $.map($("input[name='QueryFilter.Filters.EstablishmentType']:checked"), function (ele) { return $(ele).val(); });
                var acceptableLaCodes = $.map($("input[name='QueryFilter.Filters.LocalAuthority']:checked"), function (ele) { return $(ele).val(); });

                var matches = 0;

                $(".searchResultFilter").each(function () {
                    var establishmentType = $(this).attr("data-establishment-type");
                    var laCode = $(this).attr("data-la");

                    var shouldShow = (acceptableLaCodes.length === 0 || acceptableLaCodes.indexOf(laCode) > -1)
                        && (acceptablesEstablishmentTypes.length === 0 || acceptablesEstablishmentTypes.indexOf(establishmentType) > -1);

                    if (shouldShow) {
                        $(this).removeClass("hidden");
                        matches += 1;
                    }
                    else $(this).addClass("hidden");
                });

                var multipleSchoolsMsg = "we found {count} schools. Did you mean:";
                var noSchoolsFoundMsg = "no schools were found matching your search. Please refine your criteria.";

                var msEle = $("#resultCountText");

                var topLink = $('.back-to-top-link');
                if (matches > 24) {
                    topLink.removeClass("hidden");
                } else {
                    topLink.addClass("hidden");
                }

                if (matches === 0) msEle.show().html(noSchoolsFoundMsg);
                else if (matches === 1) msEle.hide();
                else msEle.show().html(multipleSchoolsMsg.replace(/\{count\}/ig, matches));
            }, 25);
        }
    }

    function SetupAllFilters() {
        SetupEstablishmentFilters();
        SetupEstablishmentLas();
    }

    function SetupEstablishmentFilters() {

        var eles = $(".EstablishmentTypeFilter input[name='QueryFilter.Filters.EstablishmentType']");
        eles.click(updateCheckboxes);

        var allEle = $("input[name='QueryFilter.Filters.EstablishmentType.Special']");
        allEle.click(function () {
            var checked = allEle.prop("checked");
            eles.prop("checked", checked);
        });

        function updateCheckboxes() {
            var checkedCount = $("input[name='QueryFilter.Filters.EstablishmentType']:checked").length;
            allEle.prop("checked", eles.length === checkedCount);
        }

        updateCheckboxes();
    }

    function SetupEstablishmentLas() {

        var eles = $(".LocalAuthorityFilter input[name='QueryFilter.Filters.LocalAuthority']");
        eles.click(updateCheckboxes);

        var allEle = $("input[name='QueryFilter.Filters.LocalAuthority.Special']");
        allEle.click(function () {
            var checked = allEle.prop("checked");
            eles.prop("checked", checked);
        });

        function updateCheckboxes() {
            var checkedCount = $("input[name='QueryFilter.Filters.LocalAuthority']:checked").length;
            allEle.prop("checked", eles.length === checkedCount);
        }

        updateCheckboxes();
    }
})();