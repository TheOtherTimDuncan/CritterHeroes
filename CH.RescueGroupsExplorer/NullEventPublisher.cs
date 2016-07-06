using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Events;

namespace CH.RescueGroupsExplorer
{
    public class NullEventPublisher : IAppEventPublisher
    {
        public void Publish<TEventType>(TEventType appEvent) where TEventType : IAppEvent
        {
        }

        public void Subscribe<TEventType>(Action<TEventType> handler) where TEventType : IAppEvent
        {
        }
    }
}
