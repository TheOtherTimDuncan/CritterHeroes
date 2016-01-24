(function (cheroes, $) {

    'use strict';

    function onEmailLoginLoaded(modalContainer) {
        var frm = modalContainer
            .find('#email-login-form')
            .submit(emailLogin);
        $.validator.unobtrusive.parse(frm);
    }

    function emailLogin(e) {

        e.preventDefault();

        var frm = $(this);
        var validator = frm.validate();

        if (frm.valid()) {
            cheroes.dataManager.sendRequest({

                url: frm.prop('action'),
                data: frm.serialize(),

                badRequest: function (data) {
                    validator.showErrors({ "Password": data.errors });
                },

                success: function () {
                    cheroes.modal({
                        url: frm.data('next'),
                        onLoad: onEmailEditLoaded
                    });
                }
            });
        }
    }

    function onEmailEditLoaded(modalContainer) {
        var frm = modalContainer
            .find('#edit-email-form')
            .submit(emailSave);
        $.validator.unobtrusive.parse(frm);
    }

    function emailSave(e) {

        e.preventDefault();

        var frm = $(this);
        var validator = frm.validate();

        if (frm.valid()) {
            var btn = $('#button-container').addClass('hidden');
            var busy = $('.busy').removeClass('hidden');
            cheroes.dataManager.sendRequest({

                url: frm.prop('action'),
                data: frm.serialize(),

                badRequest: function (data) {
                    busy.addClass('hidden');
                    btn.removeClass('hidden');
                    validator.showErrors({ "ResetPasswordEmail": data.Message });
                },

                success: function (data) {
                    $('#success').removeClass('hidden');
                    frm.find('section').addClass('hidden');
                    busy.addClass('hidden');
                    frm.find('input[type="submit"]').addClass('hidden');
                    frm.find('#close').text('Continue');
                    btn.removeClass('hidden');
                    $('#unconfirmed-email').removeClass('hidden').find('#unconfirmed-email').text($('#NewEmail').val());
                }
            });
        }

    }

    $('#edit-email').click(function (e) {
        e.preventDefault();
        cheroes.modal({
            url: $(this).data('url'),
            onLoad: onEmailLoginLoaded
        });

    });

}(this.cheroes = this.cheroes || {}, jQuery));
