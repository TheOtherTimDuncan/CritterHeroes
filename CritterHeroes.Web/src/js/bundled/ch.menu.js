(function (cheroes, $) {

    'use strict';

    $('[data-toggle="dropdown"]').dropdownify();

    $('[data-toggle="collapse"]')
        .on('click', function () {

            var target = $($(this).data('target'));

            if (target.hasClass('open')) {
                target
                    .attr('aria-expanded', false)
                    .removeClass('open');
            } else {
                target
                    .attr('aria-expanded', true)
                    .addClass('open');
            }

        });

}(this.cheroes = this.cheroes || {}, jQuery));
