(function (cheroes, $) {

    'use strict';

    cheroes.dataManager = {

        defaultOptions: {
            dataType: 'json',
            type: 'POST'
        },

        sendRequest: function (options) {

            var that = cheroes.dataManager;
            var getOptions = $.extend(that.defaultOptions, options);

            getOptions.success = function (data) {
                options.success(data);
            };

            getOptions.error = function (jqxhr, textStatus, errorThrown) {
            };

            $.ajax(getOptions);
        }
    };

}(this.cheroes = this.cheroes || {}, jQuery));