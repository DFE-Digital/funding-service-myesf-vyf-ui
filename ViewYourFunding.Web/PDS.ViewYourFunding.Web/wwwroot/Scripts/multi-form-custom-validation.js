$(document).ready(function () {
    $("form").submit(function (e) {
        ClearErrors();
        var searchTerm = $(this).find('input[name="searchTerm"]');

        if (searchTerm.val() === "") {
            ShowErrors(this);
            e.preventDefault();
            window.scrollTo(0, 0);
        }
    });


    $('input[name="searchScope"]').click(function () {
        ClearErrors();
    });

    function ShowErrors(e) {
        $(e).find('.panel-border-narrow').addClass("form-group-error");
        $(e).closest('.after-hidden-error-summary').addClass('after-error-summary').removeClass('after-hidden-error-summary');
        $(e).find('.error-message').removeClass("hidden");
        $('.error-summary').removeClass("hidden");
        $("#errorLink").attr({
            href: "#" + $(e).find('.panel-border-narrow').attr('id')
        });
       
    }

    function ClearErrors() {
        $('.panel-border-narrow').removeClass("form-group-error");
        $('.after-hidden-error-summary').removeClass('after-error-summary').addClass('after-hidden-error-summary');
        $('.error-message, .error-summary').addClass("hidden");
    }
});