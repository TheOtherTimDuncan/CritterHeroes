using System;
using System.Collections.Generic;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Domain.Contracts.Email
{
    public interface IEmailBuilder<TCommand> where TCommand : EmailCommandBase
    {
        EmailMessage BuildEmail(TCommand command);
    }
}
