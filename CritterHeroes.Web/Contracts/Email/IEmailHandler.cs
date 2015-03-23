using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Email;

namespace CritterHeroes.Web.Contracts.Email
{
    public interface IEmailHandler<T> where T : EmailCommand
    {
        Task<CommandResult> ExecuteAsync(T emailCommand);
    }
}
