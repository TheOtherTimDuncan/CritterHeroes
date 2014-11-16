(function (cheroes, $) {

    'use strict';

    var _that;
    var _originalValue;

    $.widget('ch.username', {

        options: {
            minCharacters: 4,

            available: {
                message: 'Username available',
                cssClass: 'text-success',
            },

            taken: {
                message: 'Username has been taken',
                cssClass: 'text-danger',
            },

            dataUrl: function () { alert('dataUrl has not been set'); }
        },

        _create: function () {
            _originalValue = this.element.val();
            var msgElement = $('<span>');
            this.element.parent().append(msgElement);
            _that = this;
            this.element
                .change(function () {
                    _that._updateMessage(msgElement, this);
                })
                .keyup(function () {
                    _that._updateMessage(msgElement, this);
                });
        },

        _updateMessage: function (msgElement, usernameElement) {
            var value = $(usernameElement).val();
            if (value != _originalValue && value.length > _that.options.minCharacters && $(usernameElement).valid()) {
                var request = {
                    url: _that.options.dataUrl,
                    data: {
                        username: value,
                        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                    },
                    success: function (data) {
                        msgElement.show();
                        if (data === true) {
                            msgElement
                                .text(_that.options.taken.message)
                                .attr('class', _that.options.taken.cssClass);
                        } else {
                            msgElement
                                .text(_that.options.available.message)
                                .attr('class', _that.options.available.cssClass);
                        }
                        msgElement.show();
                    }
                };
                cheroes.dataManager.sendRequest(request);
            } else {
                msgElement.hide();
            }
        }
    });

}(this.cheroes = this.cheroes || {}, jQuery));
