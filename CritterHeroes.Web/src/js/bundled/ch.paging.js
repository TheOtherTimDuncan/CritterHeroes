(function ($) {

    'use strict';

    var chunkBreak = $('<li>').addClass('page-chunk-break');

    $.fn.paging = function (model) {

        return this.each(function () {

            var element = $(this);

            if (!model || model.totalPages <= 1) {
                element.empty();
                return;
            }

            var pages = [];

            pages.push(
                $('<li>')
                    .addClass('page-prev')
                    .attr('data-page', model.previousPage)
                    .attr('title', 'Go to page ' + model.previousPage)
                    .append($('<span>'))
            );

            pages.push(
                $('<li>')
                    .toggleClass('current', model.currentPage === 1)
                    .attr('data-page', 1)
                    .attr('title', 'Go to page 1')
                    .text(1)
            );

            if (model.showLeadingChunkBreak) {
                pages.push(chunkBreak);
            }

            for (var p = model.firstPage; p <= model.lastPage; p++) {
                pages.push(
                    $('<li>')
                        .toggleClass('current', model.currentPage === p)
                       .attr('data-page', p)
                       .attr('title', 'Go to page ' + p)
                       .text(p)
                );
            }

            if (model.showTrailingChunkBreak) {
                pages.push(chunkBreak);
            }

            pages.push(
               $('<li>')
                   .toggleClass('current', model.currentPage === model.totalPages)
                   .attr('data-page', model.totalPages)
                   .attr('title', 'Go to page ' + model.totalPages)
                   .text(model.totalPages)
            );

            pages.push(
              $('<li>')
                  .addClass('page-next')
                  .attr('data-page', model.nextPage)
                  .attr('title', 'Go to page ' + model.nextPage)
                  .append($('<span>'))
            );

            element.html(pages);
        });

    };

}(jQuery));
