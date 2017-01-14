using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Contracts.Email
{
    public interface IEmailBuilder<TCommand> where TCommand : EmailCommand<BaseEmailData>
    {
        EmailMessage BuildEmail(TCommand command);
    }
}
