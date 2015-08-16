using System;
using System.Collections.Generic;
using CritterHeroes.Web.Areas.Admin.Lists.DataMappers;

namespace CritterHeroes.Web.Contracts.Dashboard
{
    public interface IDataMapperFactory
    {
        IDataMapper Create(DataSources dataSource);
    }
}
