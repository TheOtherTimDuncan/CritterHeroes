using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Notifications;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Notifications;

namespace CritterHeroes.Web.Common.NotificationHandlers
{
    public class UserActionNotificationHandler : IAsyncNotificationHandler<UserActionNotification>
    {
        private IUserLogger _userLogger;

        public UserActionNotificationHandler(IUserLogger userLogger)
        {
            this._userLogger = userLogger;
        }

        public async Task ExecuteAsync(UserActionNotification notification)
        {
            await _userLogger.LogActionAsync(notification.Action, notification.Username, notification.AdditionalData);
        }
    }
}