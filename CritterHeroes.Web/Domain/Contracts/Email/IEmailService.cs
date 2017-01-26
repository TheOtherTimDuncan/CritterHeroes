using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Shared.Commands;

namespace CritterHeroes.Web.Domain.Contracts.Email
{
    public interface IEmailService<TCommand> where TCommand : EmailCommandBase
    {
        Task<CommandResult> SendEmailAsync(TCommand command);
    }
}
