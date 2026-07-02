(function () {

    var topics = {};

    $.Topic = function (id) {
        var callbacks,
            topic = id && topics[id];
        if (!topic) {
            callbacks = $.Callbacks();
            topic = {
                publish: callbacks.fire,
                subscribe: callbacks.add,
                unsubscribe: callbacks.remove
            };
            if (id) {
                topics[id] = topic;
            }
        }
        return topic;
    };

    $.Topic("ApplyFilterAddOns").subscribe(ApplyFilterAddOns);

    function ApplyFilterAddOns() {
        updateTotalAllocation();
    }

    function updateTotalAllocation() {
        var allocationTotalAmount = 0;
        $(".filterable:not(.hidden)").each(function (index, filterableItem) {
            allocationTotalAmount += $(filterableItem).data("fundingamount");
        });
        $(".total-funding-amount").html(currencyAddCommas(allocationTotalAmount));
    }

    function currencyAddCommas(totalAllocation) {
        totalAllocation += "";
        var parts = totalAllocation.split(".");
        var firstPart = parts[0];
        var decimalSeparator = parts.length > 1 ? "." + parts[1] : "";
        var regex = /(\d+)(\d{3})/;
        while (regex.test(firstPart)) {
            firstPart = firstPart.replace(regex, "$1" + "," + "$2");
        }
        var total = firstPart + decimalSeparator;
        return "&pound;" + total;
    }
})();