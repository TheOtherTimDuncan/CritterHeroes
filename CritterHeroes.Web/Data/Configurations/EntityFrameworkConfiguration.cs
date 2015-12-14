using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace CritterHeroes.Web.Data.Configurations
{
    public class EntityFrameworkConfiguration : DbConfiguration
    {
        public EntityFrameworkConfiguration()
        {
            SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
        }
    }
}
