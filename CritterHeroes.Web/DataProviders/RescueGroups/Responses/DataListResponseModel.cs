using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Responses
{
    public class DataListResponseModel<TEntity> where TEntity :class
    {
        public int FoundRows
        {
            get;
            set;
        }

        public IEnumerable<TEntity> Data
        {
            get;
            set;
        }
    }
}
