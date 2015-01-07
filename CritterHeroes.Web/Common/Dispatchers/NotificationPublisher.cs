using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Notifications;
using SimpleInjector;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Common.Dispatchers
{
    public class NotificationPublisher : INotificationPublisher
    {
        private Container _container;

        public NotificationPublisher(Container container)
        {
            ThrowIf.Argument.IsNull(container, "container");
            this._container = container;
        }
        public async Task PublishAsync<TNotification>(TNotification notification) where TNotification : IAsyncNotification
        {
            foreach (var handler in _container.GetAllInstances<IAsyncNotificationHandler<TNotification>>())
            {
                await handler.ExecuteAsync(notification);
            }
        }

        public void Publish<TNotification>(TNotification notification) where TNotification : INotification
        {
            foreach (var handler in _container.GetAllInstances<INotificationHandler<TNotification>>())
            {
                handler.Execute(notification);
            }
        }
    }
}