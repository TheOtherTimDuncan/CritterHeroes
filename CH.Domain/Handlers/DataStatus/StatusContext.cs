using System;
using System.Collections.Generic;
using CH.Domain.Models.Data;

namespace CH.Domain.Handlers.DataStatus
{
    public class StatusContext
    {
        public IEnumerable<Species> SupportedCritters
        {
            get;
            set;
        }
    }
}
