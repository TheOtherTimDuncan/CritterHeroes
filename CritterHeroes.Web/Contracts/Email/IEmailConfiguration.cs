using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Email
{
    public interface IEmailConfiguration
    {
        string DefaultFrom
        {
            get;
        }
    }
}
