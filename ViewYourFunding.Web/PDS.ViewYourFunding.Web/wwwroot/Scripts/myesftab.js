$(document).ready(function () {
    
    function setPrintView(selectedTab) {
        // Set default css to be applied to the print view.
        var result = "#school-budget-share { display: inherit; } #minimum-funding-guarantee { display: inherit; } #high-needs { display: inherit; } #start-up-grant { display: inherit; } #protection-funding { display: inherit; } #core-programme { display: inherit; } #pupil-premium { display: inherit; } #recoupment-calculations { display: inherit; }";
        $('head').append('<style id="print-tab" type="text/css" media="print">' + result + '</style>');
    }

    function setTabProperties(tabName) {
        // de-select all tabs.
        $("#tab_school-budget-share, #tab_minimum-funding-guarantee, #tab_high-needs, #tab_start-up-grant, #tab_post-opening-grant, #tab_protection-funding, #tab_core-programme, #tab_pupil-premium, #tab_recoupment-calculations")
            .removeClass("govuk-tabs__tab--selected")
            .attr("aria-selected", false)
            .attr("tagindex", "-1")
            .attr("tabindex", "-1");       

        // select appropriate tab.
        $("#tab_" + tabName)
            .addClass("govuk-tabs__tab--selected")
            .attr("aria-selected", true)
            .attr("tagindex", "0")
            .attr("tabindex", "0");
    }

    function setPanelProperties(tabName) {
        // De-select all panels
        $("#school-budget-share, #minimum-funding-guarantee, #high-needs, #start-up-grant, #protection-funding, #post-opening-grant, #core-programme, #pupil-premium, #recoupment-calculations")
            .addClass("govuk-tabs__panel--hidden");

        // select appropriate panel
        $("#" + tabName)
            .removeClass("govuk-tabs__panel--hidden");
    }

    function getCurrentTab(url) {
        const searchTabParams = new URLSearchParams(window.location.search);

        var tabName = searchTabParams.get('Tab');


        if (tabName == null || tabName == "") {

            var tabName = searchTabParams.get('tab');

        }
        if (tabName === "school-budget-share" || tabName === "minimum-funding-guarantee" || tabName === "high-needs" || tabName === "start-up-grant" || tabName === "protection-funding" || tabName === "post-opening-grant" || tabName === "core-programme" || tabName === "pupil-premium" || tabName === "recoupment-calculations") {
            return tabName;
        }
        else {
            return $('input[name=selectedTab]').val();  // first time - get tab name from hidden field.
        }
    }

    // On load clear any CSS we don't want to appear when JavaScript is turned on
    $(".govuk-tabs__list").css("padding-bottom", "");

    var currentTab = getCurrentTab(window.location.href);

    $('input[name=selectedTab]').val(currentTab);

    setTabProperties(currentTab);

    setPanelProperties(currentTab);

    if (currentTab === "school-budget-share" || currentTab === "minimum-funding-guarantee" || currentTab === "high-needs" || currentTab === "start-up-grant" || currentTab === "protection-funding" || currentTab === "post-opening-grant" || currentTab === "core-programme" || currentTab === "pupil-premium" || currentTab === "recoupment-calculations") {
        setPrintView(currentTab);
    }
    else {
        setPrintView("school-budget-share");
    }

    // Dynamically update the CSS being applied to the print view.
    $(".print-trigger").click(function () {
        var clickedPrintSection = $(this).attr('href').replace("#", "");
        $('#print-tab').remove();
        setPrintView(clickedPrintSection);
    });

    var urlParams = new URLSearchParams(window.location.search);

    $(".govuk-radios__input").change(function () {

        if (urlParams.get('SelectedVarianceOption') == null) {
            urlParams.append('SelectedVarianceOption', $(this).attr('id'));
            $('input[id="' + $(this).attr('id') + '"]').attr('checked', 'checked');
        }
        else {
            var selectedoption = urlParams.get('SelectedVarianceOption');
            urlParams.set('SelectedVarianceOption', $(this).attr('id'));
            $('input[id="' + selectedoption + '"]').attr('checked', 'checked');
        }

        if (window.location.href.indexOf("#") + 1 > 0) {
            urlParams.set('tab', window.location.href.substr(window.location.href.indexOf("#") + 1));
        }
        window.location.replace(window.location.pathname + '?' + urlParams);

    });
});