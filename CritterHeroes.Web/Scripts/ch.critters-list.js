(function (cheroes, $) {

    'use strict';

    var query = {
    };

    var container = $('#critters-container tbody');
    var critterUrl = container.data('url');
    var pictureUrl = container.data('picture-url');

    $('select[data-filter]').each(function () {
        $(this).on('change', function () {
            query[$(this).data('filter')] = $(this).val();
            getData();
        });
    });

    getData();

    function getData() {

        cheroes.dataManager.sendRequest({

            url: critterUrl,
            data: query,

            success: function (data) {
                if (data.critters && data.critters.length > 0) {
                    var rows = [];
                    for (var c = 0; c < data.critters.length; c++) {
                        var critter = data.critters[c];
                        rows.push(
                            $('<tr>').append(
                                $('<td>').html(getCritterImageHtml(critter)),
                                $('<td>').text(critter.name),
                                $('<td>').text(critter.sexName),
                                $('<td>').text(critter.status),
                                $('<td>').text(critter.breed),
                                $('<td>').text(critter.fosterName)
                            )
                        );
                    }
                    container.html(rows);
                }
            }

        });

    }

    function getCritterImageHtml(critter) {
        if (critter.pictureFilename) {
            return $('<img>').attr('height', 50).prop('src', pictureUrl + "/" + critter.id + "/" + critter.pictureFilename + "?height=50");
        } else {
            return '&nbsp;';
        }
    }

}(this.cheroes = this.cheroes || {}, jQuery));
