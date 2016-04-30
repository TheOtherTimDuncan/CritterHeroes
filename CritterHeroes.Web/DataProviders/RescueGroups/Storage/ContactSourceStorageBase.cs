using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public abstract class ContactSourceStorageBase<T> : RescueGroupsStorage<T> where T : BaseContactSource
    {
        public ContactSourceStorageBase(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
        }

        public override string ObjectType
        {
            get
            {
                return "contacts";
            }
        }

        protected override string SortField
        {
            get
            {
                return "contactID";
            }
        }

        protected override string KeyField
        {
            get
            {
                return "contactID";
            }
        }

        public override bool IsPrivate
        {
            get
            {
                return true;
            }
        }
    }
}
