using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Events
{
    public interface IAppEventPublisher
    {
        void Subscribe<TEventType>(Action<TEventType> handler) where TEventType : IAppEvent;
        void Publish<TEventType>(TEventType appEvent) where TEventType : IAppEvent;
    }
}
