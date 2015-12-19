(function (cheroes, $) {

    'use strict';

    function loadResetPassword(data) {
        var modalContainer = $('#modal-container')
            .html(data)
            .find('div:first').modal({
                backdrop: false
            });

        var frm = modalContainer
            .find('#reset-password-form')
            .submit(resetPassword);
        $.validator.unobtrusive.parse(frm);
    }

    function resetPassword(e) {
        var frm = $(this);
        var validator = frm.validate();
        if (frm.valid()) {
            var btn = $('#button-container').hide();
            var busy = $('.busy').show();
            var request = {

                url: frm.prop('action'),
                data: frm.serialize(),

                badRequest: function (data) {
                    busy.hide();
                    btn.show();
                    validator.showErrors({ "ResetPasswordEmail": data.Message });
                },

                success: function (data) {
                    $('#message').hide();
                    $('#success').show();
                    frm.find('.form-group').hide();
                    busy.hide();
                    btn.find('input[type="submit"]').hide();
                    btn.find('#close').text('Continue');
                    btn.show();
                }
            };
            cheroes.dataManager.sendRequest(request);
        }
        e.preventDefault();
    }

    $('#forgot-password').click(function (e) {
        var request = {
            url: $(this).data('url'),
            success: loadResetPassword
        };
        cheroes.dataManager.getHtml(request);

        e.preventDefault();
    });


}(this.cheroes = this.cheroes || {}, jQuery));
