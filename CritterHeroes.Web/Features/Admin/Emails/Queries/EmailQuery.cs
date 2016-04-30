using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Admin.Emails.Models;

namespace CritterHeroes.Web.Features.Admin.Emails.Queries
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
