using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Features.Common.Models
{
    public class SelectOptionModel
    {
        public int Value
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
