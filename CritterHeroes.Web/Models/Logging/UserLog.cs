using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Models.Logging
{
    public class UserLog
    {
        public UserLog(Guid logID, UserActions userAction, string userName, DateTime whenOccurredUtc)
        {
            this.ID = logID;
            this.Action = userAction;
            this.Username = userName;
            this.WhenOccurredUtc = whenOccurredUtc;
        }

        public UserLog(UserActions userAction, string userName, DateTime whenOccurredUtc)
            : this(Guid.NewGuid(), userAction, userName, whenOccurredUtc)
        {
        }

        public Guid ID
        {
            get;
            private set;
        }

        public UserActions Action
        {
            get;
            private set;
        }

        public string Username
        {
            get;
            private set;
        }

        public DateTime WhenOccurredUtc
        {
            get;
            private set;
        }

        public string AdditionalData
        {
            get;
            set;
        }
    }
}
