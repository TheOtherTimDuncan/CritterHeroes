(function (cheroes, $) {

    'use strict';

    $('[data-item-container]').hide();

    $('[data-source]').each(function () {

        var dataContainer = $(this);

        dataContainer.find('.refresh').each(function () {
            var url = $(this).data('url');
            $(this).click(function () {
                refreshStatus($(this), dataContainer, url);
            });
            refreshStatus($(this), dataContainer, url);
        });

        dataContainer.find('.sync').each(function () {
            var url = $(this).data('url');
            $(this).click(function () {
                refreshStatus($(this), dataContainer, url);
            });
        });

        dataContainer.find('[data-toggle-values]').click(function () {
            var itemContainers = dataContainer.find('[data-item-container]').toggle();
            var isVisible = itemContainers.is(':visible');
            dataContainer.find('.glyphicon')
                .toggleClass('glyphicon-plus', !isVisible)
                .toggleClass('glyphicon-minus', isVisible);
        });
    });

    function setStatusCount(element, storageItem) {
        element
            .find('[data-status="valid"]')
            .find('.badge')
            .text(storageItem.ValidCount)
            .toggle(storageItem.ValidCount > 0);

        element
            .find('[data-status="invalid"]')
            .find('.badge')
            .text(storageItem.InvalidCount)
            .toggle(storageItem.InvalidCount > 0);
    }

    function addDataItem(parentElement, itemValue, originalVisibility, iconClass, textClass) {
        parentElement.append(
            $('<div>')
                .toggle(originalVisibility)
                .addClass('row border-top')
                .attr('data-item-container', '')
                .append(
                    $('<div>')
                        .addClass('col-xs-10 border-right')
                        .addClass(textClass)
                        .text(itemValue)
                )
                .append(
                    $('<div>')
                        .addClass('col-xs-2')
                        .append(
                            $('<span>')
                                .addClass(iconClass)
                                .html('&nbsp;')
                        )
                )
        );
    }

    function refreshStatus(element, parentRow, url) {

        element.hide();
        var indicator = parentRow.find('.indicator').show();
        var errorMessage = parentRow.find('[data-error]').text('');

        var onSuccess = function (data) {

            var target = parentRow.find('[data-target]');
            var source = parentRow.find('[data-source]');

            var originalVisibility = parentRow.find('[data-item-container]').is(':visible');

            setStatusCount(target, data.TargetItem);
            setStatusCount(source, data.SourceItem);

            var targetContainer = target.find('[data-item-container]');
            targetContainer.remove();

            var sourceContainer = source.find('[data-item-container]');
            sourceContainer.remove();

            for (var i = 0; i < data.DataItemCount; i++) {

                var targetItem = data.TargetItem.Items[i];
                var sourceItem = data.SourceItem.Items[i];

                var targetIconClass = '';
                var targetTextClass = '';
                var targetValue = targetItem.Value;
                if (!targetItem.Value && sourceItem.Value) {
                    targetIconClass = 'glyphicon glyphicon-arrow-left text-warning';
                    targetTextClass = 'text-transparent';
                    targetValue = sourceItem.Value;
                } else if (targetItem.Value && !sourceItem.Value) {
                    targetIconClass = 'glyphicon glyphicon-remove text-danger';
                }

                var sourceIconClass = '';
                var sourceTextClass = '';
                var sourceValue = sourceItem.Value;
                if (targetItem.Value && sourceItem.Value && (targetItem.Value === sourceItem.Value)) {
                    sourceIconClass = 'glyphicon glyphicon-ok text-success';
                    targetIconClass = sourceIconClass;
                }

                addDataItem(target, targetValue, originalVisibility, targetIconClass, targetTextClass);
                addDataItem(source, sourceValue, originalVisibility, sourceIconClass, sourceTextClass);
            }

            indicator.hide();
            element.show();
        };

        var onError = function () {
            errorMessage.text('An error has occurred.')
            indicator.hide();
            element.show();
        };

        var options = {
            data: {
                dataSource: parentRow.data('source'),
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: onSuccess,
            error: onError,
            url: url
        };

        cheroes.dataManager.sendRequest(options);
    }

}(this.cheroes = this.cheroes || {}, jQuery));
