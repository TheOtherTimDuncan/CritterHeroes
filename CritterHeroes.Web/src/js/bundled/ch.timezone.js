(function (cheroes) {


    var key = 'CritterHeroes_Timezone';
    var expirationDays = 1;

    if (!cheroes.cookieManager.readCookie(key)) {

        var date = new Date();

        var tz = {
            offsetMinutes: date.getTimezoneOffset(),
            javascriptTime: date.toString()
        };

        cheroes.cookieManager.createCookie(key, JSON.stringify(tz), expirationDays);
    }

}(this.cheroes = this.cheroes || {}));
