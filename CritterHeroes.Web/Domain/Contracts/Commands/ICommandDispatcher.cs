using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Shared.Commands;

namespace CritterHeroes.Web.Domain.Contracts.Commands
{
    public interface ICommandDispatcher
    {
        Task<CommandResult> DispatchAsync<TParameter>(TParameter command) where TParameter : class;
        CommandResult Dispatch<TParameter>(TParameter command) where TParameter : class;
    }
}
