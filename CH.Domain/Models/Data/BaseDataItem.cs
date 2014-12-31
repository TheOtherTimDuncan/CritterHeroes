using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts.Dashboard;

namespace CH.Domain.Models.Data
{
    public abstract class BaseDataItem<T> : IDataItem<T> where T : class
    {
        public abstract bool Equals(T other);
    }
}
