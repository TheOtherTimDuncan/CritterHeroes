(function (cheroes) {

    'use strict';

    var queue = [];

    /**
      * @param {string}  eventName  - The name of the event to be published
      * @param           data       - The optional data to be passed to the event subscribers
      */
    function publish(eventName, data) {

        var eventQueue = queue[eventName];

        if (eventQueue) {
            for (var i = 0; i < eventQueue.length; i++) {
                var subscriber = eventQueue[i];
                subscriber.callback(data, subscriber.context);
            }
        }

    }

    /**
     * Callback for subscribe
     * 
     * @callback subscribeCallback
     * @param {object}  options
     * @param           options.data    - Each event determines what is passed in data
     * @param           options.context - The value passed to options.context when subscribing
     */

    /**
     * @param {string}              eventName           - The name of the event to subscribe to
     * @param {object}              options             - The options for subscribing to the event
     * @param {subscribeCallback}   options.callback    - The function to call when the event is published.
     * @param                       options.context     - Optional value to be passed to the callback function when the event is published
     */
    function subscribe(eventName, options) {

        if (!queue[eventName]) {
            queue[eventName] = [];
        }

        queue[eventName].push(options);

    }

    cheroes.events = {
    };


    cheroes.pubsub = {
        publish: publish,
        subscribe: subscribe
    };

}(this.cheroes = this.cheroes || {}));
