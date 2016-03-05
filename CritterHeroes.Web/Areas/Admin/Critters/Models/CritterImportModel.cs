using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Admin.Critters.Models
{
    public class CritterImportModel
    {
        public IEnumerable<string> FieldNames
        {
            get;
            set;
        }

        public IEnumerable<string> Messages
        {
            get;
            set;
        }
    }
}
