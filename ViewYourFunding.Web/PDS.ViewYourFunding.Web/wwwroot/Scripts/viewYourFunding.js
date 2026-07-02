$(document).ready(function () {
    $(".submitDisable").on("submit",
        function () {
        $(".button").attr("disabled", true);
    });
});