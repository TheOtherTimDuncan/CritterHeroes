using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models;

namespace CritterHeroes.Web.Contracts.Storage
{
    public interface IContactsStorageContext
    {
        IQueryable<Business> Businesses
        {
            get;
        }

        IQueryable<Person> People
        {
            get;
        }

        IQueryable<Group> Groups
        {
            get;
        }

    }
}
