using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Commands;

namespace CritterHeroes.Web.Common.Commands
{
    public class BaseUserCommand : IUserCommand
    {
        public string Username
        {
            get;
            set;
        }
    }
}