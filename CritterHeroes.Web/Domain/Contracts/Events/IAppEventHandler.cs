using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Domain.Contracts.Events
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
