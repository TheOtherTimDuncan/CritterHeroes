using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Events
{
    public interface IAppEventPublisher
    {
        void Publish<TEventType>(TEventType appEvent) where TEventType : IAppEvent;
    }
}
