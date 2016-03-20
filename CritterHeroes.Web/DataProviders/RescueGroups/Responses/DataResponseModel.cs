using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Responses
{
    public class DataResponseModel<TData> : BaseResponseModel
    {
        public TData Data
        {
            get;
            set;
        }
    }
}
