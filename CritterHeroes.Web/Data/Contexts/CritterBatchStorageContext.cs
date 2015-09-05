using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Contexts
{
    public class CritterBatchStorageContext : BaseDbContext, ICritterBatchSqlStorageContext
    {
        public virtual IDbSet<Breed> _Breeds
        {
            get;
            set;
        }

        public virtual IDbSet<CritterStatus> _CritterStatus
        {
            get;
            set;
        }

        public virtual IDbSet<Species> _Species
        {
            get;
            set;
        }

        public virtual IDbSet<Critter> _Critters
        {
            get;
            set;
        }

        public virtual IDbSet<Person> _Persons
        {
            get;
            set;
        }

        public IQueryable<Breed> Breeds
        {
            get
            {
                return _Breeds;
            }
        }

        public IQueryable<Critter> Critters
        {
            get
            {
                return _Critters;
            }
        }

        public IQueryable<CritterStatus> CritterStatus
        {
            get
            {
                return _CritterStatus;
            }
        }

        public IQueryable<Species> Species
        {
            get
            {
                return _Species;
            }
        }

        public IQueryable<Person> People
        {
            get
            {
                return _Persons;
            }
        }

        public void AddCritter(Critter critter)
        {
            _Critters.Add(critter);
        }
    }
}
