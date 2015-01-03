(function ($) {

    'use strict';

    $.widget("ch.busyIndicator", {

        options: {
            message: function () { alert("Message is not set"); },
            delay: 5
        },

        _create: function () {

            var that = this;

            this.element.closest('form').submit(function () {
                if ($(this).valid()) {
                    // GIF won't animate in IE unless we delay showing the busy indicator
                    setTimeout(function () {
                        $(that.element.parent())
                            .text(that.options.message)
                            .addClass('busy message');
                    }, that.options.delay);
                }
                return true;
            });
        }
    });
}(jQuery));
