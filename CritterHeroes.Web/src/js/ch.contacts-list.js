(function (cheroes, $, handlebars) {

    'use strict';

    var query = cheroes.historyManager.copySafeQuery(cheroes.query);

    var template = handlebars.compile($('#template').html());

    var contactsContainer = $('#contacts-container tbody');
    var contactsUrl = contactsContainer.data('url');


    var pagingContainer = $('.paging-container').on('click', '[data-page]', function () {
        query.page = $(this).data('page');
        getData();
    });

    cheroes.historyManager.registerPopState(function (state) {

        query = state;
        getData();

    });

    getData();

    function getData() {

        cheroes.dataManager.sendRequest({

            url: contactsUrl,
            data: query,

            success: function (data) {

                cheroes.historyManager.pushState(getQueryState());

                if (data.paging.currentPage !== 1) {
                    window.scrollTo(0, 0);
                }

                pagingContainer.paging(data.paging);
                var html = template(data);
                contactsContainer.html(html);
            }

        });

    }

    function getQueryState() {

        var queryState = {};

        for (var p in query) {

            if (p.toLowerCase() === "page") {
                if (query[p] !== 1) {
                    queryState[p] = query[p];
                }
            } else if (query[p]) {
                queryState[p] = query[p];
            }
        }

        return queryState;
    }

}(this.cheroes = this.cheroes || {}, jQuery, Handlebars));
