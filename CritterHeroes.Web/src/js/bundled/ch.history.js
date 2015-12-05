(function (cheroes) {

    'use strict';

    function stateToQuery(state) {

        var query = [];

        for (var p in state) {
            if (state.hasOwnProperty(p)) {
                var value = state[p];
                if (p.toLowerCase() === "page") {
                    if (value == 1) {
                        value = null; // Don't show page in url if page value is first page
                    }
                }
                if (value) {
                    query.push(encodeURIComponent(p) + "=" + encodeURIComponent(state[p]));
                }
            }
        }

        return query.join("&");
    }

    cheroes.historyManager = {

        pushState: function (state, url) {

            var stateUrl;
            if (url) {
                stateUrl = url;
            } else {
                stateUrl = window.location.pathname;
                var query = stateToQuery(state);
                if (state && query.length > 0) {
                    stateUrl += '?' + query;
                }
            }

            history.pushState(state, null, stateUrl);

        },

        registerPopState: function (handler) {
            window.onpopstate = function (event) {
                handler(event.state);
                event.preventDefault();
            }
        },

        copySafeQuery: function (source) {

            var target = {};

            for (var p in source) {
                if (source.hasOwnProperty(p)) {
                    target[p.toLowerCase()] = source[p];
                }
            }

            return target;
        },

        getQueryValue: function (query, key) {

            for (var p in query) {
                if (query.hasOwnProperty(p) && p.toLowerCase() === key.toLowerCase()) {
                    return query[p];
                }
            }

            return '';

        }

    };

}(cheroes));
