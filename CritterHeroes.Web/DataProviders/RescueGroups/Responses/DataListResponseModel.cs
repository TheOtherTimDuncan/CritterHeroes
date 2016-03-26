using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.JsonConverters;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Responses
{
    public class DataListResponseModel<TEntity> : BaseResponseModel where TEntity : class
    {
        public int FoundRows
        {
            get;
            set;
        }

        [JsonConverter(typeof(EmptyDictionaryConverter))]
        public Dictionary<string, TEntity> Data
        {
            get;
            set;
        }
    }
}
