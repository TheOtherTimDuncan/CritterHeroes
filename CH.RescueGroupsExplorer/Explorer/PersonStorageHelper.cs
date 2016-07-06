using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;

namespace CH.RescueGroupsExplorer.Explorer
{
    public class PersonStorageHelper : BaseStorageHelper<PersonSource>
    {
        public PersonStorageHelper(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
        }

        protected override IRescueGroupsStorageContext<PersonSource> CreateStorageContext(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
        {
            return new PersonSourceStorage(configuration, client, publisher);
        }

        public override PersonSource CreateEntity()
        {
            throw new NotImplementedException();
        }

        public override void UpdateEntity(PersonSource entity)
        {
            throw new NotImplementedException();
        }
    }
}
