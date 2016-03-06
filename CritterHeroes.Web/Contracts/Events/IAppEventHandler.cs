using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Events
{
    public interface IAppEventHandler<TAppEvent> where TAppEvent : IAppEvent
    {
        int? Order
        {
            get;
        }

        void Handle(TAppEvent appEvent);
    }
}
