(function (cheroes, should) {

    'use strict';

    describe('pubsub', function () {

        it('should call subscriber when event is published', function () {

            var event1 = 'event';
            var data1 = 'data';

            cheroes.pubsub.subscribe(event, {
                callback: function (pubData) {
                    pubData.should.equal(data1);
                }
            });

            cheroes.pubsub.publish(event1, data1);

        });

        it('publish should include context passed to subscribe', function () {

            var event2 = 'event';

            var context = {
                test: 'test'
            };

            var data2 = {
                data: 'data'
            };

            cheroes.pubsub.subscribe(event, {
                context: context,
                callback: function (pubData, pubContext) {
                    pubData.data.should.equal(data2.data);
                    pubContext.test.should.equal(context.test);
                }
            });

            cheroes.pubsub.publish(event2, data2);
        });

    });

}(this.cheroes = this.cheroes || {}, should));
