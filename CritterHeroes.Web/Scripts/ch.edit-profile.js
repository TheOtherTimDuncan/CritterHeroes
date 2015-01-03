(function (cheroes, $) {

    'use strict';

    var usernameElement = $('#Username');
    var dataUrl = usernameElement.data('url');
    usernameElement.username({ dataUrl: dataUrl });

}(this.cheroes = this.cheroes || {}, jQuery));
