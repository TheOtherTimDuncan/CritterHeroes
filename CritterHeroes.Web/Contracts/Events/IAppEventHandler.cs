using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Events
{
    public interface IAppEventHandler<TAppEvent> where TAppEvent : IAppEvent
    {
        void Handle(TAppEvent appEvent);
    }
}
