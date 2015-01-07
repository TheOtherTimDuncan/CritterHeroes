using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Notifications
{
    public interface INotificationHandler<in TNotification>
           where TNotification : INotification
    {
        void Execute(TNotification notification);
    }

    public interface IAsyncNotificationHandler<in TNotification>
        where TNotification : IAsyncNotification
    {
        Task ExecuteAsync(TNotification notification);
    }
}
