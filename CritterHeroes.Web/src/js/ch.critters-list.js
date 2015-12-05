(function (cheroes, $, handlebars) {

    'use strict';

    var query = cheroes.historyManager.copySafeQuery(cheroes.query);

    var template = handlebars.compile($('#template').html());

    var crittersContainer = $('#critters-container tbody');
    var critterUrl = crittersContainer.data('url');
    var pictureUrl = crittersContainer.data('picture-url');

    var pagingContainer = $('.paging-container').on('click', '[data-page]', function () {
        query.page = $(this).data('page');
        getData();
    });

    var filters = $('select[data-filter]').each(function () {
        $(this).on('change', function () {
            query[$(this).data('filter')] = $(this).val();
            getData();
        });
    });

    cheroes.historyManager.registerPopState(function (state) {

        query = state;

        filters.each(function () {

            var key = $(this).data('filter').toLowerCase();
            $(this).val(cheroes.historyManager.getQueryValue(query, key));

        });

        getData();

    });

    getData();

    function getData() {

        cheroes.dataManager.sendRequest({

            url: critterUrl,
            data: query,

            success: function (data) {

                cheroes.historyManager.pushState(getQueryState());

                if (data.paging.currentPage !== 1) {
                    window.scrollTo(0, 0);
                }

                pagingContainer.paging(data.paging);
                var html = template(data);
                crittersContainer.html(html);
            }

        });

    }

    function getQueryState() {

        var queryState = {};

        for (var p in query) {

            if (p.toLowerCase() === "page") {
                if (query[p] && query[p] !== 1) {
                    queryState[p] = query[p];
                }
            } else if (query[p]) {
                queryState[p] = query[p];
            }
        }

        return queryState;
    }

}(this.cheroes = this.cheroes || {}, jQuery, Handlebars));
