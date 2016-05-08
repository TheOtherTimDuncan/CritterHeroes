(function (cheroes, $, should) {

    var key = 'test_cookie';
    var value = 'value';

    describe('cookieManager', function () {

        it('can create and read cookies', function () {
            cheroes.cookieManager.createCookie(key, value, 2);
            var result = cheroes.cookieManager.readCookie(key);
            result.should.equal(value);
            cheroes.cookieManager.removeCookie(key);
        });

    });

}(this.cheroes = this.cheroes || {}, jQuery, should));
