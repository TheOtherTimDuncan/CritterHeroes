using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IRescueGroupsStorageContext<TEntity> where TEntity : BaseSource
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

        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<TEntity> GetAsync(int entityID);
        Task<IEnumerable<TEntity>> GetAllAsync(params SearchFilter[] searchFilters);
    }
}
