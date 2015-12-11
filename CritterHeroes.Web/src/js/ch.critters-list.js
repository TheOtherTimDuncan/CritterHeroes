(function (cheroes, $, handlebars) {

    'use strict';

    var query = cheroes.historyManager.copySafeQuery(cheroes.query);

    var template = cheroes.templates.critters;

    var crittersContainer = $('#critters-container tbody');
    var critterUrl = crittersContainer.data('url');
    var pictureUrl = cheroes.pictureUrl;

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

                cheroes.historyManager.pushState(query);

                if (data.paging.currentPage !== 1) {
                    window.scrollTo(0, 0);
                }

                pagingContainer.paging(data.paging);
                data.pictureUrl = pictureUrl;
                var html = template(data);
                crittersContainer.html(html);
            }

        });

    }

}(this.cheroes = this.cheroes || {}, jQuery, Handlebars));
