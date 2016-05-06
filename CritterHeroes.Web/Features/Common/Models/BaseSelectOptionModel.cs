using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Features.Common.Models
{
    public class BaseSelectOptionModel<TValue>
    {
        public TValue Value
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public bool IsSelected
        {
            get;
            set;
        }
    }
}
