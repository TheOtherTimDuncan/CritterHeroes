using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Contracts.Email
{
    public interface IEmailBuilder<TCommand, TEmailData>
        where TCommand : EmailCommand<TEmailData>
        where TEmailData : BaseEmailData, new()
    {
        EmailMessage BuildEmail(TCommand command);
    }
}
