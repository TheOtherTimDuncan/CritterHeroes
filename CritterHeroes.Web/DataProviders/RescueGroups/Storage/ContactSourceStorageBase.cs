using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Configuration;
using CritterHeroes.Web.Domain.Contracts.Events;

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
