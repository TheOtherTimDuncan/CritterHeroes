using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;

namespace CH.RescueGroupsExplorer.Helpers
{
    public class BreedStorageHelper : BaseStorageHelper<BreedSource>
    {
        public BreedStorageHelper(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
        }

        protected override IRescueGroupsStorageContext<BreedSource> CreateStorageContext(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
        {
            return new BreedSourceStorage(configuration, client, publisher);
        }

        public override BreedSource CreateEntity()
        {
            throw new NotImplementedException();
        }

        public override void UpdateEntity(BreedSource entity)
        {
            throw new NotImplementedException();
        }
    }
}
