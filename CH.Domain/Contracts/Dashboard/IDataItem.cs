using System;
using System.Collections.Generic;

namespace CH.Domain.Contracts.Dashboard
{
    public interface IDataItem<T> where T : class
    {
        bool Equals(T other);
    }
}
