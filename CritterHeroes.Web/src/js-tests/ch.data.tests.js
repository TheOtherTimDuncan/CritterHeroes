(function (cheroes, $, should) {

    'use strict';

    describe('dataManager', function () {

        it('getHtml should execute an http get with the correct parameters', function () {

            var originalAjax = $.ajax;

            $.ajax = function (options) {
                options.dataType.should.equal('html');
                options.type.should.equal('GET');

            };

            cheroes.dataManager.getHtml();

            $.ajax = originalAjax;

        });

        it('sendRequest should execute an http post with the correct parameters', function () {

            var originalAjax = $.ajax;

            $.ajax = function (options) {
                options.dataType.should.equal('json');
                options.type.should.equal('POST');

            };

            cheroes.dataManager.sendRequest();

            $.ajax = originalAjax;

        });

    });

}(this.cheroes = this.cheroes || {}, jQuery, should));
