// Fix some issue with details not expanding correctly
$("summary").click(function (e) {
    var wasOpenPreClick = $(this).attr("open") === "open";
    var panel = $("div.govuk-details__text", this);
    var detailsEle = $(this);

   toggleDetailPanel(wasOpenPreClick, detailsEle, panel);
    setTimeout(function () { toggleDetailPanel(wasOpenPreClick, detailsEle, panel); }, 50);
});

function toggleDetailPanel(toggleToClosed, detailsEle, panel) {
    if (toggleToClosed) {
        detailsEle.removeAttr("open");
        panel.attr("aria-hidden", "true").css("display", "none");

        return;
    }

    detailsEle.attr("open", "open");
    panel.attr("aria-hidden", "false").css("display", "block");
}