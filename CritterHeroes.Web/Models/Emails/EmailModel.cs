using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Models.Emails
{
    public class EmailModel
    {
        public string From
        {
            get;
            set;
        }

        public IEnumerable<string> To
        {
            get;
            set;
        }

        public string SubjectTemplate
        {
            get;
            set;
        }

        public string HtmlTemplate
        {
            get;
            set;
        }

        public string TextTemplate
        {
            get;
            set;
        }

        public BaseEmailData EmailData
        {
            get;
            set;
        }
    }
}
