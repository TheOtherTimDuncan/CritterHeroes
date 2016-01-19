(function (cheroes, $) {

    'use strict';

    function onForgotPasswordLoaded(modalContainer) {
        var frm = modalContainer
            .find('#reset-password-form')
            .submit(resetPassword);
        $.validator.unobtrusive.parse(frm);
    }

    function resetPassword(e) {
        e.preventDefault();
        var frm = $(this);
        var validator = frm.validate();
        if (frm.valid()) {
            var btn = $('#button-container').addClass('hidden');
            var busy = $('.busy').removeClass('hidden');
            var request = {

                url: frm.prop('action'),
                data: frm.serialize(),

                badRequest: function (data) {
                    busy.addClass('hidden');
                    btn.removeClass(hidden);
                    validator.showErrors({ "ResetPasswordEmail": data.Message });
                },

                success: function (data) {
                    $('#message').addClass('hidden');
                    $('#success').removeClass('hidden');
                    frm.find('.input-group').addClass('hidden');
                    busy.addClass('hidden');
                    btn.find('input[type="submit"]').addClass('hidden');
                    btn.find('#close').text('Continue');
                    btn.removeClass('hidden');
                }
            };
            cheroes.dataManager.sendRequest(request);
        }
    }

    $('#forgot-password').click(function (e) {
        e.preventDefault();
        cheroes.modal({
            url: $(this).data('url'),
            onLoad: onForgotPasswordLoaded
        });
    });


}(this.cheroes = this.cheroes || {}, jQuery));
