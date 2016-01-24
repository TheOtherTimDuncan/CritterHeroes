(function (cheroes, $) {

    'use strict';

    var modalContainer;
    var backdrop;

    function closeModal() {
        backdrop.remove();
        backdrop = null;
        modalContainer.remove();
        modalContainer = null;
    }

    function openModal(options) {

        var body = $('body').addClass('modal-open');

        if (!backdrop) {
            backdrop = $('<div>')
                .addClass('modal-backdrop fade in')
                .appendTo(body);
        }

        cheroes.dataManager.getHtml({
            url: options.url,
            success: function (data) {

                if (modalContainer) {
                    modalContainer.remove();
                }

                modalContainer = $(data).appendTo(body);

                modalContainer
                    .on('click', '[data-modal-close]', closeModal)
                    .on('keydown', function (event) {
                        if (event.which == cheroes.KEYS.ESC) {
                            return closeModal();
                        }
                    })
                    .on('transitionend webkitTransitionEnd oTransitionEnd', function () {
                        modalContainer.find('[autofocus]').trigger('focus');
                        modalContainer.off('transitionend webkitTransitionEnd oTransitionEnd');
                    });

                modalContainer[0].offsetWidth // force reflow
                modalContainer.addClass("in"); // Slide 'er in

                if (options.onLoad) {
                    options.onLoad(modalContainer);
                }
            }
        });

        return {

            close: closeModal

        }
    }

    cheroes.modal = openModal;

}(this.cheroes = this.cheroes || {}, jQuery));
