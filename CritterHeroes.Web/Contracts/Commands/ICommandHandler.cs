using System;
using System.Collections.Generic;
using CritterHeroes.Web.Common.Services.Commands;

namespace CritterHeroes.Web.Contracts.Commands
{
    public interface ICommandHandler<in TParameter>
        where TParameter : class
    {
        CommandResult Execute(TParameter command);
    }
}
