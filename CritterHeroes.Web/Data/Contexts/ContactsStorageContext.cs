using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Data.Contexts
{
    public class ContactsStorageContext : BaseDbContext<ContactsStorageContext>, IContactsStorageContext
    {
        public ContactsStorageContext(IHistoryLogger logger)
            : base(logger)
        {
        }

        public virtual IDbSet<Business> _Businesses
        {
            get;
            set;
        }

        public virtual IDbSet<Person> _People
        {
            get;
            set;
        }

        public virtual IDbSet<Group> _Groups
        {
            get;
            set;
        }

        public IQueryable<Business> Businesses
        {
            get
            {
                return _Businesses;
            }
        }

        public IQueryable<Group> Groups
        {
            get
            {
                return _Groups;
            }
        }

        public IQueryable<Person> People
        {
            get
            {
                return _People;
            }
        }
    }
}
