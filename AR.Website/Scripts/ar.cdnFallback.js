var cdnFallback = (function ($) {

    'use strict';

    function writeScriptBundleUrl(bundleName ) {
        document.write('<script src="' + result.options.siteRoot + result.options.bundleRoot + '/' + bundleName + '"><\/script>');
    }

    var result = {
        options: {
            siteRoot: '/',
            bundleRoot: 'bundles'
        },

        jqueryFallback: function (bundleName) {
            window.jQuery || writeScriptBundleUrl(bundleName);
        },

        jqueryValidationFallback: function (bundleName) {
            window.jQuery.validator || writeScriptBundleUrl(bundleName);
        },

        unobtrusiveFallback: function (bundleName) {
            window.jQuery.validator.unobtrusive || writeScriptBundleUrl(bundleName);
        },

        bootstrapFallback: function (bundleName) {
            window.jQuery.fn.modal || writeScriptBundleUrl(bundleName);
        },

        respondFallback: function (bundleName) {
            window.respond || writeScriptBundleUrl(bundleName);
        },

        modernizerFallback: function (bundleName) {
            window.Modernizr || writeScriptBundleUrl(bundleName);
        },

        cssBootStrapFallback: function (bundlePath) {
            var checkElement = window.jQuery('<div>', { id: 'bootstrap-check', class: 'hidden' }).appendTo('body');
            if (window.jQuery('#bootstrap-check').is(':visible') === true) {
                window.jQuery('<link rel="stylesheet" type="text/css" href="' + result.options.siteRoot + '/' + bundlePath + '">').appendTo('head');
            }
            checkElement.remove();
        }
    };

    return result;

})();