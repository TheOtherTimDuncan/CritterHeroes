using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IRescureGroupsStorageContext<T> : IStorageContext<T> where T : class
    {
    }
}
