using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Domain.Contracts.Events;
using CritterHeroes.Web.Domain.Contracts.Storage;

namespace CritterHeroes.Web.Data.Contexts
{
    public class ContactsStorageContext : BaseDbContext<ContactsStorageContext>, IContactsStorageContext
    {
        public ContactsStorageContext(IAppEventPublisher publisher)
            : base(publisher)
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
                return _Businesses.AsNoTracking();
            }
        }

        public IQueryable<Group> Groups
        {
            get
            {
                return _Groups.AsNoTracking();
            }
        }

        public IQueryable<Person> People
        {
            get
            {
                return _People.AsNoTracking();
            }
        }
    }
}
