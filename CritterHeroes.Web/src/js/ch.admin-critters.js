(function (cheroes, $) {

    'use strict';

    $('input[type="file"]').change(function () {
        $(this).closest('form').submit();
    });

}(this.cheroes = this.cheroes || {}, jQuery));
