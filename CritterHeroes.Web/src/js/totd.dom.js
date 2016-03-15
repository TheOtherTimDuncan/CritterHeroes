(function (totd, global) {

    'use strict';

    var query = function (selector) {
        return new query.fn.init(selector);
    };

    query.fn = query.prototype = {

        elements: [],
        selector: '',
        length: 0

    };

    query.fn.init = function (selector) {
        this.elements = global.querySelectorAll(selector);
        this.selector = selector;
        this.length = this.elements.length;
        return this;
    };

    totd.dom = {
        query: query
    };

}(this.totd = this.totd || {}, document));
