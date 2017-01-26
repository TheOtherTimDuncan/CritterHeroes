using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Domain.Contracts
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

        bool IsAuthenticated
        {
            get;
        }

        bool IsInRole(string role);
    }
}
