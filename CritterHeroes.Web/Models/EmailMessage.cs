using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Models
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            To = new List<string>();
        }

        public string From
        {
            get;
            set;
        }

        public ICollection<string> To
        {
            get;
            private set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string HtmlBody
        {
            get;
            set;
        }

        public string TextBody
        {
            get;
            set;
        }
    }
}
