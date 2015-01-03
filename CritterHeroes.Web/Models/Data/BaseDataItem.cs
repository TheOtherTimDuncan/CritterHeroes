using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Dashboard;

namespace CritterHeroes.Web.Models.Data
{
    public abstract class BaseDataItem<T> : IDataItem<T> where T : class
    {
        public abstract bool Equals(T other);
    }
}
