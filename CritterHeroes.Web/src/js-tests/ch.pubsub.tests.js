(function (cheroes, should) {

    'use strict';

    describe('pubsub', function () {

        it('publish should include context passed to subscribe', function () {

            var event = 'event';

            var context = {
                test: 'test'
            };

            var data = {
                data: 'data'
            };

            cheroes.pubsub.subscribe(event, {
                context: context,
                callback: function (pubData, pubContext) {
                    pubData.data.should.equal(data.data);
                    pubContext.test.should.equal(context.test);
                }
            });

            cheroes.pubsub.publish(event, data);
        });

    });

}(this.cheroes = this.cheroes || {}, should));
