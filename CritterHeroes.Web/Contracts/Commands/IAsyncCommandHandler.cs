using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;

namespace CritterHeroes.Web.Contracts.Commands
{
    public interface IAsyncCommandHandler<in TParameter>
        where TParameter : class
    {
        Task<CommandResult> ExecuteAsync(TParameter command);
    }
}
