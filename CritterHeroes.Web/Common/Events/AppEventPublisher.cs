using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Events;
using SimpleInjector;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Common.Events
{
    public class AppEventPublisher : IAppEventPublisher
    {
        private Container _container;

        public AppEventPublisher(Container container)
        {
            ThrowIf.Argument.IsNull(container, nameof(container));
            this._container = container;
        }

        public void Publish<TAppEvent>(TAppEvent appEvent) where TAppEvent : IAppEvent
        {
            IEnumerable<IAppEventHandler<TAppEvent>> handlers = _container.GetAllInstances<IAppEventHandler<TAppEvent>>();
            handlers.NullSafeOrderBy(x => x.Order ?? 999).NullSafeForEach((IAppEventHandler<TAppEvent> handler) =>
            {
                handler.Handle(appEvent);
            });
        }
    }
}
