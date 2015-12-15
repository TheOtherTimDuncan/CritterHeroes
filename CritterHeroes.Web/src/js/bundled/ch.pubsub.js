(function (cheroes) {

    'use strict';

    var queue = [];

    function publish(eventName, data) {

        if (queue[eventName]) {
            for (var i = 0; i < queue[eventName].length; i++) {
                queue[eventName][i].callback(data);
            }
        }

    }

    function subscribe(eventName, callback) {

        if (!queue[eventName]) {
            queue[eventName] = [];
        }

        queue[eventName].push(
        {
            callback: callback
        });

    }

    var events = {
    };

    cheroes.pubsub = {
        events: events,
        publish: publish,
        subscribe: subscribe
    };

}(this.cheroes = this.cheroes || {}));
