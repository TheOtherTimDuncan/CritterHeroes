(function (cheroes, $) {

    'use strict';

    cheroes.fixBrokenImages = function () {
        $('img').on('error', function () {
            var img = $(this);
            img
                .attr('data-broken-url', img.attr('src'))
                .attr('src', cheroes.settings.imageNotFound)
                .off('error');  // Let's prevent an infinite loop
        });
    };

    cheroes.KEYS = {
        ESC: 27,
        UP: 38,
        DOWN: 40
    };
    
}(this.cheroes = this.cheroes || {}, jQuery));
