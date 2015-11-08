using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface ICritterBatchSqlStorageContext
    {
        IQueryable<CritterStatus> CritterStatus
        {
            get;
        }

        IQueryable<Breed> Breeds
        {
            get; 
        }

        IQueryable<Species> Species
        {
            get;
        }

        IQueryable<Critter> Critters
        {
            get;
        }

        IQueryable<Person> People
        {
            get;
        }

        IQueryable<Location> Locations
        {
            get;
        }

        void AddCritter(Critter critter);
        Task<int> SaveChangesAsync();
    }
}
