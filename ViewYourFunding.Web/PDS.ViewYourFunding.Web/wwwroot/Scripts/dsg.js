$(document).ready(function () {
    
    function setPrintView(selectedTab) {
        // Set default css to be applied to the print view.
        var base = "#schools { display: none; } #css { display: none; } #high-needs { display: none; } #early-years { display: none; }";
        var result = base.replace("#" + selectedTab + " { display: none; }", "#" + selectedTab + " { display: inherit; }");
        $('head').append('<style id="print-tab" type="text/css" media="print">' + result + '</style>');
    }

    function setTabProperties(tabName) {
        // de-select all tabs.
        $("#tab_schools, #tab_css, #tab_high-needs, #tab_early-years")
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
        $("#schools, #css, #high-needs, #early-years")
            .addClass("govuk-tabs__panel--hidden");

        // select appropriate panel
        $("#" + tabName)
            .removeClass("govuk-tabs__panel--hidden");
    }

    function getCurrentTab(url) {
        var idx = url.indexOf("#");
        var tabName = idx !== -1 ? url.substring(idx + 1) : "";
        if (tabName === "schools" || tabName === "css" || tabName === "high-needs" || tabName === "early-years") {
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

    if (currentTab === "schools" || currentTab === "css" || currentTab === "high-needs" || currentTab === "early-years") {
        setPrintView(currentTab);
    }
    else {
        setPrintView("schools");
    }

    // Dynamically update the CSS being applied to the print view.
    $(".print-trigger").click(function () {
        var clickedPrintSection = $(this).attr('href').replace("#", "");
        $('#print-tab').remove();
        setPrintView(clickedPrintSection);
    });
});