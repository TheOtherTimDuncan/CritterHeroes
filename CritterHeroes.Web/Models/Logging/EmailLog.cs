using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Common;
using Newtonsoft.Json;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Models.Logging
{
    public class EmailLog
    {
        public EmailLog(object emailData)
            : this(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
            this.EmailData = JavascriptConvert.SerializeObject(emailData).ToString();
        }

        public EmailLog(Guid logID, DateTimeOffset whenSentUtc)
        {
            this.ID = logID;
            this.WhenCreatedUtc = whenSentUtc;
        }

        public Guid ID
        {
            get;
            private set;
        }

        public DateTimeOffset WhenCreatedUtc
        {
            get;
            private set;
        }

        public string EmailData
        {
            get;
            set;
        }

        public T ConvertEmailData<T>() where T : class
        {
            if (EmailData.IsNullOrEmpty())
            {
                return null;
            }
            return JsonConvert.DeserializeObject<T>(EmailData);
        }
    }
}
