(function (cheroes, $) {

    'use strict';

    $('[data-item-container]').hide();

    $('[data-id]').each(function () {

        var dataContainer = $(this);

        dataContainer.find('.refresh').each(function () {
            $(this).click(function () {
                refreshStatus($(this), dataContainer, 'refresh');
            });
            refreshStatus($(this), dataContainer, 'refresh');
        });

        dataContainer.find('.sync').each(function () {
            $(this).click(function () {
                refreshStatus($(this), dataContainer, 'sync');
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

    function addDataItem(parentElement, item, originalVisibility, iconClass) {
        parentElement.append(
            $('<div>')
                .toggle(originalVisibility)
                .addClass('row border-top')
                .attr('data-item-container', '')
                .append(
                    $('<div>')
                        .addClass('col-xs-10')
                        .text(item.Value)
                )
                .append(
                    $('<div>')
                        .addClass('col-xs-2 border-left')
                        .append(
                            $('<span>')
                                .addClass(iconClass)
                                .html('&nbsp;')
                        )
                )
        );
    }

    function refreshStatus(element, parentRow, action) {

        element.hide();
        var indicator = parentRow.find('.indicator');
        indicator.show();

        var success = function (data) {

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

                var targetClass = '';
                if (!targetItem.Value && sourceItem.Value) {
                    targetClass = 'glyphicon glyphicon-arrow-left text-warning';
                } else if (targetItem.Value && !sourceItem.Value) {
                    targetClass = 'glyphicon glyphicon-remove text-danger';
                }

                var sourceClass = '';
                if (targetItem.Value && sourceItem.Value && (targetItem.Value === sourceItem.Value)) {
                    sourceClass = 'glyphicon glyphicon-ok text-success';
                    targetClass = sourceClass;
                }

                addDataItem(target, targetItem, originalVisibility, targetClass);
                addDataItem(source, sourceItem, originalVisibility, sourceClass);
            }

            indicator.hide();
            element.show();
        };

        var options = {
            data: {
                modelID: parentRow.data('id')
            },
            success: success,
            url: cheroes.rootUrl + 'admin/datamaintenance/' + action
        };

        cheroes.dataManager.sendRequest(options);
    }

}(this.cheroes = this.cheroes || {}, jQuery));
