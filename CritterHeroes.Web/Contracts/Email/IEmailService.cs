using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Shared.Commands;

namespace CritterHeroes.Web.Contracts.Email
{
    public interface IEmailService
    {
        Task<CommandResult> SendEmailAsync<EmailDataType>(EmailCommand<EmailDataType> command) where EmailDataType : BaseEmailData, new();
    }
}
