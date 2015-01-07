using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;

namespace CritterHeroes.Web.Contracts.Commands
{
    public interface ICommandHandler<in TParameter> where TParameter : class
    {
        CommandResult Execute(TParameter command);
    }

    public interface IAsyncCommandHandler<in TParameter> where TParameter : class
    {
        Task<CommandResult> ExecuteAsync(TParameter command);
    }
}
