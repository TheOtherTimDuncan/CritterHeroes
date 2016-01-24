(function (cheroes, $, handlebars) {

    'use strict';

    var query = cheroes.historyManager.copySafeQuery(cheroes.query);

    var template = cheroes.templates.contacts;

    var contactsContainer = $('#contacts-container');
    var contactsUrl = contactsContainer.data('url');

    var pager = $('.paging-container').pagify().on(cheroes.events.pagify.CHANGE_PAGE, function (event, page) {
        query.page = page;
        getData();
    });

    var filters = $('select[data-filter]').each(function () {
        $(this).on('change', function () {
            query.page = 1;
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

            url: contactsUrl,
            data: query,

            success: function (data) {

                cheroes.historyManager.pushState(query);

                if (data.paging.currentPage !== 1) {
                    window.scrollTo(0, 0);
                }

                pager.trigger(cheroes.events.pagify.PAGE_LOADED, data.paging);

                var html = template(data);
                contactsContainer.html(html);
            }

        });

    }

}(this.cheroes = this.cheroes || {}, jQuery, Handlebars));
