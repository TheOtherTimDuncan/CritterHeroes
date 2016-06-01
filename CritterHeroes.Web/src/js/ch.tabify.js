(function (cheroes, $) {

    'use strict';

    $.fn.tabify = function () {

        var container = this;

        return container.on('click', '[role="tab"]', function () {

            var tab = $(this);
            var parent = tab.closest('li');

            if(parent.hasClass('active')){
                return;
            }

            container.find('li').removeClass('active');
            parent.addClass('active');

            var target = $(tab.attr('href'));
            target.siblings().removeClass('active');
            target.addClass('active');

        });
    };

}(this.cheroes = this.cheroes || {}, jQuery));
