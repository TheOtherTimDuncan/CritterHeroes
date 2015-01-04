using System;

namespace CritterHeroes.Web.Contracts.Commands
{
    public interface IUserCommand
    {
        // Username is allowed to be null
        string Username
        {
            get;
        }
    }
}
