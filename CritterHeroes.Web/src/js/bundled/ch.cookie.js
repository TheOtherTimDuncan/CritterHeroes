(function (cheroes, global) {

    'use strict';

    function createCookie(name, value, expirationDays) {

        var expires;
        if (expirationDays) {
            var date = new Date();
            date.setTime(date.getTime() + (expirationDays * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        } else {
            expires = '';
        }

        global.cookie = name + "=" + encodeURIComponent(value) + expires + "; path=/";

    }

    function readCookie(name) {

        var nameKey = name + "=";
        var cookies = document.cookie.split(';');

        for (var i = 0; i < cookies.length; i++) {

            var cookie = cookies[i];

            while (cookie.charAt(0) === ' ') {
                cookie = cookie.substring(1, cookie.length);
            }

            if (cookie.indexOf(nameKey) === 0) {
                return cookie.substring(nameKey.length, cookie.length);
            }

        }

        return null;
    }

    function removeCookie(name) {
        createCookie(name, '', -1);
    }

    cheroes.cookieManager = {
        createCookie: createCookie,
        readCookie: readCookie,
        removeCookie: removeCookie
    };

}(this.cheroes = this.cheroes || {}, document));
