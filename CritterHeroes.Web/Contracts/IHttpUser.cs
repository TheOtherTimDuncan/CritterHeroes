using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts
{
    public interface IHttpUser
    {
        string Username
        {
            get;
        }

        string UserID
        {
            get;
        }

        bool IsInRole(string role);
    }
}
