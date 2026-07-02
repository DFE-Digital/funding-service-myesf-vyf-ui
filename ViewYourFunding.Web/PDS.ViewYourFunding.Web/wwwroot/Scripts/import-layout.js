$(document).ready(function () {
    var fundingViewTypeEle = $("#funding-view-type");
    var options = $("#funding-view-scope option");

    setOptionsVisibillity(options, fundingViewTypeEle);

    fundingViewTypeEle.change(function () {
        setTimeout(function () { setOptionsVisibillity(options, fundingViewTypeEle); }, 50);
    });
});

function setOptionsVisibillity(options, fundingViewTypeEle) {
    options.removeClass("hidden");
    var fundingViewType = fundingViewTypeEle.val();

    options.each(function () {
        var option = $(this);
        var applicableTypes = option.attr("data-applicable-for-types").split(',');
        var names = option.attr("data-names-for-types").split(',');

        var index = applicableTypes.indexOf(fundingViewType);

        if (index === -1) {
            option.addClass("hidden");
        }
        else {
            option.text(names[index]);
        }
    });
}