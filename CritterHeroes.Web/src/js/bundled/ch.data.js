(function (cheroes, $) {

    'use strict';

    function send(defaultOptions, options) {

        var requestOptions = $.extend({}, defaultOptions, options);

        requestOptions.success = function (data, statusText, jqXhr) {
            options.success(data, statusText, jqXhr.status);
        };

        requestOptions.error = function (jqXhr, textStatus, errorThrown) {
            if (jqXhr.status === 400) {
                if (options.badRequest) {
                    options.badRequest(jqXhr.responseJSON, jqXhr.statusText, jqXhr.status);
                }
            } else {
                if (options.error) {
                    options.error(jqXhr, textStatus, errorThrown);
                }
            }
        };

        $.ajax(requestOptions);

    }

    cheroes.dataManager = {

        sendRequest: function (options) {
            var defaultOptions = {
                dataType: 'json',
                type: 'POST'
            };
            send(defaultOptions, options);
        },

        getHtml: function (options) {
            var defaultOptions = {
                dataType: 'html',
                type: 'GET'
            };
            send(defaultOptions, options);
        }
    };

}(this.cheroes = this.cheroes || {}, jQuery));
