(function (cheroes, $) {

    'use strict';

    $('#file').change(function () {
        $(this).closest('form').submit();
    });

}(this.cheroes = this.cheroes || {}, jQuery));
