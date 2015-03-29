using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Common.Email
{
    public abstract class BaseEmailCommandHandler<T> : IEmailHandler<T> where T : EmailCommand
    {
        private IEmailClient _emailClient;

        public BaseEmailCommandHandler(IEmailClient emailClient)
        {
            this._emailClient = emailClient;
        }

        public async Task<CommandResult> ExecuteAsync(T emailCommand)
        {
            EmailMessage message = CreateEmail(emailCommand);

            message.To.Add(emailCommand.EmailTo);

            await _emailClient.SendAsync(message);

            return CommandResult.Success();
        }

        protected abstract EmailMessage CreateEmail(T emailCommand);
    }
}