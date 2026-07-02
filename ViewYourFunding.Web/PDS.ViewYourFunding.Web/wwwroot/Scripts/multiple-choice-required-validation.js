$(document).ready(function () {
    $("form").submit(function (e) {
        var selected = $('.multiple-choice.choice-required input:checked');

        if (selected.length === 0) {
            ShowErrors();
            e.preventDefault();
        } else {
            var choice = selected[0].value;

            if (actionMap !== null) {
                if (choice in actionMap === false) {
                    return;
                }

                $(this).attr('action', actionMap[choice]);
            }

            return true;
        }
    });

    $('.multiple-choice.choice-required input').click(function () {
        HideErrors();
    });

    function ShowErrors() {
        $('.error-summary, .error-message').removeClass("hidden");
        $('.after-hidden-error-summary').addClass('after-error-summary').removeClass('after-hidden-error-summary');
        $('.form-group-no-error').addClass('form-group-error').removeClass('form-group-no-error');
    };

    function HideErrors() {
        $('.error-summary, .error-message').addClass("hidden");
        $('.after-error-summary').addClass('after-hidden-error-summary').removeClass('after-error-summary');
        $('.form-group-error').addClass('form-group-no-error').removeClass('form-group-error');
    };
});