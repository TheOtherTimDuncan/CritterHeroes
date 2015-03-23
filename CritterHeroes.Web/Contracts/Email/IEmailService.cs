using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;

namespace CritterHeroes.Web.Contracts.Email
{
    public interface IEmailService
    {
        Task<CommandResult> SendEmailAsync<TParameter>(TParameter command) where TParameter : EmailCommand;
    }
}
