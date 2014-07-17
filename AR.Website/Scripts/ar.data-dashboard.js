(function (arescues, $) {

    $('tr[data-id]').each(function () {
        var that = $(this);
        that.find('.refresh').each(function(){
            $(this).click(function () {
                refreshStatus($(this), that);
            });
            refreshStatus($(this), that);
        });

    });

    function refreshStatus(element, parentRow) {

        element.hide();
        parentRow.find('.indicator').addClass('busy');

        var success = function (data) {

            for (var i = 0; i < data.StorageItems.length; i++) {
                var storageItem = data.StorageItems[i];
                parentRow.find('[data-storage="' + storageItem.StorageID + '"][data-status="valid"]').find('.badge').text(storageItem.ValidCount);
                parentRow.find('[data-storage="' + storageItem.StorageID + '"][data-status="invalid"]').find('.badge').text(storageItem.InvalidCount);
            }

            parentRow.find('.indicator').removeClass('busy');
            element.show();
        }

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
