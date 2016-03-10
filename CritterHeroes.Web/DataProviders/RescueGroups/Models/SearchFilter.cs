using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class SearchFilterOperation
    {
        public const string Equal = "equal";
        public const string NotBlank = "notblank";
        public const string GreaterThanOrEqual = "greaterthanorequal";
    }

    public class SearchFilter
    {
        public string FieldName
        {
            get;
            set;
        }

        public string Operation
        {
            get;
            set;
        }

        public string Criteria
        {
            get;
            set;
        }
    }
}
