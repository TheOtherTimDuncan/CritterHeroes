using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.Logging;

namespace CritterHeroes.Web.Contracts.Commands
{
    public interface IAsyncUserCommandHandler<in TParameter> : IAsyncCommandHandler<TParameter>
        where TParameter : class, IUserCommand
    {
        UserActions SuccessUserAction
        {
            get;
        }

        UserActions FailedUserAction
        {
            get;
        }
    }
}
