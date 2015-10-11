(function (cheroes, $) {

    'use strict';

    $('#StatusID').change(function () {
        $(this).closest('form').submit();
    });

}(this.cheroes = this.cheroes || {}, jQuery));
