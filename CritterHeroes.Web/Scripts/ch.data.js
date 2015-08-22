(function (cheroes, $) {

    'use strict';

    function send(defaultOptions, options) {

        var requestOptions = $.extend(defaultOptions, options);

        requestOptions.success = function (data) {
            options.success(data);
        };

        requestOptions.error = function (jqxhr, textStatus, errorThrown) {
            if (options.error) {
                options.error(jqxhr, textStatus, errorThrown);
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