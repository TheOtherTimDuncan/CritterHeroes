var cdnFallback = (function () {

    var options = {
        siteRoot: '/',
        bundleRoot: 'bundles'
    };

    function writeScriptBundleUrl(bundleName) {
        document.write('<script src="' + options.siteRoot + options.bundleRoot + '/' + bundleName + '"><\/script>');
    }

    var jqueryFallback = function (bundleName) {
        window.jQuery || writeScriptBundleUrl(bundleName);
    }

    var jqueryValidationFallback = function (bundleName) {
        window.jQuery.validator || writeScriptBundleUrl(bundleName);
    }

    var unobtrusiveFallback = function (bundleName) {
        window.jQuery.validator.unobtrusive || writeScriptBundleUrl(bundleName);
    }

    var bootstrapFallback = function (bundleName) {
        $.fn.model || writeScriptBundleUrl(bundleName);
    }

    var respondFallback = function (bundleName) {
        window.respond || writeScriptBundleUrl(bundleName);
    }

    var modernizerFallback = function (bundleName) {
        window.modernizer || writeScriptBundleUrl(bundleName);
    }

    var cssBootStrapFallback = function (bundlePath)
    {
        var checkElement = $('<div>', { id: 'bootstrap-check', class: 'hidden' }).appendTo('body');
        if ($('#bootstrap-check').is(':visible') == true) {
            $('<link rel="stylesheet" type="text/css" href="' + options.siteRoot + '/' + bundlePath + '">').appendTo('head');
        }
        checkElement.remove();
    }

    var result = {
        options: options,
        jqueryFallback: jqueryFallback,
        jqueryValidationFallback: jqueryValidationFallback,
        unobtrusiveFallback: unobtrusiveFallback,
        bootstrapFallback: bootstrapFallback,
        respondFallback: respondFallback,
        modernizerFallback: modernizerFallback,
        cssBootStrapFallback: cssBootStrapFallback
    }

    return result;

})();