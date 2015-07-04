(function ($) {

    'use strict';

    $.fn.busyIndicator = function (options) {

        var settings = $.extend({
            message: function () { alert("Message is not set"); },
            delay: 5
        }, options);

        return this.each(function () {

            var element = $(this);

            element.closest('form').submit(function () {
                if ($(this).valid()) {
                    // GIF won't animate in IE unless we delay showing the busy indicator
                    setTimeout(function () {
                        $(element.parent())
                            .text(settings.message)
                            .addClass('busy message');
                    }, settings.delay);
                }
                return true;
            });

        });

    }

}(jQuery));
