using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Notifications
{
    public interface INotificationPublisher
    {
        Task PublishAsync<TNotification>(TNotification notification) where TNotification : IAsyncNotification;
        void Publish<TNotification>(TNotification notification) where TNotification : INotification;
    }
}
