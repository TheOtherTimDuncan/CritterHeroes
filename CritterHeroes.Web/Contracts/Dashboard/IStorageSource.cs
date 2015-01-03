using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Dashboard
{
    public interface IStorageSource
    {
        int ID
        {
            get;
        }

        string Title
        {
            get;
        }
    }
}
