(function (cheroes, $) {

    'use strict';

    function loadEmailLogin(data) {
        var modalContainer = $('#modal-container')
            .html(data)
            .find('div:first')
            .modal()
            .on('shown.bs.modal', function () {
                $('#Password').focus();
            });
        var frm = modalContainer
            .find('#email-login-form')
            .submit(emailLogin);
        $.validator.unobtrusive.parse(frm);
    }

    function emailLogin(e) {
        var frm = $(this);
        var validator = frm.validate();

        if (frm.valid()) {
            var request = {
                url: frm.prop('action'),
                data: frm.serialize(),
                success: function () {
                    var getRequest = {
                        url: frm.data('next'),
                        success: loadEmailEdit
                    };
                    cheroes.dataManager.getHtml(getRequest);
                }
            };
            cheroes.dataManager.sendRequest(request);
        }

        e.preventDefault();
    }

    function loadEmailEdit(data) {
        var modalContainer = $('#modal-container')
            .html(data)
            .find('div:first')
            .removeClass('fade')
            .modal()
            .on('shown.bs.modal', function () {
                $('#NewEmail').focus();
            });
        var frm = modalContainer
            .find('#edit-email-form')
            .submit(emailSave);
        $.validator.unobtrusive.parse(frm);
    }

    function emailSave(e) {
        var frm = $(this);
        var validator = frm.validate();

        if (frm.valid()) {
            var btn = $('#button-container').hide();
            var busy = $('.busy').show();
            var request = {
                url: frm.prop('action'),
                data: frm.serialize(),
                success: function (data) {
                    if (data.Succeeded) {
                        $('#success').show();
                        frm.find('.form-group').hide();
                        busy.hide();
                        frm.find('input[type="submit"]').hide();
                        frm.find('#close').text('Continue');
                        btn.show();
                        $('#email').text($('#NewEmail').val());
                    } else {
                        busy.hide();
                        btn.show();
                        validator.showErrors({ "ResetPasswordEmail": data.Message });
                    }
                }
            };
            cheroes.dataManager.sendRequest(request);
        }

        e.preventDefault();
    }

    $('#edit-email').click(function (e) {
        var request = {
            url: $(this).data('url'),
            success: loadEmailLogin
        };
        cheroes.dataManager.getHtml(request);

        e.preventDefault();
    });

}(this.cheroes = this.cheroes || {}, jQuery));
