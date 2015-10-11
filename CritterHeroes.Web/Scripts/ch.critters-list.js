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
                var rows = [];
                for (var c = 0; c < data.Critters.length; c++) {
                    var critter = data.Critters[c];
                    rows.push(
                        $('<tr>').append(
                            $('<td>').html(getCritterImageHtml(critter)),
                            $('<td>').text(critter.Name),
                            $('<td>').text(critter.SexName),
                            $('<td>').text(critter.Status),
                            $('<td>').text(critter.Breed),
                            $('<td>').text(critter.FosterName)
                        )
                    );
                }
                container.html(rows);
            }

        });

    }

    function getCritterImageHtml(critter) {
        if (critter.PictureFilename) {
            return $('<img>').attr('height', 50).prop('src', pictureUrl + "/" + critter.ID + "/" + critter.PictureFilename + "?height=50");
        } else {
            return '&nbsp;';
        }
    }

}(this.cheroes = this.cheroes || {}, jQuery));
