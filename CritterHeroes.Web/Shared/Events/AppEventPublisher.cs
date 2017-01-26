using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.Events;
using SimpleInjector;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Shared.Events
{
    public class AppEventPublisher : IAppEventPublisher
    {
        private Container _container;

        private List<dynamic> _subscribers;

        public AppEventPublisher(Container container)
        {
            ThrowIf.Argument.IsNull(container, nameof(container));

            this._container = container;

            this._subscribers = new List<dynamic>();
        }

        public void Publish<TAppEvent>(TAppEvent appEvent) where TAppEvent : IAppEvent
        {
            Type handlerType = typeof(Action<>).MakeGenericType(appEvent.GetType());
            _subscribers.NullSafeForEach((dynamic handler) =>
            {
                if (handler.GetType() == handlerType)
                {
                    handler(appEvent);
                }
            });

            IEnumerable<IAppEventHandler<TAppEvent>> handlers = _container.GetAllInstances<IAppEventHandler<TAppEvent>>();
            handlers.NullSafeOrderBy(x => x.Order ?? 999).NullSafeForEach((IAppEventHandler<TAppEvent> handler) =>
            {
                handler.Handle(appEvent);
            });
        }

        public void Subscribe<TEventType>(Action<TEventType> handler) where TEventType : IAppEvent
        {
            _subscribers.Add(handler);
        }
    }
}
