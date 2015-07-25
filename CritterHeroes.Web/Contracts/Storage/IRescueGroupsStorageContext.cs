using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IRescueGroupsStorageContext<T> : IStorageContext<T> where T : class
    {
    }
}
