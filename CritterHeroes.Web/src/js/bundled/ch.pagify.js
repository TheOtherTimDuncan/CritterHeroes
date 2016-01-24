(function (cheroes, $) {

    'use strict';

    var chunkBreak = $('<li>').addClass('page-chunk-break');

    var events = {
        CHANGE_PAGE: 'changePage',
        PAGE_LOADED: 'pageLoaded'
    };

    /**
     * PagingModel
     * @typedef {object} PagingModel
     * @property    {number}    totalPages    - The total number of pages. Used to determine if the paging elements should be shown at all
     * @property    {number}    currentPage   - The number of the current page
     * @property    {number}    firstPage
     * @property    {number}    lastPage
     * @property    {number}    previousPage
     * @property    {number}    nextPage
     * @property    {bool}      showLeadingChunkBreak
     * @property    {bool}      showTrailingChunkBreak
     */

    /**
     * Triggered when a page change is requested
     * @event changePage
     * @type {number}
     */

    /**
     * Triggered when new paging data is available
     * @event pageLoaded
     * @type {PageModel}
     */

    /**
     * Assign existing paging data to options.data to build the paging elements immediately or use pageLoaded event to build or refresh the paging elements later.
     * Catch the changePage event to get notified when a page change is requested
     * 
     * @param   {object}        options
     * @param   {PagingModel}   [options.data] - Paging data to immediately build the paging elements with
     */
    $.fn.pagify = function (options) {

        return this.each(function () {

            var element = $(this);

            if (options && options.data) {
                buildPages(element, options.data);
            }

            element
                .on('click', '[data-page]', function () {
                    element.trigger('changePage', $(this).data('page'));
                })
                .on(events.PAGE_LOADED, function (event, data) {
                    buildPages(element, data);
                });

        });

    };

    cheroes.events.pagify = events;

    function buildPages(container, data) {

        if (!data || data.totalPages <= 1) {
            container.empty();
            return;
        }

        var pages = [];

        pages.push(
            $('<li>')
                .addClass('page-prev')
                .attr('data-page', data.previousPage)
                .attr('title', 'Go to page ' + data.previousPage)
                .append($('<span>'))
        );

        pages.push(
            $('<li>')
                .toggleClass('current', data.currentPage === 1)
                .attr('data-page', 1)
                .attr('title', 'Go to page 1')
                .text(1)
        );

        if (data.showLeadingChunkBreak) {
            pages.push(createChunkBreak());
        }

        for (var p = data.firstPage; p <= data.lastPage; p++) {
            pages.push(
                $('<li>')
                    .toggleClass('current', data.currentPage === p)
                   .attr('data-page', p)
                   .attr('title', 'Go to page ' + p)
                   .text(p)
            );
        }

        if (data.showTrailingChunkBreak) {
            pages.push(createChunkBreak());
        }

        pages.push(
           $('<li>')
               .toggleClass('current', data.currentPage === data.totalPages)
               .attr('data-page', data.totalPages)
               .attr('title', 'Go to page ' + data.totalPages)
               .text(data.totalPages)
        );

        pages.push(
          $('<li>')
              .addClass('page-next')
              .attr('data-page', data.nextPage)
              .attr('title', 'Go to page ' + data.nextPage)
              .append($('<span>'))
        );

        container.html(pages);

    }

    function createChunkBreak() {
        return $('<li>').addClass('page-chunk-break');
    }

}(this.cheroes = this.cheroes || {}, jQuery));
