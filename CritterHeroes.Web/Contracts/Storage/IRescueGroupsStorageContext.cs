using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IRescueGroupsStorageContext<T> : IStorageContext<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(params SearchFilter[] searchFilters);
    }
}
