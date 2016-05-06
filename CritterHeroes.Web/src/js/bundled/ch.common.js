(function (cheroes, $) {

    'use strict';

    cheroes.KEYS = {
        ESC: 27,
        UP: 38,
        DOWN: 40
    };

    // Need to append client time to url to use for default when editing the profile
    var orgLink = $('#org-link');
    orgLink.attr('href', orgLink.attr('href') + '?jstime=' + encodeURIComponent(new Date().toString()));

}(this.cheroes = this.cheroes || {}, jQuery));
