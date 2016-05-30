(function (cheroes, $, handlebars) {

    'use strict';

    var query = cheroes.historyManager.copySafeQuery(cheroes.query);

    var template = cheroes.templates.critters;

    var crittersContainer = $('#critters-container');
    var critterUrl = crittersContainer.data('url');
    var pictureUrl = cheroes.pictureUrl;

    crittersContainer.rowSelectify();

    var pager = $('.paging-container').pagify().on(cheroes.events.pagify.CHANGE_PAGE, function (event, page) {
        query.page = page;
        getData();
    });

    var filters = $('select[data-filter]').each(function () {
        $(this).on('change', function () {
            query[$(this).data('filter')] = $(this).val();
            getData();
        });
    });

    $('#summary').on('click', function (e) {
        e.preventDefault();
        var modal = cheroes.modal({
            url: $(this).data('url'),
            onLoad: function (modalContainer) {
                modalContainer.on('click', '[data-status-id]', function (e) {
                    e.preventDefault();
                    modal.close();
                    query.statusid = $(this).data('status-id');
                    $('[data-filter="statusid"]').val(query.statusid);
                    getData();
                });
            }
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

                pager.trigger(cheroes.events.pagify.PAGE_LOADED, data.paging);

                data.pictureUrl = pictureUrl;
                var html = template(data);
                crittersContainer.html(html);
                cheroes.fixBrokenImages();
            }

        });

    }

}(this.cheroes = this.cheroes || {}, jQuery, Handlebars));
