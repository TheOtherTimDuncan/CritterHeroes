using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Notifications;
using CritterHeroes.Web.Models.Logging;

namespace CritterHeroes.Web.Common.Notifications
{
    public class UserActionNotification : IAsyncNotification
    {
        public UserActionNotification(UserActions action)
            : this(action, null)
        {
        }

        public UserActionNotification(UserActions action, string userName)
            : this(action, userName, null)
        {
        }

        public UserActionNotification(UserActions action, string userName, object additionalData)
        {
            this.Action = action;
            this.Username = userName;
            this.AdditionalData = additionalData;
        }

        public string Username
        {
            get;
            private set;
        }

        public UserActions Action
        {
            get;
            private set;
        }

        public object AdditionalData
        {
            get;
            set;
        }
    }
}