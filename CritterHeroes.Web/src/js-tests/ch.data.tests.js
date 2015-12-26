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

        it('should call badRequest callback for status code 400', function () {

            var originalAjax = $.ajax;

            var testResponse = {
                status: 400,
                responseJSON: 'json',
                statusText: 'statusText'
            };

            $.ajax = function (options) {
                options.error(testResponse);
            };

            cheroes.dataManager.sendRequest({
                badRequest: function (response, statusText, status) {
                    response.should.equal(testResponse.responseJSON);
                    statusText.should.equal(testResponse.statusText);
                    status.should.equal(testResponse.status);
                }
            });

            $.ajax = originalAjax;

        });

        it('should call error callback for non-successful status code other than 400', function () {

            var originalAjax = $.ajax;

            var response = {
                jqXhr: 'jqXhr',
                textStatus: 'textStatus',
                errorThrown: 'errorThrown'
            };

            $.ajax = function (options) {
                options.error(response.jqXhr, response.textStatus, response.errorThrown);
            };

            cheroes.dataManager.sendRequest({
                error: function (jqXhr, textStatus, errorThrown) {
                    jqXhr.should.equal(response.jqXhr);
                    textStatus.should.equal(response.textStatus);
                    errorThrown.should.equal(response.errorThrown);
                }
            });

            $.ajax = originalAjax;

        });

        it('should call success callback for successful ajax request', function () {

            var originalAjax = $.ajax;

            var response = {
                responseJSON: 'json',
                jqXhr: {
                    status: 200
                },
                statusText: 'textStatus'
            };

            $.ajax = function (options) {
                options.success(response.responseJSON, response.statusText, response.jqXhr);
            };

            cheroes.dataManager.sendRequest({
                success: function (data, statusText, statusCode) {
                    data.should.equal(response.responseJSON);
                    statusText.should.equal(response.statusText);
                    statusCode.should.equal(response.jqXhr.status);
                }
            });

            $.ajax = originalAjax;

        });

    });

}(this.cheroes = this.cheroes || {}, jQuery, should));
