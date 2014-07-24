(function (arescues, $) {

    'use strict';

    arescues.dataManager = {

        defaultOptions: {
            dataType: 'json',
            type: 'POST'
        },

        sendRequest: function (options) {

            var that = arescues.dataManager;
            var getOptions = $.extend(that.defaultOptions, options);

            getOptions.success = function (data) {
                options.success(data);
            };

            getOptions.error = function (jqxhr, textStatus, errorThrown) {
            };

            $.ajax(getOptions);
        }
    };

}(this.arescues = this.arescues || {}, jQuery));