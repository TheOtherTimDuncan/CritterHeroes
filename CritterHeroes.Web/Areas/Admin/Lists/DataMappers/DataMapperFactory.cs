using System;
using System.Collections.Generic;
using CritterHeroes.Web.Contracts.Dashboard;

namespace CritterHeroes.Web.Areas.Admin.Lists.DataMappers
{
    public class DataMapperFactory : Dictionary<DataSources, Func<IDataMapper>>, IDataMapperFactory
    {
        public IDataMapper Create(DataSources dataSource)
        {
            return this[dataSource]();
        }
    }
}
