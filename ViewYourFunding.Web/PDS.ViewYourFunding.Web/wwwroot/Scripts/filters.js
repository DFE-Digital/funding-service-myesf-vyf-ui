(function () {
    SetupFilters();

    function SetupFilters() {
        $("#UpdateResults").hide();
        $("form").submit(function (e) {
            updateResults();
            e.preventDefault();
        });

        var filterCheckboxes = $(".filters input[type='checkbox']");
        filterCheckboxes.change(updateResults);

        $("#ResetSearch").click(function (e) {
            filterCheckboxes.prop('checked', false);
            updateResults();

            e.stopPropagation();
            e.preventDefault();
        });

        // Uncheck everything
        filterCheckboxes.each(function () { $(this).prop("checked", false); });

        var filterKeys = $.map($(".filter"), function(e) { return $(e).data("filterkey"); });
        var urTimer = null;

        function updateResults() {
            if (urTimer) {
                clearTimeout(urTimer);
                urTimer = null;
            }

            urTimer = setTimeout(function () {
                var selectedFilters = {};
                filterKeys.forEach(function (key) {
                    selectedFilters[key] = $.map($("input[name='QueryFilter.Filters." + key + "']:checked"), function (ele) { return $(ele).val(); });
                });

                var matches = 0;
                $(".filterable").each(function (index, filterableItem) {
                    var shouldShow = true;

                    filterKeys.forEach(function (key) {
                        if (!shouldShow) {
                            return;
                        }

                        var selectedValues = selectedFilters[key];
                        var currentValue = $(filterableItem).data(key.toLowerCase());

                        if (selectedValues.length > 0 && selectedValues.indexOf(currentValue) < 0) {
                            shouldShow = false;
                        }
                    });

                    if (shouldShow) {
                        $(filterableItem).removeClass("hidden");
                        matches += 1;
                    }
                    else $(filterableItem).addClass("hidden");
                });

                $('.resultCount').text(matches);

                if (matches === 1) {
                    $('.more-than-one-match').hide();
                    $('.one-match').show();
                } else {
                    $('.one-match').hide();
                    $('.more-than-one-match').show();
                }

                var topLink = $('.back-to-top-link');
                if (matches > 24) {
                    topLink.removeClass("hidden");
                } else {
                    topLink.addClass("hidden");
                }

                $.Topic('ApplyFilterAddOns').publish();
            }, 25);
        }
    }

})();