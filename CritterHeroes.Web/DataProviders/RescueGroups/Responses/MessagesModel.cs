﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Responses
{
    public class MessagesModel
    {
        public IEnumerable<GeneralMessageModel> GeneralMessages
        {
            get;
            set;
        }

        public IEnumerable<RecordMessageModel> RecordMessages
        {
            get;
            set;
        }
    }
}
