using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Models
{
    public class SearchModel
    {
        public int ResultStart
        {
            get;
            set;
        }

        public int ResultLimit
        {
            get;
            set;
        }

        public string ResultSort
        {
            get;
            set;
        }

        public string ResultOrder
        {
            get
            {
                return "asc";
            }
        }

        public string CalcFoundRows
        {
            get
            {
                return "Yes";
            }
        }

        public IEnumerable<string> Fields
        {
            get;
            set;
        }

        public IEnumerable<SearchFilter> Filters
        {
            get;
            set;
        }
    }
}
