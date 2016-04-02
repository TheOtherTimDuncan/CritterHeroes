using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IRescueGroupsStorageContext<T> where T : class
    {
        int ResultLimit
        {
            get;
            set;
        }

        IEnumerable<SearchField> Fields
        {
            get;
        }

        string FilterProcessing
        {
            get;
            set;
        }

        Task<T> GetAsync(string entityID);
        Task<IEnumerable<T>> GetAllAsync(params SearchFilter[] searchFilters);
    }
}
