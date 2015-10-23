using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Admin.Emails.Models;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Admin.Emails.Queries
{
    public class EmailQuery : IAsyncQuery<EmailModel>
    {
        public bool SendEmail
        {
            get;
            set;
        }
    }
}
