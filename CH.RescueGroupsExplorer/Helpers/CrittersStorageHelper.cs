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
    public class CrittersStorageHelper : BaseStorageHelper<CritterSource>
    {
        public CrittersStorageHelper(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
        }

        protected override IRescueGroupsStorageContext<CritterSource> CreateStorageContext(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
        {
            return new CritterSourceStorage(configuration, client, publisher);
        }
    }
}
