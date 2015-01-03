using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Dashboard
{
    public interface IDataItem<T> where T : class
    {
        bool Equals(T other);
    }
}
