﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Domain.Contracts.Storage
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

        IQueryable<CritterColor> Colors
        {
            get;
        }

        void AddCritter(Critter critter);
        Task<int> SaveChangesAsync();
    }
}
