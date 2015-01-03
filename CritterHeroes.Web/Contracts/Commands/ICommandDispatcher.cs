using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Services.Commands;

namespace CritterHeroes.Web.Contracts.Commands
{
    public interface ICommandDispatcher
    {
        Task<CommandResult> DispatchAsync<TParameter>(TParameter command)
            where TParameter : class;

        CommandResult Dispatch<TParameter>(TParameter command)
        where TParameter : class;
    }
}
