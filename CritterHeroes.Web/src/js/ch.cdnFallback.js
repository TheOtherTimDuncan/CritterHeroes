(function (cheroes) {

    'use strict';

    function writeScriptBundleUrl(url) {
        document.write('<script src="' + url + '"><\/script>');
    }

    var result = {

        jqueryFallback: function (url) {
            window.jQuery || writeScriptBundleUrl(url);
        },

        jqueryValidationFallback: function (url) {
            window.jQuery.validator || writeScriptBundleUrl(url);
        },

        jqueryUIFallback: function (url) {
            window.jQuery.ui || writeScriptBundleUrl(url);
        },

        unobtrusiveFallback: function (url) {
            window.jQuery.validator.unobtrusive || writeScriptBundleUrl(url);
        },

        unobtrusiveajaxFallback: function (url) {
            window.jQuery.validator.unobtrusive || writeScriptBundleUrl(url);
        },

        bootstrapFallback: function (url) {
            window.jQuery.fn.modal || writeScriptBundleUrl(url);
        },

        respondFallback: function (url) {
            window.respond || writeScriptBundleUrl(bundleName);
        },

        modernizerFallback: function (url) {
            window.Modernizr || writeScriptBundleUrl(url);
        },

        cssBootStrapFallback: function (bundlePath) {
            var checkElement = window.jQuery('<div>', { id: 'bootstrap-check', class: 'hidden' }).appendTo('body');
            if (window.jQuery('#bootstrap-check').is(':visible') === true) {
                window.jQuery('<link rel="stylesheet" type="text/css" href="' + result.options.siteRoot + '/' + bundlePath + '">').appendTo('head');
            }
            checkElement.remove();
        }
    };

    cheroes.cdnFallback = result;

})(this.cheroes = this.cheroes || {});
