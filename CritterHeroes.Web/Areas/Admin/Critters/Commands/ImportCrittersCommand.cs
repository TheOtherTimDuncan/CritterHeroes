using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Areas.Admin.Critters.Commands
{
    public class ImportCrittersCommand
    {
        public IEnumerable<string> Messages
        {
            get;
            set;
        }
    }
}
