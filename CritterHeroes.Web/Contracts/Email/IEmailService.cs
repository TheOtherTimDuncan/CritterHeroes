using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Models.Emails;

namespace CritterHeroes.Web.Contracts.Email
{
    public interface IEmailService
    {
        Task<CommandResult> SendEmailAsync<EmailDataType>(EmailCommand<EmailDataType> command) where EmailDataType : BaseEmailData, new();
    }
}
