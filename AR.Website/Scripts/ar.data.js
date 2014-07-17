(function (arescues, $) {

    var _sendRequest = function (options) {

        var that = arescues.dataManager;
        var getOptions = $.extend(that.defaultOptions, options);

        getOptions.success = function (data) {
            options.success(data);
        };

        getOptions.error = function (jqxhr, textStatus, errorThrown) {
        };

        $.ajax(getOptions);
    };

    arescues.dataManager = {

        defaultOptions: {
            dataType: 'json',
            type: 'POST'
        },

        sendRequest: _sendRequest
    };

}(this.arescues = this.arescues || {}, jQuery));