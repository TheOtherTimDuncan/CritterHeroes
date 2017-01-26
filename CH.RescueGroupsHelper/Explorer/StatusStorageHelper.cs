using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Configuration;
using CritterHeroes.Web.Domain.Contracts.Events;
using CritterHeroes.Web.Domain.Contracts.Storage;

namespace CH.RescueGroupsHelper.Explorer
{
    public class StatusStorageHelper : BaseStorageHelper<CritterStatusSource>
    {
        public StatusStorageHelper(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
        }

        protected override IRescueGroupsStorageContext<CritterStatusSource> CreateStorageContext(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
        {
            return new CritterStatusSourceStorage(configuration, client, publisher);
        }

        public override CritterStatusSource CreateEntity()
        {
            throw new NotImplementedException();
        }

        public override void UpdateEntity(CritterStatusSource entity)
        {
            throw new NotImplementedException();
        }
    }
}
