(function (arescues, $) {

    'use strict';

    $('[data-id]').each(function () {
        var that = $(this);
        that.find('.refresh').each(function(){
            $(this).click(function () {
                refreshStatus($(this), that);
            });
            refreshStatus($(this), that);
        });

    });

    $('.item-container').hide();
    $('.sync-container').hide();

    $('[data-toggle-values]').click(function () {
        var _this = $(this);
        _this.parent().parent().find('.sync-container').toggle();
        var container = _this.parent().parent().find('.item-container').toggle();
        var isVisible = container.is(':visible');
        _this.find('.glyphicon')
            .toggleClass('glyphicon-plus', !isVisible)
            .toggleClass('glyphicon-minus', isVisible);
    });

    function getStatusIcon(targetItem, sourceItem) {
        if (!targetItem.Value && sourceItem.Value) {
            return 'glyphicon-arrow-left text-warning';
        }

        if (targetItem.Value && !sourceItem.Value) {
            return 'glyphicon-remove text-danger';
        }

        if (targetItem.Value && sourceItem.Value && (targetItem.Value === sourceItem.Value)) {
            return 'glyphicon-ok text-success';
        }

        return ''; // Default
    }

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

    function addDataItem(parentElement, item) {
        parentElement.append(
            $('<li/>')
                .append(
                    $("<span>")
                        .text(item.Value)
                )
                .append('&nbsp;')
        );
    }

    function refreshStatus(element, parentRow) {

        element.hide();
        var indicator = parentRow.find('.indicator');
        indicator.show();

        var success = function (data) {

            var target = parentRow.find('[data-target]');
            var source = parentRow.find('[data-source]');

            setStatusCount(target, data.TargetItem);
            setStatusCount(source, data.SourceItem);

            var targetContainer = target.find('.item-container');
            var targetOriginalVisibility = targetContainer.is(':visible');
            targetContainer.hide().empty();

            var sourceContainer = source.find('.item-container');
            var sourceOriginalVisibility = sourceContainer.is(':visible');
            sourceContainer.hide().empty();

            var syncContainer = parentRow.find('.sync-container');
            var syncOriginalVisibility = syncContainer.is(':visible');
            syncContainer.hide().empty();

            for (var i = 0; i < data.DataItemCount; i++) {
                var targetItem = data.TargetItem.Items[i];
                addDataItem(targetContainer, targetItem);

                var sourceItem = data.SourceItem.Items[i];
                addDataItem(sourceContainer, sourceItem);

                syncContainer.append(
                    $('<li>').append(
                        $('<span>')
                            .addClass('glyphicon')
                            .addClass(getStatusIcon(targetItem, sourceItem))
                    )
                );
            }

            targetContainer.toggle(targetOriginalVisibility);
            sourceContainer.toggle(sourceOriginalVisibility);
            syncContainer.toggle(syncOriginalVisibility);

            indicator.hide();
            element.show();
        };

        var options = {
            data: {
                modelID: parentRow.data('id')
            },
            success: success,
            url: arescues.rootUrl + 'admin/datamaintenance/getmodelstatus'
        };

        arescues.dataManager.sendRequest(options);
    }

}(this.arescues = this.arescues || {}, jQuery));
